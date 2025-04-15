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
        public List<bool> shouldFocusCamera; // �C�ӥy�l�O�_�ݭn�B��
        public List<Vector2> focusCameraPositions; // FocusCamera ���ؼЦ�m�]2D�^
        public List<bool> shouldShakeCamera; // �O�_�n���Y�_��
    }

    public List<DialogEntry> dialogs = new List<DialogEntry>();

    private Dictionary<string, DialogEntry> dialogDict;

    private void OnEnable()
    {
        // �ഫ List �� Dictionary�A��K�ֳt�d��
        dialogDict = new Dictionary<string, DialogEntry>();

        foreach (var entry in dialogs)
        {
            // ����debug��
            /*
            if (entry.sentences == null || entry.shouldFocusCamera == null ||
                entry.focusCameraPositions == null)
            {
                Debug.LogError($"DialogEntry {entry.key} has null data!");
                continue;
            }

            if (entry.sentences.Count != entry.shouldFocusCamera.Count ||
                entry.sentences.Count != entry.focusCameraPositions.Count)
            {
                Debug.LogError($"DialogEntry {entry.key} has mismatched data lengths!");
                continue;
            }
            */

            if (!dialogDict.ContainsKey(entry.key))
            {
                dialogDict.Add(entry.key, entry);
            }
        }
    }
    public DialogEntry GetDialog(string key)
    {
        if (dialogDict.ContainsKey(key))
        {
            return dialogDict[key];
        }
        return null;
    }
}
