using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPaddle : MonoBehaviour
{
    public RectTransform ball;
    public float speed = 5f;
    public RectTransform canvasRect;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // AI 自動跟隨球的 Y 位置
        float targetY = ball.localPosition.y;
        float newY = Mathf.MoveTowards(rectTransform.localPosition.y, targetY, speed * Time.deltaTime);

        // 限制 AI 球拍不超出範圍
        float minY = -canvasRect.rect.height / 2 + rectTransform.rect.height / 2;
        float maxY = canvasRect.rect.height / 2 - rectTransform.rect.height / 2;
        newY = Mathf.Clamp(newY, minY, maxY);

        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, newY, 0);
    }
}
