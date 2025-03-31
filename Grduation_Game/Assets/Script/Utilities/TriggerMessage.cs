using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMessage : MonoBehaviour
{
    public Animator anim;
    public string Key;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetTrigger("Message");
            DialogManager.Instance.StartDialog(Key);
            gameObject.SetActive(false); // Ãö³¬ª«¥ó
        }
    }
}
