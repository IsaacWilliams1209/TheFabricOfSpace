using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Manager : MonoBehaviour
{
    GameObject currSheepType;
    bool isVoxelSheep;
    Vector3 originalTransform;
    GameObject defaultSheepIcon;
    GameObject voxelSheepIcon;
    Vector3 uiScaleSize = new Vector3(1.4f, 1.4f, 1.4f);
    float uiEffectSpeed = 0.8f;

    private void Start()
    {
        currSheepType = GameObject.Find("Shepherd");
        defaultSheepIcon = GameObject.Find("Sheep_Icon01");
        voxelSheepIcon = GameObject.Find("Sheep_Icon02");
    }

    void Update()
    {
        IconUpdater();
        Debug.Log(GameObject.Find("Shepherd").GetComponentInChildren<Sheep>().voxel);
    }

    void IconUpdater()
    {
        if (currSheepType.GetComponent<Shepherd>().activeSheep.GetComponent<Sheep>().voxel)
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
