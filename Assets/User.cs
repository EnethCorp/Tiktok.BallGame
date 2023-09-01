using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
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
    [SerializeField] public Texture UserTexture;

    private int textureAttempt;
    private void Update()
    {
        lastSpawnedTime += Time.deltaTime;
    }
    public IEnumerator SpawnPlayer(GameObject PlayerPrefab, Transform PlayerSpawner)
    {
        /* ENSURE THAT THERE ARE NOT TOO MANY PLAYERS*/ 
        if (GameManager.Instance.PlayerList.Count > GameManager.MAX_PLAYERS)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SpawnPlayer(PlayerPrefab, PlayerSpawner));
            yield break;
        }

        UserTexture = MaterialCreator.Instance.ImageLoader(this.Username);
        if (UserTexture == null && textureAttempt < 10)
        {
            //AssetDatabase.Refresh();
            Debug.Log("Trying to find texture, " + textureAttempt);
            textureAttempt++;
            yield return new WaitForSeconds(1f);
            StartCoroutine(SpawnPlayer(PlayerPrefab, PlayerSpawner));
            yield break;
        }

        lastSpawnedTime = 0f;
        Vector3 randomOffset = new Vector3(Random.Range(-3.35f, 3.35f), Random.Range(0f, 2f), 0f);
        PlayerController playerChild = Instantiate(PlayerPrefab, PlayerSpawner.position + randomOffset, PlayerSpawner.rotation).GetComponent<PlayerController>();
        GameManager.Instance.PlayerList.Add(playerChild);
        playerChild.parent = this;
        StartCoroutine(playerChild.SetMaterial(UserTexture));
    }
    public void AddPoints(int amount)
    {
        this.Points += amount;
    }
    public void RemoveUser()
    {
        DestroyImmediate(gameObject);
    }
}
