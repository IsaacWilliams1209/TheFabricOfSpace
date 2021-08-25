using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject currObject;
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        UI_Manager.currLvTitle = name;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {

    }
}
