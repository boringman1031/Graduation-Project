/*----------by017-----------*/
/*----此腳本用來記錄資料----*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Data
{
    public string sceneTosave;   

    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>();//角色位置資料

    public Dictionary<string, float> flaotSaveDataDict = new Dictionary<string, float>();//玩家基礎數值資料

    /// <summary>
    /// 保存遊戲場景並將場景專換成Json
    /// </summary>
    /// <param name="_gamescene">遊戲場景物件</param>
    public void SaveGameScene(GameSceneSO _gamescene)
    {
        sceneTosave = JsonUtility.ToJson(_gamescene);
    }

    /// <summary>
    /// 獲取保存的場景的Json並轉換成遊戲場景物件
    /// </summary>
    /// <returns>遊戲場景物件</returns>
    public GameSceneSO GetSaveScene()
    {
       var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneTosave, newScene);
        return newScene;

    }
}
