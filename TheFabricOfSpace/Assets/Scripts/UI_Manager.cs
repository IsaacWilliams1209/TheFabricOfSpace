using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UI_Manager : MonoBehaviour
{

    [SerializeField]
    List<GameObject> menuUiElements = new List<GameObject>();

    //[SerializeField]
    //List<GameObject> menus = new List<GameObject>();

    [SerializeField]
    private float uiEffectSpeed;

    [SerializeField]
    private Vector3 uiScaleSize;

    [SerializeField]
    public GameObject mainMenu;

    [SerializeField]
    public GameObject levelSelection;

    [SerializeField]
    public GameObject settingsMenu;

    [SerializeField]
    public GameObject creditsMenu;

    private int currIndex = 0;
    private Vector3 originalTransform;
    static public string currLvTitle;

    EventSystem curr;

    private void Start()
    {
        EventSystem.current.firstSelectedGameObject = menuUiElements[currIndex];
    }

    private void Update()
    {
        
        if (!LeanTween.isTweening(menuUiElements[currIndex]))
        {
            PulseEffect();
        }
        currUiElement();
    }

    private void PulseEffect()
    {
        originalTransform = menuUiElements[currIndex].gameObject.transform.localScale;
        LeanTween.scale(menuUiElements[currIndex].gameObject, uiScaleSize, uiEffectSpeed).setLoopPingPong();
    }

    private int currUiElement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CancelTween();
            currIndex = currIndex ==  0 ? currIndex = menuUiElements.Count -1 : currIndex -1;
            EventSystem.current.SetSelectedGameObject(menuUiElements[currIndex].gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CancelTween();
            currIndex = currIndex == menuUiElements.Count - 1 ? currIndex = 0 : currIndex +1;
            EventSystem.current.SetSelectedGameObject(menuUiElements[currIndex].gameObject);
        }
        return currIndex;
    }

    private void CancelTween()
    {
        menuUiElements[currIndex].gameObject.transform.localScale = originalTransform;
        LeanTween.cancel(menuUiElements[currIndex]); 
    }


    public void GoToMainMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void GoToSettings()
    {
        //curr = EventSystem.current;
        
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(menuUiElements[0].gameObject);
    }
}
