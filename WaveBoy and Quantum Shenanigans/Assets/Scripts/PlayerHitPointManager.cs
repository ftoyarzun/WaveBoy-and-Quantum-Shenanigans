using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHitPointManager : MonoBehaviour
{
    public static PlayerHitPointManager Instance;

    [SerializeField] Image ElectronHitPointsSlider;
    [SerializeField] Image PositronHitPointsSlider;
    [SerializeField] GameObject HealthBars;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Player.Instance.OnChangeInHP += Player_OnChangeInHP;
        Hide();
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

    public void Show()
    {
        HealthBars.SetActive(true);
    }

    public void Hide()
    {
        HealthBars.SetActive(false);
    }
}
