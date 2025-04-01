using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIItem : MonoBehaviour
{
    public Image icon;
    public Text nameText;
    public GameObject equippedMark; // ✅ 裝備標記（可設定勾勾圖示）

    private SkillData data;
    private System.Action<SkillData> onClick;

    public void Setup(SkillData skill, System.Action<SkillData> callback, bool isEquipped)
    {
        data = skill;
        onClick = callback;
        icon.sprite = skill.icon;
        nameText.text = skill.skillName;
        if (equippedMark != null) equippedMark.SetActive(isEquipped);
    }

    public void OnClick() => onClick?.Invoke(data);
}

