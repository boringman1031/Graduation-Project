using UnityEngine;

public class Skill_ThisBallIsMine : MonoBehaviour, ISkillEffect
{
    [Header("�ޯ�Ѽ�")]
    public GameObject ballPrefab;
    public float energyCost = 50f;
    public float damage = 100f;
    public Vector3 spawnOffset = new Vector3(0.5f, 5f, 0); // �b����e�W��

    private Transform origin;

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;

        CharactorBase character = origin.GetComponent<CharactorBase>();
        if (character == null)
        {
            Debug.LogWarning("Skill_ThisBallIsMine: �䤣�� CharactorBase");
            Destroy(gameObject);
            return;
        }

        if (character.CurrentPower < energyCost)
        {
            Debug.Log("��q�����A�L�k�I��ޯ�");
            Destroy(gameObject);
            return;
        }

        // ����q
        character.AddPower(-energyCost);

        // �ߧY�ͦ��Ʋy
        Vector3 pos = origin.position + spawnOffset;
        int faceDir = origin.localScale.x >= 0 ? 1 : -1;

        GameObject ball = Instantiate(ballPrefab, pos, Quaternion.identity);
        BallProjectile proj = ball.GetComponent<BallProjectile>();
        if (proj != null)
        {
            float finalDamage = damage;
            var stats = origin.GetComponent<PlayerStats>();
            if (stats != null)
                finalDamage += stats.attack;

            proj.Launch(faceDir, finalDamage);
        }

        Destroy(gameObject);
    }


    private void SpawnBall()
    {
        Vector3 pos = origin.position + spawnOffset;
        int faceDir = origin.localScale.x >= 0 ? 1 : -1;

        GameObject ball = Instantiate(ballPrefab, pos, Quaternion.identity);
        BallProjectile proj = ball.GetComponent<BallProjectile>();
        if (proj != null)
        {
            proj.Launch(faceDir, damage);
        }
    }

    public void SetPlayerAnimator(Animator animator) { }
}
