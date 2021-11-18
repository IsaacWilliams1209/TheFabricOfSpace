using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Menu : MonoBehaviour
{
    GUI_Manager icons;
    GameObject startIcon;

    private void Start()
    {
        startIcon = gameObject.transform.GetChild(8).gameObject;
        IconSetUp();
    }

    private void Update()
    {
        UpdateAwakeIcons();
        UpdateAsleepIcons();
    }

    void UpdateAwakeIcons()
    {

    }

    void UpdateAsleepIcons()
    {

    }

    private void IconSetUp()
    {
        for (int i = 0; i < icons.screenIcons.Count; i++)
        {
            if (icons.allSheepOnLevel[i].GetComponent<Sheep>().awake)
            {

            }
        }
    }
}
