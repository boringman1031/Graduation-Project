using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTutorialTrigger : MonoBehaviour
{
    [Header("�s��")]
    public VoidEventSO tutorialJumpEvent; // ���D�оǨƥ�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // �ˬd�O�_�O���a
        {
            tutorialJumpEvent.RaiseEvent(); // Ĳ�o���D�оǨƥ�
        }
    }
}
