using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MenuType
{
    MainMenu,
    LevelSelection,
    ControlsMenu,
    Credits,
    PauseMenu,
    WinScreen,
    GUI
}

public class MenuManager : Singleton<MenuManager>
{
    List<MenuController> menus;
    MenuController lastActiveMenu;

    protected override void Awake()
    {
        base.Awake();
        Time.timeScale = 0.0f;
        menus = GetComponentsInChildren<MenuController>().ToList(); 
        menus.ForEach(x => x.gameObject.SetActive(false));
        SwitchMenu(MenuType.MainMenu);
    }

    private void ResumeGame()
    {

    }

    public void SwitchMenu(MenuType type)
    {
        if (lastActiveMenu != null)
        {
            lastActiveMenu.gameObject.SetActive(false);
        }

        MenuController desiredMenu = menus.Find(x => x.menuType == type);

        if (desiredMenu != null)
        {
            desiredMenu.gameObject.SetActive(true);
            lastActiveMenu = desiredMenu;
        }
        else { Debug.LogWarning("The desired canvas was not found!"); }
    }

    public void TurnMenusOff()
    {
        menus.ForEach(x => x.gameObject.SetActive(false));
    }

    public void TurnMenuOff(MenuType type)
    {
        MenuController desiredMenu = menus.Find(x => x.menuType == type);
        if (desiredMenu != null)
        {
            desiredMenu.gameObject.SetActive(false);
        }
        else { Debug.LogWarning("The desired canvas was not found!"); }
    }

}
