using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User : MonoBehaviour
{
    public User(string _Username, int _Points)
    {
        this.Username = _Username;
        this.Points = _Points;
    }

    [Header("REFERENCES")]
    [SerializeField] public string Username;
    [SerializeField] public int Points;
    [SerializeField] public float lastSpawnedTime;

    public IEnumerator SpawnPlayer(GameObject PlayerPrefab, Transform PlayerSpawner)
    {
        Texture texture = MaterialCreator.Instance.CreateTexture(this.Username);
        if (texture == null)
        {
            Debug.Log("Trying to find texture");
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SpawnPlayer(PlayerPrefab, PlayerSpawner));
            yield break;
        }

        //Texture2D texture = Resources.Load<Texture2D>(this.Username);
        //Debug.Log("Texture: " + texture + ", Username: " + this.Username);
        //while (texture == null)
        //{
        //    texture = Resources.Load<Texture2D>(this.Username);
        //    yield return new WaitForSeconds(0.05f);
        //}
        lastSpawnedTime = Time.time;
        Vector3 randomOffset = new Vector3(Random.Range(-3.35f, 3.35f), Random.Range(0f, 2f), 0f);
        PlayerController playerChild = Instantiate(PlayerPrefab, PlayerSpawner.position + randomOffset, PlayerSpawner.rotation).GetComponent<PlayerController>();
        playerChild.parent = this;
        //playerChild.SetMaterial();
        StartCoroutine(playerChild.SetMaterial());
        //StartCoroutine(playerChild.SetImage());
    }
    public void AddPoints(int amount)
    {
        this.Points += amount;
    }
    public void RemoveUser()
    {
        Destroy(gameObject);
    }
}
