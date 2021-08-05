using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]
    Image lavishTides;
    [SerializeField]
    Image gustyCliffs;
    [SerializeField]
    Image frostyFields;
    [SerializeField]
    private string plan01Name;
    [SerializeField]
    private string plan02Name;
    [SerializeField]
    private string plan03Name;

    private GameObject lvlNameDisplay;

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
            lvlNameDisplay.GetComponent<Text>().text = plan01Name;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            lvlNameDisplay.GetComponent<Text>().text = plan02Name;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            lvlNameDisplay.GetComponent<Text>().text = plan03Name;
        }
    }
}
