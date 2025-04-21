using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " QuestionDataSO", menuName = "Dialog/QuestionDataSO")]
public class QuestionDataSO : ScriptableObject
{
    public string question;
    public List<string> options = new List<string>(3); // 選項文字

    [System.Serializable]
    public class AnswerResponse
    {
        public string reply;        // 女生回應
        public int affectionChange; // 對應好感度變化
    }

    public List<AnswerResponse> responses = new List<AnswerResponse>(3);
}
