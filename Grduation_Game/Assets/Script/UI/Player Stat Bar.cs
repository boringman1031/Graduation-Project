/*-----------------BY017-------------------*/
/*--------血條控制-----*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    [SerializeField] Image Hp;
    [SerializeField] Image HpDelay;//延遲顯示的血條
    [SerializeField] Image Mp;
    [SerializeField] Image PlayerAvator;//玩家頭像

    private void Update()
    {
        if(Hp.fillAmount> HpDelay.fillAmount)//血條延遲效果
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
