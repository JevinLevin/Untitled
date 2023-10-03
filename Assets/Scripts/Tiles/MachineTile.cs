using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

public class MachineTile : MonoBehaviour, ISaving
{

    [Header("Object Instance")]
    public string id;
    [Button]
    private void GenerateGUID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    [Header("Machine Instance")]
    [SerializeField] public LootTableScriptableObject lootTable;

    [Header("Machine Components")]
    [SerializeField] private Image sprite;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Image progressWheel;
    [AssetsOnly] [SerializeField] private GameObject machineUIPrefab;
    [SerializeField] private GameObject machineUIPanel;

    [Header("Attributes")]
    [SerializeField] private float generateTimeMax = 5;
    [SerializeField] private float machineRestartDelay = 0.15f;
    [SerializeField] private float capacity = 100;

    [Header("Upgrades")]
    [SerializeField] private UpgradeInterface upgradeUI;
    [ReadOnly]public float generateTimeMultiplyer = 1;
    [ReadOnly]public float capacityMultiplyer = 1;

    public Dictionary<string, MachineUI> machineDictionary = new Dictionary<string, MachineUI>();
    private bool machineFinished;
    private float generateTime;
    private Tweener machinePopTween;
    private Tweener machineFullTween;
    private bool machineFull;
    private bool inMenu;

    public void LoadData(PlayerGameData playerData, WorldGameData worldData)
    {
        MachineTileData machineData;
        if(!worldData.machineTiles.TryGetValue(id, out machineData))
        {
            machineData = new MachineTileData();
        }

        // Loads inventory
        inventory.invList = machineData.machineInventory;
        inventory.SetupDictionary(machineData.machineInventory);

        inventory.Count = machineData.inventoryCount;
        // Displays full UI if the machine is already full
        if(CheckFull())
        {
            StartFull();
        }

        generateTime = machineData.generateTime;

        UpdateUI();
    }

    public void SaveData(PlayerGameData playerData, WorldGameData worldData)
    {
        if(worldData.machineTiles.ContainsKey(id))
        {
            worldData.machineTiles.Remove(id);
        }

        MachineTileData saveMachineData = new MachineTileData();
        saveMachineData.SetMachineTileData(inventory.invList, inventory.Count, generateTime);
        worldData.machineTiles.Add(id, saveMachineData); 
    }


    protected void Update()
    {
        if (inventory.Count < capacity * capacityMultiplyer)
        {
            if(machineFull)
            {
                EndFull();
            }
            Generate();
        }

        if (inMenu && Input.GetKeyDown(KeyCode.Escape))
        {
            upgradeUI.gameObject.SetActive(false);
            inMenu = false;
        }

    }

    public void OnMainClick()
    {
        Collect();
    }

    public void OnSecondaryClick()
    {
        if(!inMenu)
        {
            upgradeUI.Activate();
            inMenu = true;
        }
    }

    private void Generate()
    {
        generateTime += Time.deltaTime;

        float finalMaxTime = generateTimeMax / generateTimeMultiplyer;

        // The delay is added so that the progress bar smoothly resets
        if (generateTime >= finalMaxTime - machineRestartDelay && !machineFinished) 
        {
            Finish();
        }
        if (generateTime >= finalMaxTime)
        {
            generateTime = 0;
            machineFinished = false;
            ResetLoadingCircle();
        }

        progressWheel.fillAmount = generateTime/(finalMaxTime - machineRestartDelay); 

        if(CheckFull())
        {
            StartFull();
        }

    }

    private bool CheckFull()
    {
        return inventory.Count >= capacity * capacityMultiplyer;
    }

    private void StartFull()
    {
        machineFull = true;

        progressWheel.fillAmount = 1.0f;
        machineFullTween = progressWheel.DOColor(GameManager.Instance.colourLightRed, 0.1f).OnComplete(()=> 
            progressWheel.DOFade(0.0f, 1f).SetEase(Ease.InSine).OnComplete(()=>
            progressWheel.DOFade(255.0f, 1f).SetEase(Ease.OutSine)).SetLoops(-1));
    }

    private void EndFull()
    {
        machineFull = false;

        machineFullTween.Kill();

        ResetLoadingCircle();

    }

    private void ResetLoadingCircle()
    {  
        progressWheel.DOKill();
        progressWheel.color = new Color(1.0f,1.0f,1.0f,0.4f);
    }

    private void Finish()
    {
        machineFinished = true;

        List<ItemScriptableObject> drops;
        drops = lootTable.GenerateLoot();

        foreach(ItemScriptableObject item in drops)
        {
            inventory.AddToInventory(item.ItemID, 1);
        }

        UpdateUI();

        progressWheel.DOFade(0.0f, machineRestartDelay);

        machinePopTween.Complete();
        machinePopTween = sprite.transform.DOPunchScale(new Vector3(0.1f,0.1f,0.1f), 0.2f, 3, 0f); 
    }

    private void Collect()
    {
        //foreach(var item in inventory.invList)
        for (int i = 0; i < inventory.invList.Count; i++)
        {
            for(int j = 1; j <= inventory.invList[i].StackSize; j++)
            {
                //inventory.invList[i].ItemType.Drop(transform.position);
                ItemSpawn.NewItem(inventory.invList[i].ItemID, transform.position);
            }
        }
        inventory.Clear();
        foreach(Transform child in machineUIPanel.transform)
        {
            Destroy(child.gameObject);
        }
        machineDictionary.Clear();



    }

    private void UpdateUI()
    {
        foreach(InventoryItem item in inventory.invList)
        {
            if(machineDictionary.TryGetValue(item.ItemID, out MachineUI icon))
            {
                icon.ChangeCount(item.StackSize);
            }
            else 
            {
                MachineUI newIcon = Instantiate(machineUIPrefab, transform.position, Quaternion.identity, machineUIPanel.transform).GetComponent<MachineUI>();
                newIcon.Setup(item.ItemID, item.StackSize);
                machineDictionary.Add(item.ItemID, newIcon);
            }
            
        }

    }
}
