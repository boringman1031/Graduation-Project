using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewClass", menuName = "Classes/ClassData")]
public class ClassData : ScriptableObject
{
    public string className;
    public SkillData ultimateSkill; // 绑定的大招
    public Sprite classIcon;

    [TextArea]
    public string description; // 加入說明文字

    public bool isUnlocked = false; // 是否解鎖（預設為 true，可依遊戲流程調整）
}
