using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System;
using DG.Tweening;

public class SaveSlotMenu : MonoBehaviour
{
    
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private MainBackButton backButton;

    [Header("Slots")]
    [SerializeField] [AssetsOnly] private GameObject slotPrefab;
    [SerializeField] private GameObject slotPanel;
    [SerializeField] private Scrollbar scrollbar;

    [Header("Animation")]
    [SerializeField] private float spawnSlotDelay;
    [SerializeField] private float deactivateLength;
    [SerializeField] private float deleteLength;
    private List<SaveSlot> saveSlots = new List<SaveSlot>();
    private SaveSlot currentSaveSlot;
    private SaveSlot deleteSaveSlot;

    [Header("PopupEvents")]
    public UnityEvent onYesCallback;
    public UnityEvent onNoCallback;

    private bool isFocused = true;

    public void Update()
    {

        // Reloads slots if player tabs out and back in
        if(!Application.isFocused && isFocused)
        {
            isFocused = false;
        }
        if(Application.isFocused && !isFocused)
        {
            isFocused = true;
            RefreshMenu();
        }
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        SetInteractable(false);

        SavingManager.Instance.ChangeSelectedProfileID(saveSlot.GetProfileID());

        SavingManager.Instance.SaveGame();

        DOTween.KillAll();

        ButtonScale.ButtonActive = false;

        SceneManager.LoadScene("Main");
    }

    public void RenameProfile(string newProfileID, string oldProfileID)
    {
        SavingManager.Instance.RenameProfile(newProfileID, oldProfileID);
    }

    public void OnBackClicked()
    {
        StartCoroutine(DeactivateMenuStart());
    }

    public void OnDeleteClicked(SaveSlot saveSlot)
    {
        currentSaveSlot = saveSlot;

        Action yesCallback = null;
        Action noCallback = null;

        if(onYesCallback.GetPersistentEventCount() > 0)
        {
            yesCallback = onYesCallback.Invoke;
        }
        if(onNoCallback.GetPersistentEventCount() > 0)
        {
            noCallback = onNoCallback.Invoke;
        }

        UIManager.Instance.ModalWindow.ShowQuestion("DELETE SAVE?", "You cannot undo this action", yesCallback, noCallback, "Yes", "Cancel");
    }

    public void OnDeleteAccept()
    {
        StartCoroutine(SlotDeletedAnimation());
    }

    public void OnDeleteCancel()
    {
        
    }

    IEnumerator SlotDeletedAnimation()
    {
        SetInteractable(false);


        SavingManager.Instance.DeleteGame(currentSaveSlot.GetProfileID());

        bool slotDeleted = false;

        foreach(SaveSlot slot in saveSlots)
        {
            if(slotDeleted)
            {
                slot.UpdatePositionAnimation(deleteLength);
            }

            if(slot.ProfileID == currentSaveSlot.GetProfileID())
            {
                deleteSaveSlot = slot;
                slot.SetInvisible();
                slotDeleted = true;
            }
        }
        yield return new WaitForSeconds(deleteLength);

        Destroy(deleteSaveSlot.gameObject);

        SetInteractable(true);
        RefreshMenu();
    }

    private void DeleteSlots()
    {
        foreach(SaveSlot slot in saveSlots)
        {
            slot.saveSlotTransform.DOKill();
            Destroy(slot.gameObject);
        }
        saveSlots.Clear();
    }

    public void ActivateMenu()
    {
        gameObject.SetActive(true);

        SetInteractable(true);
        RefreshMenu(true);

        scrollbar.value = 1;

        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1.0f,0.25f);

        backButton.SetOnClickFunction(this.OnBackClicked);
        backButton.FadeIn(0.25f);

        StartCoroutine(ActivateMenuAnimation());

    }

    IEnumerator ActivateMenuAnimation()
    {
        int slotNo = 0;
        foreach(SaveSlot slot in saveSlots)
        {
            slotNo ++;
            slot.PlayStartAnimation();
            yield return new WaitForSeconds(slotNo < 8 ? spawnSlotDelay : 0);
        }
    }

    public void RefreshMenu(bool firstLoad = false)
    {
        Dictionary<string, GameDataMaster> profilesGameData = SavingManager.Instance.GetAllProfiles();

        DeleteSlots();

        if(profilesGameData.Count > 0)
        {
        foreach(KeyValuePair<string, GameDataMaster> slot in profilesGameData.OrderByDescending(slot => slot.Value.playerGameData.lastUpdated))
        {
            PlayerGameData profileData = slot.Value.playerGameData;

            SaveSlot saveSlot = Instantiate(slotPrefab, transform.position, Quaternion.identity, slotPanel.transform).GetComponent<SaveSlot>();

            saveSlot.ProfileID = slot.Key;
            saveSlot.saveSlotMenu = this;

            saveSlots.Add(saveSlot);

            saveSlot.SetData(profileData, saveSlot.ProfileID);

            if(firstLoad)
            {
                saveSlot.SetStartAnimation();
            }
        }
        }
        else
        {
            SavingManager.Instance.ClearSelectedProfileID();
            StartCoroutine(DeactivateMenuStart());
        }
    }

    IEnumerator DeactivateMenuStart()
    {
        DeactivateMenu();

        yield return new WaitForSeconds(0.05f);

        mainMenu.ActivateMenu();
    }

    public void DeactivateMenu()
    {
        SetInteractable(false);

        foreach(SaveSlot slot in saveSlots)
        {
            slot.PlayEndAnimation(deactivateLength);
        }

        backButton.FadeOut(deactivateLength);

        canvasGroup.DOFade(0.0f,deactivateLength).OnComplete(() =>
        DeactivateMenuEnd());
    }

    public void DeactivateMenuEnd()
    {
        DeleteSlots();
        gameObject.SetActive(false);
    }

    private void SetInteractable(bool value)
    {
        canvasGroup.interactable = value;
        backButton.SetInteractable(value);
    }
}
