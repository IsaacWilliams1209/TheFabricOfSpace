using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GUI_Manager : MonoBehaviour
{
    public Shepherd currSheep;
    public bool guiNeedsUpdate = false;
    Vector3 originalTransform;
    [SerializeField] Sprite defaultSheepIcon, slabSheepIcon, snowballSheepIcon, shockSheepIcon, sheepAsleepIcon;
    GameObject activeSheepIcon;
    Vector3 uiScaleSize = new Vector3(2.2f, 2.2f, 2.2f);
    float uiEffectSpeed = 0.8f;
    PopUp_Manager popUpManager;
    Player currPlanetFace;
    [SerializeField] Vector3 newIconScale;
    Vector3 iconOffset = new Vector3(0, 100, 0);
    public List<GameObject> allSheepOnLevel = new List<GameObject>(); //[0] component will always be the icon of the active sheep and shouldn't be touched.
    public List<GameObject> screenIcons = new List<GameObject>(); //tracks what sheep icons are currently inactive.
    GameObject ePopUp, shiftPopUp, spacePopUp;
    Sprite currPowerIcon;
    public bool isSheepSwapping;
    int amountOfGoldBerries = 0;
    TextMeshProUGUI berryCount;
    public bool switchGUI = false;
    GameObject popUp;
    private float popUpTime = 0.5f;
    bool cancelPopUp = false;

    private void Start()
    {
        IconSetUp();
        popUp = gameObject.transform.GetChild(4).gameObject;
        currSheep.activeSheep.GetComponent<Sheep>().sheepIcons = this;
        berryCount = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        currPlanetFace = GameObject.Find("/GameObject").GetComponent<Player>();
        InitialGUILayOut();
    }

    private void PopUpTimer()
    {
        popUpTime -= Time.deltaTime;
        if (popUpTime <= 0.0f) { cancelPopUp = true; }
    }

    void Update()
    {
        if (switchGUI) { SwitchGUILayout(); }
        if (guiNeedsUpdate)
        {
            currSheep.activeSheep.GetComponent<Sheep>().sheepIcons = this;
            UpdateGUI();
            UpdateActiveIcon();
        }
        IconAnimation();
        PopUpManager();
    }


    private void UpdateGUI()
    {
        berryCount.text = amountOfGoldBerries + " / 6";
        for (int i = 0; i < allSheepOnLevel.Count; i++)
        {
            if (allSheepOnLevel[i].GetComponent<Sheep>().awake == false)
            {
                screenIcons[i].GetComponent<Image>().sprite = sheepAsleepIcon;
            }
            else
            {
                screenIcons[i].GetComponent<Image>().sprite = UpdateIconType(allSheepOnLevel[i].GetComponent<Sheep>());

            }
        }
        guiNeedsUpdate = false;

    }

    public void SwitchGUILayout()
    {
        switchGUI = false;
        ResetList();
        if (currPlanetFace.sidesCompleted == 1)
        {

            //List that will track each individual sheep on the face. Will use its index to update the icons.
            allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(0).gameObject);
            allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(1).gameObject);
            allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(2).gameObject);

            //The GUI icons that are on the screen for the player to see.
            iconOffset = UpdateVector(iconOffset, new Vector3(100, iconOffset.y, iconOffset.z));
            screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep", 1));
            iconOffset = UpdateVector(iconOffset, new Vector3(0, iconOffset.y, iconOffset.z));
            screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep ", 2));
            iconOffset = UpdateVector(iconOffset, new Vector3(-100, iconOffset.y, iconOffset.z));
            screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep ", 3));

            amountOfGoldBerries = currPlanetFace.sidesCompleted;
        }
        if (currPlanetFace.sidesCompleted == 2)
        {
            //List that will track each individual sheep on the face. Will use its index to update the icons.
            allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(0).gameObject);
            allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(1).gameObject);
            allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(2).gameObject);
            allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(3).gameObject);

            //The GUI icons that are on the screen for the player to see.
            iconOffset = UpdateVector(iconOffset, new Vector3(50, iconOffset.y, iconOffset.z));
            screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep", 1));
            iconOffset = UpdateVector(iconOffset, new Vector3(0, iconOffset.y, iconOffset.z));
            screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep ", 2));
            iconOffset = UpdateVector(iconOffset, new Vector3(-50, iconOffset.y, iconOffset.z));
            screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep ", 3));
            iconOffset = UpdateVector(iconOffset, new Vector3(-100, iconOffset.y, iconOffset.z));
            screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep ", 4));

            amountOfGoldBerries = currPlanetFace.sidesCompleted;
            guiNeedsUpdate = true;
        }
        if (currPlanetFace.sidesCompleted == 3)
        {
            //List that will track each individual sheep on the face. Will use its index to update the icons.
            allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(0).gameObject);

            //The GUI icons that are on the screen for the player to see.
            iconOffset = UpdateVector(iconOffset, new Vector3(0, iconOffset.y, iconOffset.z));
            screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep ", 1));

            amountOfGoldBerries = currPlanetFace.sidesCompleted;
            guiNeedsUpdate = true;
        }
        if (currPlanetFace.sidesCompleted == 4)
        {
            //List that will track each individual sheep on the face. Will use its index to update the icons.
            allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(0).gameObject);
            allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(1).gameObject);

            //The GUI icons that are on the screen for the player to see.
            iconOffset = UpdateVector(iconOffset, new Vector3(50, iconOffset.y, iconOffset.z));
            screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep", 1));
            iconOffset = UpdateVector(iconOffset, new Vector3(-50, iconOffset.y, iconOffset.z));
            screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep ", 2));

            amountOfGoldBerries = currPlanetFace.sidesCompleted;
            guiNeedsUpdate = true;
        }
        if (currPlanetFace.sidesCompleted == 5)
        {
            //List that will track each individual sheep on the face. Will use its index to update the icons.
            allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(0).gameObject);
            allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(1).gameObject);

            //The GUI icons that are on the screen for the player to see.
            iconOffset = UpdateVector(iconOffset, new Vector3(50, iconOffset.y, iconOffset.z));
            screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep", 1));
            iconOffset = UpdateVector(iconOffset, new Vector3(-50, iconOffset.y, iconOffset.z));
            screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep ", 2));

            amountOfGoldBerries = currPlanetFace.sidesCompleted;
            guiNeedsUpdate = true;
        }
        guiNeedsUpdate = true;
    }

    private void InitialGUILayOut()
    {
        //List that will track each individual sheep on the face. Will use its index to update the icons.
        allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(0).gameObject);
        allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(1).gameObject);
        allSheepOnLevel.Add(currSheep.gameObject.transform.GetChild(2).gameObject);

        //The GUI icons that are on the screen for the player to see.
        iconOffset = UpdateVector(iconOffset, new Vector3(100, iconOffset.y, iconOffset.z));
        screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep", 1));
        iconOffset = UpdateVector(iconOffset, new Vector3(0, iconOffset.y, iconOffset.z));
        screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep ", 2));
        iconOffset = UpdateVector(iconOffset, new Vector3(-100, iconOffset.y, iconOffset.z));
        screenIcons.Add(CreateSheepIcon(iconOffset, "Non Active Sheep ", 3));

        guiNeedsUpdate = true;
    }

    private void UpdateActiveIcon()
    {
        if (currSheep.activeSheep.GetComponent<Sheep>().sheepType == SheepType.Sheared)
        {
            activeSheepIcon.GetComponent<Image>().sprite = defaultSheepIcon;
        }
        else if (currSheep.activeSheep.GetComponent<Sheep>().sheepType == SheepType.Slab)
        {
            activeSheepIcon.GetComponent<Image>().sprite = slabSheepIcon;

        }
        else if (currSheep.activeSheep.GetComponent<Sheep>().sheepType == SheepType.Snowball)
        {
            activeSheepIcon.GetComponent<Image>().sprite = snowballSheepIcon;
        }
        else if (currSheep.activeSheep.GetComponent<Sheep>().sheepType == SheepType.Static)
        {
            activeSheepIcon.GetComponent<Image>().sprite = shockSheepIcon;
        }
    }

    public Sprite UpdateIconType(Sheep sheepToUpdate)
    {
        if (sheepToUpdate.sheepType == SheepType.Sheared)
        {
            return currPowerIcon = defaultSheepIcon;
        }
        else if (sheepToUpdate.sheepType == SheepType.Slab)
        {
            return currPowerIcon = slabSheepIcon;
        }
        else if (sheepToUpdate.sheepType == SheepType.Snowball)
        {
            return currPowerIcon = snowballSheepIcon;
        }
        else if (sheepToUpdate.sheepType == SheepType.Static)
        {
            return currPowerIcon = shockSheepIcon;
        }
        return currPowerIcon;
    }

    private void IconAnimation()
    {
        if (!LeanTween.isTweening(activeSheepIcon))
        {
            PulseEffect(activeSheepIcon);
        }
        if (!LeanTween.isTweening(popUp))
        {
            PulseEffect(popUp);
        }
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
        //Need to track when the player can do things and update the icon based on that.
        if (currSheep.activeSheep.GetComponent<Sheep>().canJump)
        {
            ePopUp.SetActive(true);
            popUp = ePopUp;
        }
        else if (currSheep.activeSheep.GetComponent<Sheep>().canWake)
        {
            spacePopUp.SetActive(true);
            popUp = spacePopUp;
        }
        else if (currSheep.activeSheep.GetComponent<Sheep>().sheepType == SheepType.Slab && currSheep.activeSheep.GetComponent<Sheep>().poweredUp)
        {
            shiftPopUp.SetActive(true);
            popUp = shiftPopUp;
        }
        else
        {
            PopUpTimer();
            if (cancelPopUp)
            {
                ePopUp.SetActive(false);
                shiftPopUp.SetActive(false);
                spacePopUp.SetActive(false);
                popUpTime = 0.5f;
                cancelPopUp = false;
            }
        }
    }

    private void IconSetUp()
    {
        //Sheep type icons
        currSheep = GameObject.Find("Shepherd").GetComponent<Shepherd>();
        defaultSheepIcon = Resources.Load<Sprite>("UI/Sheep/DefaultSheepIcon") as Sprite;
        slabSheepIcon = Resources.Load<Sprite>("UI/Sheep/SlabSheepIcon") as Sprite;
        snowballSheepIcon = Resources.Load<Sprite>("UI/Sheep/SnowballSheepIcon") as Sprite;
        shockSheepIcon = Resources.Load<Sprite>("UI/Sheep/ShockSheepIcon") as Sprite;
        sheepAsleepIcon = Resources.Load<Sprite>("UI/Sheep/SheepAsleepIcon") as Sprite;
        activeSheepIcon = transform.GetChild(0).gameObject;
        popUpManager = PopUp_Manager.GetInstance();

        //Player action icons
        ePopUp = gameObject.transform.GetChild(4).gameObject;
        shiftPopUp = gameObject.transform.GetChild(5).gameObject;
        spacePopUp = gameObject.transform.GetChild(6).gameObject;

    }

    private void PulseEffect(GameObject currIcon)
    {
        if (currIcon == popUp)
        {
            originalTransform = currIcon.gameObject.transform.localScale;
            LeanTween.scale(currIcon.gameObject, new Vector3(originalTransform.x, originalTransform.y + 0.2f, 1.8f), uiEffectSpeed).setLoopPingPong();
        }
        else
        {
            originalTransform = currIcon.gameObject.transform.localScale;
            LeanTween.scale(currIcon.gameObject, uiScaleSize, uiEffectSpeed).setLoopPingPong();
        }

    }

    private void CancelTween(GameObject lastIcon)
    {
        lastIcon.gameObject.transform.localScale = originalTransform;
        LeanTween.cancel(lastIcon);
    }

    private void ResetList()
    {
        //Need to make sure to delete the game objects otherwise the icons will continue to overlay over the top of each other.
        for (int i = 0; i < screenIcons.Count; i++)
        {
            Destroy(screenIcons[i]);
        }

        allSheepOnLevel.Clear();
        screenIcons.Clear();
    }

}
