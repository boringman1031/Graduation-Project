/*--------by017--------*/
/*----------角色基礎數值----------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharactorBase : MonoBehaviour,ISaveable
{
    [Header("事件監聽")]
    public VoidEventSO newGameEvent;//新遊戲事件

    [Header("基礎數值")]
    public float Health;
    public float Defence;

    [Header("當前數值")]
    public float MaxHealth;
    public float MaxPower;
    [HideInInspector]public float CurrentHealth;
    [HideInInspector] public float CurrentPower;
    public float healthRegenAmount; // 每次回復的生命值
    public float healthRegenInterval; // 回血的時間間隔（秒）
    private Coroutine regenCoroutine; // 用來存儲 Coroutine，確保不會多次啟動

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
        regenCoroutine = StartCoroutine(AutoRegenHealth());//自動回血
    }
    private void OnDisable()
    {
        if (newGameEvent != null)
        {
            newGameEvent.OnEventRaised -= NewGame;
        }
        else
        {
            Debug.LogWarning("newGameEvent is null in OnDisable()");
        }

        ISaveable saveable = this;
        if (DataManager.instance != null)
        {
            saveable.UnRegisterSaveData();
        }
        else
        {
            Debug.LogWarning("DataManager instance is null in OnDisable()");
        }
      
        if (regenCoroutine != null)  // 關閉自動回血
        {
            StopCoroutine(regenCoroutine);
        }
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
        else //死亡
        {
            CurrentHealth = 0;
            OnDead?.Invoke();          
        }
      OnHealthChange?.Invoke(this);//觸發血量改變事件
    }
    
    public void AddHealth(float _health)//增加血量
    {
        if (CurrentHealth + _health <= MaxHealth)
        {
            CurrentHealth += _health;
        }
        else
        {
            CurrentHealth = MaxHealth;
        }
        OnHealthChange?.Invoke(this);//觸發血量改變事件
    }
    public void AddPower(float _power)//增加能量
    {
        if (CurrentPower + _power <= MaxPower)
        {
            CurrentPower += _power;
        }
        else
        {
            CurrentPower = MaxPower;
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

    private IEnumerator AutoRegenHealth()//自動回血
    {    
        while (true)
        {
            yield return new WaitForSeconds(healthRegenInterval); // 等待時間間隔
            if (CurrentHealth < MaxHealth) // 如果血量未滿才回血
            {
                AddHealth(healthRegenAmount);
                OnHealthChange?.Invoke(this);              
            }
        }
    }

    public DataDefination GetDataID()//獲取ID
    {
        return  GetComponent<DataDefination>(); 
    }

    public void GetSaveData(Data _data)
    {
        if(_data.characterPosDict.ContainsKey(GetDataID().ID))//如果有這個ID的位置數據       
        {
           _data.characterPosDict[GetDataID().ID] = transform.position;//更改玩家位置數據
            _data.flaotSaveDataDict[GetDataID().ID + "health"] = this.CurrentHealth;//更改玩家血量數據
            _data.flaotSaveDataDict[GetDataID().ID + "power"] = this.CurrentPower;//更改玩家能量數據
        }
        else
        {
            _data.characterPosDict.Add(GetDataID().ID, transform.position);//新增玩家位置數據
            _data.flaotSaveDataDict.Add(GetDataID().ID+"health",this.CurrentHealth);//新增玩家血量數據
            _data.flaotSaveDataDict.Add(GetDataID().ID + "power", this.CurrentPower);//新增玩家能量數據
        }

    }

    public void LoadData(Data _data)
    {
        if(_data.characterPosDict.ContainsKey(GetDataID().ID))//如果有這個ID的玩家位置數據
        {
            transform.position = _data.characterPosDict[GetDataID().ID];//讀取玩家位置數據
            this.CurrentHealth = _data.flaotSaveDataDict[GetDataID().ID + "health"];//讀取玩家血量數據
            this.CurrentPower = _data.flaotSaveDataDict[GetDataID().ID + "power"];//讀取玩家能量數據

            OnHealthChange?.Invoke(this);//觸發血量改變事件
        }
    }

}
