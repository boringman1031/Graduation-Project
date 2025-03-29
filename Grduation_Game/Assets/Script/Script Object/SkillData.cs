using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public SkillType type;
    public GameObject skillPrefab; // 技能prefab（含邏輯）
    public bool isUnlocked; // 是否已解鎖
    [TextArea] public string description;

    // 新增：技能冷卻時間，單位秒
    public float cooldownTime;
}
