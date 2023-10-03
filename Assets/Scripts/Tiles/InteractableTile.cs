using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableTile : MonoBehaviour
{

    [Header("Interact Objects")]
    [SerializeField] public GameObject ClickHitbox;

    [Header("Interact Events")]
    public UnityEvent OnHover;
    public UnityEvent OnMainClick;
    public UnityEvent OnSecondaryClick;

    private Indicator indicator;

    private bool inRange;

    private void Awake()
    {
        indicator = Player.Instance.IndicatorScript;
    }

    private void Update()
    {
        RangeCheck();

        // Checks if the player is range with their mouse over the tile
        if (inRange && indicator.CheckMouse(this))
        {
            indicator.Select(this);

            OnHover?.Invoke();


            if(Input.GetMouseButtonDown(0))
            {
                OnMainClick?.Invoke();
            }


            if(Input.GetMouseButtonDown(1))
            {
                OnSecondaryClick?.Invoke();
            }
        }

        // Starts deselecting the tile if the mouse goes off or the player leaves the range
        if (indicator.ActiveObject == this.gameObject && (!inRange || !indicator.TileHovered))
        {
            indicator.StartDeselectBuffer();
        }

    }

    // Checks the player is in range of the tile
    private void RangeCheck()
    {
        float distance = Vector3.Distance (transform.position, Player.Instance.transform.position);

        if (distance < Player.Instance.range + transform.localScale.x)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }
    }

}
