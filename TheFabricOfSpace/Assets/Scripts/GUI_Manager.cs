using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Manager : MonoBehaviour
{
    Shepherd currSheepType;
    Vector3 originalTransform;
    GameObject defaultSheepIcon, slabSheepIcon, snowballSheepIcon, shockSheepIcon, sheepAsleepIcon;
    GameObject activeSheepIcon, lastActiveIcon;
    Vector3 uiScaleSize = new Vector3(2.4f, 2.4f, 2.4f);
    float uiEffectSpeed = 0.8f;
    PopUp_Manager popUpManager;
    Camera camera;

    Player currPlanetFace;

    private void Start()
    {
        currSheepType = GameObject.Find("Shepherd").GetComponent<Shepherd>();
        defaultSheepIcon = GameObject.Find("DefaultSheepIcon");
        slabSheepIcon = GameObject.Find("SlabSheepIcon");
        snowballSheepIcon = GameObject.Find("SnowballSheepIcon");
        shockSheepIcon = GameObject.Find("ShockSheepIcon");
        sheepAsleepIcon = GameObject.Find("SheepAsleepIcon");
        popUpManager = PopUp_Manager.GetInstance();
        camera = Camera.main;
        InitialGUILayOut();
    }

    void Update()
    {
        //IconUpdater();
        PopUpManager();
    }

    void IconUpdater()
    {
        if (currSheepType.activeSheep.GetComponent<Sheep>().sheepType == SheepType.Slab) {
            if (!LeanTween.isTweening(slabSheepIcon))
            {
                activeSheepIcon = slabSheepIcon;
                PulseEffect(slabSheepIcon);
                CancelTween(lastActiveIcon);
            }
        }
        else if (currSheepType.activeSheep.GetComponent<Sheep>().sheepType == SheepType.Snowball) {
            if (!LeanTween.isTweening(snowballSheepIcon))
            {
                activeSheepIcon = snowballSheepIcon;
                PulseEffect(snowballSheepIcon);
                CancelTween(lastActiveIcon);
            }
        }
        else if (currSheepType.activeSheep.GetComponent<Sheep>().sheepType == SheepType.Static) {
            if (!LeanTween.isTweening(shockSheepIcon))
            {
                activeSheepIcon = shockSheepIcon;
                PulseEffect(shockSheepIcon);
                CancelTween(lastActiveIcon);
            }
        }
        else {
            if (!LeanTween.isTweening(defaultSheepIcon)) {
                activeSheepIcon = defaultSheepIcon;
                PulseEffect(defaultSheepIcon);
                CancelTween(lastActiveIcon);
            }
        }

    }

    private void SwitchGUILayout()
    {
        if (currPlanetFace.sidesCompleted == 1)
        {

        }
    }

    private void InitialGUILayOut()
    {
        activeSheepIcon = defaultSheepIcon;
        GameObject temp = Instantiate(gameObject.transform.GetChild(0).gameObject, new Vector3(757, 241, 0), new Quaternion(0, 0, 0, 0));
        int sheepCount = 1;
        temp.name = "Asleep Sheep " + sheepCount;
        temp.transform.parent = gameObject.transform;
    }

    void PopUpManager()
    {
        if (currSheepType.activeSheep.GetComponent<Sheep>().canJump)
        {
            Vector3 tempPos = currSheepType.activeSheep.GetComponent<Sheep>().gameObject.transform.position;
            popUpManager.popUps[0].gameObject.transform.position = new Vector3(tempPos.x, tempPos.y + 2, tempPos.z);
            popUpManager.popUps[0].gameObject.transform.LookAt(popUpManager.popUps[0].gameObject.transform.position + camera.transform.rotation * Vector3.forward,
                camera.transform.rotation * Vector3.up);
            popUpManager.SwitchPopUp(PopUpType.RButton);
        }
        else if(currSheepType.activeSheep.GetComponent<Sheep>().canWake)
        {
            Vector3 tempPos = currSheepType.activeSheep.GetComponent<Sheep>().gameObject.transform.position;
            popUpManager.popUps[1].gameObject.transform.position = new Vector3(tempPos.x, tempPos.y + 2, tempPos.z);
            popUpManager.popUps[1].gameObject.transform.LookAt(popUpManager.popUps[1].gameObject.transform.position + camera.transform.rotation * Vector3.forward,
                camera.transform.rotation * Vector3.up);
            popUpManager.SwitchPopUp(PopUpType.SpaceBar);
        }
        else
        {
            popUpManager.ClosePopUps();

        }
    }

    private void PulseEffect(GameObject currIcon)
    {
        originalTransform = currIcon.gameObject.transform.localScale;
        LeanTween.scale(currIcon.gameObject, uiScaleSize, uiEffectSpeed).setLoopPingPong();
    }

    private void CancelTween(GameObject lastIcon)
    {
        lastIcon.gameObject.transform.localScale = originalTransform;
        LeanTween.cancel(lastIcon);
    }


}
