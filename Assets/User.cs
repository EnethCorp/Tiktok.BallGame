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

    public void SpawnPlayer(GameObject PlayerPrefab, Transform PlayerSpawner)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-2.35f, 2.35f), 0f, 0f);
        PlayerController playerChild = Instantiate(PlayerPrefab, PlayerSpawner.position + randomOffset, PlayerSpawner.rotation).GetComponent<PlayerController>();
        playerChild.parent = this;
    }
    public void AddPoints(int amount)
    {
        this.Points += amount;
    }
}
