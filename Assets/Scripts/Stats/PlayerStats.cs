using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerStats : CharacterStats
{
    private Player player;

    [Header("Currency")]
    public Stat soul = new(); // 灵魂，用来学习技能

    protected override void Start()
    {
        base.Start();

        player = PlayerManager.instance.player;
    }

    public override void Die()
    {
        base.Die();
    }

    protected override void HandleElectrifiedEffect()
    {
        if (isElectrified)
        {
            player.stateMachine.ChangeState(player.idleState);
        }

        base.HandleElectrifiedEffect();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        Inventory.Instance.UseArmor();
    }

    protected override void OnEvasion()
    {
        SkillManager.instance.dodge.MakeMirageOnDodge(player.transform);
    }
}
