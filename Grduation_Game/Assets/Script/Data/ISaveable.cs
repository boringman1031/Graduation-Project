using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable 
{
    DataDefination GetDataID();
    void RegisterSaveData() => DataManager.instance.RegisterSaveData(this);//��ں޲z�����U

    void UnRegisterSaveData()=>DataManager.instance.UnRegisterSaveData(this);//��ں޲z���Ѱ����U
    //void RegisterSaveData()
    //{
    //    if (DataManager.instance == null)
    //    {
    //        Debug.LogError("DataManager.instance is null! �X��");
    //        return;
    //    }
    //    DataManager.instance.RegisterSaveData(this);
    //    Debug.Log($"Registered: {GetDataID()}");
    //}

    //void UnRegisterSaveData()
    //{
    //    if (DataManager.instance == null)
    //    {
    //        Debug.LogError("DataManager.instance is null! �X��");
    //        return;
    //    }
    //    DataManager.instance.UnRegisterSaveData(this);
    //    Debug.Log($"Unregistered: {GetDataID()}");
    //}

    void GetSaveData(Data _data);//����s�ɼƾ�

    void LoadData(Data _data);//�[���s�ɼƾ�
}
