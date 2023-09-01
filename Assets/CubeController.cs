using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeController : MonoBehaviour
{
    [SerializeField, Range(0f, 50f)] private int Points;
    [SerializeField] private float totalCollisions;
    [SerializeField] private bool multiply;

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
            totalCollisions++;
            try
            {
                PlayerController playerController = collision.transform.GetComponent<PlayerController>();
               
                GameManager.Instance.PlayerList.Remove(playerController);

                if (multiply)
                    playerController.AddPoints(playerController.parent.Points * this.Points);
                else
                    playerController.AddPoints(this.Points);

                StartCoroutine(CubeAnimation());
            }
            catch (Exception e) 
            {
                Debug.LogWarning("Catched Exception" + e);
            }
            finally
            {
                GameManager.Instance.UpdateLeaderBoard();
                Destroy(collision.gameObject);
            }
        }
    }

    private IEnumerator CubeAnimation()
    {
        animator.SetBool("StartCubeAnimation", true);
        yield return null;
        animator.SetBool("StartCubeAnimation", false);
    }
}
