using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossDialogTrigger : MonoBehaviour
{
    public Animator BossAnim;
    public Animator Girlanim;

    public VoidEventSO dialogEndEvent; // ��ܵ����ƥ�

    public string Key;

    private bool isTalk = false;

    private void OnEnable()
    {
        dialogEndEvent.OnEventRaised += OnDialogEnd;
    }
    private void OnDisable()
    {
        dialogEndEvent.OnEventRaised -= OnDialogEnd;
    }

    void OnDialogEnd()
    {
        if (isTalk)
        {
            StartCoroutine(WaitForGirlDeathAnimation());
        }
    }

    private IEnumerator WaitForGirlDeathAnimation()
    {
        // ���]�k�D���`�ʵe��Ĳ�o�ѼƬO "GirlDeath"
        Girlanim.SetTrigger("Fade");
        // ���ݤk�D���`�ʵe���񧹲��A���]�ʵe�ɪ���3��
        yield return new WaitForSeconds(2.3f);
        BossAnim.SetBool("BossShow", true);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogManager.Instance.StartDialog(Key);
            isTalk = true;
        }
    }
    
}
