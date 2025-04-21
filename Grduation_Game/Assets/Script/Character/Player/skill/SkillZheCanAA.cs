using System.Collections;
using UnityEngine;

public class SkillZheCanAA : MonoBehaviour, ISkillEffect
{
    private Transform origin;                  // �ޯ�I��ӷ��]���a�^
    private Animator playerAnimator;

    [Header("�ޯ�]�w")]
    public GameObject foodProjectilePrefab;    // ���������w�m��
    public float projectileSpeed = 10f;        // ��������t��
    public float baseDamage = 50f;             // �ޯ��¦�ˮ`
    public float energyCost = 20f;             // ��q����

    public CharacterEventSO powerChangeEvent;  // ����q��Ĳ�o UI ��s

    private float finalDamage = 0f;

    public void SetOrigin(Transform originT)
    {
        origin = originT;
        transform.position = origin.position;

        CharactorBase playerChar = origin.GetComponent<CharactorBase>();
        PlayerStats playerStats = origin.GetComponent<PlayerStats>();

        if (playerChar == null || playerStats == null)
        {
            Debug.LogWarning("�ʤ֥��n�ե� CharactorBase �� PlayerStats�A�ޯ����");
            Destroy(gameObject);
            return;
        }

        // ��q�����h�����ޯ�
        if (playerChar.CurrentPower < energyCost)
        {
            Debug.Log("��q�����A�L�k�I��ޯ�");
            Destroy(gameObject);
            return;
        }

        // ����q�üs����s
        playerChar.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(playerChar);

        // �ˮ` = �ޯ��¦�ˮ` + ���a�����O
        finalDamage = baseDamage + playerStats.attack;
    }

    public void SetPlayerAnimator(Animator animator)
    {
        playerAnimator = animator;
    }

    private void Start()
    {
        if (origin == null || foodProjectilePrefab == null)
        {
            Debug.LogError("SkillZheCanAA: ���n�ݩʥ��]�w");
            Destroy(gameObject);
            return;
        }

        // �����k�ͦ���������
        SpawnProjectile(Vector2.left);
        SpawnProjectile(Vector2.right);

        // �ĪG�����Y�P���]�y�L����T�O�l����إߡ^
        Destroy(gameObject, 0.1f);
    }

    private void SpawnProjectile(Vector2 direction)
    {
        GameObject projectile = Instantiate(foodProjectilePrefab, origin.position, Quaternion.identity);

        // ���ճ]�w�����}��
        FoodProjectile fp = projectile.GetComponent<FoodProjectile>();
        if (fp != null)
        {
            fp.SetDamage(finalDamage);
            fp.SetDirection(direction, projectileSpeed);
        }
        else
        {
            // �Y�S�����}���N���ե� Rigidbody2D ����
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }
        }
    }
}
