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

    void OnEnable()
    {
        PopulateSkillUI();
        PopulateClassUI();
    }

    void PopulateSkillUI()
    {
        foreach (Transform child in skillListParent) Destroy(child.gameObject);

        foreach (var skill in SkillManager.Instance.allSkills)
        {
            if (!skill.isUnlocked) continue;
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
            var go = Instantiate(classItemPrefab, classListParent);
            bool isSelected = SkillManager.Instance.selectedClass == cls;
            go.GetComponent<ClassUIItem>().Setup(cls, OnClassSelected, isSelected);
        }
    }
    private int lastEquipIndex = 0;
    void OnSkillSelected(SkillData skill)
    {
        // 輪替裝備技能
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
        FindObjectOfType<SkillUIController>()?.RefreshSkillIcons(); // ✅ 更新圖示與冷卻
    }
}

// 其餘子類 SkillUIItem / ClassUIItem / PlayerCostumeChanger 不變
