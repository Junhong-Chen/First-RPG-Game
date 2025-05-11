using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    public event Action OnValueChanged;

    [SerializeField] private int value;

    public List<int> modifiers = new List<int>();

    public int GetValue()
    {
        int finalValue = value;

        foreach (var modifier in modifiers)
            finalValue += modifier;

        return finalValue;
    }

    public void SetValue(int newValue)
    {
        value = newValue;
        OnValueChanged?.Invoke();
    }

    public void ModifierAdd(int modifier)
    {
        modifiers.Add(modifier);
        OnValueChanged?.Invoke();
    }

    public void ModifierRemove(int modifier)
    {
        modifiers.Remove(modifier);
        OnValueChanged?.Invoke();
    }
}
