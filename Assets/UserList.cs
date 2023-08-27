using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class UserList : MonoBehaviour
{
    [SerializeField] private GameObject UserPrefab;
    [SerializeField] private List<User> list = new List<User>();

    public User CreatePlayer(string _Username)
    {
        if (!CheckExistence(_Username))
        {
            GameObject obj = Instantiate(UserPrefab, transform.position, transform.rotation);
            Debug.Log(obj);
            User newUser = obj.GetComponent<User>();
            Debug.Log(newUser.name);

            list.Add(newUser);
            return newUser;
        }

        return null;
    }


    // Uninteresting Methods
    public User GetPlayer(string _Username)
    {
        foreach (User player in list)
        {
            if (player.name == _Username)
            {
                return player;
            }
        }
        return null;
    }
    public void PrintList()
    {
        int loopNumber = 0;
        foreach (User user in list)
        {
            Debug.Log(loopNumber + ". Name: " + user.Username + ", Points: " + user.Points);
            loopNumber++;
        }
    }
    public bool CheckExistence(string _Username)
    {
        foreach(User player in list)
        {
            Debug.Log("Checking name: " + player.name + " with comparison " + _Username);
            if (player.name == _Username)
            {
                return true;
            }
        }
        return false;
    }
    public List<User> GetList()
    {
        return list;
    }
}
