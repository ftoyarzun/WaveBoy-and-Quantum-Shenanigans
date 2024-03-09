using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVisuals : MonoBehaviour
{
    [SerializeField] Boss boss;
    [SerializeField] Material material;


    private Vector3 electronColor = new Vector3(4f,1f,1f);
    private Vector3 positronColor = new Vector3(1f,2f,5f);
    private Vector3 photonUpColor = new Vector3(1f, 5f, 1f);
    private Vector3 photonDownColor = new Vector3(2f, 0.5f, 4f);

    private Vector3 currentColor;
    private Vector3 targetColor;

    private float currentBlend;
    private float targetBlend;
    private float smooth = 5f;

    private void Start()
    {
        boss.OnBehaviorChanged += Boss_OnBehaviorChanged;
    }

    private void Boss_OnBehaviorChanged(object sender, System.EventArgs e)
    {
        switch (boss.GetIsWhat())
        {
            case GameManager.IsWhat.Photon_up:
                targetColor = photonUpColor;
                targetBlend = 1f;
                break;
            case GameManager.IsWhat.Photon_down:
                targetColor = photonDownColor;
                targetBlend = 1f;
                break;
            case GameManager.IsWhat.Electron:
                targetColor = electronColor;
                targetBlend = 0f;
                break;
            case GameManager.IsWhat.Positron:
                targetColor = positronColor;
                targetBlend = 0f;
                break;
            case GameManager.IsWhat.Player:
                break;
        }
    }

    private void Update()
    {
        currentColor = Vector3.Lerp(currentColor, targetColor, Time.deltaTime*smooth);
        currentBlend = Mathf.Lerp(currentBlend, targetBlend, Time.deltaTime * smooth);
        material.SetFloat("_blendFactor", currentBlend);
        material.SetVector("_colorVector", currentColor);
    }
}