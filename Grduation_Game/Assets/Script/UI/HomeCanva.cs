using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCanva : MonoBehaviour
{
    public GameObject SkillAndClassPanel;

    public void OpenSkillAndClass()
    {
        SkillAndClassPanel.SetActive(true);
    }
    public void CloseSkillAndClass()
    {
        SkillAndClassPanel.SetActive(false);
    }
}
