using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public System.Action OnClassChanged; // �s�W�ƥ�

    [Header("��e�t�m")]
    public ClassData selectedClass; // ��e��¾�~
    public SkillData[] equippedSkills = new SkillData[3]; // Q,W,E �Ѧ�

    [Header("�Ҧ��i�μƾ�")]
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

    // ����ޯ�]�԰������եΡ^
    public void UnlockSkill(string skillName)
    {
        SkillData skill = allSkills.Find(s => s.skillName == skillName);
        if (skill != null) skill.isUnlocked = true;
    }

    // �t�m�ޯ�ѡ]�è��B�եΡ^
    public void EquipSkill(SkillData skill, int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < 3)
            equippedSkills[slotIndex] = skill;
    }

}
