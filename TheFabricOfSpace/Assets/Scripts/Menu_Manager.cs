using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Menu_Manager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> menus = new List<GameObject>();
    private GameObject lastActiveMenu;

    private void Awake()
    {
        MenuSetUp();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        SwitchCanvas(menus[1]);
    //    }
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        SwitchCanvas(menus[3]);
    //    }
    //}

    private void MenuSetUp()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            GameObject temp = Instantiate(menus[i]);
            temp.transform.parent = gameObject.transform;
        }
        //menus.ForEach(x => x.gameObject.SetActive(false));
        //SwitchCanvas(menus[0]);
    }

    private void SwitchCanvas(GameObject currCanvas)
    {
        if (currCanvas != null)
        {
            currCanvas.SetActive(false);
        }

        GameObject desiredMenu = menus.Find(x => x.gameObject == currCanvas);
        if (desiredMenu != null)
        {
            desiredMenu.SetActive(true);
            lastActiveMenu = desiredMenu;
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
