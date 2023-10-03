using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class ModalWindow : MonoBehaviour
{

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Header")]
    public GameObject Header;
    public TextMeshProUGUI HeaderTitle;


    [Header("Content")]
    public GameObject Content;
    public Image ContentImage;


    [Header("Footer")]
    public GameObject Footer;
    public TextMeshProUGUI FooterWarning;
    public Button FooterYes;
    public TextMeshProUGUI FooterYesText;
    public Button FooterNo;
    public TextMeshProUGUI FooterNoText;


    private Action onYes;
    private Action onNo;

    public void Yes()
    {
        onYes?.Invoke();
        CloseMenu();
    }

    public void No()
    {
        onNo?.Invoke();
        CloseMenu();
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);

        rectTransform.localScale = new Vector3(0.5f,0.5f,0.5f);
        rectTransform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutQuad);

        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1.0f,0.15f);

    }

    public void CloseMenu()
    {
        canvasGroup.DOFade(0.0f,0.1f).OnComplete(() =>
        gameObject.SetActive(false));
    }

    public void ShowQuestion(string title, string warning, Action yesAction, Action noAction, string yesText, string noText)
    {
        OpenMenu();

        Content.SetActive(false);

        HeaderTitle.text = title;

        FooterWarning.text = warning;

        FooterYesText.text = yesText;
        FooterNoText.text = noText;

        onYes = yesAction;
        onNo = noAction;
    }

}
