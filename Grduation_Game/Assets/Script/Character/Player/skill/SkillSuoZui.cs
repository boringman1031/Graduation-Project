using UnityEngine;

public class SkillSuoZui : MonoBehaviour, ISkillEffect
{
    [Header("�ޯ�]�w")]
    public GameObject vomitProjectilePrefab;  // �æR�����w�m��
    public AudioClip activationSound;           // �ޯ�Ұʮɪ�����

    // �o��O���ޯ�O�_�����H�۪��a���ʡA�J�K�o�ۤ��ݭn���H���a�A�ҥH���]���l����
    private Transform origin;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
        // �N���ޯફ���b���a��m�]�����]�w������^
        transform.position = origin.position;
    }

    public void SetPlayerAnimator(Animator animator)
    {
        // �p�G�ݭn�]�w�ʵe�Ѽƥi�H�B�z
    }

    private void Start()
    {
        ActivateSkill();
    }

    public void ActivateSkill()
    {
        if (origin == null)
        {
            Debug.LogWarning("SkillSuoZui: Origin not set");
            return;
        }

        // ���o���a���}���]���]�ϥ� CharactorBase�^
        CharactorBase player = origin.GetComponent<CharactorBase>();
        if (player != null)
        {
            // �������a10%���ͩR��
            float hpDeduct = player.MaxHealth * 0.1f;
            player.CurrentHealth -= hpDeduct;
            if (player.CurrentHealth < 0)
                player.CurrentHealth = 0;
            player.OnHealthChange?.Invoke(player);

            // ���ӯ�q 20
            player.CurrentPower -= 20;
        }

        // ����ޯ�Ұʭ��ġ]�b���a��m����^
        if (activationSound != null)
        {
            AudioSource.PlayClipAtPoint(activationSound, origin.position);
        }

        // �ھڪ��a�¦V�M�w�w�m�������ʤ�V�]���] localScale.x �j��0��ܴ¥k�^
        float facing = -Mathf.Sign(origin.localScale.x);
        // �]�w���ਤ�סG�¥k��0�סA�¥���180��
        Quaternion projectileRotation = Quaternion.Euler(0, 0, (facing < 0 ? 180f : 0f));

        // �ͦ��æR�w�m���A��m�P���a�@�P�A����ھڴ¦V�M�w
        GameObject projectile = Instantiate(vomitProjectilePrefab, origin.position, projectileRotation);
        // �`�N�G���n�N�ͦ����]�w�����a�l����A����W�߲���
        projectile.transform.parent = null;

        // �p�G�w�m���}���� direction �ܼơA�N facing ��ȶi�h
        SuoZuiProjectile szProj = projectile.GetComponent<SuoZuiProjectile>();
        if (szProj != null)
        {
            szProj.direction = facing;
        }

        // �̫�P���o�ӧޯફ��]�p�G���O�@���ʨϥΪ��^
        Destroy(gameObject);
    }
}
