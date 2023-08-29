using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;
    void Start()
    {
        timer = GetComponent<TextMeshProUGUI>(); 
    }
    void Update()
    {
        string minute = Mathf.FloorToInt(GameManager.Instance.RoundTimer / 60).ToString();
        string seconds = (GameManager.Instance.RoundTimer % 60 < 10) ? "0" + Mathf.FloorToInt(GameManager.Instance.RoundTimer % 60).ToString(): Mathf.FloorToInt(GameManager.Instance.RoundTimer % 60).ToString();
        timer.text = minute + ":" + seconds;
    }
}
