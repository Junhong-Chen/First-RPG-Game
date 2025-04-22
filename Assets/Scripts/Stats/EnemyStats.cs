using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
    }

    protected override void HandleElectrifiedEffect()
    {
        if (isElectrified)
        {
            enemy.ResetAction();
        }

        base.HandleElectrifiedEffect();
    }
}
