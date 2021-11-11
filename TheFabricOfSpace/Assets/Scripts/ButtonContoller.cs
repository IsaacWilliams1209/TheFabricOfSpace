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
    ExitGame
}

[RequireComponent(typeof(Button))]
public class ButtonContoller : MonoBehaviour
{
    public ButtonType buttonType;

    AudioManager audioManager;
    MenuManager menuManager;
    Button currButton;

    private void Start()
    {
        currButton = GetComponent<Button>();
        currButton.onClick.AddListener(OnButtonClicked);
        menuManager = MenuManager.GetInstance();
        audioManager = AudioManager.GetInstance();
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
                break;
            case ButtonType.GoToMainMenu:
                menuManager.SwitchMenu(MenuType.MainMenu);
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
            default:
                break;
        }

    }
}
