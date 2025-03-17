using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEditor.Search;

// DialogSystem 只用來顯示對話

public class DialogSystem : MonoBehaviour
{
    [Header("廣播")]
    public VoidEventSO dialogEndEvent;

    [Header("UI組件")]
    public Text textLabel; // UI對話框文字組件
    public Image faceImage; // UI對話框頭像圖片
    public Image Panel;

    [Header("Cinemachine 鏡頭")]
    public CinemachineVirtualCamera defaultCamera; // 預設鏡頭
    public CinemachineVirtualCamera focusCamera;  // 聚焦鏡頭

    private Queue<(string sentence, bool shouldFocusCamera, Vector2 focusPosition)> dialogQueue; // 儲存句子和運鏡標記

    public int index;
    public float textSpeed;

    bool textFinished; // 是否完成打字
    bool cancelTyping; // 取消打字

    void Awake()
    {
        dialogQueue = new Queue<(string, bool, Vector2)>();
    }
    private void OnEnable()
    {
        textFinished = true;
    }
    // 設置並顯示對話
    public void SetDialog(DialogData.DialogEntry dialogEntry)
    {
        // debug用
        /*
        if (dialogEntry == null)
        {
            Debug.LogError("DialogEntry is null!");
            return;
        }

        if (dialogEntry.sentences == null || dialogEntry.shouldFocusCamera == null ||
            dialogEntry.focusCameraPositions == null)
        {
            Debug.LogError("DialogEntry has null data!");
            return;
        }

        if (dialogEntry.sentences.Count != dialogEntry.shouldFocusCamera.Count ||
            dialogEntry.sentences.Count != dialogEntry.focusCameraPositions.Count)
        {
            Debug.LogError("DialogEntry has mismatched data lengths!");
            return;
        }
        */

        StopAllCoroutines(); // 停止可能正在運行的逐字顯示
        textLabel.text = ""; // 清空文字
        dialogQueue.Clear();

        // 將句子、運鏡標記、位置加入隊列
        for (int i = 0; i < dialogEntry.sentences.Count; i++)
        {
            dialogQueue.Enqueue((
                dialogEntry.sentences[i], 
                dialogEntry.shouldFocusCamera[i],
                dialogEntry.focusCameraPositions[i]
                ));
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

            var (currentLine, shouldFocus, focusPosition) = dialogQueue.Dequeue();

            if (shouldFocus)
            {
                SetFocusCamera(focusPosition); // 設置 FocusCamera 的位置
                SwitchToFocusCamera(); // 切換到聚焦鏡頭
            }
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

            // 恢復到預設鏡頭
            if (shouldFocus)
            {
                SwitchToDefaultCamera();
            }
        }

        Panel.gameObject.SetActive(false); // 關閉對話框
        dialogEndEvent.RaiseEvent();
    }
    // 設置 FocusCamera 的位置
    private void SetFocusCamera(Vector2 position)
    {
        focusCamera.transform.position = new Vector3(position.x, position.y, focusCamera.transform.position.z);
    }

    // 切換到聚焦鏡頭
    private void SwitchToFocusCamera()
    {
        defaultCamera.Priority = 0; // 降低預設鏡頭的優先級
        focusCamera.Priority = 10;  // 提高聚焦鏡頭的優先級
    }

    // 切換回預設鏡頭
    private void SwitchToDefaultCamera()
    {
        focusCamera.Priority = 0;  // 降低聚焦鏡頭的優先級
        defaultCamera.Priority = 10; // 提高預設鏡頭的優先級
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
