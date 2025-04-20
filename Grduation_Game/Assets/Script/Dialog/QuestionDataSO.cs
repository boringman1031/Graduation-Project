using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = " QuestionDataSO", menuName = "Dialog/QuestionDataSO")]
public class QuestionDataSO : ScriptableObject
{
    public string question;
    public List<string> options = new List<string>(3); // �ﶵ��r

    [System.Serializable]
    public class AnswerResponse
    {
        public string reply;        // �k�ͦ^��
        public int affectionChange; // �����n�P���ܤ�
    }

    public List<AnswerResponse> responses = new List<AnswerResponse>(3);
}
