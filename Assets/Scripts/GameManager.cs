using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action StartGame;

    public void OnPressedStart()
    {
        StartGame?.Invoke();
    }
}