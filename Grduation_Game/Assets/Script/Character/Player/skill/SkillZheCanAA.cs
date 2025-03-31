using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillZheCanAA : MonoBehaviour, ISkillEffect
{
    // ���a�ӷ�
    private Transform origin;
    private Animator playerAnimator;

    // �������������w�m���]�Ҧp�]�t�����P�w���w�m���^
    public GameObject foodProjectilePrefab;
    // �w�m�����ʳt�ס]�i�̻ݨD�վ�^
    public float projectileSpeed = 10f;
    // ���������y�����ˮ`
    public float damage = 50f;
    // �ޯ���ӯ�q
    public float energyCost = 20f;

    // ISkillEffect ��@�G�]�w�ޯ�o�X��
    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        // ���Ӫ��a��q�G�o�䰲�]���a�ϥ� CharactorBase �޲z��q
        CharactorBase playerChar = origin.GetComponent<CharactorBase>();
        if (playerChar != null)
        {
            playerChar.CurrentPower -= energyCost;
            if (playerChar.CurrentPower < 0)
                playerChar.CurrentPower = 0;
            // �Y����q��s�ƥ�A�i�b���I�s
        }
    }

    // ISkillEffect ��@�G�]�w���a�ʵe�]�p���ݭn�^
    public void SetPlayerAnimator(Animator animator)
    {
        playerAnimator = animator;
    }

    private void Start()
    {
        // �T�O origin �w�]�w�A�_�h���~���ܨþP��
        if (origin == null)
        {
            Debug.LogError("SkillZheCanAA: origin is not set!");
            Destroy(gameObject);
            return;
        }

        // ���ޯ�w�m�����H���a�G�����]�����a���l����
        transform.position = origin.position;
        transform.SetParent(origin);

        // �V���ͦ���������
        SpawnProjectile(Vector2.left);
        // �V�k�ͦ���������
        SpawnProjectile(Vector2.right);

        // �ͦ���i�P�����ޯ�w�m���]����@�� frame �H�T�O�ͦ������^
        Destroy(gameObject, 0.1f);
    }

    // �ھڤ�V�ͦ����������w�m��
    private void SpawnProjectile(Vector2 direction)
    {
        if (foodProjectilePrefab != null)
        {
            // �ͦ���m�H���a��m����ǡA�]�i�[�J�����q
            Vector3 spawnPosition = origin.position;
            GameObject projectile = Instantiate(foodProjectilePrefab, spawnPosition, Quaternion.identity);

            // ���ը��o�w�m���W���۩w�q�����}���]�Ҧp FoodProjectile�^
            FoodProjectile fp = projectile.GetComponent<FoodProjectile>();
            if (fp != null)
            {
                fp.SetDamage(damage);
                fp.SetDirection(direction, projectileSpeed);
            }
            else
            {
                // �p�G�S�� FoodProjectile �}���A�h���ե� Rigidbody2D �]�w�t��
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = direction * projectileSpeed;
                }
            }
        }
        else
        {
            Debug.LogWarning("SkillZheCanAA: foodProjectilePrefab is not assigned!");
        }
    }
}
