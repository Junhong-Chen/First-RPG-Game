using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Crystal: Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration = 7f;

    private GameObject crystal;

    [Header("Mirage Crystal")]
    [SerializeField] private bool canMirror = false;

    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode = false;

    [Header("Moving Crystal")]
    [SerializeField] private bool canMoveToEnemy = false;
    [SerializeField] private float moveSpeed = 1f;

    [Header("Multi Stacking Crystals")]
    [SerializeField] private bool canMulti = false;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystals = new List<GameObject>();

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;

        if (crystal)
        {
            if (canMoveToEnemy)
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = crystal.transform.position; // Teleport the player to the crystal
            crystal.transform.position = playerPos; // Move the crystal to the player's position

            if (canMirror) // 在原地留下一个镜像，并传送到水晶位置，然后销毁水晶
            {
                SkillManager.instance.clone.CreateClone(crystal.transform);
                Destroy(crystal);
            }
            else
                crystal.GetComponent<Skill_Crystal_Controller>()?.Completed(); // Trigger the crystal's completion
        }
        else
        {
            CreateCrystal();
        }
    }

    public void CreateCrystal()
    {
        crystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Transform closestEnemy = FindClosestEnemy(crystal.transform);
        Skill_Crystal_Controller contrallerScript = crystal.GetComponent<Skill_Crystal_Controller>();
        contrallerScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, closestEnemy); // Set the duration of the crystal
    }

    public void ChooseEnemy(Transform enemy)
    {
        if (crystal)
        {
            crystal.GetComponent<Skill_Crystal_Controller>().ChooseEnemy(enemy);
        }
    }

    private bool CanUseMultiCrystal()
    {
        if (canMulti)
        {
            if (crystals.Count > 0)
            {
                if (crystals.Count == amountOfStacks) // First crystal usage
                    Invoke("ResetAbility", useTimeWindow); // Reset the ability after a certain time window

                cooldown = 0; // Reset cooldown for multi-crystal usage

                GameObject crystalToSpawn = crystals[crystals.Count - 1]; // Get the last crystal in the list
                GameObject crystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystals.Remove(crystalToSpawn); // Remove the crystal from the list

                crystal.GetComponent<Skill_Crystal_Controller>()?.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(crystal.transform)); // Set the crystal

                if (crystals.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefilCrystals(); // Refill the crystals if they are all used
                }

                return true;
            }
        }
        return false;
    }

    private void RefilCrystals()
    {
        while(crystals.Count < amountOfStacks)
        {
            crystals.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;
        RefilCrystals();
    }
}
