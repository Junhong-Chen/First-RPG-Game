using UnityEngine.UI;
using UnityEngine;

public class CustomGridLayoutGroup : GridLayoutGroup
{
    public override void SetLayoutHorizontal()
    {
        base.SetLayoutHorizontal();
        SkipSpecialChildren();
    }

    public override void SetLayoutVertical()
    {
        base.SetLayoutVertical();
        SkipSpecialChildren();
    }

    private void SkipSpecialChildren()
    {
        foreach (RectTransform child in transform)
        {
            if (child.CompareTag("IgnoreGrid"))
            {
                Debug.Log(child.CompareTag("IgnoreGrid"));
                child.anchoredPosition = child.anchoredPosition; // 可以自定义位置
            }
        }
    }
}
