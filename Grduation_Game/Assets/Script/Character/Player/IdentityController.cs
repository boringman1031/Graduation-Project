/*----------BY017--------------------
.---------ōだ锣传╰参ノち传à︹ōだ
-----------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class IdentityController : MonoBehaviour
{
    public List<SpriteResolver> spriteResolvers = new List<SpriteResolver>();
    public GameObject item;
    SpriteResolver BodyResolver;
    void Start()
    {
        foreach (var resolver in FindObjectsOfType<SpriteResolver>())
        {
            spriteResolvers.Add(resolver);
            if(resolver.GetCategory()== "Body")
            {
                BodyResolver = resolver;
            }
        }   
    }
    public void ChangeIdentity()//ち传ōだ
    {
        //TODO:if(ち传ōだ兵ン笷Θ)
        foreach (var resolver in FindObjectsOfType<SpriteResolver>())
        {
            resolver.SetCategoryAndLabel(resolver.GetCategory(),"8+9");
        }
        if(BodyResolver.GetLabel() == "normal")
        {
            item.SetActive(false);
        }

    }
}
