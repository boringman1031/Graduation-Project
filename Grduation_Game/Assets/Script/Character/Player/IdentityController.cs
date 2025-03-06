/*----------BY017--------------------
.---------�����ഫ�t�ΥΩ�������⨭��
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
    public void ChangeIdentity()//��������
    {
        //TODO:if(������������F��)
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
