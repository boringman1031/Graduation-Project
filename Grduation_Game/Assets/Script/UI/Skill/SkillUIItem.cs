using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIItem : MonoBehaviour
{
    public Image icon;
    public Text nameText;
    private SkillData data;
    private System.Action<SkillData> onClick;

    public void Setup(SkillData skill, System.Action<SkillData> callback)
    {
        data = skill;
        onClick = callback;
        icon.sprite = skill.icon;
        nameText.text = skill.skillName;
    }

    public void OnClick() => onClick?.Invoke(data);
}
