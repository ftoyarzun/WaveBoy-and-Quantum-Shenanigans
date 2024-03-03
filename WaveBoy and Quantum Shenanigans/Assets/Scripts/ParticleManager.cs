using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance { get; private set; }


    private void Awake()
    {
        instance = this;
    }

    public int GetNumberOfParticles()
    {
        return transform.childCount;
    }
}
