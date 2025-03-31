using System.Collections.Generic;
using UnityEngine;

public class NegativeEnergyEffect : MonoBehaviour
{
    public float damagePerSecond = 10f;  // �C��ˮ`�]�����@���ɴN���o�Ӽƭȡ^
    public float lifeTime = 3f;          // �w�m���s�b�ɶ�

    public GameObject hitEffectPrefab;   // �����S�Ĺw�m��
    public AudioClip hitEffectSound;     // ��������

    // �ΨӰO���C�ӸI���쪺�ĤH�W�@�����˪��ɶ�
    private Dictionary<Collider2D, float> damageTimers = new Dictionary<Collider2D, float>();

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // �ư����a�]���]���a�� Tag �� "Player"�^
        if (collision.CompareTag("Player"))
            return;

        // �ˬd�I����H�O�_���ĤH�ե�]�Ҧp CharactorBase�^
        CharactorBase enemy = collision.GetComponent<CharactorBase>();
        if (enemy != null)
        {
            // �p�G�S�������A��l�Ʈɶ�
            if (!damageTimers.ContainsKey(collision))
            {
                damageTimers[collision] = Time.time;
            }

            // �Y�W�L 1 ��A���@���ˮ`
            if (Time.time - damageTimers[collision] >= 1f)
            {
                Debug.Log("����ˮ` " + damagePerSecond);
                Attack tempAttack = new Attack();
                tempAttack.Damage = damagePerSecond;
                enemy.TakeDamage(tempAttack);
                

                // �ͦ������S��
                if (hitEffectPrefab != null)
                {
                    Instantiate(hitEffectPrefab, enemy.transform.position, Quaternion.identity);
                }
                // �ͦ���������
                if (hitEffectSound != null)
                {
                    AudioSource.PlayClipAtPoint(hitEffectSound, transform.position);
                }

                // ��s���ˮɶ�
                damageTimers[collision] = Time.time;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ��ĤH���}��A�M������
        if (damageTimers.ContainsKey(collision))
        {
            damageTimers.Remove(collision);
        }
    }
}
