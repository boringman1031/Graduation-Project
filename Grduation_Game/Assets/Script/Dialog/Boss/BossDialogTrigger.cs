using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossDialogTrigger : MonoBehaviour
{
    public Animator BossAnim;
    public Animator Girlanim;

    public VoidEventSO dialogEndEvent; // 對話結束事件

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
        // 假設女主死亡動畫的觸發參數是 "GirlDeath"
        Girlanim.SetTrigger("Fade");
        // 等待女主死亡動畫播放完畢，假設動畫時長為3秒
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
