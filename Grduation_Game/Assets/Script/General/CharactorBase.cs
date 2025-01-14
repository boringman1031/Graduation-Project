/*--------by017--------*/
/*----------角色基礎數值----------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharactorBase : MonoBehaviour,ISaveable
{
    [Header("事件監聽")]
    public VoidEventSO newGameEvent;

    [Header("基礎數值")]
    public float Health;
    public float Defence;

    [Header("當前數值")]
    public float MaxHealth;
    public float MaxPower;
    public float CurrentHealth;
    public float CurrentPower;

    [Header("狀態")]
    public float SuperArmourTime;//霸體時間
    private float SuperArmourTimeCounter;//霸體時間計數器
    public bool SuperArmour;//是否在霸體狀態

    public UnityEvent<CharactorBase> OnHealthChange;//血量改變事件(用於更新UI廣播)
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDead;

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
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
    private void NewGame()
    {
        MaxHealth = Health + Defence;
        CurrentHealth = MaxHealth;
        CurrentPower = MaxPower;
        OnHealthChange?.Invoke(this);
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
            OnDead?.Invoke();
            //死亡
        }
      OnHealthChange?.Invoke(this);//觸發血量改變事件
    }

    private void TriggerSuperArmour()//觸發霸體
    {
        if(!SuperArmour) 
        {
            SuperArmour = true;
            SuperArmourTimeCounter = SuperArmourTime;
        }
    }

    public DataDefination GetDataID()
    {
        return  GetComponent<DataDefination>(); 
    }

    public void GetSaveData(Data _data)
    {
        if(_data.characterPosition.ContainsKey(GetDataID().ID))//如果有這個ID的位置數據       
        {
           _data.characterPosition[GetDataID().ID] = transform.position;//更改玩家位置數據
            _data.flaotSaveData[GetDataID().ID + "health"] = this.CurrentHealth;//更改玩家血量數據
            _data.flaotSaveData[GetDataID().ID + "power"] = this.CurrentPower;//更改玩家能量數據
        }
        else
        {
            _data.characterPosition.Add(GetDataID().ID, transform.position);//新增玩家位置數據
            _data.flaotSaveData.Add(GetDataID().ID+"health",this.CurrentHealth);//新增玩家血量數據
            _data.flaotSaveData.Add(GetDataID().ID + "power", this.CurrentPower);//新增玩家能量數據
        }

    }

    public void LoadData(Data _data)
    {
        if(_data.characterPosition.ContainsKey(GetDataID().ID))//如果有這個ID的玩家位置數據
        {
            transform.position = _data.characterPosition[GetDataID().ID];//讀取玩家位置數據
            this.CurrentHealth = _data.flaotSaveData[GetDataID().ID + "health"];//讀取玩家血量數據
            this.CurrentPower = _data.flaotSaveData[GetDataID().ID + "power"];//讀取玩家能量數據

            OnHealthChange?.Invoke(this);//觸發血量改變事件
        }
    }
}
