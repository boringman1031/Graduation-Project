using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlDialogTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogManager.Instance.StartDialog("FirstScene_meetGirl");
        }    

    }
}
