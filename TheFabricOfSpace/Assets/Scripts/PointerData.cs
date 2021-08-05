using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        UI.testName = name;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {

    }
}
