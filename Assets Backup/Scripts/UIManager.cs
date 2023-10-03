using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance { get; private set; }

    public InventoryUI Inventory;
    public ModalWindow ModalWindow;

    public bool InvActive { get; private set; }

    private void Awake()
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
            return;
        } 
        else 
        { 
            Instance = this; 
        } 

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        bool pressTab = Input.GetKeyDown(KeyCode.Tab);

        if(!InvActive && pressTab)
        {
            Inventory.gameObject.SetActive(true);
            Inventory.UpdateUI();
            InvActive = true;

            pressTab = false;
        }
        if (InvActive && pressTab)
        {
            Inventory.gameObject.SetActive(false);
            InvActive = false;

            pressTab = false;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ButtonScale.ButtonActive = true;
    }

}
