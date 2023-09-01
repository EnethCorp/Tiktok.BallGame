using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public User parent;
    [SerializeField] private MaterialCreator _MaterialCreator;
    public bool destroyed = false;
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
    void Update()
    {
        if (!this.parent)
        {
            Destroy(gameObject);
        }   
    }

    public void AddPoints(int amount)
    {
        parent.AddPoints(amount);
    }

    public IEnumerator SetMaterial(Texture PlayerTexture)
    {
        Renderer renderer = GetComponent<Renderer>();
        //Texture texture = _MaterialCreator.CreateTexture(this.parent.Username);
        Texture texture = PlayerTexture;
        renderer.material.mainTexture = texture;

        yield return null;
    }
}
