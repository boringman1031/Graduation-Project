/*--------by017--------*/
/*----------�����¦�ƭ�----------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharactorBase : MonoBehaviour,ISaveable
{
    [Header("�ƥ��ť")]
    public VoidEventSO newGameEvent;//�s�C���ƥ�

    [Header("��¦�ƭ�")]
    public float Health;
    public float Defence;

    [Header("��e�ƭ�")]
    public float MaxHealth;
    public float MaxPower;
    [HideInInspector]public float CurrentHealth;
    [HideInInspector] public float CurrentPower;
    public float healthRegenAmount; // �C���^�_���ͩR��
    public float healthRegenInterval; // �^�媺�ɶ����j�]��^
    private Coroutine regenCoroutine; // �ΨӦs�x Coroutine�A�T�O���|�h���Ұ�

    [Header("���A")]
    public float SuperArmourTime;//�Q��ɶ�
    private float SuperArmourTimeCounter;//�Q��ɶ��p�ƾ�
    public bool SuperArmour;//�O�_�b�Q�骬�A

    public UnityEvent<CharactorBase> OnHealthChange;//��q���ܨƥ�(�Ω��sUI�s��)
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDead;

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
        regenCoroutine = StartCoroutine(AutoRegenHealth());//�۰ʦ^��
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
      
        if (regenCoroutine != null)  // �����۰ʦ^��
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
    public void TakeDamage(Attack _attacker)//����ˮ`�P�w
    {           
        if (SuperArmour)
        {
            return;
        }
        if(CurrentHealth-_attacker.Damage > 0)
        {           
            CurrentHealth -= _attacker.Damage;
            TriggerSuperArmour();
            //���ˮɭn�F���g�b�o
            OnTakeDamage?.Invoke(_attacker.transform);//Ĳ�o���˨ƥ�

        }
        else //���`
        {
            CurrentHealth = 0;
            OnDead?.Invoke();          
        }
      OnHealthChange?.Invoke(this);//Ĳ�o��q���ܨƥ�
    }
    
    public void AddHealth(float _health)//�W�[��q
    {
        if (CurrentHealth + _health <= MaxHealth)
        {
            CurrentHealth += _health;
        }
        else
        {
            CurrentHealth = MaxHealth;
        }
        OnHealthChange?.Invoke(this);//Ĳ�o��q���ܨƥ�
    }
    public void AddPower(float _power)//�W�[��q
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
    private void TriggerSuperArmour()//Ĳ�o�Q��
    {
        if(!SuperArmour) 
        {
            SuperArmour = true;
            SuperArmourTimeCounter = SuperArmourTime;
        }
    }

    private IEnumerator AutoRegenHealth()//�۰ʦ^��
    {    
        while (true)
        {
            yield return new WaitForSeconds(healthRegenInterval); // ���ݮɶ����j
            if (CurrentHealth < MaxHealth) // �p�G��q�����~�^��
            {
                AddHealth(healthRegenAmount);
                OnHealthChange?.Invoke(this);              
            }
        }
    }

    public DataDefination GetDataID()//���ID
    {
        return  GetComponent<DataDefination>(); 
    }

    public void GetSaveData(Data _data)
    {
        if(_data.characterPosDict.ContainsKey(GetDataID().ID))//�p�G���o��ID����m�ƾ�       
        {
           _data.characterPosDict[GetDataID().ID] = transform.position;//��缾�a��m�ƾ�
            _data.flaotSaveDataDict[GetDataID().ID + "health"] = this.CurrentHealth;//��缾�a��q�ƾ�
            _data.flaotSaveDataDict[GetDataID().ID + "power"] = this.CurrentPower;//��缾�a��q�ƾ�
        }
        else
        {
            _data.characterPosDict.Add(GetDataID().ID, transform.position);//�s�W���a��m�ƾ�
            _data.flaotSaveDataDict.Add(GetDataID().ID+"health",this.CurrentHealth);//�s�W���a��q�ƾ�
            _data.flaotSaveDataDict.Add(GetDataID().ID + "power", this.CurrentPower);//�s�W���a��q�ƾ�
        }

    }

    public void LoadData(Data _data)
    {
        if(_data.characterPosDict.ContainsKey(GetDataID().ID))//�p�G���o��ID�����a��m�ƾ�
        {
            transform.position = _data.characterPosDict[GetDataID().ID];//Ū�����a��m�ƾ�
            this.CurrentHealth = _data.flaotSaveDataDict[GetDataID().ID + "health"];//Ū�����a��q�ƾ�
            this.CurrentPower = _data.flaotSaveDataDict[GetDataID().ID + "power"];//Ū�����a��q�ƾ�

            OnHealthChange?.Invoke(this);//Ĳ�o��q���ܨƥ�
        }
    }

}
