using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private GameManager gameManager;
    void Start()
    {
        timer = GetComponent<TextMeshProUGUI>();
        gameManager = GameManager.Instance;
    }
    void Update()
    {
        string minute = Mathf.FloorToInt(Mathf.Clamp((gameManager.RoundTimer / 60), 0f, Mathf.Infinity)).ToString();
        string seconds = (gameManager.RoundTimer % 60 < 10) ? "0" + Mathf.FloorToInt(Mathf.Clamp((gameManager.RoundTimer % 60), 0f, Mathf.Infinity)).ToString(): Mathf.FloorToInt(Mathf.Clamp((gameManager.RoundTimer % 60), 0f, Mathf.Infinity)).ToString();
        timer.text = minute + ":" + seconds;
    }
}
