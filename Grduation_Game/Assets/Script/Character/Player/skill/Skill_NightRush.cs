using System.Collections;
using UnityEngine;

public class Skill_NightRush : MonoBehaviour, ISkillEffect
{
    [Header("技能參數")]
    public float dashSpeed = 8f;
    public float duration = 1f;
    public float damage = 10f;
    public float energyCost = 10f;

    [Header("音效設定")]
    public AudioDefination audioPlayer;
    public AudioClip spawnSound;
    public AudioClip hitSound;

    [Header("掛載位置設定")]
    public Transform playerMountPoint;

    public CharacterEventSO powerChangeEvent;

    private Transform origin;
    private Rigidbody2D rb;
    private Rigidbody2D playerRb;
    private FixedJoint2D joint;
    private float dashDirection;
    private bool hasHit = false;

    void costPower(CharactorBase character)
    {
        character.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(character);
    }

    public void SetPlayerAnimator(Animator animator) { }

    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;

        CharactorBase character = origin.GetComponent<CharactorBase>();
        if (character == null || character.CurrentPower < energyCost)
        {
            Debug.Log("能量不足或角色缺失");
            Destroy(gameObject);
            return;
        }

        costPower(character);

        if (audioPlayer != null && spawnSound != null)
        {
            audioPlayer.audioClip = spawnSound;
            audioPlayer.PlayAudioClip();
        }

        transform.position = origin.position;

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        playerRb = origin.GetComponent<Rigidbody2D>();

        // ✅ 設定移動方向 & 翻轉圖像（根據你前面定義的需求）
        if (origin.localScale.x < 0) // 面左
        {
            dashDirection = 1f;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else // 面右
        {
            dashDirection = -1f;
            transform.localScale = new Vector3(1, 1, 1);
        }

        // ✅ 加上 Joint，讓 player 跟著走（不再直接控制 position）
        joint = gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = playerRb;
        joint.enableCollision = false;

        StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            rb.MovePosition(rb.position + new Vector2(dashSpeed * dashDirection * Time.fixedDeltaTime, 0));

            if (origin != null && playerMountPoint != null)
            {
                origin.position = playerMountPoint.position;
            }

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        if (playerRb != null)
            playerRb.isKinematic = false;

        if (joint != null)
            Destroy(joint);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;

        CharactorBase target = collision.GetComponent<CharactorBase>();
        if (target != null && !target.CompareTag("Player"))
        {
            hasHit = true;
            if (audioPlayer != null && hitSound != null)
            {
                audioPlayer.audioClip = hitSound;
                audioPlayer.PlayAudioClip();
            }

            target.TakeDamage(damage, transform);
        }
    }
}
