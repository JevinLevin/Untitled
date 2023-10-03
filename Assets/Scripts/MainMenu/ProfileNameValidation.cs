using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using System;

public class ProfileNameValidation : MonoBehaviour
{

    [Header("Objects")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI errorDisplay;

    private Tween errorTween;

    public void Start()
    {
        inputField.onValidateInput += delegate(string input, int charIndex, char addedChar) { return ValidateName(addedChar); };
    }

    public string GetText()
    {
        return inputField.text;
    }

    public void ClearText()
    {
        inputField.text = "";
    }

    public void SetInteractable(bool value)
    {
        inputField.interactable = value;
    }
    
    public void DisplayError(string error)
    {
        errorDisplay.DOKill();

        errorTween = errorDisplay.DOFade(1.0f, 0.1f).OnComplete(() =>
        errorDisplay.DOFade(0.0f, 2.0f));

        errorDisplay.text = error;
    }

    public char ValidateName(char newChar)
    {

        if(GetText().Length > 15)
        {
            DisplayError("Name is too long");
            return('\0');
        }
        if(!Char.IsLetter(newChar) && !Char.IsNumber(newChar) && !Char.IsWhiteSpace(newChar))
        {
            DisplayError("Only numbers and letters are permitted");
            return('\0');
        }
        return newChar;
    }

    public bool CheckDuplicate()
    {
        return SavingManager.Instance.CheckDuplicate(GetText());
    }

    public void SetFocus()
    {
        inputField.Select();
        inputField.ActivateInputField();
    }

    public bool GetFocus()
    {
        return inputField.isFocused;
    }

    
}
