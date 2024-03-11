using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    public int GetNumberOfParticles()
    {
        return transform.childCount;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void DestroyParticles()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
