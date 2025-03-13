using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// DialogSystem 只用來顯示對話

public class DialogSystem : MonoBehaviour
{
    [Header("廣播")]
    public VoidEventSO dialogEndEvent;

    [Header("UI組件")]
    public Text textLabel; // UI對話框文字組件
    public Image faceImage; // UI對話框頭像圖片
    public Image Panel;

    private Queue<string> dialogQueue;

    public int index;
    public float textSpeed;

    [Header("頭像")]
    public Sprite face01;
    public Sprite face02;

    bool textFinished; // 是否完成打字
    bool cancelTyping; // 取消打字

    void Awake()
    {
        dialogQueue = new Queue<string>();
    }
    private void OnEnable()
    {
        textFinished = true;
    }
    // 設置並顯示對話
    public void SetDialog(List<string> dialogLines)
    {
        StopAllCoroutines(); // 停止可能正在運行的逐字顯示
        textLabel.text = ""; // 清空文字
        dialogQueue.Clear();

        foreach (string line in dialogLines)
        {
            dialogQueue.Enqueue(line);
        }

        Panel.gameObject.SetActive(true);
        StartCoroutine(DisplayText());
    }
    // 逐字顯示對話
    IEnumerator DisplayText()
    {
        while (dialogQueue.Count > 0)
        {
            textFinished = false;
            textLabel.text = ""; // 清空舊對話

            string currentLine = dialogQueue.Dequeue();
            foreach (char letter in currentLine.ToCharArray())
            {
                if (cancelTyping) // 按下跳過鍵時，直接顯示完整句子
                {
                    textLabel.text = currentLine;
                    break;
                }
                textLabel.text += letter;
                yield return new WaitForSeconds(textSpeed);
            }

            textFinished = true;
            cancelTyping = false; // 重置標記
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R)); // 按R繼續
        }

        Panel.gameObject.SetActive(false); // 關閉對話框
        dialogEndEvent.RaiseEvent();
    }
    // 按下 R 時，跳過逐字輸出，直接顯示完整句子
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!textFinished)
            {
                cancelTyping = true; // 跳過打字動畫
            }
        }
    }
}
