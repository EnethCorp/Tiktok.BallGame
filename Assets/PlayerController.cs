using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public User parent;
    private Rigidbody _rb;
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void AddPoints(int amount)
    {
        parent.AddPoints(amount);
    }

    void FixedUpdate()
    {
        if (_rb.velocity.magnitude <= 0.1f)
        {
            _rb.velocity = new Vector3(Random.Range(-0.5f, 0.5f), 0f, 0f);
        }
    }
}
