using UnityEngine;

public class UI_Tooltip : MonoBehaviour
{
    private RectTransform canvasRectTransform;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Canvas canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
            canvasRectTransform = canvas.GetComponent<RectTransform>();

        Hide();
    }

    protected void Update()
    {
        FollowMouse();
    }

    public virtual void Show(string[] texts)
    {
        FollowMouse();

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void FollowMouse()
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            Input.mousePosition,
            null,
            out anchoredPosition
        );

        Vector2 tooltipSize = rectTransform.sizeDelta;
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

        Vector2 offset = new Vector2(10, (10 + tooltipSize.y));
        anchoredPosition += offset;

        //// ±ß½ç¼ì²â£¨·ÀÖ¹³ö½ç£©
        float rightLimit = canvasSize.x / 2f - tooltipSize.x;
        float leftLimit = -canvasSize.x / 2f;
        float topLimit = canvasSize.y / 2f;
        float bottomLimit = -canvasSize.y / 2f + tooltipSize.y;

        anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, leftLimit, rightLimit);
        anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, bottomLimit, topLimit);

        rectTransform.anchoredPosition = anchoredPosition;
    }
}
