using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ButtonScale : OnPointerEnterExit
{

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Button button;

    [Header("Attributes")]
    [SerializeField] private float scale = 1.1f;
    [SerializeField] private float length = 0.2f;

    public static bool ButtonActive = false;

    private Tween scaleTween;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponentInParent<CanvasGroup>(true);

        onPointerEnter.AddListener(OnHoverStart);
        onPointerExit.AddListener(OnHoverEnd);
    }

    void OnEnable()
    {
        rectTransform.localScale = Vector3.one;
    }


    public void OnHoverStart()
    {
        if(button.interactable && ButtonActive)
        {
            scaleTween.Complete();

            if(rectTransform != null)
            {
            scaleTween = rectTransform.DOScale(Vector3.one * scale,length).SetEase(Ease.OutQuart);
            }
        }
    }

    public void OnHoverEnd()
    {
        if(button.interactable && ButtonActive)
        {
            scaleTween.Complete();

            if(rectTransform != null)
            {
            scaleTween = rectTransform.DOScale(new Vector3(1f,1f,1f),length).SetEase(Ease.OutCubic);
            }
            
        }
    }

}
