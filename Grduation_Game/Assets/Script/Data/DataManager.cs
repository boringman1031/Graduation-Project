/*-------------BY017---------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private List<ISaveable> saveableList = new List<ISaveable>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;       
        }
        else
        {
            Destroy(this.gameObject);
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
}
