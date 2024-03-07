using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScientistVisualsLoading : MonoBehaviour
{
    [SerializeField] Image scientistImage;
    [SerializeField] Sprite surprisedScientist;
    [SerializeField] Sprite wonderingScientist;
    [SerializeField] Sprite ideaScientist;
    [SerializeField] Sprite regretScientist;

    public void SetScientistImage(int index)
    {
        switch (index)
            {
            case 0:

                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
            }
    }
}
