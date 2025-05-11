using UnityEngine;

public class Skill_Dash : Skill
{
    public bool dashUnlocked { get; private set; }
    private bool createCloneOnDashStart;
    private bool createCloneOnDashEnd;

    protected override void Start()
    {
        base.Start();

        SkillManager.instance.onUnlockSkill += UpdateStatus;
    }

    public override void UseSkill()
    {
        base.UseSkill();

        SkillManager.instance.onUseSkill.Invoke(SkillType.Dash);
    }

    private void UpdateStatus(int skillId, bool unlocked)
    {
        switch (skillId)
        {
            case 18:
                dashUnlocked = unlocked;
                break;
            case 19:
                createCloneOnDashStart = unlocked;
                break;
            case 20:
                createCloneOnDashEnd = unlocked;
                break;
        }
    }

    public void CreateCloneOnDashStart(Transform target)
    {
        if (createCloneOnDashStart)
            SkillManager.instance.clone.CreateClone(target);
    }

    public void CreateCloneOnDashEnd(Transform target)
    {
        if (createCloneOnDashEnd)
            SkillManager.instance.clone.CreateClone(target);
    }
}
