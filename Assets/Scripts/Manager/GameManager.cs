using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public BatManager[] batManagers;
    public GameObject panelWin;
    
    private static GameManager _gameManager;
    public static GameManager Instance => _gameManager;

    private int _numberOfBatToCorrupt;
    
    private void Awake()
    {
        _gameManager = this;
    }

    public void CheckIfBatAreCorrupted()
    {
        for (int i = 0; i < batManagers.Length; i++)
        {
            if (batManagers[i].batCorrupted)
            {
                _numberOfBatToCorrupt++;

                if (_numberOfBatToCorrupt == batManagers.Length)
                {
                    Win();
                }
            }
        }

        _numberOfBatToCorrupt = 0;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    
    private void Win()
    {
        panelWin.SetActive(true);
    }
}
