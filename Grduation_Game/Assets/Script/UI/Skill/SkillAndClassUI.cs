// 掛在 SkillAndClassPanel 上的主控制器
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;

public class SkillAndClassUI : MonoBehaviour
{
    [Header("技能區")]
    public Transform skillListParent; // 技能顯示容器
    public GameObject skillItemPrefab; // 技能 UI 項目 prefab

    [Header("職業區")]
    public Transform classListParent;
    public GameObject classItemPrefab; // 職業 UI 項目 prefab

    private int lastEquipIndex = 0; // 用來記錄下次裝到哪一格

    void OnEnable()
    {
        PopulateSkillUI();
        PopulateClassUI();
    }

    void PopulateSkillUI()
    {
        foreach (Transform child in skillListParent) Destroy(child.gameObject);

        // 先收集所有職業技能（R鍵用的 ultimateSkill）
        HashSet<SkillData> classSkills = new HashSet<SkillData>();
        foreach (var cls in SkillManager.Instance.allClasses)
        {
            if (cls.ultimateSkill != null)
                classSkills.Add(cls.ultimateSkill);
        }

        foreach (var skill in SkillManager.Instance.allSkills)
        {
            // ✅ 過濾掉職業專屬技能，並檢查是否解鎖
            if (!skill.isUnlocked || classSkills.Contains(skill)) continue;

            var go = Instantiate(skillItemPrefab, skillListParent);
            var item = go.GetComponent<SkillUIItem>();

            bool isEquipped = false;
            foreach (var equipped in SkillManager.Instance.equippedSkills)
            {
                if (equipped == skill)
                {
                    isEquipped = true;
                    break;
                }
            }

            item.Setup(skill, OnSkillSelected, isEquipped);
        }
    }


    void PopulateClassUI()
    {
        foreach (Transform child in classListParent) Destroy(child.gameObject);

        foreach (var cls in SkillManager.Instance.allClasses)
        {
            if (!cls.isUnlocked) continue; // ✅ 加上這行只顯示已解鎖的職業

            var go = Instantiate(classItemPrefab, classListParent);
            bool isSelected = SkillManager.Instance.selectedClass == cls;
            go.GetComponent<ClassUIItem>().Setup(cls, OnClassSelected, isSelected);
        }
    }

    void OnSkillSelected(SkillData skill)
    {
        // 如果已經裝備了，就取消裝備
        for (int i = 0; i < SkillManager.Instance.equippedSkills.Length; i++)
        {
            if (SkillManager.Instance.equippedSkills[i] == skill)
            {
                SkillManager.Instance.EquipSkill(null, i);
                PopulateSkillUI();
                FindObjectOfType<SkillUIController>()?.RefreshSkillIcons();
                return;
            }
        }

        // 沒裝過就裝到下一個位置
        SkillManager.Instance.EquipSkill(skill, lastEquipIndex);
        lastEquipIndex = (lastEquipIndex + 1) % 3;

        PopulateSkillUI();
        FindObjectOfType<SkillUIController>()?.RefreshSkillIcons();
    }

    void OnClassSelected(ClassData cls)
    {
        SkillManager.Instance.selectedClass = cls;
        FindObjectOfType<PlayerCostumeChanger>()?.ChangeCostume(cls.className);
        FindObjectOfType<PlayerController>()?.UpdateUltimateSkill();
        SkillManager.Instance.OnClassChanged?.Invoke();
        PopulateClassUI();
        FindObjectOfType<SkillUIController>()?.RefreshSkillIcons();
    }
}
