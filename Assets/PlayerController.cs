using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public User parent;

    public void AddPoints(int amount)
    {
        parent.AddPoints(amount);
    }
}
