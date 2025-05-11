using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffect", menuName = "Scriptable Objects/ItemEffect")]
public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Entity from, Entity to)
    {
        // Default implementation (if any)
        Debug.Log("Executing default item effect.");
    }
}
