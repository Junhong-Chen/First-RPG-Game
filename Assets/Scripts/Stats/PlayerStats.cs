using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerStats : CharacterStats
{
    private Player player;

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
}
