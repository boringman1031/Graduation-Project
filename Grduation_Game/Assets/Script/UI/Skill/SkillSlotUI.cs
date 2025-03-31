using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    public int slotIndex; // 0=Q, 1=W, 2=E
    public Image iconImage;

    void Update()
    {
        SkillData skill = SkillManager.Instance.equippedSkills[slotIndex];
        iconImage.sprite = skill?.icon ?? null; // 如果无技能则清空图标
    }
}
