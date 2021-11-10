using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialPromt : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI tutePrompt;

    public void UpdateText(Sheep sheep)
    {
        switch (sheep.sheepType)
        {
            case SheepType.Sheared:
                tutePrompt.text = "Wool Style: Sheared Sheep\nPress E when near a 1 block gap to jump across\n\nCan also jump across water ";
                break;
            case SheepType.Slab:
                tutePrompt.text = "Wool Style: Slab Sheep\nPress E to Puff making the sheep become a block that can be walked on by other sheep but restricts movement";
                break;
            case SheepType.Snowball:
                tutePrompt.text = "Wool Style: Snowball Sheep\nMoving in any direction makes this sheep roll until reaching an edge or colliding with an object\n\nRolling over water makes traversable Ice Lilies";
                break;
            case SheepType.Static:
                tutePrompt.text = "Wool Style: Shock Sheep\nPress E Can attach other sheep to its sides\n\nIf the snowball sheep rolls into this sheep it’ll stop in its tracks";
                break;
            default:
                break;
        }

    }
}
