using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 統一管理，DialogSystem 只需要負責顯示，不用自己找對話文本。
// 透過 StartDialog 來開始對話，不同腳本都能簡單呼叫。
public class DialogManager : MonoBehaviour
{
    [SerializeField] private SceneLoadedEventSO sceneLoadedEvent;

    public static DialogManager Instance { get; private set; } // Singleton 實例

    public DialogData dialogData;  // 當前使用的 DialogData
    private DialogSystem dialogSystem;

    void Awake()
    {
        // 確保 Singleton 實例唯一
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨場景保留
        }
        else
        {
            Destroy(gameObject); // 避免重複實例
            return;
        }

        FindDialogSystem();
    }
    private void FindDialogSystem()
    {
        dialogSystem = FindObjectOfType<DialogSystem>();
        if (dialogSystem == null)
        {
            Debug.LogError("找不到 DialogSystem！");
        }
    }
    // 場景加載後重新查找 DialogSystem
    private void OnEnable()
    {
        sceneLoadedEvent.OnSceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        sceneLoadedEvent.OnSceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(GameSceneSO scene)
    {
        if (!string.IsNullOrEmpty(scene.dialogKey))
        {
            StartDialog(scene.dialogKey);
        }
    }
    public void StartDialog(string key)
    {

        if (dialogData == null)
        {
            Debug.LogError("❌ DialogData 未連結到 DialogManager，請在 Inspector 設定！");
            return;
        }

        DialogData.DialogEntry dialogEntry = dialogData.GetDialog(key);
        if (dialogEntry == null || dialogEntry.sentences.Count == 0)
        {
            Debug.LogError($"❌ 找不到對話 Key：{key}，請確認 DialogData 是否有設定！");
            return;
        }

        dialogSystem.SetDialog(dialogEntry);
    }
}
