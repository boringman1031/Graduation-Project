using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �s�񤣦P�����Ψ��⪺��ܡ]�ϥ� key �ӼаO�^�C
// �z�L GetDialog("FirstScene") �������ܡA��ֵw�g TextAsset�C
// ��K�s��G�b Unity ���إ� DialogData�A�����b Inspector ���s���ܡC
[CreateAssetMenu(fileName = "DialogData", menuName = "Dialog/DialogData")]
public class DialogData : ScriptableObject
{
    [System.Serializable]
    public class DialogEntry
    {
        public string key;  // ��ܪ���A�Ҧp "FirstScene"�B"BossFight"
        [TextArea(3, 10)]
        public List<string> sentences;  // �ӹ�ܪ��y�l�C��
    }

    public List<DialogEntry> dialogs = new List<DialogEntry>();

    private Dictionary<string, List<string>> dialogDict;

    private void OnEnable()
    {
        // �ഫ List �� Dictionary�A��K�ֳt�d��
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
