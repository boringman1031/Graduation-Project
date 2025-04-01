using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassUIItem : MonoBehaviour
{
    public Image icon;
    public Text nameText;
    public GameObject selectedMark; // ✅ 當前職業標記

    private ClassData data;
    private System.Action<ClassData> onClick;

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
