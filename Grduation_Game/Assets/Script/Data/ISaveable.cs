using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable 
{
    DataDefination GetDataID();
    void RegisterSaveData() => DataManager.instance.RegisterSaveData(this);//住據管理器註冊

    void UnRegisterSaveData()=>DataManager.instance.UnRegisterSaveData(this);//住據管理器解除註冊
    //void RegisterSaveData()
    //{
    //    if (DataManager.instance == null)
    //    {
    //        Debug.LogError("DataManager.instance is null! 出事");
    //        return;
    //    }
    //    DataManager.instance.RegisterSaveData(this);
    //    Debug.Log($"Registered: {GetDataID()}");
    //}

    //void UnRegisterSaveData()
    //{
    //    if (DataManager.instance == null)
    //    {
    //        Debug.LogError("DataManager.instance is null! 出事");
    //        return;
    //    }
    //    DataManager.instance.UnRegisterSaveData(this);
    //    Debug.Log($"Unregistered: {GetDataID()}");
    //}

    void GetSaveData(Data _data);//獲取存檔數據

    void LoadData(Data _data);//加載存檔數據
}
