using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeController : MonoBehaviour
{
    [SerializeField, Range(0f, 50f)] private int Points;

    public bool inAnimation = false;
    private int coroutinesNumber = 0;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerController playerController = collision.transform.GetComponent<PlayerController>();
            playerController.AddPoints(this.Points);

            StartCoroutine(CubeAnimation());

            Destroy(collision.gameObject);
        }
    }

    private IEnumerator CubeAnimation()
    {
        animator.SetBool("StartCubeAnimation", true);
        yield return null;
        animator.SetBool("StartCubeAnimation", false);
    }
}
