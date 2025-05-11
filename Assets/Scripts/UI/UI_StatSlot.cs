using System.Collections;
using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour
{
    [SerializeField] private StatType statType; // ��������
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;

    private Stat stat;
    private string statName;

    private void OnValidate()
    {
        statName = statType.ToString(); // ��ȡ��������

        gameObject.name = "Stat - " + statName;
        statNameText.text = statName;
    }

    private void Start()
    {
         StartCoroutine(DelayedInit());
    }

    private IEnumerator DelayedInit()
    {
        yield return null; // �ȴ�һ֡

        CharacterStats playerStats = PlayerManager.instance.player.stats;
        stat = playerStats.GetStatByType(statType);

        stat.OnValueChanged += UpdateStatSlot; // ��������ֵ�仯�¼�

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
            stat.OnValueChanged -= UpdateStatSlot; // ȡ����������ֵ�仯�¼�
        }
    }
}
