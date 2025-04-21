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
        // AI �۰ʸ��H�y�� Y ��m
        float targetY = ball.localPosition.y;
        float newY = Mathf.MoveTowards(rectTransform.localPosition.y, targetY, speed * Time.deltaTime);

        // ���� AI �y�礣�W�X�d��
        float minY = -canvasRect.rect.height / 2 + rectTransform.rect.height / 2;
        float maxY = canvasRect.rect.height / 2 - rectTransform.rect.height / 2;
        newY = Mathf.Clamp(newY, minY, maxY);

        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, newY, 0);
    }
}
