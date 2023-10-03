using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class NewGameCreation : MonoBehaviour
{

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private SaveSlotMenu loadMenu;
    [SerializeField] private MainBackButton backButton;
    [Header("Menu Buttons")]
    [SerializeField] private Button enterButton;

    [Header("Profile Name")]
    [SerializeField] private ProfileNameValidation nameValidation;


    public void Update()
    {
        // Submit button should only be interactable once
        if(nameValidation.GetText().Length > 0)
        {
            enterButton.interactable = true;
        }
        else
        {
            enterButton.interactable = false;
        }
    }

    public void OnBackClicked()
    {
        DeactivateMenu(mainMenu.ActivateMenu, true);
    
    }

    public void OnClickCreate()
    {
        if(!nameValidation.CheckDuplicate())
        {
            SetInteractable(false);

            SavingManager.Instance.ChangeSelectedProfileID(nameValidation.GetText());
            
            SavingManager.Instance.NewGame();
            
            SavingManager.Instance.SaveGame();


            DeactivateMenu(loadMenu.ActivateMenu, false);
        }
        else
        {
            nameValidation.DisplayError("Save name already exists");
        }
        
    }

    public void SetInteractable(bool value)
    {
        nameValidation.SetInteractable(value);
        canvasGroup.interactable = value;
        backButton.SetInteractable(value);
    }

    public void ActivateMenu()
    {
        gameObject.SetActive(true);
        SetInteractable(true);
        nameValidation.ClearText();

        // Fade opacity
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1.0f,0.25f);

        // Rotate panel
        rectTransform.transform.rotation = Quaternion.Euler(0.0f,0.0f,-45.0f);
        rectTransform.transform.DOLocalRotate(Vector3.zero,0.35f).SetEase(Ease.OutBack);

        // Back button setup
        backButton.SetOnClickFunction(this.OnBackClicked);
        backButton.FadeIn(0.25f);
    }

    private void DeactivateMenu(Action newMenu, bool toMain)
    {

        // Fade out back button if returning to main menu
        if(toMain)
        {
        backButton.FadeOut(0.15f);
        }

        rectTransform.transform.DOLocalRotate(new Vector3(0.0f,0.0f,45f),0.15f).SetEase(Ease.InCirc);
        canvasGroup.DOFade(0.0f,0.15f).OnComplete(() =>
        DeactivateMenuEnd());

        StartCoroutine(LoadNewMenu(newMenu));
    }

    private void DeactivateMenuEnd()
    {
        gameObject.SetActive(false);
    }

    IEnumerator LoadNewMenu(Action newMenu)
    {
        yield return new WaitForSeconds(0.05F);

        newMenu.Invoke();
    }

}
