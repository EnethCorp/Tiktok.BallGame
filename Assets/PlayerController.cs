using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public User parent;
    [SerializeField] private MaterialCreator _MaterialCreator;
    private Rigidbody _rb;
 
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _MaterialCreator = MaterialCreator.Instance;
    }
    void FixedUpdate()
    {
        if (_rb.velocity.magnitude <= 0.1f)
        {
            _rb.velocity = new Vector3(Random.Range(-0.5f, 0.5f), 0f, 0f);
        }
    }
    public void AddPoints(int amount)
    {
        parent.AddPoints(amount);
    }

    public void SetMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = _MaterialCreator.CreateTexture(this.parent.Username);
        //renderer.material = _MaterialCreator.CreateMaterial(this.parent.Username);
    }
}
