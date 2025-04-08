using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [Header("�s���ƥ�")]
    public VoidEventSO goHomeEvent;
    public VoidEventSO loadRandomSceneEvent;

    [Header("��ܸ��")]
    public List<QuestionDataSO> dialogueList;
    private List<QuestionDataSO> dialoguePool;
    private QuestionDataSO currentQuestion;

    [Header("UI�ե�")]
    public List<Button> optionButtons;
    public Text dialogueText; // �X�ְ��D�P�^�Ъ���r��    
    public Text affectionText;//�n�P�פ�r��
    public GameObject nextButton;
    public Button Button;
    public GameObject UI; // ���UI

    private int affection = 0;
    public VoidEventSO unlockSkillEvent;
    void Start()
    {
        dialoguePool = new List<QuestionDataSO>(dialogueList);
        NextRandomQuestion();
    }
    private void OnEnable()
    {
        Button.onClick.AddListener(() =>
        {
            loadRandomSceneEvent.OnEventRaised();
        });
    }
    private void OnDisable()
    {
        Button.onClick.RemoveAllListeners();
    }
     
    public void closeUI()
    {
        UI.SetActive(false);
    }
    void NextRandomQuestion()
    {
        if (dialoguePool.Count == 0)
        {
            EndDialogue();
            return;
        }

        int index = Random.Range(0, dialoguePool.Count);
        currentQuestion = dialoguePool[index];
        dialoguePool.RemoveAt(index);

        ShowDialogue(currentQuestion);
    }

    void ShowDialogue(QuestionDataSO data)
    {
        dialogueText.text = data.question;

        for (int i = 0; i < optionButtons.Count; i++)
        {
            Text btnText = optionButtons[i].GetComponentInChildren<Text>();
            btnText.text = data.options[i];

            int tempIndex = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => ChooseOption(tempIndex));
        }

        UpdateAffectionText();
    }

    void ChooseOption(int selectedIndex)
    {
        bool isCorrect = selectedIndex == currentQuestion.correctAnswerIndex;

        if (isCorrect)
        {
            affection += currentQuestion.affectionChangeOnCorrect;
            dialogueText.text = currentQuestion.correctReply;
        }
        else
        {
            affection += currentQuestion.affectionChangeOnWrong;
            dialogueText.text = currentQuestion.wrongReply;
        }

        UpdateAffectionText();

        foreach (var btn in optionButtons)
            btn.interactable = false;

        StartCoroutine(DelayNextDialogue(1.5f));
    }

    IEnumerator DelayNextDialogue(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var btn in optionButtons)
            btn.interactable = true;

        NextRandomQuestion();
    }

    void UpdateAffectionText()
    {
        affectionText.text = $"�n�P�סG{affection}";
    }

    void EndDialogue()
    {
        unlockSkillEvent.RaiseEvent();
        if (affection >= 40)
        {
            dialogueText.text = "���Ѳ�o�ܶ}�ߡA�U���A�@�_�ӧa�I���L�A�p�G�����|�A�ڧ�Q��ӧ�������a��A�u�M�A�@�_�C";
            nextButton.SetActive(true);
        }
        else
        {
            dialogueText.text = "...���٦��I�ơA�����F...(�A�o�ӭ����L�٬O�^�s�}�a)";      
            nextButton.SetActive(true);
        }
        foreach (var btn in optionButtons)
        btn.gameObject.SetActive(false);
    }
}
