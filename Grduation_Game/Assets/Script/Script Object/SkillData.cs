using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName; // 技能名稱
    public Sprite icon; // UI的icon
    public SkillType type; // Q W E R
    public GameObject skillPrefab; // 技能prefab（含邏輯）
    public bool isUnlocked; // 是否已解鎖
    [TextArea] public string description; // 技能的說明

    public float cooldownTime; // 新增：技能冷卻時間，單位秒
    public bool isFollowPlayer; // 是否跟隨玩家
}
