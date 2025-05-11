using TMPro;
using UnityEngine;

public class UI_Tooltip_Skill : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI content;

    public override void Show(string[] texts)
    {
        title.text = "";
        content.text = "";

        if (texts.Length > 0)
            title.text = texts[0];

        if (texts.Length > 1)
            content.text = texts[1];

        base.Show(texts);
    }
}
