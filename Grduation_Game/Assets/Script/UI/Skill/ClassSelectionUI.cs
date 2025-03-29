using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelectionUI : MonoBehaviour
{
    public GameObject classButtonPrefab;  // 按鈕預製物
    public Transform classButtonParent;     // 按鈕父物件

    // 先取得 PlayerSkillController 的參考，避免每次點擊都用 FindObjectOfType 搜尋
    private PlayerSkillController playerSkillController;

    private void Awake()
    {
        // 嘗試在場景中找到 PlayerSkillController
        playerSkillController = FindObjectOfType<PlayerSkillController>();
    }

    void Start()
    {
        // 遍歷 SkillManager 中所有的 ClassData
        foreach (ClassData classData in SkillManager.Instance.allClasses)
        {
            // 複製一份，避免閉包捕捉問題（較舊的 C# 版本可能有這個問題）
            ClassData data = classData;

            // 建立按鈕
            GameObject button = Instantiate(classButtonPrefab, classButtonParent);

            // 設定按鈕圖片（假設預製物有 Image 組件）
            Image image = button.GetComponent<Image>();
            if (image != null)
                image.sprite = data.classIcon;

            // 設定按鈕上顯示的文字（假設按鈕子物件有 Text 組件）
            Text text = button.GetComponentInChildren<Text>();
            if (text != null)
                text.text = data.className;

            // 取得 Button 組件並訂閱 onClick 事件
            Button btn = button.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => {
                    // 當按鈕被點擊時，將選取的職業更新到 SkillManager
                    SkillManager.Instance.selectedClass = data;
                    // 通知玩家技能控制器更新大招（R 技能）
                    if (playerSkillController != null)
                        playerSkillController.UpdateUltimateSkill();
                });
            }
        }
    }
}
