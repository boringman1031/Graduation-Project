using System.Collections;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerCostumeChanger : MonoBehaviour
{
    [Header("Sprite Resolvers")]
    public SpriteResolver head;
    public SpriteResolver body;
    public SpriteResolver leftArmUp;
    public SpriteResolver leftArmDown;
    public SpriteResolver rightArmUp;
    public SpriteResolver rightArmDown;
    public SpriteResolver leftLegUp;
    public SpriteResolver leftLegDown;
    public SpriteResolver rightLegUp;
    public SpriteResolver rightLegDown;
    public SpriteResolver left;
    public SpriteResolver right;

    [Header("對應的 SpriteSkin（順序要和 Resolver 對應）")]
    public SpriteSkin[] spriteSkins;

    public void ChangeCostume(string label)
    {
        StartCoroutine(SafeChangeCostume(label));
    }

    private IEnumerator SafeChangeCostume(string label)
    {
        // 1. 停用所有 SpriteSkin 避免報錯
        foreach (var skin in spriteSkins)
        {
            if (skin != null) skin.enabled = false;
        }

        yield return null; // 等一幀確保圖切換不被干擾

        // 2. 安全換裝
        head.SetCategoryAndLabel("Head", label);
        body.SetCategoryAndLabel("Body", label);
        leftArmUp.SetCategoryAndLabel("Left Arm UP", label);
        leftArmDown.SetCategoryAndLabel("Left Arm Down", label);
        rightArmUp.SetCategoryAndLabel("Right Arm UP", label);
        rightArmDown.SetCategoryAndLabel("Right Arm Down", label);
        leftLegUp.SetCategoryAndLabel("Left Leg UP", label);
        leftLegDown.SetCategoryAndLabel("Left Leg Down", label);
        rightLegUp.SetCategoryAndLabel("Right Leg UP", label);
        rightLegDown.SetCategoryAndLabel("Right Leg Down", label);
        left.SetCategoryAndLabel("Left", label);
        right.SetCategoryAndLabel("Right", label);

        yield return null; // 再等一幀，讓新 Sprite 生效

        // 3. 啟用 SpriteSkin，會自動更新骨架資料（相當於舊版 Bake）
        foreach (var skin in spriteSkins)
        {
            if (skin != null) skin.enabled = true;
        }

        Debug.Log("✅ 換裝完成：" + label);
    }
}
