using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Manager : MonoBehaviour
{
    Shepherd currSheepType;
    public bool iconNeedsUpdate = false;
    Vector3 originalTransform;
    [SerializeField] Sprite defaultSheepIcon, slabSheepIcon, snowballSheepIcon, shockSheepIcon, sheepAsleepIcon;
    GameObject activeSheepIcon, lastActiveIcon;
    Vector3 uiScaleSize = new Vector3(2.4f, 2.4f, 2.4f);
    float uiEffectSpeed = 0.8f;
    PopUp_Manager popUpManager;
    Camera camera;
    Player currPlanetFace;
    [SerializeField] Vector3 newIconScale;
    Vector3 iconOffset = new Vector3(50, 100, 0);
    [SerializeField] List<GameObject> screenIcons = new List<GameObject>(); //[0] component will always be the icon of the active sheep and shouldn't be touched.
    int iconIndex = 0;

    private void Start()
    {
        currSheepType = GameObject.Find("Shepherd").GetComponent<Shepherd>();
        defaultSheepIcon = Resources.Load<Sprite>("UI/Sheep/DefaultSheepIcon") as Sprite;
        slabSheepIcon = Resources.Load<Sprite>("UI/Sheep/SlabSheepIcon") as Sprite;
        snowballSheepIcon = Resources.Load<Sprite>("UI/Sheep/SnowballSheepIcon") as Sprite;
        shockSheepIcon = Resources.Load<Sprite>("UI/Sheep/ShockSheepIcon") as Sprite;
        sheepAsleepIcon = Resources.Load<Sprite>("UI/Sheep/SheepAsleepIcon") as Sprite;
        activeSheepIcon = transform.GetChild(0).gameObject;
        popUpManager = PopUp_Manager.GetInstance();
        camera = Camera.main;
        currSheepType.activeSheep.GetComponent<Sheep>().sheepIcons = this;
        InitialGUILayOut();
    }

    void Update()
    {
        if (iconNeedsUpdate) 
        { 
            currSheepType.activeSheep.GetComponent<Sheep>().sheepIcons = this;
            SwapIcon(); 
        }
        IconUpdater();
        //PopUpManager();
    }

    public void SwapIcon()
    {
        screenIcons[0].GetComponent<Image>().sprite = lastActiveIcon.GetComponent<Image>().sprite;
        iconNeedsUpdate = false;
    }

    private void IconUpdater()
    {
        if (currSheepType.activeSheep.GetComponent<Sheep>().sheepType == SheepType.Sheared)
        {
            lastActiveIcon = activeSheepIcon;
            activeSheepIcon.GetComponent<Image>().sprite = defaultSheepIcon;
        }
        else if (currSheepType.activeSheep.GetComponent<Sheep>().sheepType == SheepType.Slab)
        {
            lastActiveIcon = activeSheepIcon;
            activeSheepIcon.GetComponent<Image>().sprite = slabSheepIcon;
        }
        else if (currSheepType.activeSheep.GetComponent<Sheep>().sheepType == SheepType.Snowball)
        {
            activeSheepIcon.GetComponent<Image>().sprite = snowballSheepIcon;
        }
        else if (currSheepType.activeSheep.GetComponent<Sheep>().sheepType == SheepType.Static)
        {
            activeSheepIcon.GetComponent<Image>().sprite = shockSheepIcon;
        }

        if (!LeanTween.isTweening(activeSheepIcon))
        {
            PulseEffect(activeSheepIcon);
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
        screenIcons.Add(activeSheepIcon);
        screenIcons.Add(CreateSheepIcon(iconOffset, "Asleep Sheep ", 1));
        iconOffset = UpdateVector(iconOffset, new Vector3(-50, iconOffset.y, iconOffset.z));
        screenIcons.Add(CreateSheepIcon(iconOffset, "Asleep Sheep ", 2));
        lastActiveIcon = activeSheepIcon;
    }

    private GameObject CreateSheepIcon(Vector3 iconOffset, string name, int iconNumber)
    {
        GameObject temp = Instantiate(gameObject.transform.GetChild(0).gameObject, activeSheepIcon.transform.position, new Quaternion(0, 0, 0, 0));
        temp.transform.position -= iconOffset;
        temp.transform.localScale = newIconScale;
        Color tempColour = new Color(0.6f, 0.6f, 0.6f, 0);
        temp.GetComponent<Image>().color -= tempColour;
        temp.GetComponent<Image>().sprite = sheepAsleepIcon;
        temp.name = name + iconNumber;
        temp.transform.parent = gameObject.transform;
        return temp;
    }

    private Vector3 UpdateVector(Vector3 vectorToAdjust, Vector3 updateVector)
    {
        vectorToAdjust.x = updateVector.x;
        vectorToAdjust.y = updateVector.y;
        vectorToAdjust.z = updateVector.z;
        return vectorToAdjust;
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
        else if (currSheepType.activeSheep.GetComponent<Sheep>().canWake)
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
