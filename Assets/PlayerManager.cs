using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerManager : MonoBehaviour
{
    [Header("PLAYER")]
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private Transform PlayerSpawner;

    [Header("USER")]
    [SerializeField] private GameObject UserPrefab;
    [SerializeField] private List<User> UserList = new List<User>();

    [Header("LEADERBOARD")]
    [SerializeField] private TextMeshProUGUI[] LeaderBoardNames = new TextMeshProUGUI[3];
    [SerializeField] private TextMeshProUGUI[] LeaderBoardPoints = new TextMeshProUGUI[3];

    [Header("DEBUG")]
    [SerializeField] private int TotalPlayersSpawned = 0;
    [SerializeField] private string[] testUsers = new String[3];

    public static PlayerManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("More than one PlayerManager in Scene.");
        }
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SpawnPlayer(testUsers[Random.Range(0, testUsers.Length)]);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            QuickSort(UserList, 0, UserList.Count - 1);
            PrintList();
            UpdateLeaderBoard();
        }
    }

    public void UpdateLeaderBoard()
    {
        QuickSort(UserList, 0, UserList.Count - 1);

        for (int i = 0; i < LeaderBoardNames.Length; i++)
        {
            LeaderBoardNames[i].text = UserList[i].Username;
        }
        for (int i = 0; i < LeaderBoardPoints.Length; i++)
        {
            LeaderBoardPoints[i].text = UserList[i].Points.ToString();
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

        TotalPlayersSpawned++;
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

    public static void QuickSort(List<User> list, int start, int end)
    {
        if (end <= start) return; // base case

        int pivot = Partition(list, start, end);
        QuickSort(list, start, pivot - 1);
        QuickSort(list, pivot + 1, end);
    }
    private static int Partition(List<User> list, int start, int end)
    {
        User temp = null;
        int pivot = list.Count - 1;
        int i = start - 1;

        for (int j = start; j < end; j++)
        {
            if (list[j].Points > list[pivot].Points)
            {
                i++;
                temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        i++;
        temp = list[i];
        list[i] = list[end];
        list[end] = temp;

        return i;
    }
}

