using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum SkillType
{
    Skill1,
    Skill2,
    Skill3,
    Skill4,
    Skill5
}

public class PlayerSkill : MonoBehaviour
{
    //------�Ф䴩��Ĺ--------
    public GameObject minionPrefab; // ���w�p��Prefab
    public Transform summonPoint; // �l���m
    public float cooldown = 3; // �N�o�ɶ�
    float lastTime; // �Ψӭp��N�o�ɶ���
    public Animator animator;

    public void SkillEnd()
    {
        print("skillEnd!!!!");
        animator.SetInteger("skill_Index", 0);
    }
    public void WinOrLoseSkill()
    {
        if (Time.time - lastTime > cooldown)
        {
            animator.SetInteger("skill_Index", 5);
            summonPoint = this.GetComponent<Transform>();
            Instantiate(minionPrefab, summonPoint.position, Quaternion.identity); // �l��X�p��
            lastTime = Time.time;
        }
    }
    public void KillTheBeat()
    {
        animator.SetInteger("skill_Index", 1);
    }
    public void YoBattle()
    {
        animator.SetInteger("skill_Index", 2);
    }
    public void �o�y�ڨ�()
    {
        animator.SetInteger("skill_Index", 3);
    }
    public void ���SĹ()
    {
        animator.SetInteger("skill_Index", 4);
    }
}
