using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Manager : MonoBehaviour
{
    Shepherd currSheepType;
    bool isVoxelSheep;
    Vector3 originalTransform;
    GameObject defaultSheepIcon;
    GameObject voxelSheepIcon;
    Vector3 uiScaleSize = new Vector3(2.4f, 2.4f, 2.4f);
    float uiEffectSpeed = 0.8f;
    PopUp_Manager popUpManager;
    Camera camera;

    private void Start()
    {
        currSheepType = GameObject.Find("Shepherd").GetComponent<Shepherd>();
        defaultSheepIcon = GameObject.Find("Sheep_Icon01");
        voxelSheepIcon = GameObject.Find("Sheep_Icon02");
        popUpManager = PopUp_Manager.GetInstance();
        camera = Camera.main;
    }

    void Update()
    {
        IconUpdater();
        PopUpManager();
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(currSheepType.GetComponentInChildren<Sheep>().gameObject.transform.position);

        }
    }

    void IconUpdater()
    {
        if (currSheepType.activeSheep.GetComponent<Sheep>().sheepType == SheepType.Slab)
        {
            if (!LeanTween.isTweening(voxelSheepIcon))
            {
                PulseEffect(voxelSheepIcon);
                CancelTween(defaultSheepIcon);
            }
        }
        else
        {
            if (!LeanTween.isTweening(defaultSheepIcon))
            {
                PulseEffect(defaultSheepIcon);
                CancelTween(voxelSheepIcon);
            }
        }

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
