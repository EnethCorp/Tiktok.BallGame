using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTransition : MonoBehaviour
{
    [SerializeField] public Gradient gradient1;
    [SerializeField] public Gradient gradient2;
    [SerializeField] public Material material;

    private bool ColorMode = false;

    void Start()
    {
        if (gradient1 == null || gradient2 == null || material == null)
        {
            Debug.LogError("Gradient or Material is zero on TransitionColor Object.");
            return;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Debug.Log("Switching Color mode");
            ColorMode = !ColorMode;
        }
        Gradient gradient = (!ColorMode) ? gradient1 : gradient2;

        material.color = gradient.Evaluate((Time.time / 45) % 1);
    }
}