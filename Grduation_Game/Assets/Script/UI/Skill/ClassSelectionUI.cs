using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelectionUI : MonoBehaviour
{
    public GameObject classButtonPrefab;
    public Transform classButtonParent;

    void Start()
    {
        foreach (ClassData classData in SkillManager.Instance.allClasses)
        {
            GameObject button = Instantiate(classButtonPrefab, classButtonParent);
            button.GetComponent<Image>().sprite = classData.classIcon;
            button.GetComponentInChildren<Text>().text = classData.className;
            button.GetComponent<Button>().onClick.AddListener(() => {
                SkillManager.Instance.selectedClass = classData;
                // �q�����a�����s R �ޯ�
                PlayerSkillController player = FindObjectOfType<PlayerSkillController>();
                if (player != null) player.UpdateUltimateSkill();
            });
        }
    }
}
