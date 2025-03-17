using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer Sp;

    public Sprite defaltImage;
    public Sprite PressImage;

    public KeyCode key;

    private void Start()
    {
        Sp = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Sp.sprite = PressImage;
        }
        if (Input.GetKeyUp(key))
        {
            Sp.sprite = defaltImage;
        }
    }

}
