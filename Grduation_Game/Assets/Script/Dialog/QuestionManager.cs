using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [Header("廣播事件")]
    public VoidEventSO goHomeEvent;
    public VoidEventSO loadRandomSceneEvent;

    [Header("對話資料")]
    public List<QuestionDataSO> dialogueList;
    private List<QuestionDataSO> dialoguePool;
    private QuestionDataSO currentQuestion;

    [Header("UI組件")]
    public List<Button> optionButtons;
    public Text dialogueText; // 合併問題與回覆的文字框    
    public Text affectionText;//好感度文字框
    public GameObject nextButton;
    public Button Button;
    public GameObject UI; // 對話UI

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
        affectionText.text = $"好感度：{affection}";
    }

    void EndDialogue()
    {
        unlockSkillEvent.RaiseEvent();
        if (affection >= 40)
        {
            dialogueText.text = "今天聊得很開心，下次再一起來吧！不過，如果有機會，我更想找個更浪漫的地方，只和你一起。";
            nextButton.SetActive(true);
        }
        else
        {
            dialogueText.text = "...我還有點事，先走了...(你這個哥布林還是回山洞吧)";      
            nextButton.SetActive(true);
        }
        foreach (var btn in optionButtons)
        btn.gameObject.SetActive(false);
    }
}
