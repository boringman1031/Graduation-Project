using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDragHandler : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("绑定的技能数据")]
    public SkillData skill; // 在 SkillLibraryUI 中自动设置

    private GameObject dragIcon;    // 拖拽时的临时图标
    private Vector2 startPosition;  // 初始位置

    // 开始拖拽时调用
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 记录初始位置
        startPosition = transform.position;

        // 创建拖拽图标
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(transform.root); // 防止被 ScrollView 裁剪
        dragIcon.transform.SetAsLastSibling();       // 显示在最上层

        // 添加图像组件
        Image image = dragIcon.AddComponent<Image>();
        image.sprite = skill.icon;
        image.raycastTarget = false; // 防止遮挡射线检测

        // 禁用原图标的射线检测
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    // 拖拽过程中调用
    public void OnDrag(PointerEventData eventData)
    {
        // 更新图标位置（使用屏幕坐标）
        if (dragIcon != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                dragIcon.transform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPos
            );
            dragIcon.transform.localPosition = localPos;
        }
    }

    // 结束拖拽时调用
    public void OnEndDrag(PointerEventData eventData)
    {
        // 销毁临时图标
        Destroy(dragIcon);

        // 恢复原图标的射线检测
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // 检测是否拖到技能槽
        SkillSlotUI slot = eventData.pointerCurrentRaycast.gameObject?.GetComponent<SkillSlotUI>();
        if (slot != null && skill.isUnlocked)
        {
            // 更新技能槽数据
            SkillManager.Instance.EquipSkill(skill, slot.slotIndex);
        }
        else
        {
            // 回到原位
            transform.position = startPosition;
        }
    }
}