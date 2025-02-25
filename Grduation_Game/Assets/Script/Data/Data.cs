/*----------by017-----------*/
/*----���}���ΨӰO�����----*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Data
{
    public string sceneTosave;   

    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>();//�����m���

    public Dictionary<string, float> flaotSaveDataDict = new Dictionary<string, float>();//���a��¦�ƭȸ��

    /// <summary>
    /// �O�s�C�������ñN�����M����Json
    /// </summary>
    /// <param name="_gamescene">�C����������</param>
    public void SaveGameScene(GameSceneSO _gamescene)
    {
        sceneTosave = JsonUtility.ToJson(_gamescene);
    }

    /// <summary>
    /// ����O�s��������Json���ഫ���C����������
    /// </summary>
    /// <returns>�C����������</returns>
    public GameSceneSO GetSaveScene()
    {
       var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneTosave, newScene);
        return newScene;

    }
}
