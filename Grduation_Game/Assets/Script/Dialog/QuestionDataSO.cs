using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " QuestionDataSO", menuName = "Dialog/QuestionDataSO")]
public class QuestionDataSO : ScriptableObject
{
    public string question;
    public List<string> options = new List<string>(3); // 選項
    public int correctAnswerIndex;
    public int affectionChangeOnCorrect = 10;
    public int affectionChangeOnWrong = -5;

    public string correctReply; // 答對時女生的回應
    public string wrongReply;   // 答錯時女生的回應
}
