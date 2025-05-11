using UnityEngine;

public class Skill_Dodge : Skill
{
    public bool dodgeUnlocked {  get; private set; }
    private bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        SkillManager.instance.onUnlockSkill += UpdateStatus;
    }

    private void UpdateStatus(int skillId, bool unlocked)
    {
        switch (skillId)
        {
            case 16:
                dodgeUnlocked = unlocked;

                if (dodgeUnlocked)
                    player.stats.evasion.ModifierAdd(10);
                else
                    player.stats.evasion.ModifierRemove(10);

                break;
            case 17:
                dodgeMirageUnlocked = unlocked;
                break;
        }
    }

    public void MakeMirageOnDodge(Transform target)
    {
        if (dodgeMirageUnlocked)
        {
            SkillManager.instance.clone.CreateClone(target);
        }
    }
}
