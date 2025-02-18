/*------------BY017------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("�ƥ�s��")]
    public VoidEventSO onAllEnemiesDefeated; // ��Ҧ��ĤH�Q���Ѯɪ��ƥ�
    [Header("�ƥ��ť")]
    public EnemyEventSO OnEnemyDied; // ��ĤH���`�ɪ��ƥ�

    private List<EnemyBase> enemies = new List<EnemyBase>(); // �x�s�Ҧ��ĤH

    private void Start()
    {
        // ����Ҧ����������ĤH�å[�J�C��
        enemies.AddRange(FindObjectsOfType<EnemyBase>());    
    }

    private void OnEnable()
    {
        // �q�\�ĤH���`�ƥ�
        OnEnemyDied.OnEventRaised += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        // �����q�\�ĤH���`�ƥ�
        OnEnemyDied.OnEventRaised -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(EnemyBase enemy)
    {
        // �q�C���������`���ĤH
        enemies.Remove(enemy);

        // �p�G�ĤH�ƶq��0�B�������O�D�����A�s���ƥ�
        if (enemies.Count == 0 && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Lobby")
        {
            Debug.Log("�Ҧ��ĤH�w�Q���ѡA�s���ƥ�q�� UIManager");
            onAllEnemiesDefeated.RaiseEvent();
        }
    }
}
