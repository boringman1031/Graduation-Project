using System.Collections;
using UnityEngine;

public class Skill_NightRush : MonoBehaviour, ISkillEffect
{
    [Header("技能參數")]
    public float dashSpeed = 2f;         // 衝刺速度
    public float duration = 2f;          // 衝刺持續時間
    public float damage = 10f;           // 沿途對敵人的傷害
    public float energyCost = 10f;       // 消耗能量

    [Header("音效設定")]
    public AudioClip spawnSound;         // 生成音效
    public AudioClip hitSound;           // 撞擊音效

    private Transform origin;            // 觸發技能的玩家 Transform
    private Rigidbody2D rb;              // MountPoint 的剛體
    private Rigidbody2D playerRb;        // 玩家自己的 Rigidbody2D
    private float dashDirection;         // 用來記錄玩家原始面向：正數向右，負數向左

    public void SetPlayerAnimator(Animator animator)
    {
        // 可依需求實作
    }

    // 此方法會由玩家技能動畫事件呼叫
    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;

        // 播放生成音效
        if (spawnSound != null)
        {
            AudioSource.PlayClipAtPoint(spawnSound, origin.position);
        }

        // 設定 MountPoint 位置與旋轉
        transform.position = origin.position;
        transform.rotation = origin.rotation;

        // 取得或加入 Rigidbody2D，確保 MountPoint 為動態
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.isKinematic = false;

        // 禁用玩家移動控制及玩家 Rigidbody2D 物理運算
        //PlayerController playerCtrl = origin.GetComponent<PlayerController>();
        //if (playerCtrl != null)
        //{
        //    playerCtrl.enabled = false;
        //}
        playerRb = origin.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.isKinematic = true;
        }

        // 取得原始玩家面向 (未受 reparent 影響)
        dashDirection = (originTransform.localScale.x > 0 ? -1f : 1f);
        Debug.Log("dashDirection: " + dashDirection);

        // 根據 dashDirection 翻轉 MountPoint
        transform.localScale = new Vector3(-dashDirection, 1, 1);

        // 將玩家 reparent 到 MountPoint 下，保留世界位姿
        origin.SetParent(transform, true);

        // 啟動衝刺協程，使用 dashDirection 來設定速度
        StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(dashSpeed * dashDirection, 0f);
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        // 解除玩家與 MountPoint 的父子關係
        origin.SetParent(null);

        // 恢復玩家的 Rigidbody2D 物理運算與移動控制
        if (playerRb != null)
        {
            playerRb.isKinematic = false;
        }
        //PlayerController playerCtrl = origin.GetComponent<PlayerController>();
        //if (playerCtrl != null)
        //{
        //    playerCtrl.enabled = true;
        //}

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharactorBase target = collision.GetComponent<CharactorBase>();
        if (target != null && !target.CompareTag("Player"))
        {
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, collision.transform.position);
            }

            target.TakeDamage(damage, transform);
        }
    }
}
