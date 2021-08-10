using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    GameObject lavishTides;
    [SerializeField]
    GameObject gustyCliffs;
    [SerializeField]
    GameObject frostyFields;
    [SerializeField]
    private string planet01Name;
    [SerializeField]
    private string planet02Name;
    [SerializeField]
    private string planet03Name;

    private GameObject lvlNameDisplay;

    static public string currPlanet;
    static public bool isCursorHovering;
    private Vector3 test = new Vector3(1.3f,1.3f,1.3f);

    private void Start()
    {
        gustyCliffs.name = planet01Name;
        lavishTides.name = planet02Name;
        frostyFields.name = planet03Name;

        lvlNameDisplay = transform.GetChild(7).gameObject;

    }

    private void Update()
    {
        LvlTitleUpdate();
        //LeanTween.cancel(gustyCliffs);
        if (Input.GetKey(KeyCode.W))
        {
            gustyCliffs.transform.LeanScale(test, 0.3f).setLoopPingPong();

        }

    }

    private void LvlTitleUpdate()
    {
        lvlNameDisplay.GetComponent<TextMeshProUGUI>().text = currPlanet;
    }
}
