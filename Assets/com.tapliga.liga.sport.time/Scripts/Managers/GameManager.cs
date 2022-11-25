using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get => FindObjectOfType<GameManager>(); }
    public static Action OnGameStarted { get; set; } = delegate { };
    public static Action OnGameFinished { get; set; } = delegate { };
}