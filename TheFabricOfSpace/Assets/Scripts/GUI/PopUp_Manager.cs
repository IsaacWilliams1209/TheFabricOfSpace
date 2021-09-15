using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum PopUpType
{
    RButton,
    SpaceBar
}

public class PopUp_Manager : Singleton<PopUp_Manager>
{
    public List<PopUpController> popUps;
    PopUpController lastActivePopUp;

    protected override void Awake()
    {
        base.Awake();
        popUps = GetComponentsInChildren<PopUpController>().ToList();
        popUps.ForEach(x => x.gameObject.SetActive(false));
    }

    public void SwitchPopUp(PopUpType type)
    {
        if (lastActivePopUp != null)
        {
            lastActivePopUp.gameObject.SetActive(false);
        }

        PopUpController desiredPopUp = popUps.Find(x => x.popUpType == type);

        if (desiredPopUp != null)
        {
            desiredPopUp.gameObject.SetActive(true);
            lastActivePopUp = desiredPopUp;
        }
        else { Debug.LogWarning("The desired pop up was not found!"); }
    }

    public void ClosePopUps()
    {
        popUps.ForEach(x => x.gameObject.SetActive(false));
    }
}
