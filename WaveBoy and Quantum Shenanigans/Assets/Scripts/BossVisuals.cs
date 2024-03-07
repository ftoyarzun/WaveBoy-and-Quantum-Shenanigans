using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVisuals : MonoBehaviour
{
    [SerializeField] Boss boss;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer electronSR;
    [SerializeField] SpriteRenderer positronSR;
    [SerializeField] SpriteRenderer photonUPSR;
    [SerializeField] SpriteRenderer photonDownSR;

    private void Start()
    {
        boss.OnBehaviorChanged += Boss_OnBehaviorChanged;
        spriteRenderer.color = electronSR.color;
    }

    private void Boss_OnBehaviorChanged(object sender, System.EventArgs e)
    {
        switch (boss.GetIsWhat())
        {
            case GameManager.IsWhat.Photon_up:
                spriteRenderer.color = photonUPSR.color;
                break;
            case GameManager.IsWhat.Photon_down:
                spriteRenderer.color = photonDownSR.color;
                break;
            case GameManager.IsWhat.Electron:
                spriteRenderer.color = electronSR.color;
                break;
            case GameManager.IsWhat.Positron:
                spriteRenderer.color = positronSR.color;
                break;
            case GameManager.IsWhat.Player:
                break;
        }
    }
}