using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable 
{
    void RegisterSaveData() => DataManager.instance.RegisterSaveData(this);
   
    void UnRegisterSaveData()=>DataManager.instance.UnRegisterSaveData(this);


    void GetSaveData();

    void LoadData();
}
