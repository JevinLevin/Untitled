using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{

    public bool TileHovered {get; set;}
    public bool IndicatorActive {get; set;}
    public GameObject activeObject {get; set;}

    private SpriteRenderer indicatorSprite;
    private float deselectBuffer;

    void Update()
    {

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

    public void CheckMouse(InteractableTile tile)
    {
        // Checks if the mouse is over the tile
        Ray clickCheck = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D clickHit = Physics2D.Raycast(clickCheck.origin, clickCheck.direction);

        if (clickHit.collider != null && clickHit.collider.gameObject == tile.ClickHitbox) {
            if (!TileHovered)
            {
                TileHovered = true;
                Select(tile);
            }
        }
        else
        {
            TileHovered = false;
        }
    }

    public void Select(InteractableTile tile)
    {
        transform.position = tile.transform.position;
        gameObject.SetActive(true);
        indicatorSprite.size = tile.transform.localScale;

        activeObject = tile.gameObject;

        IndicatorActive = true;

        deselectBuffer = 0.0f;
    }

    public void Deselect()
    {
        gameObject.SetActive(false);

        TileHovered = false;

        activeObject = null;

        IndicatorActive = false;

    }

    public void StartDeselectBuffer()
    {
        if (deselectBuffer <= 0.0f)
        {
        deselectBuffer = 0.2f;
        }
    }

    public bool TileSelected(GameObject tile)
    {
        if (activeObject == tile && TileHovered)
        {
            return true;
        }
        return false;
    }



}
