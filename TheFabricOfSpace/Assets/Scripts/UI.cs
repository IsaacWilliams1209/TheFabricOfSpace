using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField]
    GameObject lavishTides;
    [SerializeField]
    GameObject gustyCliffs;
    [SerializeField]
    GameObject frostyFields;
    [SerializeField]
    private string plan01Name;
    [SerializeField]
    private string plan02Name;
    [SerializeField]
    private string plan03Name;
    //[SerializeField]
    //private GameObject test;

    private GameObject lvlNameDisplay;

    private PointerData componentName;

    static public string testName;

    private string test;

    private void Start()
    {
        lavishTides.name = plan01Name;
        gustyCliffs.name = plan02Name;
        frostyFields.name = plan03Name;

        lvlNameDisplay = transform.GetChild(7).gameObject;

    }

    private void Update()
    {
     
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(testName);
            lvlNameDisplay.GetComponent<TextMeshProUGUI>().text = plan01Name;

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            lvlNameDisplay.GetComponent<TextMeshProUGUI>().text = plan02Name;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            lvlNameDisplay.GetComponent<TextMeshProUGUI>().text = plan03Name;
        }
    }

    private bool IsSelectionOverUI()
    {
        Debug.Log("Mouse Over: " + EventSystem.current.name);
        return EventSystem.current.IsPointerOverGameObject();
    }

}
