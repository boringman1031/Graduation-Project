/*----------BY017--------------------
.---------�����ഫ�t�ΥΩ�������⨭��
-----------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using static UnityEngine.InputSystem.InputSettings;

public class IdentityController : MonoBehaviour
{
    [Header("�ƥ��ť")]
    public VoidEventSO afterSceneLoadedEvent;

    public List<SpriteResolver> spriteResolvers = new List<SpriteResolver>();
    public GameObject item;
    SpriteResolver BodyResolver;
    private SpriteSkin spriteSkin; // ���⪺ SpriteSkin�]���[����^
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
        spriteResolvers.Clear(); // �M���ª�
        foreach (var resolver in GetComponentsInChildren<SpriteResolver>())
        {
            spriteResolvers.Add(resolver);
            if (resolver.GetCategory() == "Body") // ��� `Body` ������
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
            spriteSkin.autoRebind = true; // **�j�� Rebind �ץ����[����**         
        }
        else
        {
            Debug.LogWarning("SpriteSkin �����A�нT�{����O�_�����[�ʵe�I");
        }
    }
    public void ChangeIdentity()//��������
    {
        //TODO:if(������������F��)
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
