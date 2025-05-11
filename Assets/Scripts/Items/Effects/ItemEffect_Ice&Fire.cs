using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffect_Ice&Fire", menuName = "Scriptable Objects/ItemEffect/ItemEffect_Ice&Fire")]
public class ItemEffect_IceAndFire : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;

    public override void ExecuteEffect(Entity from, Entity to)
    {
        Player player = from as Player;
        bool isActive = player.primaryAttackState.comboCounter == 2;

        if (isActive)
        {
            GameObject iceAndFire = Instantiate(iceAndFirePrefab, to.transform.position, player.transform.rotation);
            iceAndFire.GetComponent<Skill_IceAndFire_Controller>().Setup(player);
        }
    }
}
