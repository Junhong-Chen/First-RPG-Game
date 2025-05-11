using System.Collections;
using UnityEngine;

public class Skill_Clone : Skill
{
    [Header("Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float stayDuration;
    [SerializeField] private float attackDamageMultiplier;

    [Header("Attack")]
    [SerializeField] private bool cloneAttackUnlocked;
    [SerializeField] private float cloneAttackDamageMultiplier = .3f;

    [Header("Aggresive")]
    [SerializeField] public bool cloneAggressiveUnlocked { get; private set; }
    [SerializeField] private float cloneAggressiveMultiplier = .8f;

    [Header("Duplicate")]
    [SerializeField] private bool cloneDuplicateUnlocked;
    [SerializeField] private float cloneDuplicateDamageMultiplier = .3f;
    [SerializeField] private float duplicateRatio = .33f;

    [Header("Crystal Instead Of Clone")]
    [SerializeField] public bool crystalInsteadUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();

        SkillManager.instance.onUnlockSkill += UpdateStatus;
    }

    public void CreateClone(Transform clonePosition, Vector3 offset = default)
    {
        if (crystalInsteadUnlocked)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject clone = Instantiate(clonePrefab);

        Transform closestEnemy = FindClosestEnemy(clonePosition);
        clone.GetComponent<Skill_Clone_Controller>().SetupClone(clonePosition, closestEnemy, stayDuration, cloneAttackUnlocked, offset, cloneDuplicateUnlocked, duplicateRatio, attackDamageMultiplier);
    }

    public void CreateCloneWithDelay(Transform target)
    {
        StartCoroutine(CloneDelayCoroutine(target, new Vector3(player.facingDir, 0)));
    }

    private IEnumerator CloneDelayCoroutine(Transform pos, Vector3 offset)
    {
        yield return new WaitForSeconds(.3f);
        CreateClone(pos, offset);
    }

    private void UpdateStatus(int skillId, bool unlocked)
    {
        switch (skillId)
        {
            case 6:
                cloneAttackUnlocked = unlocked;
                if (unlocked) attackDamageMultiplier = cloneAttackDamageMultiplier;
                break;
            case 7:
                cloneAggressiveUnlocked = unlocked;
                if (unlocked) attackDamageMultiplier = cloneAggressiveMultiplier;
                break;
            case 8:
                cloneDuplicateUnlocked = unlocked;
                if (unlocked) attackDamageMultiplier = cloneDuplicateDamageMultiplier;
                break;
            case 9:
                crystalInsteadUnlocked = unlocked;
                break;
        }
    }
}
