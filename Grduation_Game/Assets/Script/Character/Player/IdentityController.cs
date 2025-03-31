/*----------BY017--------------------
.---------身分轉換系統用於切換角色身分
-----------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using static UnityEngine.InputSystem.InputSettings;

public class IdentityController : MonoBehaviour
{
    [Header("事件監聽")]
    public VoidEventSO afterSceneLoadedEvent;

    public List<SpriteResolver> spriteResolvers = new List<SpriteResolver>();
    public GameObject item;
    SpriteResolver BodyResolver;
    private SpriteSkin spriteSkin; // 角色的 SpriteSkin（骨架控制）
    void Start()
    {
        FindAllSpriteResolvers();
    }
    private void OnEnable()
    {
        afterSceneLoadedEvent.OnEventRaised += UpdateBone;
    }
    private void OnDisable()
    {
        afterSceneLoadedEvent.OnEventRaised -= UpdateBone;
    }

    private void FindAllSpriteResolvers()
    {
        spriteResolvers.Clear(); // 清空舊的
        foreach (var resolver in GetComponentsInChildren<SpriteResolver>())
        {
            spriteResolvers.Add(resolver);
            if (resolver.GetCategory() == "Body") // 找到 `Body` 的部件
            {
                BodyResolver = resolver;
            }
        }
        spriteSkin = GetComponentInChildren<SpriteSkin>();
    }

    public void UpdateBone()
    {
        if (spriteSkin != null)
        {
            spriteSkin.enabled = false;
            spriteSkin.enabled = true;
            spriteSkin.autoRebind = true; // **強制 Rebind 修正骨架錯亂**         
        }
        else
        {
            Debug.LogWarning("SpriteSkin 未找到，請確認角色是否有骨架動畫！");
        }
    }
    public void ChangeIdentity()//切換身分
    {
        //TODO:if(切換身分條件達成)
        foreach (var resolver in FindObjectsOfType<SpriteResolver>())
        {
            resolver.SetCategoryAndLabel(resolver.GetCategory(), "8+9");
        }
        if (BodyResolver.GetLabel() == "normal")
        {
            item.SetActive(false);
        }

    }
}
