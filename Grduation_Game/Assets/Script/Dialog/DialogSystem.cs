using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// DialogSystem �u�Ψ���ܹ��

public class DialogSystem : MonoBehaviour
{
    [Header("�s��")]
    public VoidEventSO dialogEndEvent;

    [Header("UI�ե�")]
    public Text textLabel; // UI��ܮؤ�r�ե�
    public Image faceImage; // UI��ܮ��Y���Ϥ�
    public Image Panel;

    private Queue<string> dialogQueue;

    public int index;
    public float textSpeed;

    [Header("�Y��")]
    public Sprite face01;
    public Sprite face02;

    bool textFinished; // �O�_�������r
    bool cancelTyping; // �������r

    void Awake()
    {
        dialogQueue = new Queue<string>();
    }
    private void OnEnable()
    {
        textFinished = true;
    }
    // �]�m����ܹ��
    public void SetDialog(List<string> dialogLines)
    {
        StopAllCoroutines(); // ����i�ॿ�b�B�檺�v�r���
        textLabel.text = ""; // �M�Ť�r
        dialogQueue.Clear();

        foreach (string line in dialogLines)
        {
            dialogQueue.Enqueue(line);
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

            string currentLine = dialogQueue.Dequeue();
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
        }

        Panel.gameObject.SetActive(false); // ������ܮ�
        dialogEndEvent.RaiseEvent();
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
