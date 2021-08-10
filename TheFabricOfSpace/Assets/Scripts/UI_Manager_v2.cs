using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UI_Manager_v2 : MonoBehaviour
{
    [SerializeField]
    private GameObject button01, button02, button03, button04;

    List<GameObject> UiElements = new List<GameObject>();

    private int currIndex = 0;

    private void Start()
    {
        ListSetUp();
        EventSystem.current.firstSelectedGameObject = button01;
    }

    private void Update()
    {
        
    }

    private void ListSetUp()
    {
        UiElements.Add(button01);
        UiElements.Add(button02);
        UiElements.Add(button03);
        UiElements.Add(button04);
    }

}
