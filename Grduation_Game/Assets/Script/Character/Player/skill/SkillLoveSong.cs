using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class SkillLoveSong : MonoBehaviour, ISkillEffect
{
    [Header("�ޯ�Ѽ�")]
    public float speedBoost = 4f;       // ���ɪ��t�׼ƭ�
    public float boostDuration = 2f;    // �[�t����ɶ��]��^
    public float projectileSpeed = 25f; // ����D��t��
    public float damage = 50f;          // �ˮ`�ƭ�
    public float energyCost = 10f;      // ���ӯ�q

    [Header("���įS�ĳ]�w")]
    public AudioDefination audioPlayer; // ���ļ���
    public AudioClip spawnSound;        // �ޯ�ͦ�����
    public AudioClip hitSound;          // �R���ĤH����
    public GameObject hitEffect;          // �R���ĤH����

    private Transform origin;

    public CharacterEventSO powerChangeEvent;

    void costPower(CharactorBase _Charater) //������q
    {
        _Charater.AddPower(-energyCost);
        powerChangeEvent.OnEventRaised(_Charater);
    }
    // �o�Ӥ�k�|�b���a�ޯ�ʵe�ƥ󤤩I�s
    public void SetOrigin(Transform originTransform)
    {
        origin = originTransform;
        transform.position = origin.position;

        // ���o���a�ͩR�P��q�޲z�ե� (�Ҧp CharactorBase)
        CharactorBase character = origin.GetComponent<CharactorBase>();
        if (character == null)
        {
            Debug.LogWarning("����� CharactorBase �ե�A�L�k�I��ޯ�");
            Destroy(gameObject);
            return;
        }

        // �ˬd��q�O�_����
        if (character.CurrentPower < energyCost)
        {
            Debug.Log("��q�����A�L�k�I��ޯ�");
            Destroy(gameObject);
            return;
        }
        costPower(character); //������q

        // ����ͦ��ɭ���
        if (audioPlayer != null && spawnSound != null)
        {
            audioPlayer.audioClip = spawnSound;
            audioPlayer.PlayAudioClip();
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
            if (audioPlayer != null && hitSound != null)
            {
                audioPlayer.audioClip = hitSound;
                audioPlayer.PlayAudioClip();
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
