using UnityEngine;

public class Skill_Blackhole : Skill
{
    [SerializeField] private GameObject blackholePrefab;
    [Space]
    [SerializeField] private float duration = 4; // 技能持续时间
    [SerializeField] private float maxSize = 15;
    [SerializeField] private float growSpeed = 3;
    [SerializeField] private float shrinkSpeed = 6; // 收缩速度
    [SerializeField] private int amountOfAttacks = 4; // 允许的攻击次数
    [SerializeField] private float attackCooldown = 0.3f; // 攻击间隔

    private Skill_Blackhole_Controller blackholeController;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        // Instantiate the blackhole prefab at the player's position
        GameObject blackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);

        // Get ScriptableObject from the blackhole prefab
        blackholeController = blackhole.GetComponent<Skill_Blackhole_Controller>();

        // Setup the blackhole with parameters
        blackholeController.SetupBlackhole(
            maxSize,
            growSpeed,
            shrinkSpeed,
            amountOfAttacks,
            attackCooldown,
            duration
        );
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if (blackholeController && blackholeController.canExit)
        {
            blackholeController = null;
            return true;
        }

        return false;
    }
}
