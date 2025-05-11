using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                player.stats.DoDamage(enemyStats);

                Inventory.Instance.GetEquipmentByType(EquipmentType.Weapon)?.Effects(player, enemy); // ´¥·¢ÎäÆ÷ÌØÐ§
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
