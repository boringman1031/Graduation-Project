using UnityEngine;
using UnityEngine.UI;

public class SkillLibraryUI : MonoBehaviour
{
    [Header("UI 组件")]
    public GameObject skillIconPrefab; // 技能图标预制体
    public Transform skillIconParent;  // ScrollView 的 Content

    void Start()
    {
        // 遍历所有技能数据
        foreach (SkillData skill in SkillManager.Instance.allSkills)
        {
            // 生成技能图标
            GameObject icon = Instantiate(skillIconPrefab, skillIconParent);

            // 设置图标和名称
            Image iconImage = icon.GetComponent<Image>();
            Text nameText = icon.GetComponentInChildren<Text>();

            iconImage.sprite = skill.icon;
            nameText.text = skill.skillName;

            // 控制未解锁技能的显示
            if (!skill.isUnlocked)
            {
                iconImage.color = Color.gray; // 灰色显示
                nameText.color = Color.red;   // 红色文字提示
            }

            // 绑定拖拽脚本
            SkillDragHandler dragHandler = icon.GetComponent<SkillDragHandler>();
            if (dragHandler != null)
            {
                dragHandler.skill = skill; // 传递技能数据
            }
        }
    }
}