using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

// DialogSystem 只用來顯示對話

public class DialogSystem : MonoBehaviour
{
    [Header("廣播")]
    public VoidEventSO dialogEndEvent;

    [Header("UI組件")]
    public Text textLabel; // UI對話框文字組件
    public Image faceImage; // UI對話框頭像圖片
    public Image Panel;
    public Button SkipButton; // 繼續按鈕

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
        SkipButton.onClick.AddListener(onSkipButtonClick);
    }
    // 設置並顯示對話
    public void SetDialog(DialogData.DialogEntry dialogEntry)
    {
        StopAllCoroutines();
        textLabel.text = "";
        dialogQueue.Clear();

        // 加入句子
        for (int i = 0; i < dialogEntry.sentences.Count; i++)
        {
            dialogQueue.Enqueue((
                dialogEntry.sentences[i],
                dialogEntry.shouldFocusCamera[i],
                dialogEntry.focusCameraPositions[i]
            ));
        }

        // 禁止玩家移動
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.canMove = false;
            }
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
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F)); // 按R繼續

            // 恢復到預設鏡頭
            if (shouldFocus)
            {
                SwitchToDefaultCamera();
            }
        }

        Panel.gameObject.SetActive(false); // 關閉對話框
        dialogEndEvent.RaiseEvent();

        // 對話完成後開啟玩家移動
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.canMove = true;
            }
        }
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
    
    public void onSkipButtonClick()
    {
        if (!textFinished)
        {
            cancelTyping = true; // 跳過打字動畫
        }
    }
   
}
