/*--------by017--------*/
/*----------角色基礎數值----------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharactorBase : MonoBehaviour
{
    [Header("基礎數值")]
    public float Health;
    public float Defence;

    [Header("當前數值")]
    public float MaxHealth;
    public float CurrentHealth;
    public float CurrentPower;

    [Header("狀態")]
    public float SuperArmourTime;//霸體時間
    private float SuperArmourTimeCounter;//霸體時間計數器
    public bool SuperArmour;//是否在霸體狀態

    public UnityEvent<Transform> OnTakeDamage;

    private void Start()
    {
        MaxHealth = Health+Defence;
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        if (SuperArmour)
        {
            SuperArmourTimeCounter -= Time.deltaTime;
            if (SuperArmourTimeCounter <= 0)
            {
                SuperArmour = false;
            }
        }
    }
    public void TakeDamage(Attack _attacker)//受到傷害判定
    {
        if (SuperArmour)
        {
            return;
        }
        if(CurrentHealth-_attacker.Damage > 0)
        {
            CurrentHealth -= _attacker.Damage;
            TriggerSuperArmour();
            //受傷時要幹嘛寫在這
            OnTakeDamage?.Invoke(_attacker.transform);//觸發受傷事件

        }
        else
        {
            CurrentHealth = 0;
            //死亡
        }
      
    }

    private void TriggerSuperArmour()//觸發霸體
    {
        if(!SuperArmour) 
        {
            SuperArmour = true;
            SuperArmourTimeCounter = SuperArmourTime;
        }
    }
}
