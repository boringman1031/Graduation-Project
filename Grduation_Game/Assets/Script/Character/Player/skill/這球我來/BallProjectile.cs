using UnityEngine;

public class BallProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float maxLifetime = 2f;
    public float rotateSpeed = 360f; // 每秒旋轉 360 度
    public GameObject hitEffect;
    public AudioClip hitSound;
    public AudioDefination audioPlayer;

    private float damage;
    private Vector2 velocity;

    public void Launch(int faceDir, float _damage)
    {
        damage = _damage;

        // 修正：根據角色面向，往斜下方向飛（左下 / 右下）
        Vector2 dir = new Vector2(-faceDir, -1).normalized;
        velocity = dir * speed;

        Destroy(gameObject, maxLifetime); // 保險：2 秒後自動銷毀
    }

    private void Update()
    {
        transform.position += (Vector3)(velocity * Time.deltaTime);
        transform.Rotate(0f, 0f, -rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            if (hitEffect)
                Instantiate(hitEffect, transform.position, Quaternion.identity);

            if (audioPlayer && hitSound)
            {
                audioPlayer.audioClip = hitSound;
                audioPlayer.PlayAudioClip();
            }

            CharactorBase target = other.GetComponent<CharactorBase>();
            if (target != null)
                target.TakeDamage(damage, transform);

            Destroy(gameObject); // ✅ 碰到地板或敵人立即自爆
        }
    }
}
