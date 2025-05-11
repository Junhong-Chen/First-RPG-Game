using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy enemy => GetComponentInParent<Skeleton>();
    private void AnimationTrigger()
    {
        enemy.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var collider in colliders)
        {
            Player player = collider.GetComponent<Player>();
            if (player != null)
            {
                enemy.stats.DoDamage(player.stats);
                //collider.GetComponent<Player>().Damage();
            }

        }
    }

    private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();
}
