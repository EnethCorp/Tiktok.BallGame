using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float delay = 7;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(delay); 
        Destroy(gameObject);
    }
}
