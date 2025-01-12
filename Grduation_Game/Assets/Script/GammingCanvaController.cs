using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerController;
/*------------------by 017-----------------------*/
public class GammingCanvaController : MonoBehaviour
{
    private PlayerController player;

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
    private Slider healthBar;//血條
    [SerializeField]
    private Slider PowerBar;//魔力條

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        if (player == null)
        {
            Debug.LogWarning("Player not found in the scene.");
            return;
        }
        // 訂閱事件
        player.OnPlayerHit += UpdateHealthBar;
        player.OnPlayerUseSkill1 += UpdatePowerBar;
    }
    private void Start()
    {  
        healthBar.maxValue = player.PlayerHp;
        PowerBar.maxValue = player.PlayerPower;
        healthBar.value = healthBar.maxValue;
        PowerBar.value = PowerBar.maxValue;
        Debug.Log($"Health bar max value: {player.PlayerHp}");
        Debug.Log($"Power bar max value: {player.PlayerPower}");
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
        // 取消訂閱
        player.OnPlayerHit -= UpdateHealthBar;
        player.OnPlayerUseSkill1 -= UpdatePowerBar;
    }
}
