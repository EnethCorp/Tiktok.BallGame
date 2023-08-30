using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winner 
{
    public string Username = "";
    public int Wins = 1;

    public Winner(string _Username)
    {
        Username = _Username;
    }
    public void addWin()
    {
        this.Wins++;
    }
}
