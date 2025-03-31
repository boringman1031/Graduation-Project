using System.Collections;
using UnityEngine;

public class SkillLoveSong : MonoBehaviour, ISkillEffect
{
    [Header("�ޯ�Ѽ�")]
    public float speedBoost = 4f;       // ���ɪ��t�׼ƭ�
    public float boostDuration = 2f;    // �[�t����ɶ��]��^
    public float projectileSpeed = 25f; // ����D��t��
    public float damage = 50f;          // �ˮ`�ƭ�
    public float energyCost = 10f;      // ���ӯ�q

    [Header("���įS�ĳ]�w")]
    public AudioClip spawnSound;        // �ޯ�ͦ�����
    public AudioClip hitSound;          // �R���ĤH����
    public GameObject hitEffect;          // �R���ĤH����

    private Transform origin;

    // �o�Ӥ�k�|�b���a�ޯ�ʵe�ƥ󤤩I�s
    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;
        transform.position = origin.position;

        // ����ͦ��ɭ���
        if (spawnSound != null)
        {
            AudioSource.PlayClipAtPoint(spawnSound, transform.position);
        }

        // �ھڪ��a���V�M�w�����V
        float direction = -origin.localScale.x;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(projectileSpeed * direction, 0f);
        }

        // �I�s���a�� ApplySpeedBoost ��k�A���t�״����޿�b���a���W����
        PlayerController player = origin.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ApplySpeedBoost(speedBoost, boostDuration);
        }

        // 3 ���۰ʾP���ޯ� prefab
        Destroy(gameObject, 1f);
    }
    public void SetPlayerAnimator(Animator animator)
    {
        // �p���ݭn�i�H�P�B�ʵe�Ѽ�
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��ޯ�I��ĤH�ɡA��ĤH�y���ˮ`�ü���R������
        CharactorBase enemy = collision.GetComponent<CharactorBase>();
        if (collision.CompareTag("Player"))
            return;

        if (enemy != null)
        {
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            enemy.TakeDamage(damage, transform);

            Destroy(gameObject);
        }
    }
}
