using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class MainBackButton : MonoBehaviour
{

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button button;

    private Action onClickFunction;

    public void SetInteractable(bool value)
    {
        canvasGroup.interactable = value;
    }

    public void SetOnClickFunction(Action function)
    {
        onClickFunction = function;
    }

    public void OnBackClicked()
    {
        onClickFunction.Invoke();
    }

    public void FadeIn(float length)
    {
        canvasGroup.DOKill();
        gameObject.SetActive(true);
        canvasGroup.DOFade(1.0f,length);
    }

    public void FadeOut(float length)
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(0.0f,length).OnComplete(() =>
        gameObject.SetActive(false));
    }

}
