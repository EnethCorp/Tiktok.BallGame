using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainScene : MonoBehaviour
{
    [SerializeField] private float delay = 1f;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("main");
    }
}
