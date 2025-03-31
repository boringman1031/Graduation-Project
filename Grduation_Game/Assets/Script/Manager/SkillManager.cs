using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public System.Action OnClassChanged; // 新增事件

    [Header("當前配置")]
    public ClassData selectedClass; // 當前的職業
    public SkillData[] equippedSkills = new SkillData[3]; // Q,W,E 槽位

    [Header("所有可用數據")]
    public List<SkillData> allSkills = new List<SkillData>();
    public List<ClassData> allClasses = new List<ClassData>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // 解鎖技能（戰鬥場景調用）
    public void UnlockSkill(string skillName)
    {
        SkillData skill = allSkills.Find(s => s.skillName == skillName);
        if (skill != null) skill.isUnlocked = true;
    }

    // 配置技能槽（藏身處調用）
    public void EquipSkill(SkillData skill, int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < 3)
            equippedSkills[slotIndex] = skill;
    }

}
