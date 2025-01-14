using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable 
{
    void RegisterSaveData();

    void UnRegisterSaveData();

    void GetSaveData();

    void LoadData();
}
