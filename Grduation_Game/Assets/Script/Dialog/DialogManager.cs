using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 統一管理，DialogSystem 只需要負責顯示，不用自己找對話文本。
// 透過 StartDialog 來開始對話，不同腳本都能簡單呼叫。
public class DialogManager : MonoBehaviour
{
    public DialogData dialogData;  // 連結 `DialogData`
    private DialogSystem dialogSystem;

    void Awake()
    {
        dialogSystem = FindObjectOfType<DialogSystem>();
    }

    public void StartDialog(string key)
    {

        if (dialogData == null)
        {
            Debug.LogError("❌ DialogData 未連結到 DialogManager，請在 Inspector 設定！");
            return;
        }

        List<string> dialogLines = dialogData.GetDialog(key);
        if (dialogLines == null || dialogLines.Count == 0)
        {
            Debug.LogError($"❌ 找不到對話 Key：{key}，請確認 DialogData 是否有設定！");
            return;
        }

        DialogSystem dialogSystem = FindObjectOfType<DialogSystem>();
        if (dialogSystem == null)
        {
            Debug.LogError("❌ 找不到 DialogSystem，請確認場景中是否有 DialogSystem。");
            return;
        }

        dialogSystem.SetDialog(dialogLines);

        
        if (dialogLines != null)
        {
            dialogSystem.SetDialog(dialogLines);
        }
        else
        {
            Debug.LogError($"找不到對話 Key：{key}");
        }
    }
}
