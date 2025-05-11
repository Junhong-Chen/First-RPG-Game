using UnityEngine;

public class Skill_Parry : Skill
{
    public bool parryUnlocked { get; private set; } = false;
    public bool parryWithRestoreUnlocked { get; private set; } = false;
    public bool parryWithMirageUnlocked { get; private set; } = false;

    [Range(0, 1)]
    [SerializeField] private float restorePrecent = .05f;

    protected override void Start()
    {
        base.Start();

        SkillManager.instance.onUnlockSkill += UpdateStatus;
    }

    public override void UseSkill()
    {
        base.UseSkill();

        // 弹反成功后恢复生命值
        RestoreHealth();

        SkillManager.instance.onUseSkill.Invoke(SkillType.Parry);
    }

    private void UpdateStatus(int skillId, bool unlocked)
    {
        switch (skillId)
        {
            case 21:
                parryUnlocked = unlocked;
                break;
            case 22:
                parryWithRestoreUnlocked = unlocked;
                break;
            case 23:
                parryWithMirageUnlocked = unlocked;
                break;
        }
    }
    private void RestoreHealth()
    {
        if (parryWithRestoreUnlocked)
        {
            int maxHealth = player.stats.GetMaxHealth();
            player.stats.IncreaseHealth(Mathf.RoundToInt(maxHealth * restorePrecent));
        }
    }

    public void MakeMirageOnParry(Transform target)
    {
        if (parryWithMirageUnlocked)
        {
            SkillManager.instance.clone.CreateCloneWithDelay(target);
        }
    }
}
