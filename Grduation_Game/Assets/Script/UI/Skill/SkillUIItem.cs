using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SkillUIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipController.Instance.Show(data.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipController.Instance.Hide();
    }
    

    public void OnClick() => onClick?.Invoke(data);
}

