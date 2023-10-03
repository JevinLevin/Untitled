using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class SavingManager : MonoBehaviour
{
    public static SavingManager Instance {get; private set;}

    [Header("Debugging")]
    [SerializeField] private bool disableSaving = false;
    [SerializeField] private bool initialiseDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileID = false;
    [SerializeField] private string testSelectedProfileID = "";

    [Header("File Storage Settings")]
    [SerializeField] private string playerFileName;
    [SerializeField] private string worldFileName;
    [SerializeField] private bool useEncryption;

    private FileDataHandler gameDataHandler;

    private string selectedProfileID = "";

    public GameDataMaster gameDataMaster;
    private List<ISaving> savingObjects;

    public List<ItemScriptableObject> ItemList;
    public static Dictionary<string, ItemScriptableObject> ItemDict = new Dictionary<string, ItemScriptableObject>();

    [Button]
    public void FindItems()
    {
        ItemList = Resources.LoadAll<ItemScriptableObject>("Items").ToList();
        ItemDict.Clear();
        foreach(ItemScriptableObject item in ItemList)
        {
            ItemDict.Add(item.ItemID, item);
        }
    }

    void Awake()
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

        if(disableSaving)
        {
            Debug.LogWarning("SAVING IS DISABLED");
        }
        
        FindItems();
        gameDataHandler = new FileDataHandler(Application.persistentDataPath, playerFileName, worldFileName, useEncryption);

        SetSelectedProfileID();

        
    }

    private void SetSelectedProfileID()
    {
        selectedProfileID = gameDataHandler.GetMostRecentlyUpdatedProfileID();
        if(overrideSelectedProfileID)
        {
            selectedProfileID = testSelectedProfileID;
            Debug.LogWarning("TESTING PROFILE ID " + selectedProfileID);
        }
    }

    public void DeleteGame(string profileID)
    {
        gameDataHandler.Delete(profileID);

        SetSelectedProfileID();

        LoadGame();
        
    }

    public void NewGame()
    {
        gameDataMaster = new GameDataMaster();

        gameDataMaster.playerGameData = new PlayerGameData();
        gameDataMaster.worldGameData = new WorldGameData();
    }

    public void LoadGame()
    {
        if(disableSaving)
        {
            return;
        }

        gameDataMaster = gameDataHandler.Load(selectedProfileID); 

        if(gameDataMaster == null && initialiseDataIfNull)
        {
            NewGame();
        }

        if(gameDataMaster == null)
        {
            return;
        }

        foreach(ISaving savingObject in savingObjects)
        {
            savingObject.LoadData(gameDataMaster.playerGameData, gameDataMaster.worldGameData);
        }
    }

    public void SaveGame()
    {
        if(disableSaving)
        {
            return;
        }

        if(gameDataMaster == null)
        {
            return;
        }
        foreach(ISaving savingObject in savingObjects)
        {
            savingObject.SaveData(gameDataMaster.playerGameData, gameDataMaster.worldGameData);
        }

        gameDataMaster.playerGameData.lastUpdated = System.DateTime.Now.ToBinary();

        gameDataHandler.Save(gameDataMaster, selectedProfileID);
    }

    public void RenameProfile(string newProfileID, string oldProfileID)
    {
        gameDataHandler.Rename(newProfileID, oldProfileID);
        ChangeSelectedProfileID(newProfileID);
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
        savingObjects = FindAllSavingObjects();

        LoadGame();
    }

    public void ChangeSelectedProfileID(string newProfileID)
    {
        selectedProfileID = newProfileID;
        LoadGame();
    }

    public void ClearSelectedProfileID()
    {
        gameDataMaster = null;
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaving> FindAllSavingObjects()
    {
        IEnumerable<ISaving> savingObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<ISaving>();

            return new List<ISaving>(savingObjects);
    }

    public bool HasGameData()
    {
        return gameDataMaster != null;
    }

    public Dictionary<string, GameDataMaster> GetAllProfiles()
    {
        return gameDataHandler.LoadAllProfiles();
    }

    public bool CheckDuplicate(string name)
    {
        return gameDataHandler.CheckDuplicate(name);
    }

}
