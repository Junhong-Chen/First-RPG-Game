using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "ItemEffect_Thunder", menuName = "Scriptable Objects/ItemEffect/ItemEffect_Thunder")]
public class ItemEffect_Thunder : ItemEffect
{
    [SerializeField] private GameObject thunderPrefab;

    public override void ExecuteEffect(Entity from, Entity to)
    {
        GameObject thunder = Instantiate(thunderPrefab, to.transform.position, Quaternion.identity);
        thunder.GetComponent<Skill_Thunder_Controller>().Setup(to.stats, from.stats.lightningDamage.GetValue());
    }
}
