using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelectionUI : MonoBehaviour
{
    public GameObject classButtonPrefab;  // ���s�w�s��
    public Transform classButtonParent;     // ���s������

    // �����o PlayerSkillController ���ѦҡA�קK�C���I������ FindObjectOfType �j�M
    private PlayerSkillController playerSkillController;

    private void Awake()
    {
        // ���զb��������� PlayerSkillController
        playerSkillController = FindObjectOfType<PlayerSkillController>();
    }

    void Start()
    {
        // �M�� SkillManager ���Ҧ��� ClassData
        foreach (ClassData classData in SkillManager.Instance.allClasses)
        {
            // �ƻs�@���A�קK���]�������D�]���ª� C# �����i�঳�o�Ӱ��D�^
            ClassData data = classData;

            // �إ߫��s
            GameObject button = Instantiate(classButtonPrefab, classButtonParent);

            // �]�w���s�Ϥ��]���]�w�s���� Image �ե�^
            Image image = button.GetComponent<Image>();
            if (image != null)
                image.sprite = data.classIcon;

            // �]�w���s�W��ܪ���r�]���]���s�l���� Text �ե�^
            Text text = button.GetComponentInChildren<Text>();
            if (text != null)
                text.text = data.className;

            // ���o Button �ե�íq�\ onClick �ƥ�
            Button btn = button.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => {
                    // ����s�Q�I���ɡA�N�����¾�~��s�� SkillManager
                    SkillManager.Instance.selectedClass = data;
                    // �q�����a�ޯ౱���s�j�ۡ]R �ޯ�^
                    if (playerSkillController != null)
                        playerSkillController.UpdateUltimateSkill();
                });
            }
        }
    }
}
