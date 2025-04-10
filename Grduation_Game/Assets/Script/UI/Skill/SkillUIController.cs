using UnityEngine;
using UnityEngine.UI;

public class SkillUIController : MonoBehaviour
{

    [System.Serializable]
    public class SkillSlot
    {
        public KeyCode key;
        public Image skillIcon;
        public SkillCooldownUI cooldownUI;
    }

    public SkillSlot[] skillSlots = new SkillSlot[4]; // QWER ������ slot

    private SkillData GetSkillByIndex(int index)
    {
        if (index < 3)
            return SkillManager.Instance.equippedSkills[index];
        else if (index == 3)
            return SkillManager.Instance.selectedClass?.ultimateSkill;
        else
            return null;
    }

    void Start()
    {
        RefreshSkillIcons(); // �Ұʮɪ�l�ƹϥ�
    }

    void Update()
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            var slot = skillSlots[i];
            var skill = GetSkillByIndex(i);
            if (skill == null) continue;

            if (Input.GetKeyDown(slot.key))
            {
                if (!slot.cooldownUI.IsCooling())
                {
                    slot.cooldownUI.StartCooldown(skill.cooldownTime);
                }
            }
        }
    }

    public void RefreshSkillIcons()
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            var skill = GetSkillByIndex(i);
            if (skill != null && skill.isUnlocked)
            {
                skillSlots[i].skillIcon.sprite = skill.icon;
            }
            else
            {
                skillSlots[i].skillIcon.sprite = null; // �γ]�w�w�]�ϥ�
            }

            skillSlots[i].cooldownUI?.ResetCooldown(); // ���m�N�o�B�n
        }
    }
}