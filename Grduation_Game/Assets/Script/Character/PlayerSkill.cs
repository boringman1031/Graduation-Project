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
    //------請支援輸贏--------
    public GameObject minionPrefab; // 指定小弟Prefab
    public Transform summonPoint; // 召喚位置
    public float cooldown = 3; // 冷卻時間
    float lastTime; // 用來計算冷卻時間用
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
            Instantiate(minionPrefab, summonPoint.position, Quaternion.identity); // 召喚出小弟
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
    public void 這球我來()
    {
        animator.SetInteger("skill_Index", 3);
    }
    public void 長又贏()
    {
        animator.SetInteger("skill_Index", 4);
    }
}
