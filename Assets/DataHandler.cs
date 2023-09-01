
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    [Header("LISTENER")]
    Socket socket;
    public int port = 25001;
    System.Net.IPAddress adress = System.Net.IPAddress.Parse("127.0.0.1");

    [Header("DATA")]
    [SerializeField] private int MininumLikes = 25;

    public bool testing = false;
    public bool connected = false;
    bool success = false;

    [SerializeField] private string TestString = "{\"user\": \"enething\", \"event\": \"like\", \"count\": \"15\"}";
    
    Dictionary<string, string> data;
    GameManager gameManager;

    public static DataHandler Instance;


    public IEnumerator setupSocket()
    {                     
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(adress, port);

            Debug.Log("connected to server");
            var message = "setMinLikes " + MininumLikes.ToString();
            var messageBytes = Encoding.UTF8.GetBytes(message);

            socket.Send(messageBytes, SocketFlags.None);
            Debug.Log($"Socket client sent message: \"{message}\"");
            success = true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        if (success)
        {
            yield return new WaitForSeconds(3f);
            EndRound();
            connected = true;
        }

        if (!connected)
        {
            yield return new WaitForSeconds(5f);
            Debug.Log("Retrying to connect");
            StartCoroutine(setupSocket());
            Instance = this;
        }
    }

    void Start()
    {
        if (Instance)
        {
            Debug.LogError("More than one DataHandler in Scene.");
        }
        if (testing)
        {
            Debug.LogWarning("Testing in TikTokApi is activated.");
        }
        Instance = this;
        gameManager = GameManager.Instance;
        gameManager.SetMinimumLikes(MininumLikes);

        if (!testing)
            StartCoroutine(setupSocket());
    }

    public static Dictionary<string, string> ParseData(string dataString)
    {
        if (dataString == "")
        {
            Debug.LogError("DataString is empty");
            return null;
        }
        Dictionary<string, string> dict = new Dictionary<string, string>();
        Debug.Log(dataString);
        string tiktok_user = "user|" + dataString.Split("user\":")[1].Split(",")[0].Replace("\"", "");
        dict.Add(tiktok_user.Split("|")[0], tiktok_user.Split("|")[1].Replace(" ", ""));

    
        string tiktok_event = "event|" + dataString.Split("event\":")[1].Split(",")[0].Replace("\"", "");
        dict.Add(tiktok_event.Split("|")[0], tiktok_event.Split("|")[1].Replace(" ", ""));
        tiktok_event = tiktok_event.Split("|")[1].Replace(" ", "").Replace("}","");
 

        //Debug.Log(tiktok_event);
        if (tiktok_event == "join")
        {
            Debug.Log("Player joined");
        }
        else if (tiktok_event == "like")
        {
            string tiktok_args = "count|" + dataString.Split("count\":")[1].Split(",")[0].Replace("\"", "").Replace("}", "");
            dict.Add(tiktok_args.Split("|")[0], tiktok_args.Split("|")[1].Replace(" ", ""));
        }
        else if (tiktok_event == "comment")
        {
            string tiktok_comment = "comment|" + dataString.Split("comment\":")[1].Split(",")[0].Replace("\"", "").Replace("}", "");
            dict.Add(tiktok_comment.Split("|")[0], tiktok_comment.Split("|")[1].Replace(" ", ""));
            Debug.Log("This is the comment: \"" + tiktok_comment.Split("|")[1].Replace(" ", "") + "}\"");
        }
        else if (tiktok_event == "gift")
        {
            string tiktok_gift = "gift|" + dataString.Split("gift\":")[1].Split(",")[0].Replace("\"", "").Replace("}", "");
            dict.Add(tiktok_gift.Split("|")[0], tiktok_gift.Split("|")[1].Replace(" ", ""));

            string tiktok_count = "count|1";
            try
            {
                tiktok_count = "count|" + dataString.Split("count\":")[1].Split(",")[0].Replace("\"", "").Replace("}", "").Replace(" ", "");
            }
            catch (Exception e)
            {
                Debug.LogError("Gift has no valid count.");
            }
            dict.Add(tiktok_count.Split("|")[0], tiktok_count.Split("|")[1].Replace(" ", ""));
        }
        else if (tiktok_event == "follow")
        {
            dict.Add("count", "3");
        }


        //Debug.Log("User: " + dict["user"] + ", Event: " + dict["event"]);
        return dict;
    }
    void Update()
    {
        if (testing && Input.GetKeyDown(KeyCode.X))
        {
            data = ParseData(TestString);
            gameManager.PassData(ref data);
        }

        if (connected)
        {
            string response = GetEvent();
            while (response != "" && response != "None")
            {
                Debug.Log(response);
                data = ParseData(response);
                gameManager.PassData(ref data);
                response = GetEvent();
            }
        }

        //data.Clear();
    }

    string GetEvent()
    {
        var message = "getEvent";
        var messageBytes = Encoding.UTF8.GetBytes(message);

        socket.Send(messageBytes, SocketFlags.None);

        byte[] buffer = new byte[4096];
        int bytesRead = socket.Receive(buffer, buffer.Length, 0);
        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        if (response != "None")
        {
            Debug.Log(response);
            return response;
        }
        else return "";
    }

    public void EndRound()
    {
        var message = "endRound";
        var messageBytes = Encoding.UTF8.GetBytes(message);

        socket.Send(messageBytes, SocketFlags.None);
    }

    private void OnApplicationQuit()
    {
        if (connected)
        {
            Debug.Log("Closing socket");
            socket.Disconnect(reuseSocket:false);   
        }
    }
}
