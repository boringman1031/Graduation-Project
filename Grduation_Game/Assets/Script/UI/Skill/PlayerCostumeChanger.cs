using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerCostumeChanger : MonoBehaviour
{
    public SpriteLibrary spriteLibrary;

    public void ChangeCostume(string label)
    {
        if (spriteLibrary != null)
        {
            spriteLibrary.spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>($"Costumes/{label}");
            Debug.Log($"更換服裝為：{label}");
        }
        else
        {
            Debug.LogWarning("SpriteLibrary 未設定！");
        }
    }
}
