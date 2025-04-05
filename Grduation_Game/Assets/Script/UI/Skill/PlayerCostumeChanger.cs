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

    public void ChangeCostume(string label)
    {
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

        Debug.Log("¤w¤Á´«¸Ë§ê¬°¡G" + label);
    }
}
