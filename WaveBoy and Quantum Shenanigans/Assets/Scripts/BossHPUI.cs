using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    public static BossHPUI Instance { get; private set; }

    [SerializeField] Image BossHitPointsSlider;

    private Boss boss;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetBoss(Boss boss)
    {
        this.boss = boss;
        boss.OnHitPointChanged += Boss_OnHitPointChanged;
    }

    private void Boss_OnHitPointChanged(object sender, System.EventArgs e)
    {
        BossHitPointsSlider.fillAmount = boss.GetCurrentHPNormalized();
    }
}
