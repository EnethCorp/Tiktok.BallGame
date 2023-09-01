using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTransition : MonoBehaviour
{
    [SerializeField] public Gradient gradient;
    [SerializeField] public Gradient gradient2;
    [SerializeField] public Material material;

    void Update()
    {
        if (gradient == null || material == null)
        {
            Debug.LogError("Gradient or Material is zero on TransitionColor Object.");
            return;
        }

        material.color = gradient2.Evaluate((Time.time / 45) % 1);
    }
}