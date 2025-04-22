using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Skill_Clone : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float stayDuration;
    [Space]
    [SerializeField] private bool canAttack;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashEnd;
    [SerializeField] private bool createCloneOnCounterAttack;

    [Header("Clone Duplicate")]
    [SerializeField] private bool createCloneDuplicate;
    [SerializeField] private float createDuplicateRatio = .33f;

    [Header("Crystal Instead Of Clone")]
    [SerializeField] public bool useCrystalInsteadOfClone;

    public void CreateClone(Transform clonePosition, Vector3 offset = default)
    {
        if (useCrystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject clone = Instantiate(clonePrefab);

        Transform closestEnemy = FindClosestEnemy(clonePosition);
        clone.GetComponent<Skill_Clone_Controller>().SetupClone(clonePosition, closestEnemy, stayDuration, canAttack, offset, createCloneDuplicate, createDuplicateRatio);
    }

    public void CreateCloneOnDashStart(Transform target)
    {
        if (createCloneOnDashStart)
            CreateClone(target);
    }

    public void CreateCloneOnDashEnd(Transform target)
    {
        if (createCloneOnDashEnd)
            CreateClone(target);
    }

    public void CreateCloneOnCounterAttack(Transform target)
    {

        if (createCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(target, new Vector3(player.facingDir, 0)));
    }

    private IEnumerator CreateCloneWithDelay(Transform pos, Vector3 offset)
    {
        yield return new WaitForSeconds(.3f);
        CreateClone(pos, offset);
    }
}
