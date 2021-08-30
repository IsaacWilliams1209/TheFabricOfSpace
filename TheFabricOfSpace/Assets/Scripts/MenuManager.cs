using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MenuType
{
    MainMenu,
    LevelSelection,
    Settings,
    Credits,
    PauseMenu
}

public class MenuManager : Singleton<MenuManager>
{
    List<MenuController> menus;
    MenuController lastActiveMenu;

    protected override void Awake()
    {
        base.Awake();
        menus = GetComponentsInChildren<MenuController>().ToList(); 
        menus.ForEach(x => x.gameObject.SetActive(false));
        SwitchMenu(MenuType.MainMenu);
    }

    private void SwitchMenu(MenuType type)
    {
        if (lastActiveMenu != null)
        {
            lastActiveMenu.gameObject.SetActive(false);
        }

        MenuController desiredMenu = menus.Find(x => x.menuType == type);
        if (desiredMenu != null)
        {
            desiredMenu.gameObject.SetActive(true);
        }
        else { Debug.LogWarning("The desired canvas was not found!"); }
    }
    
    private void GoToMainMenu()
    {

    }

    private void GoToSettings()
    {

    }

    private void GoToCredits()
    {

    }

    private void GoToLevelSelection()
    {

    }
}
