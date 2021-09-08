using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    MenuManager menuManager;
    public bool playerWon = false;

    private void Start()
    {
        menuManager = MenuManager.GetInstance();
        
    }

    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.P)) { playerWon = true; }

        if (playerWon) {
            playerWon = false;
            Time.timeScale = 0.0f;
            menuManager.SwitchMenu(MenuType.WinScreen);
        }
    }

}
