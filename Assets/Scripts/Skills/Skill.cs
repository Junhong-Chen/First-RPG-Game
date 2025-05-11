using System;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float _cooldown;
    public float cooldown { get => _cooldown; protected set => _cooldown = value; }

    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        return false;
    }

    public virtual void UseSkill()
    {
        // do something
    }

    public virtual Transform FindClosestEnemy(Transform check)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(check.position, 25);

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(check.position, collider.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = collider.transform;
                }
            }
        }

        return closestEnemy;
    }
}
