// ���b SkillAndClassPanel �W���D���
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillAndClassUI : MonoBehaviour
{
    [Header("�ޯ��")]
    public Transform skillListParent; // �ޯ���ܮe��
    public GameObject skillItemPrefab; // �ޯ� UI ���� prefab

    [Header("¾�~��")]
    public Transform classListParent;
    public GameObject classItemPrefab; // ¾�~ UI ���� prefab

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
        // �۰ʸ˳Ƨޯ��Ĥ@�ӪŦ�δ��� index 0
        for (int i = 0; i < SkillManager.Instance.equippedSkills.Length; i++)
        {
            if (SkillManager.Instance.equippedSkills[i] == null)
            {
                SkillManager.Instance.EquipSkill(skill, i);
                return;
            }
        }
        // �����A�۰ʴ�����0��
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