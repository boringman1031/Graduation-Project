/*-----------------BY017-------------------*/
/*--------�������-----*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    [SerializeField] Image Hp;
    [SerializeField] Image HpDelay;//������ܪ����
    [SerializeField] Image Mp;
    [SerializeField] Image PlayerAvator;//���a�Y��

    private void Update()
    {
        if(Hp.fillAmount> HpDelay.fillAmount)//�������ĪG
        {
            HpDelay.fillAmount -= Time.deltaTime * 5;
        }
    }

    public void OnHealthChange(float persentage)
    {
        Hp.fillAmount = persentage;
    }

    public void OnPowerChange(float persentage)
    {
        Mp.fillAmount = persentage;
    }

}
