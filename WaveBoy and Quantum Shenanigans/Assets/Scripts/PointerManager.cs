using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerManager : MonoBehaviour
{

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Player.Instance.GetAimingAngle());
    }
}
