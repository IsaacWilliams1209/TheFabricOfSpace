using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonType
{
    SelectLevel,
    StartLevel,
    GoToSettings,
    GoToCredits,
    GoToMainMenu,
    ExitGame
}

[RequireComponent(typeof(Button))]
public class ButtonContoller : MonoBehaviour
{
    public ButtonType buttonType;

    MenuManager menuManager;
    Button currButton;

    private void Start()
    {
        currButton = GetComponent<Button>();
        currButton.onClick.AddListener(OnButtonClicked);
        menuManager = MenuManager.GetInstance();
    }

    public void OnButtonClicked()
    {
        switch (buttonType) 
        {
            case ButtonType.GoToMainMenu:
                menuManager.SwitchMenu(MenuType.MainMenu);
                break;
            case ButtonType.GoToSettings:
                menuManager.SwitchMenu(MenuType.SettingsMenu);
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
