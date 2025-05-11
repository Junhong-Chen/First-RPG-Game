using System.Collections;
using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour
{
    [SerializeField] private StatType statType; // 属性类型
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;

    private Stat stat;
    private string statName;

    private void OnValidate()
    {
        statName = statType.ToString(); // 获取属性名称

        gameObject.name = "Stat - " + statName;
        statNameText.text = statName;
    }

    private void Start()
    {
         StartCoroutine(DelayedInit());
    }

    private IEnumerator DelayedInit()
    {
        yield return null; // 等待一帧

        CharacterStats playerStats = PlayerManager.instance.player.stats;
        stat = playerStats.GetStatByType(statType);

        stat.OnValueChanged += UpdateStatSlot; // 订阅属性值变化事件

        UpdateStatSlot();
    }

    public void UpdateStatSlot()
    {
        statValueText.text = stat.GetValue().ToString();
    }

    private void OnDestroy()
    {
        if (stat != null)
        {
            stat.OnValueChanged -= UpdateStatSlot; // 取消订阅属性值变化事件
        }
    }
}
