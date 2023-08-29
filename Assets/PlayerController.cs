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

    public IEnumerator SetMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();
        Texture texture = _MaterialCreator.CreateTexture(this.parent.Username);
        renderer.material.mainTexture = texture;

        //Texture2D texture = null;
        //Debug.Log(this.parent.Username);
        //TextAsset imageAsset = Resources.Load<TextAsset>(this.parent.Username);
        //Debug.Log(imageAsset);
        //ImageConversion.LoadImage(texture, imageAsset.bytes);
        //GetComponent<Renderer>().material.mainTexture = texture;

        //Debug.Log("Texture is " + texture);
        //Debug.Log("Running Coroutine for " + parent.Username);
        //Debug.Log("Able to convert: " + ImageConversion.LoadImage(texture, imageAsset.bytes));

        yield return null;
        //if (texture == null)
        //{
        //    StartCoroutine(this.SetMaterial());
        //    Debug.Log("Running Coroutine for " + parent.Username);
        //}
    }
}
