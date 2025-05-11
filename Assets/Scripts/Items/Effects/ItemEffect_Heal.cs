using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffect_Heal", menuName = "Scriptable Objects/ItemEffect/ItemEffect_Heal")]
public class ItemEffect_Heal : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercentage;

    public override void ExecuteEffect(Entity from, Entity to)
    {
        CharacterStats targetStats = to.stats;

        targetStats.IncreaseHealth(Mathf.RoundToInt(targetStats.GetMaxHealth() * healPercentage));
    }
}
