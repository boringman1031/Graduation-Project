using System.Collections;
using UnityEngine;

public class Skill_NightRush : MonoBehaviour, ISkillEffect
{
    [Header("�ޯ�Ѽ�")]
    public float dashSpeed = 2f;         // �Ĩ�t��
    public float duration = 2f;          // �Ĩ����ɶ�
    public float damage = 10f;           // �u�~��ĤH���ˮ`
    public float energyCost = 10f;       // ���ӯ�q

    [Header("���ĳ]�w")]
    public AudioClip spawnSound;         // �ͦ�����
    public AudioClip hitSound;           // ��������

    private Transform origin;            // Ĳ�o�ޯ઺���a Transform
    private Rigidbody2D rb;              // MountPoint ������
    private Rigidbody2D playerRb;        // ���a�ۤv�� Rigidbody2D
    private float dashDirection;         // �ΨӰO�����a��l���V�G���ƦV�k�A�t�ƦV��

    public void SetPlayerAnimator(Animator animator)
    {
        // �i�̻ݨD��@
    }

    // ����k�|�Ѫ��a�ޯ�ʵe�ƥ�I�s
    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;

        // ����ͦ�����
        if (spawnSound != null)
        {
            AudioSource.PlayClipAtPoint(spawnSound, origin.position);
        }

        // �]�w MountPoint ��m�P����
        transform.position = origin.position;
        transform.rotation = origin.rotation;

        // ���o�Υ[�J Rigidbody2D�A�T�O MountPoint ���ʺA
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.isKinematic = false;

        // �T�Ϊ��a���ʱ���Ϊ��a Rigidbody2D ���z�B��
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

        // ���o��l���a���V (���� reparent �v�T)
        dashDirection = (originTransform.localScale.x > 0 ? -1f : 1f);
        Debug.Log("dashDirection: " + dashDirection);

        // �ھ� dashDirection ½�� MountPoint
        transform.localScale = new Vector3(-dashDirection, 1, 1);

        // �N���a reparent �� MountPoint �U�A�O�d�@�ɦ쫺
        origin.SetParent(transform, true);

        // �ҰʽĨ��{�A�ϥ� dashDirection �ӳ]�w�t��
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

        // �Ѱ����a�P MountPoint �����l���Y
        origin.SetParent(null);

        // ��_���a�� Rigidbody2D ���z�B��P���ʱ���
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
