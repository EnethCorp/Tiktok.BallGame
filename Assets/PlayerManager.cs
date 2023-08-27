using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("PLAYER")]
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private Transform PlayerSpawner;

    [Header("USER")]
    [SerializeField] private GameObject UserPrefab;
    [SerializeField] private List<User> UserList = new List<User>();

    [Header("DEBUG")]
    [SerializeField] private float SpawnTimer = TIMER_PRESET;
    private const float TIMER_PRESET = 2f;

    void Start()
    {
        this.CreatePlayer("dragojak");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnPlayer("TESTNAME");
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            PrintList();
        }

        SpawnTimer -= Time.deltaTime;

        if (SpawnTimer <= 0)
        {
            SpawnPlayer("TESTNAME");
            SpawnTimer = TIMER_PRESET;
        }
    }
    void SpawnPlayer(string _Username)
    {
        User user = null;

        if (!CheckExistence(_Username))
        {
            user = CreatePlayer(_Username);
        }
        else
        {
            user = GetPlayer(_Username);
        }

        user.SpawnPlayer(PlayerPrefab, PlayerSpawner);
    }


    /* USER */ 
    public User CreatePlayer(string _Username)
    {
        if (!CheckExistence(_Username))
        {
            User newUser = Instantiate(UserPrefab, transform.position, transform.rotation).GetComponent<User>();
            newUser.Username = _Username;

            UserList.Add(newUser);
            return newUser;
        }

        return null;
    }
    public User GetPlayer(string _Username)
    {
        foreach (User user in UserList)
        {
            if (user.Username == _Username)
            {
                return user;
            }
        }
        return null;
    }
    public void PrintList()
    {
        int loopNumber = 0;
        foreach (User user in UserList)
        {
            Debug.Log(loopNumber + ". Name: " + user.Username + ", Points: " + user.Points);
            loopNumber++;
        }
    }
    public bool CheckExistence(string _Username)
    {
        foreach (User user in UserList)
        {
            if (user.Username == _Username)
            {
                return true;
            }
        }
        return false;
    }
}

