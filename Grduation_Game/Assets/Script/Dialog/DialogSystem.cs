using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEditor.Search;

// DialogSystem �u�Ψ���ܹ��

public class DialogSystem : MonoBehaviour
{
    [Header("�s��")]
    public VoidEventSO dialogEndEvent;

    [Header("UI�ե�")]
    public Text textLabel; // UI��ܮؤ�r�ե�
    public Image faceImage; // UI��ܮ��Y���Ϥ�
    public Image Panel;

    [Header("Cinemachine ���Y")]
    public CinemachineVirtualCamera defaultCamera; // �w�]���Y
    public CinemachineVirtualCamera focusCamera;  // �E�J���Y

    private Queue<(string sentence, bool shouldFocusCamera, Vector2 focusPosition)> dialogQueue; // �x�s�y�l�M�B��аO

    public int index;
    public float textSpeed;

    bool textFinished; // �O�_�������r
    bool cancelTyping; // �������r

    void Awake()
    {
        dialogQueue = new Queue<(string, bool, Vector2)>();
    }
    private void OnEnable()
    {
        textFinished = true;
    }
    // �]�m����ܹ��
    public void SetDialog(DialogData.DialogEntry dialogEntry)
    {
        // debug��
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

        StopAllCoroutines(); // ����i�ॿ�b�B�檺�v�r���
        textLabel.text = ""; // �M�Ť�r
        dialogQueue.Clear();

        // �N�y�l�B�B��аO�B��m�[�J���C
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
    // �v�r��ܹ��
    IEnumerator DisplayText()
    {
        while (dialogQueue.Count > 0)
        {
            textFinished = false;
            textLabel.text = ""; // �M���¹��

            var (currentLine, shouldFocus, focusPosition) = dialogQueue.Dequeue();

            if (shouldFocus)
            {
                SetFocusCamera(focusPosition); // �]�m FocusCamera ����m
                SwitchToFocusCamera(); // ������E�J���Y
            }
            foreach (char letter in currentLine.ToCharArray())
            {
                if (cancelTyping) // ���U���L��ɡA������ܧ���y�l
                {
                    textLabel.text = currentLine;
                    break;
                }
                textLabel.text += letter;
                yield return new WaitForSeconds(textSpeed);
            }

            textFinished = true;
            cancelTyping = false; // ���m�аO
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R)); // ��R�~��

            // ��_��w�]���Y
            if (shouldFocus)
            {
                SwitchToDefaultCamera();
            }
        }

        Panel.gameObject.SetActive(false); // ������ܮ�
        dialogEndEvent.RaiseEvent();
    }
    // �]�m FocusCamera ����m
    private void SetFocusCamera(Vector2 position)
    {
        focusCamera.transform.position = new Vector3(position.x, position.y, focusCamera.transform.position.z);
    }

    // ������E�J���Y
    private void SwitchToFocusCamera()
    {
        defaultCamera.Priority = 0; // ���C�w�]���Y���u����
        focusCamera.Priority = 10;  // �����E�J���Y���u����
    }

    // �����^�w�]���Y
    private void SwitchToDefaultCamera()
    {
        focusCamera.Priority = 0;  // ���C�E�J���Y���u����
        defaultCamera.Priority = 10; // �����w�]���Y���u����
    }
    // ���U R �ɡA���L�v�r��X�A������ܧ���y�l
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!textFinished)
            {
                cancelTyping = true; // ���L���r�ʵe
            }
        }
    }
}
