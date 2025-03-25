using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Image fillImage;
    private float targetFill = 1f;
    public float smoothSpeed = 3f;

    void Update()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, Time.deltaTime * smoothSpeed);
        }
    }

    public void UpdateHealth(float current, float max)
    {
        targetFill = current / max;
    }
}
