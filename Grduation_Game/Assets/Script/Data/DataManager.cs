/*-------------BY017---------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataManager : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO saveDataEvent;

    public static DataManager instance;

    private List<ISaveable> saveableList = new List<ISaveable>();//此列表存儲所有需要保存的數據

    private Data saveData;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;      
            Debug.Log("DataManager 出現了");
        }
        else
        {
            Destroy(this.gameObject);
        }
       saveData = new Data();
    }

    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
    }
    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
    }

    private void Upadate()
    {
        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }

    }
    public void RegisterSaveData(ISaveable saveable)
    {
        if(!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }
    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    public void Save()
    {
        foreach(var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }
        foreach (var item in saveData.characterPosDict)
        {
            Debug.Log(item.Key + " " + item.Value);
        }
    }
    public void Load()
    {
        foreach (var saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
    }
}
