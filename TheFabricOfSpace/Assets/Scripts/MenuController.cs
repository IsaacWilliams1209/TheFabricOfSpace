using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> It is important to have this class here as it allows for enum types to have gameobject related access calls (E.G SetActive(bool)). 
/// This happens because the script derives from MonoBehaviour which means that anything that has the script attached to it is a component of unity.
/// </summary>
public class MenuController : MonoBehaviour
{
    public MenuType menuType;
}
