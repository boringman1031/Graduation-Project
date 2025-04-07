using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClassUIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public Text nameText;
    public GameObject selectedMark; // ✅ 當前職業標記

    private ClassData data;
    private System.Action<ClassData> onClick;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipController.Instance.Show(data.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipController.Instance.Hide();
    }
    public void Setup(ClassData cls, System.Action<ClassData> callback, bool isSelected)
    {
        data = cls;
        onClick = callback;
        icon.sprite = cls.classIcon;
        nameText.text = cls.className;
        if (selectedMark != null) selectedMark.SetActive(isSelected);
    }

    public void OnClick() => onClick?.Invoke(data);
}
