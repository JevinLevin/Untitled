using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SaveSlot : MonoBehaviour
{

    public SaveSlotMenu saveSlotMenu;

    [HideInInspector] public string ProfileID = "";

    [Header("Content")]
    [SerializeField] private GameObject empty;
    [SerializeField] private GameObject full;
    [SerializeField] private TextMeshProUGUI percentage;
    [SerializeField] private TextMeshProUGUI saveName;
    [SerializeField] private TextMeshProUGUI timestamp;
    [SerializeField] private ProfileNameValidation inputField;

    [Header("Other Buttons")]
    [SerializeField] private Button saveSlotButton;
    [SerializeField] public RectTransform saveSlotTransform;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button renameButton;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;

    private bool hoveringAccept = false;
    private bool renaming = false;


    public void Update()
    {
        if(inputField.GetText().Length > 0)
        {
            acceptButton.interactable = true;

            // Allows pressing enter to accept
            if (renaming && Input.GetKeyUp(KeyCode.Return))
            {
                OnRenameAccept();
            }
        }
        else
        {
            acceptButton.interactable = false;
        }

        // Allows pressing esc to reset
        if (renaming && Input.GetKeyDown(KeyCode.Escape))
        {
            RenameCancel();
        }

    }

    public void SetData(PlayerGameData data, string profileID)
    {
        ProfileID = profileID;

        if (data == null)
        {
            empty.SetActive(true);
            full.SetActive(false);
            deleteButton.gameObject.SetActive(false);

            percentage.gameObject.SetActive(false);
        }
        else
        {
            empty.SetActive(false);
            full.SetActive(true);
            deleteButton.gameObject.SetActive(true);

            percentage.gameObject.SetActive(true);
            saveName.text = profileID;
            timestamp.text = System.DateTime.FromBinary(data.lastUpdated).ToString("dd/MM hh:mmtt");
        }
    }

    public string GetProfileID()
    {
        return ProfileID;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        deleteButton.interactable = interactable;
        renameButton.interactable = interactable;
        acceptButton.interactable = interactable;
    }

    public void OnDeleteClicked()
    {

        saveSlotMenu.OnDeleteClicked(this);
    }

    public void OnSaveSlotClicked()
    {
        saveSlotMenu.OnSaveSlotClicked(this);
    }

    public void OnRenameClicked()
    {
        inputField.gameObject.SetActive(true);
        inputField.SetFocus();

        acceptButton.gameObject.SetActive(true);

        declineButton.gameObject.SetActive(true);
        renameButton.gameObject.SetActive(false);

        deleteButton.colors = acceptButton.colors;
        deleteButton.interactable = false;

        saveName.gameObject.SetActive(false);

        renaming = true;
    }

    public void OnRenameCancel()
    {
        if(!hoveringAccept)
        {
            RenameCancel();
        }
    }

    private void RenameCancel()
    {
        renaming = false;

        inputField.ClearText();
        inputField.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        acceptButton.interactable = false;
        acceptButton.transform.localScale = Vector3.one;

        declineButton.gameObject.SetActive(false);
        renameButton.gameObject.SetActive(true);

        deleteButton.colors = renameButton.colors;
        deleteButton.interactable = true;

        saveName.gameObject.SetActive(true);
    }

    public void OnHoverAcceptStart()
    {
        hoveringAccept = true;
    }

    public void OnHoverAcceptStop()
    {
        hoveringAccept = false;
    }
    

    public void OnRenameAccept()
    {
        renaming = false;

        string newProfileID = inputField.GetText();

        if(newProfileID.Length <= 0)
        {
            RenameCancel();
            return;
        }

        if(SavingManager.Instance.CheckDuplicate(newProfileID))
        {
            if(newProfileID == ProfileID)
            {
                RenameCancel();
            }
            else
            {
                inputField.DisplayError("Save name already exists");
                inputField.SetFocus();
            }
        }
        else
        {
            saveSlotMenu.RenameProfile(newProfileID, ProfileID);

            ProfileID = newProfileID;
            saveName.text = ProfileID;

            RenameCancel();
        }
    }

    public void SetStartAnimation()
    {
        saveSlotTransform.transform.localPosition -= new Vector3(0.0f,1000.0f,0.0f);
    }

    public void PlayStartAnimation()
    {
        saveSlotTransform.transform.DOLocalMoveY(saveSlotTransform.transform.localPosition.y + 1000.0f,0.35f).SetEase(Ease.OutCubic);
    }
    public void PlayEndAnimation(float length)
    {
        saveSlotTransform.transform.DOScale(new Vector3(0.25f,0.25f,0.25f), length);
    }
    public void UpdatePositionAnimation(float length)
    {
        saveSlotTransform.transform.DOLocalMoveY(saveSlotTransform.transform.localPosition.y + saveSlotTransform.rect.height + 25,length).SetEase(Ease.OutCubic);
    }

    public void SetInvisible()
    {
        saveSlotTransform.gameObject.SetActive(false);
    }

    public void KillTweens()
    {
        deleteButton.transform.DOKill();
        renameButton.transform.DOKill();
        declineButton.transform.DOKill();
        acceptButton.transform.DOKill();
        saveSlotTransform.DOKill();
    }
}
