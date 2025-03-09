using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogSystem : MonoBehaviour
{
    [Header("UI�ե�")]
    public Text textLabel; // UI��ܮؤ�r�ե�
    public Image faceImage; // UI��ܮ��Y���Ϥ�

    [Header("�奻���")]
    public TextAsset textFile; // ��ܤ�r���
    public int index;
    public float textSpeed;
    private Dictionary<string, TextAsset> dialogDict = new Dictionary<string, TextAsset>();

    [Header("�Y��")]
    public Sprite face01;
    public Sprite face02;

    bool textFinished; // �O�_�������r
    bool cancelTyping; // �������r

    List<string> textList = new List<string>();
    void Awake()
    {
        GetTextFromFile(textFile);
    }
    private void OnEnable()
    {
        textFinished = true;
        StartCoroutine(SetTextUI());
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
            return;
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (textFinished && !cancelTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!textFinished && !cancelTyping)
            {
                cancelTyping = true;
            }
        }
    }
    // Ū���奻��󪺦r
    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineData = file.text.Split('\n');
        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }
    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";

        switch(textList[index].Trim().ToString())
        {
            case "A":
                faceImage.sprite = face01;
                index++;
                break;
            case "B":
                faceImage.sprite = face02;
                index++;
                break;
        }

        int letter = 0;
        while (!cancelTyping && letter < textList[index].Length -1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];
        cancelTyping = false;
        textFinished = true;
        index++;
    }
}
