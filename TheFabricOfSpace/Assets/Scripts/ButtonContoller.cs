using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum ButtonType
{
    SelectLevel,
    StartLevel,
    GoToControlsMenu,
    GoToCredits,
    GoToMainMenu,
    ResumeGame,
    ExitGame
}

[RequireComponent(typeof(Button))]
public class ButtonContoller : MonoBehaviour
{
    public ButtonType buttonType;

    AudioManager audioManager;
    MenuManager menuManager;
    Button currButton;

    Player player;

    private void Start()
    {
        currButton = GetComponent<Button>();
        currButton.onClick.AddListener(OnButtonClicked);
        menuManager = MenuManager.GetInstance();
        audioManager = AudioManager.GetInstance();
        player = GameObject.Find("/GameObject").GetComponent<Player>();
    }

    public void OnButtonClicked()
    {
        switch (buttonType) 
        {
            case ButtonType.StartLevel:
                Time.timeScale = 1.0f;
                menuManager.SwitchMenu(MenuType.GUI);
                audioManager.PlayAudio(AudioType.SpaceAmbience);
                audioManager.PlayAudio(AudioType.WaterAmbience);
                player.inMainMenu = false;
                break;
            case ButtonType.GoToMainMenu:
                menuManager.SwitchMenu(MenuType.MainMenu);
                player.inMainMenu = true;
                break;
            case ButtonType.GoToControlsMenu:
                menuManager.SwitchMenu(MenuType.ControlsMenu);

                break;
            case ButtonType.GoToCredits:
                menuManager.SwitchMenu(MenuType.Credits);
                break;
            case ButtonType.ExitGame:
                Application.Quit();
                break;
            case ButtonType.ResumeGame:
                Time.timeScale = 1.0f;
                menuManager.TurnMenuOff(MenuType.PauseMenu);
                break;
            default:
                break;
        }

    }
}
