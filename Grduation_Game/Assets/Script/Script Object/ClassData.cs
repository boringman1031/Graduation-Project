using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewClass", menuName = "Classes/ClassData")]
public class ClassData : ScriptableObject
{
    public string className;
    public SkillData ultimateSkill; // 绑定的大招
    public Sprite classIcon;
}
