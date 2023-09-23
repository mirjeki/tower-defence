using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;
    
    int currentBalance;

    UIHandler uiHandler;

    private void Start()
    {
        uiHandler = GetComponent<UIHandler>();
        uiHandler.UpdateGoldDisplay(currentBalance);
    }

    public int CurrentBalance
    {
        get { return currentBalance; }
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);

        uiHandler.UpdateGoldDisplay(currentBalance);
    }

    public void Withdrawal(int amount)
    {
        currentBalance -= Mathf.Abs(amount);

        uiHandler.UpdateGoldDisplay(currentBalance);

        if (currentBalance < 0)
        {
            //lose game
            ReloadScene();
        }
    }

    private void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    void Awake()
    {
        currentBalance = startingBalance;
    }

    void Update()
    {
        
    }
}
