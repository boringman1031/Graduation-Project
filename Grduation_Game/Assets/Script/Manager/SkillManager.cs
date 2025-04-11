using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public System.Action OnClassChanged; // 用來通知 UI 職業已切換

    [Header("當前配置")]
    public ClassData selectedClass; // 當前裝備的職業
    public SkillData[] equippedSkills = new SkillData[3]; // Q,W,E 三個技能槽

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

    /// <summary>
    /// 解鎖技能（小招）並自動裝備到空位，並更新 UI 與冷卻狀態
    /// </summary>
    public void UnlockSkill(string skillName)
    {
        SkillData skill = allSkills.Find(s => s.skillName == skillName);

        if (skill != null)
        {
            skill.isUnlocked = true;

            // 檢查是否已經裝備過這個技能
            bool alreadyEquipped = false;
            for (int i = 0; i < equippedSkills.Length; i++)
            {
                if (equippedSkills[i] == skill)
                {
                    alreadyEquipped = true;
                    break;
                }
            }

            if (!alreadyEquipped)
            {
                // 自動裝到第一個空的技能槽（或未解鎖的格子）
                for (int i = 0; i < equippedSkills.Length; i++)
                {
                    if (equippedSkills[i] == null || !equippedSkills[i].isUnlocked)
                    {
                        EquipSkill(skill, i);
                        break;
                    }
                }
            }

            // 更新 UI 與冷卻
            FindObjectOfType<SkillUIController>()?.RefreshSkillIcons();
            var player = FindObjectOfType<PlayerController>();
            player?.UpdateUltimateSkill();
            player?.ResetSkillCooldowns();
        }
        else
        {
            Debug.LogWarning($"找不到技能：{skillName}，請確認 allSkills 中是否有加入");
        }
    }


    /// <summary>
    /// 解鎖職業，並切換當前職業、解鎖大招，更新外觀與 UI
    /// </summary>
    public void UnlockClassAndEquip(string className)
    {
        ClassData cls = allClasses.Find(c => c.className == className);
        if (cls != null)
        {
            cls.isUnlocked = true;
            selectedClass = cls;

            if (cls.ultimateSkill != null)
            {
                // 為保證引用一致，使用 UnlockSkill 方法解鎖
                UnlockSkill(cls.ultimateSkill.skillName);
            }

            // 外觀與 UI 更新
            FindObjectOfType<PlayerCostumeChanger>()?.ChangeCostume(cls.className);
            var player = FindObjectOfType<PlayerController>();
            player?.UpdateUltimateSkill();
            player?.ResetSkillCooldowns();
            FindObjectOfType<SkillUIController>()?.RefreshSkillIcons();
            OnClassChanged?.Invoke();
        }
        else
        {
            Debug.LogWarning($"找不到職業：{className}，請確認 allClasses 中是否有加入");
        }
    }

    /// <summary>
    /// 將技能裝到指定槽位（0~2 為 QWE）
    /// </summary>
    public void EquipSkill(SkillData skill, int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < 3)
        {
            equippedSkills[slotIndex] = skill;
        }
    }

    /// <summary>
    /// 重製技能與職業（通常用在新遊戲）
    /// </summary>
    public void resetSkillAndClass()
    {
        foreach (SkillData skillData in allSkills)
        {
            skillData.isUnlocked = false;
        }

        foreach (ClassData classData in allClasses)
        {
            classData.isUnlocked = false;
            if (classData.name == "normal")
                classData.isUnlocked = true;
        }
    }
}