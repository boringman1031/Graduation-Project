using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 存放不同場景或角色的對話（使用 key 來標記）。
// 透過 GetDialog("FirstScene") 來獲取對話，減少硬寫 TextAsset。
// 方便編輯：在 Unity 中建立 DialogData，直接在 Inspector 內編輯對話。
[CreateAssetMenu(fileName = "DialogData", menuName = "Dialog/DialogData")]
public class DialogData : ScriptableObject
{
    [System.Serializable]
    public class DialogEntry
    {
        public string key;  // 對話的鍵，例如 "FirstScene"、"BossFight"
        [TextArea(3, 10)]
        public List<string> sentences;  // 該對話的句子列表
    }

    public List<DialogEntry> dialogs = new List<DialogEntry>();

    private Dictionary<string, List<string>> dialogDict;

    private void OnEnable()
    {
        // 轉換 List 為 Dictionary，方便快速查找
        dialogDict = new Dictionary<string, List<string>>();
        foreach (var entry in dialogs)
        {
            if (!dialogDict.ContainsKey(entry.key))
            {
                dialogDict.Add(entry.key, entry.sentences);
            }
        }
    }
    public List<string> GetDialog(string key)
    {
        if (dialogDict.ContainsKey(key))
        {
            return dialogDict[key];
        }
        return null;
    }
}
