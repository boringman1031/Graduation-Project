using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerBase;
/*------------------by 017-----------------------*/
public class GammingCanvaController : MonoBehaviour
{
    private PlayerBase player;

    [SerializeField]
    private Text money_Count;
    [SerializeField]    
    private Image Skill1_Image;
    [SerializeField]
    private Button Skill1_Button;
    [SerializeField]
    private Image Skill2_Image;
    [SerializeField] 
    private Button Skill2_Button;
    [SerializeField]
    private Image Skill3_Image;
    [SerializeField]
    private Button Skill3_Button;
    [SerializeField]
    private Image Skill4_Image;
    [SerializeField]
    private Button Skill4_Button;

    [SerializeField]
    private Slider healthBar;//���
    [SerializeField]
    private Slider PowerBar;//�]�O��

    private void Awake()
    {
        player = FindObjectOfType<PlayerBase>();
        healthBar.maxValue = player.PlayerHp;

        // �q�\�ƥ�
        player.OnPlayerHit += UpdateHealthBar;
        player.OnPlayerUseSkill1 += UpdatePowerBar;
    }


    private void UpdateHealthBar(int damage)
    {
        healthBar.value = player.PlayerHp;
        Debug.Log($"Health bar updated. Current health: {player.PlayerHp}");
    }

    private void UpdatePowerBar(int powercoust)
    {
        PowerBar.value = player.PlayerPower;
        Debug.Log($"Health bar updated. Current health: {player.PlayerPower}");
    }

    void OnDestroy()
    {
        // �����q�\
        player.OnPlayerHit -= UpdateHealthBar;
        player.OnPlayerUseSkill1 -= UpdatePowerBar;
    }
}
