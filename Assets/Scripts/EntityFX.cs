using System.Collections;
using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration = 0.2f;
    private Material originalMat;

    [Header("Ailment Colors")]
    [SerializeField] private Color[] burnedColors;
    [SerializeField] private Color frozenColor;
    [SerializeField] private Color electrifiedColor;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    public void MakeTransparent(bool transparent)
    {
        if (transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color color = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.color = color;
        sr.material = originalMat;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        StopCoroutine("FrozenColorFx");
        sr.color = Color.white;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    public void BurnedFxFor(float seconds, float interval)
    {
        InvokeRepeating("BurnedColorFx", 0, interval);
        Invoke("CancelColorChange", seconds);
    }

    private void BurnedColorFx()
    {
        if (sr.color != burnedColors[0])
            sr.color = burnedColors[0];
        else
            sr.color = burnedColors[1];
    }

    public void FrozenFxFor(float seconds)
    {
        CancelInvoke("CancelColorChange");
        StartCoroutine(FrozenColorFx());
        Invoke("CancelColorChange", seconds);
    }

    private IEnumerator FrozenColorFx()
    {
        yield return new WaitForSeconds(flashDuration);
        sr.color = frozenColor;
    }

    public void ElectrifiedFxFor(float seconds)
    {
        ElectrifiedColorFx();
        Invoke("CancelColorChange", seconds);
    }

    private void ElectrifiedColorFx()
    {
        sr.color = electrifiedColor;
    }
}
