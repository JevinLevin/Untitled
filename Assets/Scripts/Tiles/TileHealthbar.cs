using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TileHealthbar : MonoBehaviour
{

    [SerializeField] private SlicedFilledImage healthBar;
    [SerializeField] private SlicedFilledImage healthBarDifference;
    [SerializeField] private Image panel;
    [SerializeField] private Color[] healthColors;

    private Tweener healthbarAdjust;
    private float fadeTimer;
    [SerializeField] private float fadeMax;
    [SerializeField] private float fadeDelay;


    void Start()
    {
        healthBar.color = healthColors[^1];
    }

    void Update()
    {
        if(fadeTimer == 0)
        {
            healthBar.color = new Color(healthBar.color.r,healthBar.color.g,healthBar.color.b, 1);
            panel.color = new Color(panel.color.r,panel.color.g,panel.color.b, 1);
        }

        fadeTimer += Time.deltaTime;

        if(fadeTimer >= fadeDelay)
        {
            float opacity = Mathf.Lerp(1.0f,0.0f,(fadeTimer-fadeDelay)/(fadeMax-fadeDelay));

            healthBar.color = new Color(healthBar.color.r,healthBar.color.g,healthBar.color.b, opacity);
            panel.color = new Color(panel.color.r,panel.color.g,panel.color.b, opacity);

            if(fadeTimer >= fadeMax)
            {
                gameObject.SetActive(false);
            }
        }



    }


    public void UpdateValue(float value, float maxValue)
    {

        // Sets the healthbars width
        healthBar.fillAmount = value / maxValue;

        // Makes sure it overwrites the if tween is already running
        if (healthbarAdjust != null && healthbarAdjust.active) {healthbarAdjust.Complete();}

        healthBarDifference.color = new Color(1.0f,1.0f,1.0f,1.0f);
        // Smoothly moves the white bar to match the new value
        healthbarAdjust = DOTween.To(() => healthBarDifference.fillAmount, diff => healthBarDifference.fillAmount = diff, healthBar.fillAmount, 0.25f).SetEase(Ease.InOutSine).OnComplete(() =>
        healthBarDifference.color = new Color(1.0f,1.0f,1.0f,0.0f));

        // Changes the colour using an editor created list
        int colourIndex = Mathf.FloorToInt((value / maxValue) * healthColors.Length) - 1;
        healthBar.color = healthColors[colourIndex];

    }

    public void Destroyed()
    {
        healthBar.fillAmount = 0;
        healthBarDifference.fillAmount = 0;

        healthbarAdjust.Kill();
    }

    public void SetVisible()
    {
        gameObject.SetActive(true);
        fadeTimer = 0;
    }

}
