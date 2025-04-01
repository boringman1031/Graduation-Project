using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassUIItem : MonoBehaviour
{
    public Image icon;
    public Text nameText;
    private ClassData data;
    private System.Action<ClassData> onClick;

    public void Setup(ClassData cls, System.Action<ClassData> callback)
    {
        data = cls;
        onClick = callback;
        icon.sprite = cls.classIcon;
        nameText.text = cls.className;
    }

    public void OnClick() => onClick?.Invoke(data);
}
