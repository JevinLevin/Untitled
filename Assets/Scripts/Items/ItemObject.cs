using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemObject : MonoBehaviour
{

    public string itemID;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private Sequence spawnTween;
    private float distance;
    private Vector3 collectPosition;
    private Vector3 mergePosition;
    private ItemObject mergeTarget;

    private bool validItem = true;
    private bool isPickup;
    private float pickupTime = 0.0f;

    [Header("Pickup")]
    [SerializeField] private float pickupStartDistance = 2;
    [SerializeField] private float pickupEndDistance;
    [SerializeField] private float pickupSpeedMin;
    [SerializeField] private float pickupSpeed;
    [SerializeField] private float pickupTimeMax = 0.25f;
    [SerializeField] private float pickupCollectTime;
    [SerializeField] private AnimationCurve pickupCurve;

    [Header("Merge")]

    [SerializeField] private float mergeSpeedMin;
    [SerializeField] private float mergeSpeedMax;
    private float mergeTime;
    [SerializeField] private float mergeTimeMax;
    [SerializeField] private AnimationCurve mergeCurve;
    private int itemCount;
    public bool IsMerging {get; set;}

    private Vector3 pickupStart;

    void Awake()
    {
      //spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      itemCount = 1;
    }

    void Update()
    {

        if (!isPickup && InRange(pickupStartDistance))
        {
            StartPickup(); 
        }

        if (isPickup)
        {
            Pickup();
        }
        else
        {
            if(IsMerging)
            {
            Merging();
            }
        }

        if(!isPickup && !IsMerging && !spawnTween.active)
        {
            transform.position = spriteRenderer.transform.position;
            spriteRenderer.transform.localPosition = Vector3.zero;
        }


        
    }

    public void Spawn(int count)
    {
        CheckMerge();
        GameManager.Items.Add(this);

        SavingManager.ItemDict.TryGetValue(itemID, out ItemScriptableObject item);

        spriteRenderer.sprite = item.ItemSprite; 

        itemCount = count;


        if (!IsMerging)
        {
        // Determines the movement
            // Chooses left or right direcition
            bool goLeft;
            goLeft = Random.value < 0.5;
            float direction = goLeft ? -1 : 1;

            if (InRange(pickupStartDistance))
            {
                direction = Player.Instance.GetPlayerXDirection(transform.position.x) * -1;

            }


            // Random Position
            transform.position += new Vector3(Random.Range(0.0f,0.25f*direction),Random.Range(0.0f,0.25f*direction));

            Vector3 spriteTransform = spriteRenderer.transform.localPosition;
            spawnTween = spriteRenderer.transform.DOLocalJump(new Vector3(spriteTransform.x + Random.Range(0.1f*direction,0.5f*direction),spriteTransform.y + Random.Range(-0.25f,0.25f)), 0.3f, 1, 0.3f, false);
        }
    }

    private void CheckMerge()
    {
        foreach(ItemObject item in GameManager.Items)
        {
            if(!item.IsMerging && item.itemID == itemID && Vector3.Distance(transform.position, item.transform.position) < 3.0f)
            {
                IsMerging = true;
                mergeTime = 0;
                mergePosition = item.transform.position;
                mergeTarget = item;
            }
        }
    }

    private void Merging()
    {

        mergeTime += Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, mergePosition, Mathf.Lerp(mergeSpeedMin,mergeSpeedMax,pickupCurve.Evaluate(mergeTime/mergeTimeMax)) * Time.deltaTime);

        if(Vector3.Distance(transform.position, mergePosition) < 0.1f && validItem)
        {
            Merge();
        }

    }
    
    private void Merge()
    {
        DOTween.Kill(transform);
        mergeTarget.AddItemCount(itemCount);
        Delete();
    }

    public void AddItemCount(int otherCount)
    {
        itemCount += otherCount;
    }

    private void StartPickup()
    {
        //DOTween.Kill(transform);
        isPickup = true;

        pickupTime = 0.0f;

        pickupStart = transform.position;
    }

    private void Pickup()
    {

        pickupTime += Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, collectPosition, Mathf.Lerp(pickupSpeedMin,pickupSpeed,pickupCurve.Evaluate(pickupTime/pickupTimeMax)) * Time.deltaTime);

        if (InRange(pickupEndDistance) && validItem)
        {
            Player.Instance.PickupItem(itemID, itemCount);

            Delete();
        }
    }

    private void Delete()
    {
    spawnTween.Kill();
    validItem = false;
    GameManager.Items.Remove(this);
    spriteRenderer.DOFade(0.0f, pickupCollectTime).OnComplete(() =>
    Destroy(gameObject));
    }

    private bool InRange(float range)
    {
        collectPosition = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y + 0.35f, Player.Instance.transform.position.z);
        distance = Vector3.Distance (transform.position, collectPosition);

        if (distance < range)
        {
            return true;
        }
        return false;
    }
}
