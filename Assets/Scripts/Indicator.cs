using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{

    public bool TileHovered {get; set;}
    public bool IndicatorActive {get; set;}
    public GameObject ActiveObject {get; set;}

    private SpriteRenderer indicatorSprite;
    private float deselectBuffer;

    void Update()
    {

        // If the mouse hasn't been over the tile for enough time, deselect it
        if (deselectBuffer > 0)
        {
            deselectBuffer -= Time.deltaTime;
            if (deselectBuffer <= 0)
            {
                Deselect();
            }
        }

    }

    void Awake()
    {
        indicatorSprite = Player.Instance.Indicator.GetComponent<SpriteRenderer>();
    }

    // Checks if the mouse if over the passed tile
    public bool CheckMouse(InteractableTile tile)
    {
        Ray clickCheck = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D clickHit = Physics2D.Raycast(clickCheck.origin, clickCheck.direction);

        // If the mouse is ontop of a collider, and that collider is the same as the one passed in
        if (clickHit.collider != null && clickHit.collider.gameObject == tile.ClickHitbox) 
        { 
            TileHovered = true;
            return true;
        }
        else
        {
            TileHovered = false;
            return false;
        }
    }

    public void Select(InteractableTile tile)
    {
        if(ActiveObject != tile.gameObject)
        {
            transform.position = tile.transform.position;

            gameObject.SetActive(true);

            // Resizes the indicator according to the size of the tile
            // Since the sprite is tiled the visuals should match accordingly
            indicatorSprite.size = tile.transform.localScale;

            ActiveObject = tile.gameObject;

            IndicatorActive = true;

            deselectBuffer = 0.0f;
        }
    }

    public void Deselect()
    {
        gameObject.SetActive(false);

        TileHovered = false;

        ActiveObject = null;

        IndicatorActive = false;

    }

    public void StartDeselectBuffer()
    {
        if (deselectBuffer <= 0.0f)
        {
        deselectBuffer = 0.2f;
        }
    }

    public bool TileSelected(InteractableTile tile)
    {
        if (ActiveObject == tile.gameObject && TileHovered)
        {
            return true;
        }
        return false;
    }



}
