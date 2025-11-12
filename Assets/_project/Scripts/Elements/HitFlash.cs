using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    public float flashDuration;

    public List<SkinnedMeshRenderer> smrs = new List<SkinnedMeshRenderer>();
    public Material flashMaterial;

    private List<Material> originalMaterials = new List<Material>();

    private Coroutine _hitFlashCoroutine;

    private void Start()
    {
        foreach (var s in smrs)
        {
            originalMaterials.Add(s.material);
        }
    }

    public void PlayHitFlash()
    {
        if (_hitFlashCoroutine != null)
        {
            StopCoroutine(_hitFlashCoroutine);
        }
        _hitFlashCoroutine = StartCoroutine(HitFlashCoroutine());
    }

    IEnumerator HitFlashCoroutine()
    {
        ChangeColorToFlash();
        yield return new WaitForSeconds(flashDuration);
        ChangeColorToNormal();
    }

    private void ChangeColorToNormal()
    {
        for (int i = 0; i < smrs.Count; i++)
        {
            smrs[i].material = originalMaterials[i];
        }
    }

    private void ChangeColorToFlash()
    {
        foreach (var s in smrs)
        {
            s.material = flashMaterial;
        }
    }
}
