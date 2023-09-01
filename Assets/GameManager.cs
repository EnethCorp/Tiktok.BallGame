using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("PLAYER")]
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private Transform PlayerSpawner;

    [Header("USER")]
    [SerializeField] private GameObject UserPrefab;
    [SerializeField] public List<User> UserList = new List<User>();
    [SerializeField] public List<PlayerController> PlayerList = new List<PlayerController>();
    [SerializeField] public List<Winner> WinnerList = new List<Winner>();

    [Header("LEADERBOARD")]
    [SerializeField] private TextMeshProUGUI[] LeaderBoardNames = new TextMeshProUGUI[3];
    [SerializeField] private TextMeshProUGUI[] LeaderBoardPoints = new TextMeshProUGUI[3];
    [SerializeField] private TextMeshProUGUI[] WinnerTexts = new TextMeshProUGUI[2];


    [Header("DEBUG")]
    [SerializeField] private int TotalPlayersSpawned = 0;
    [SerializeField] private float SpawnDelay = 2f;
    [SerializeField] public float RoundTimer = ROUND;
    [SerializeField] public bool RoundEnded = false;
    [SerializeField] private string[] testUsers = new String[3];
    [SerializeField] private int MininumLikes = 1;
    [SerializeField] private bool testing = false;
    [SerializeField] public int CurrentPlayers = 0;

    [HideInInspector] public bool ResetProfilePictures;
    public const float ROUND = 120;
    public const float MAX_PLAYERS = 1000;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("More than one PlayerManager in Scene.");
        }
        if (testing)
        {
            Time.timeScale = 20f;
            Debug.LogWarning("Testing in GameManager is activated.");
        }
        Instance = this;
        RoundTimer = ROUND;
    }
    void Update()
    {
        RoundTimer -= Time.deltaTime;

        if (RoundTimer <= 0 && !RoundEnded)
        {
            ResetRound();
            return;
        }
        if (RoundEnded && RoundTimer > 5f)
        {
            RoundEnded = false;
        }

        if (testing && Time.frameCount % 15 == 0)
        {
            SpawnUser(testUsers[Random.Range(0, testUsers.Length)], 1);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < 3000; i++)
            {
                 SpawnUser(testUsers[Random.Range(0, testUsers.Length)], 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            RemoveOldUsers();
            QuickSort(UserList, 0, UserList.Count - 1);
            PrintList();
            UpdateLeaderBoard();
        }

        if (Time.frameCount % 500 == 0)
        {
            RemoveEmptyPlayers();
        }
    }
    /* Methods */
    private void ResetRound()
    {
        Debug.Log("Round has ended - Restarting");
        if (!testing)
        {
            //DataHandler.Instance.EndRound();
        }
        if (UserList.Count > 0)
        {
            AddWinner(UserList[0].Username);
            PrintWinners();
            WinnerTexts[0].text = WinnerList[0].Username;
            WinnerTexts[1].text = WinnerList[0].Wins.ToString();
        }
        ResetProfilePictures = true;
        ForceRemoveUsers();
        RemoveEmptyPlayers();
        ResetLeaderBoard();
        RoundTimer = ROUND;
        RoundEnded = true;
    }
    public void UpdateLeaderBoard()
    {
        QuickSort(UserList, 0, UserList.Count - 1);

        for (int i = 0; i < Mathf.Clamp(UserList.Count, 0, 3); i++)
        {
            LeaderBoardNames[i].text = UserList[i].Username;
        }
        for (int i = 0; i < Mathf.Clamp(UserList.Count, 0, 3); i++)
        {
            LeaderBoardPoints[i].text = UserList[i].Points.ToString();
        }
    }
    public void ResetLeaderBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            LeaderBoardNames[i].text = "Username";
            LeaderBoardPoints[i].text = 0.ToString();
        }
    }
    public void SpawnUser(string _Username, int amount)
    {
        User user = null;

        if (!CheckPlayerExistence(_Username))
        {
            user = CreateUser(_Username);
        }
        else
        {
            user = GetUser(_Username);
        }

        TotalPlayersSpawned += amount;
        for (int i = 0; i < amount; i++)
        {
             StartCoroutine(DelaySpawnPlayer(user));
        }
    }



    /* USER */ 
    public User CreateUser(string _Username)
    {
        if (!CheckPlayerExistence(_Username))
        {
            User newUser = Instantiate(UserPrefab, transform.position, transform.rotation).GetComponent<User>();
            newUser.Username = _Username;

            UserList.Add(newUser);
            return newUser;
        }

        return GetUser(_Username);
    }
    public User GetUser(string _Username)
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
    public bool CheckPlayerExistence(string _Username)
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
    private IEnumerator DelaySpawnPlayer(User user)
    {
        yield return new WaitForSeconds(SpawnDelay);
        StartCoroutine(user.SpawnPlayer(PlayerPrefab, PlayerSpawner));
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
        int pivot = end;
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
    void RemoveOldUsers()
    {
        for (int i = UserList.Count; i > 0; i--)
        {
            if (UserList[i].lastSpawnedTime > 60f)
            {
                UserList[i].RemoveUser();
            }
            if (UserList[i] == null) UserList.Remove(UserList[i]);
        }
    }
    void RemoveEmptyPlayers()
    {
        for (int i = PlayerList.Count-1; i > 0; i--)
        {
            if (PlayerList[i] == null) PlayerList.Remove(PlayerList[i]);
        }
    }
    void ForceRemoveUsers()
    {
        for (int i = UserList.Count - 1; i >= 0; i--)
        {
            Debug.Log(UserList[i].name);
            if (UserList[i])
            {
                UserList[i].RemoveUser();
            }
            UserList.Remove(UserList[i]);
        }
    }



    /* TikTok API */
    public void PassData(ref Dictionary<string, string> Data)
    {
        string _username = Data["user"];
        string _event = Data["event"];

        //int likeCount = int.Parse(_args);
        int amount = 0;

        User user;
        if (!CheckPlayerExistence(_username))
        {
            user = CreateUser(_username);
        } else
        {
            user = GetUser(_username);
        }

        _event = _event.Replace("}", "");
        Debug.Log(_event);

        if (_event.Contains("join"))
        {
            Debug.Log("Player joined");
            amount = 1;
        }
        else if (_event == "like") // && likeCount >= 25)
        {
            int likeCount = int.Parse(Data["count"]);
            if (likeCount < MininumLikes)
                return;

            amount = (int) (1 * (likeCount / MininumLikes));
        }
        else if (_event == "comment")
        {
            string comment = Data["comment"];
            amount = 3;
        }
        else if (_event == "follow")
        {
            amount = 5 * int.Parse(Data["count"]);
        }
        else if (_event == "gift")
        {
            string gift = Data["gift"].ToLower();
            int giftAmount = 1;
            try
            {
                giftAmount = int.Parse(Data["count"]);
            }
            catch (Exception e)
            {
                Debug.LogError("No valid count for gift");
            }

            Debug.Log("Gift: " + gift);

            if (gift == "rose")
            {
                amount = 10 * giftAmount;
            }
            else if (gift == "iloveyou")
            {
                amount = 500 * giftAmount;
            }
            else if (gift == "cap")
            {
                amount = 1000 * giftAmount;
            }
            else if (gift == "hearts")
            {
                amount = 2000 * giftAmount;
            }
        }

        // Spawn the right amount of players
        TotalPlayersSpawned += amount;
        for (int i = 0; i < amount; i++)
        { 
            StartCoroutine(DelaySpawnPlayer(user));
        }
    }
    public void SetMinimumLikes(int _MininumLikes)
    {
        MininumLikes = _MininumLikes;
    }



    /* Winner */
    public bool CheckWinnerExistence(string _Username)
    {
        foreach (Winner winner in WinnerList)
        {
            if (winner.Username == _Username)
            {
                return true;
            }
        }
        return false;
    }
    public void AddWinner(string _Username)
    {
        if (!CheckWinnerExistence(_Username))
        {
            WinnerList.Add(new Winner(_Username));
        }
        else
        {
            Winner winner = GetWinner(_Username);
            winner.addWin();
        }

        QuickSortWinners(WinnerList, 0, WinnerList.Count-1);
    }
    public Winner GetWinner(string _Username)
    {
        foreach (Winner winner in WinnerList)
        {
            if (winner.Username == _Username)
            {
                return winner;
            }
        }
        return null;
    }
    public static void QuickSortWinners(List<Winner> list, int start, int end)
    {
        if (end <= start) return; // base case

        int pivot = PartitionWinners(list, start, end);
        QuickSortWinners(list, start, pivot - 1);
        QuickSortWinners(list, pivot + 1, end);
    }
    private static int PartitionWinners(List<Winner> list, int start, int end)
    {
        Winner temp = null;
        int pivot = end;
        int i = start - 1;

        for (int j = start; j < end; j++)
        {
            if (list[j].Wins > list[pivot].Wins)
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
    private void PrintWinners()
    {
        int loopNumber = 0;
        foreach (Winner winner in WinnerList)
        {
            Debug.Log(loopNumber + ". Winner:" + winner.Username + ", Wins: " + winner.Wins);
            loopNumber++;
        }
    }
}

