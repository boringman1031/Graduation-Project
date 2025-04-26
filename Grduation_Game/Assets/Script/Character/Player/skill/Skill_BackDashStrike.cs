using UnityEngine;
using System.Collections;

public class Skill_BackDashStrike : MonoBehaviour, ISkillEffect
{
    [Header("技能參數")]
    public float dashDistance = 3f;
    public float dashDuration = 0.3f;
    public float baseDamage = 50f;
    public float energyCost = 10f;

    [Header("特效與音效")]
    public GameObject dashEffect;
    public AudioClip dashSound;
    public AudioDefination audioPlayer;

    [Header("攻擊設定")]
    public GameObject attackTriggerPrefab; // 觸發攻擊範圍用的 prefab（可加 Collider + Attack.cs）

    private Transform origin;
    private bool isDashing = false;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        transform.position = origin.position;

        var player = origin.GetComponent<CharactorBase>();
        var stats = origin.GetComponent<PlayerStats>();

        if (player.CurrentPower < energyCost)
        {
            Debug.Log("能量不足！");
            Destroy(gameObject);
            return;
        }

        player.AddPower(-energyCost);

        if (dashEffect) Instantiate(dashEffect, origin.position, Quaternion.identity);
        if (audioPlayer && dashSound)
        {
            audioPlayer.audioClip = dashSound;
            audioPlayer.PlayAudioClip();
        }

        StartCoroutine(DashRoutine(stats));
    }

    private IEnumerator DashRoutine(PlayerStats stats)
    {
        isDashing = true;

        float timer = 0f;
        float speed = dashDistance / dashDuration;
        int faceDir = origin.localScale.x >= 0 ? -1 : 1;

        // ✅ [1] 在 dash 開始時生成一次攻擊區域
        GameObject atk = Instantiate(attackTriggerPrefab, origin.position, Quaternion.identity);
        atk.transform.SetParent(origin); // 綁在玩家身上
        var atkScript = atk.GetComponent<AttackBackDash>();
        if (atkScript != null)
            atkScript.Init(origin, baseDamage, true);

        // ✅ [2] 進入 dash 移動過程
        while (timer < dashDuration)
        {
            origin.Translate(Vector3.right * faceDir * speed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // ✅ [3] dash 結束時銷毀攻擊區域
        Destroy(atk);

        isDashing = false;
        Destroy(gameObject); // 技能物件自己清除
    }


    public void SetPlayerAnimator(Animator animator) { }
}
