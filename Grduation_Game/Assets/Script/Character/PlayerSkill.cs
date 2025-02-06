using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkill : MonoBehaviour
{
    //------�Ф䴩��Ĺ--------
    public GameObject minionPrefab; // ���w�p��Prefab
    public Transform summonPoint; // �l���m
    public float cooldown = 3; // �N�o�ɶ�
    float lastTime; // �Ψӭp��N�o�ɶ���
    public void WinOrLoseSkill()
    {
        if (Time.time - lastTime < cooldown)
        {
            summonPoint = this.GetComponent<Transform>();
            Instantiate(minionPrefab, summonPoint.position, Quaternion.identity); // �l��X�p��
            lastTime = Time.time;
        }
    }
}
