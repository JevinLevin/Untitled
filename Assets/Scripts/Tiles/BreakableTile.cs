using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BreakableTile : MonoBehaviour
{

    [Header("Breakable Instance")]
    [SerializeField] private LootTableScriptableObject lootTable;


    [Header("Breakable Components")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private TileHealthbar healthBar;

    [Header("Attributes")]
    [SerializeField] private float hitPoints = 1;



    // DOTween
    private Tween attackTweenScale;
    private Tween attackTweenRotation;

    private float maxHitPoints;

    private void Awake()
    {
        maxHitPoints = hitPoints;

        //sprite.material = new Material(sprite.material);
    }


    public void OnHover()
    {
        healthBar.SetVisible();
    }

    public void OnMainClick()
    {
        Attack();
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
