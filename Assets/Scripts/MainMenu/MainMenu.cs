using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{   
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Menu Navigations")]
    [SerializeField] private SaveSlotMenu saveSlotsMenu;
    [SerializeField] private NewGameCreation newGameMenu;
    [SerializeField] private MainBackButton backButton;
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private RectTransform newGameTransform;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private RectTransform continueGameTransform;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private RectTransform loadGameTransform;

    // Button colours
    UnityEngine.UI.ColorBlock defaultButtonColors;
    UnityEngine.UI.ColorBlock invisButtonColors;

    void Awake()
    {
        // Sets disabled colour variables
        defaultButtonColors = newGameButton.colors;
        invisButtonColors = defaultButtonColors;
        invisButtonColors.disabledColor = Color.white;
    }

    void Start()
    {
        ActivateAnimationStart();
    }
    
    private void AdjustButtonStatus(bool startup)
    {
        if(!SavingManager.Instance.HasGameData())
        {
            continueGameButton.gameObject.SetActive(false);
            loadGameButton.interactable = false;
            loadGameButton.colors = defaultButtonColors;
        }
        else
        {
            if(startup){continueGameButton.gameObject.SetActive(true);}
            loadGameButton.interactable = true;
        }
    }

    public void NewGameClicked()
    {
        StartCoroutine(SwitchToNewGame());
    }

    IEnumerator SwitchToNewGame()
    {
        DeactivateMenu();

        yield return new WaitForSeconds(0.05f);

        newGameMenu.ActivateMenu();
    }

    public void LoadGameClicked()
    {
        saveSlotsMenu.ActivateMenu();
        DeactivateMenu();
    }

    public void ContinueGame()
    {
        DOTween.KillAll();

        ButtonScale.ButtonActive = false;

        SceneManager.LoadScene("Main");
    }

    public void ActivateMenu()
    {
        gameObject.SetActive(true);

        EndTweens();
        RemoveDisableColours();

        AdjustButtonStatus(false);
        SetInteractable(false);

        ActivateAnimationStart();

    }

    private void ActivateAnimationStart()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;

        RemoveDisableColours();

        StartCoroutine(ActivateAnimation());
    }

    IEnumerator ActivateAnimation()
    {
        yield return new WaitForSeconds(0.01f);
        AdjustButtonStatus(true);
        
        canvasGroup.alpha = 0;
        continueGameTransform.position += new Vector3(100,0,0);
        loadGameTransform.position += new Vector3(150,0,0);
        newGameTransform.position += new Vector3(200,0,0);


        canvasGroup.DOFade(1.0f,0.2f);

        continueGameTransform.DOMoveX(continueGameTransform.transform.position.x - 100.0f, 0.2f).SetEase(Ease.OutQuart);

        loadGameTransform.DOMoveX(loadGameTransform.transform.position.x - 150.0f, 0.25f).SetEase(Ease.OutQuart);

        newGameTransform.DOMoveX(newGameTransform.transform.position.x - 200.0f, 0.3f).SetEase(Ease.OutQuart).OnComplete(() =>
        ActivateAnimationEnd());
    }


    private void ActivateAnimationEnd()
    {
        canvasGroup.interactable = true;
        AddDisableColours();
    }

    public void DeactivateMenu()
    {

        SetInteractable(false, false);

        EndTweens();
        RemoveDisableColours();


        continueGameTransform.DOScale(new Vector3(0.5f,0.5f,0.5f),0.15f);
        loadGameTransform.DOScale(new Vector3(0.5f,0.5f,0.5f),0.15f);
        newGameTransform.DOScale(new Vector3(0.5f,0.5f,0.5f),0.15f);

        canvasGroup.DOFade(0.0f,0.15f).OnComplete(() =>
        DeactivateMenuEnd());
        
    }

    public void DeactivateMenuEnd()
    {
        EndTweens();

        gameObject.SetActive(false);

        // Resets button scale incase after animation
        continueGameTransform.localScale = Vector3.one;
        loadGameTransform.localScale = Vector3.one;
        newGameTransform.localScale = Vector3.one;
    }

    private void RemoveDisableColours()
    {
        continueGameButton.colors = invisButtonColors;
        newGameButton.colors = invisButtonColors;
        loadGameButton.colors = invisButtonColors;
    }

    private void AddDisableColours()
    {
        continueGameButton.colors = defaultButtonColors;
        newGameButton.colors = defaultButtonColors;
        loadGameButton.colors = defaultButtonColors;
    }

    private void EndTweens()
    {
        canvasGroup.DOComplete();
        continueGameTransform.DOComplete();
        loadGameTransform.DOComplete();
        newGameTransform.DOComplete();
    }

    public void SetInteractable(bool value, bool includeBack = true)
    {
        canvasGroup.interactable = value;
        if(includeBack)
        {
        backButton.SetInteractable(value);
        }
    }

}
