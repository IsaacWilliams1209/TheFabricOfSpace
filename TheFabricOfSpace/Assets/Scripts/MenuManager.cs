using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MenuType
{
    MainMenu,
    LevelSelection,
    SettingsMenu,
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

    public void SwitchMenu(MenuType type)
    {
        if (lastActiveMenu != null)
        {
            lastActiveMenu.gameObject.SetActive(false);
        }

        MenuController desiredMenu = menus.Find(x => x.menuType == type);
        Debug.Log(desiredMenu.name);

        if (desiredMenu != null)
        {
            desiredMenu.gameObject.SetActive(true);
            lastActiveMenu = desiredMenu;
        }
        else { Debug.LogWarning("The desired canvas was not found!"); }
    }

}
