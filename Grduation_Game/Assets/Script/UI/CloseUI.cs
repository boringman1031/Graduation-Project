using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI : MonoBehaviour
{
    public GameObject UI;
    public void closeUI()
    {
        UI.SetActive(false);
    }
}
