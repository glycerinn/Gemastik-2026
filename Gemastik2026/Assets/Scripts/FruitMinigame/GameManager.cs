using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // private bool isGameOver = false;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
    }

    // void GameOver()
    // {
    //     isGameOver = true;

    //     Time.timeScale = 0f;
    // }
}