using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffect_Frozen", menuName = "Scriptable Objects/ItemEffect/ItemEffect_Frozen")]
public class ItemEffect_Frozen : ItemEffect
{
    [SerializeField] private float frozenDuration = 1f; // Duration of the frozen effect in seconds

    public override void ExecuteEffect(Entity from, Entity to)
    {
        Player player = from as Player;

        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, 2f, mask);

        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.FreezeTimeFor(frozenDuration);
                enemy.fx.FrozenFxFor(frozenDuration);
            }
        }
    }
}
