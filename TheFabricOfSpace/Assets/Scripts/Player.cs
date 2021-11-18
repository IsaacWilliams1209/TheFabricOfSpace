using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    MenuManager menuManager;
    public int sidesCompleted;
    public bool playerWon = false;
    public bool inMainMenu = true;

    private void Start()
    {
        menuManager = MenuManager.GetInstance();
    }

    private void Update()
    {
        if (sidesCompleted == 5) { playerWon = true; }

        if (playerWon) {
            playerWon = false;
            sidesCompleted = 0;
            Time.timeScale = 0.0f;
            menuManager.SwitchMenu(MenuType.WinScreen);
            GameObject worldReset = GameObject.Find("/LavishPlanet");
            for (int i = 0; i < 6; i++)
            {
                Destroy(worldReset.transform.GetChild(i+1).gameObject);
            }
            worldReset.GetComponent<Planet_Generation>().PlanetSpawn();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!inMainMenu)
            {
                Time.timeScale = 0;
                menuManager.SwitchMenu(MenuType.PauseMenu);
            }
        }
    }
}
