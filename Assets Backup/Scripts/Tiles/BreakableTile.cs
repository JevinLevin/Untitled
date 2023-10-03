using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BreakableTile : InteractableTile
{

    [Header("Breakable Objects")]
    [SerializeField] private GameObject itemPrefab;

    [SerializeField] private LootTableScriptableObject lootTable;
    [SerializeField] private TileHealthbar healthBar;




    // DOTween
    private Tween attackTweenScale;
    private Tween attackTweenRotation;

    [Header("Attributes")]
    [SerializeField] protected float hitPoints = 1;
    private float maxHitPoints;

    protected override void Awake()
    {
        base.Awake();

        maxHitPoints = hitPoints;

        //sprite.material = new Material(sprite.material);
    }

    protected override void Update()
    {
        base.Update();

        // Checks if they're hovering over the tile
        if (indicator.TileSelected(gameObject))
        {
            healthBar.SetVisible();
            // Checks if they click the tile
            if (Input.GetMouseButtonDown(0))
                {
                    TriggerClicked();
                
                }
        }
        

    }

    private void TriggerClicked()
    {
        if (inRange)
        {
            Attack();
        }
    }

    private void Attack()
    {
        float playerDirection = Player.Instance.GetPlayerXDirection(transform.position.x);

        hitPoints -= Player.Instance.Damage;
        
        if (hitPoints <= 0)
        {
            sprite.transform.DOKill();
            BreakTile();
        }
        else
        {

            healthBar.UpdateValue(hitPoints, maxHitPoints);



            attackTweenScale = 
                sprite.transform.DOScale(1.05f, 0.075f).SetEase(Ease.InQuad).OnComplete(() =>
                sprite.transform.DOScale(1f, 0.075f));


            attackTweenRotation = 
                sprite.transform.DOLocalRotate(transform.rotation.eulerAngles + new Vector3(0.0f,0.0f,-5.0f*playerDirection), 0.075f).SetEase(Ease.InQuad).OnComplete(() =>
                sprite.transform.DOLocalRotate(new Vector3(0.0f,0.0f,0.0f), 0.075f)); 
            
        }
        
    }
    private void BreakTile()
    {
        healthBar.Destroyed();

        Destroy(gameObject);
        Player.Instance.IndicatorScript.Deselect();

        lootTable.SpawnLoot(gameObject);
    }

}
