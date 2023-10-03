using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using DG.Tweening;

public class Player : MonoBehaviour, ISaving
{

    private Vector2 movement;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private GameObject indicator;
    public GameObject Indicator 
    {
        get { return indicator;}
        set { indicator = value;}
    }
    [SerializeField] private Inventory inventory;
    public Inventory Inventory 
    {
        get { return inventory;}
        set { inventory = value;}
    }
    public Indicator IndicatorScript {get; set;}
    [SerializeField] private GameObject itemPrefab;
    public GameObject ItemPrefab 
    {
        get { return itemPrefab;}
        set { itemPrefab = value;}
    }

    [Header("Attributes")]
    [SerializeField] private float speed = 8;
    [SerializeField] private float damage = 1;
    [SerializeField] public float range = 0.75f;
    public float Damage
    {
        get {return damage;}
        set {damage = value;}
    }

    public static Player Instance { get; private set; }


    public float MovementDirection {get; set;}

    public void LoadData(PlayerGameData playerData, WorldGameData worldData)
    {
        inventory.invList = playerData.playerInventory;
        inventory.SetupDictionary(playerData.playerInventory);

        transform.position = playerData.playerPosition;
    }

    public void SaveData(PlayerGameData playerData, WorldGameData worldData)
    {
        playerData.playerInventory = inventory.invList;
        playerData.playerPosition = transform.position;
    }

    void OnEnable()
    {
        Instance = this;
    }
    void OnDisable()
    {
        Instance = null;
    }

    void Awake()
    {
        IndicatorScript = Indicator.GetComponent<Indicator>();
    }


    private void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();

        if(movement.x != 0)
        {
            MovementDirection = movement.x < 0 ? -1 : 1;
        }

    }

    void FixedUpdate()
    {
        
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    public void PickupItem(string itemID, int itemCount)
    {
        
        SavingManager.ItemDict.TryGetValue(itemID, out ItemScriptableObject item);

        for(int i = 0; i < itemCount; i++)
        {
            inventory.AddToInventory(itemID);
        }

        if (UIManager.Instance.InvActive)
        {
        UIManager.Instance.Inventory.UpdateUI();
        }
        
        GameManager.Instance.ItemNotifications.NewItem(item.ItemSprite, itemCount);
    }

    public float GetPlayerXDirection(float Xorigin)
    {
        float direction = 1;

        float distanceX = transform.position.x - Xorigin;
        if (distanceX > 0)
        {
            direction = -1;
        }

        return direction;

    }

}
