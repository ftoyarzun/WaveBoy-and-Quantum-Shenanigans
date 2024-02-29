using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHitPointManager : MonoBehaviour
{
    [SerializeField] Image ElectronHitPointsSlider;
    [SerializeField] Image PositronHitPointsSlider;
    private void Start()
    {
        Player.Instance.OnChangeInHP += Player_OnChangeInHP;
    }

    private void Player_OnChangeInHP(object sender, System.EventArgs e)
    {
        UpdateHPBars();
    }

    private void UpdateHPBars()
    {
        ElectronHitPointsSlider.fillAmount = Player.Instance.GetHitPointValueNormalized(Player.HitPointType.ElectronHP);
        PositronHitPointsSlider.fillAmount = Player.Instance.GetHitPointValueNormalized(Player.HitPointType.PositronHP);
    }
}
