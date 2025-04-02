using UnityEngine;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour
{
    public static TooltipController Instance;

    public Text tooltipText; // ¨Ï¥Î Legacy Text
    public RectTransform background;
    public Vector2 padding = new Vector2(8f, 8f);

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out pos);
            transform.localPosition = pos;
        }
    }

    public void Show(string content)
    {
        gameObject.SetActive(true);
        tooltipText.text = content;
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipText.rectTransform);
        background.sizeDelta = tooltipText.rectTransform.sizeDelta + padding;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}