using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable 
{
    DataDefination GetDataID();
    void RegisterSaveData() => DataManager.instance.RegisterSaveData(this);
   
    void UnRegisterSaveData()=>DataManager.instance.UnRegisterSaveData(this);


    void GetSaveData(Data _data);

    void LoadData(Data _data);
}
