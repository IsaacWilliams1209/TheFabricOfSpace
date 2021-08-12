using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UI_Manager_v2 : MonoBehaviour
{

    [SerializeField]
    List<GameObject> UiElements = new List<GameObject>();

    private int currIndex = 0;

    private void Start()
    {

        //EventSystem.current.firstSelectedGameObject = button01;
    }

    private void Update()
    {
        
    }


}
