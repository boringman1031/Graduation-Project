using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 10f;
    public RectTransform canvasRect;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 玩家使用鍵盤控制球拍
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        // 限制球拍範圍
        float minY = -canvasRect.rect.height / 2 + rectTransform.rect.height / 2;
        float maxY = canvasRect.rect.height / 2 - rectTransform.rect.height / 2;

        Vector3 newPosition = rectTransform.localPosition + new Vector3(0, moveY, 0);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        rectTransform.localPosition = newPosition;
    }
}
