// 掛在 SkillAndClassPanel 上的主控制器
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            go.GetComponent<SkillUIItem>().Setup(skill, OnSkillSelected);
        }
    }

    void PopulateClassUI()
    {
        foreach (Transform child in classListParent) Destroy(child.gameObject);

        foreach (var cls in SkillManager.Instance.allClasses)
        {
            var go = Instantiate(classItemPrefab, classListParent);
            go.GetComponent<ClassUIItem>().Setup(cls, OnClassSelected);
        }
    }

    void OnSkillSelected(SkillData skill)
    {
        // 自動裝備技能到第一個空位或替換 index 0
        for (int i = 0; i < SkillManager.Instance.equippedSkills.Length; i++)
        {
            if (SkillManager.Instance.equippedSkills[i] == null)
            {
                SkillManager.Instance.EquipSkill(skill, i);
                return;
            }
        }
        // 全滿，自動替換第0位
        SkillManager.Instance.EquipSkill(skill, 0);
    }

    void OnClassSelected(ClassData cls)
    {
        SkillManager.Instance.selectedClass = cls;
        FindObjectOfType<PlayerCostumeChanger>()?.ChangeCostume(cls.className);
        FindObjectOfType<PlayerController>()?.UpdateUltimateSkill();
        SkillManager.Instance.OnClassChanged?.Invoke();
    }
}