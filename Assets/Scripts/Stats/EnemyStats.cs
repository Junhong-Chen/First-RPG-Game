using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    [Header("Level")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float levelUpRate = 0.3f;

    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
    }
    public override void Die()
    {
        base.Die();
    }

    protected override void HandleElectrifiedEffect()
    {
        if (isElectrified)
        {
            enemy.ResetAction();
        }

        base.HandleElectrifiedEffect();
    }

    private void ApplyLevelModifiers()
    {
        Modify(damage);

        Modify(maxHealth);
        Modify(armor);
    }

    private void Modify(Stat stat)
    {
        for (int i = 1; i < level; i++)
        {
            stat.ModifierAdd(Mathf.FloorToInt(stat.GetValue() * levelUpRate));
        }
    }
}
