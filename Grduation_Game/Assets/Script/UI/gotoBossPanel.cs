using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gotoBossPanel : MonoBehaviour
{
   public Button button;

   public VoidEventSO gotoBossPanelEvent;

    private void Start()
    {
        button.onClick.AddListener(OnGotoBossPanel);
    }

    public void OnGotoBossPanel()
    {
        gotoBossPanelEvent.RaiseEvent();
        gameObject.SetActive(false);
    }
}
