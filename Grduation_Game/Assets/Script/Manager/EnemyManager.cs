/*------------BY017------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("�ƥ�s��")]
    public VoidEventSO onAllEnemiesDefeated; // ��Ҧ��ĤH�Q���Ѯɪ��ƥ�

    private List<GameObject> enemies = new List<GameObject>(); // �x�s�Ҧ��ĤH

    private void OnEnable()
    {
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy")); // ���Ҧ��ĤH
        Debug.Log($"��e�����ĤH�ƶq: {enemies.Count}");
    }
    public void RegisterEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void HandleEnemyDeath(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            Debug.Log($"�ĤH���`�A�Ѿl�ĤH�ƶq: {enemies.Count}");
        }

        if (enemies.Count == 0)
        {
            Debug.Log("�Ҧ��ĤH�w�Q���ѡA�s���ƥ�I");
            onAllEnemiesDefeated.RaiseEvent();
        }
    }
}
