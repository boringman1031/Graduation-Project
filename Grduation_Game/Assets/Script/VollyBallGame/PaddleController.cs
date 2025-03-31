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
        // ���a�ϥ���L����y��
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        // ����y��d��
        float minY = -canvasRect.rect.height / 2 + rectTransform.rect.height / 2;
        float maxY = canvasRect.rect.height / 2 - rectTransform.rect.height / 2;

        Vector3 newPosition = rectTransform.localPosition + new Vector3(0, moveY, 0);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        rectTransform.localPosition = newPosition;
    }
}
