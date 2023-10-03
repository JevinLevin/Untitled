using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTile : MonoBehaviour
{

    [Header("Interact Objects")]
    [SerializeField] protected GameObject clickHitbox;
    public GameObject ClickHitbox 
    {
        get { return clickHitbox;}
    }
    [SerializeField] protected SpriteRenderer sprite;

    protected Indicator indicator;

    protected bool inRange;

    protected virtual void Awake()
    {
        indicator = Player.Instance.IndicatorScript;
    }

    protected virtual void Update()
    {
        RangeCheck();

        if (inRange)
        {
            indicator.CheckMouse(this);
        }

        if (indicator.activeObject == this.gameObject && (!inRange || !indicator.TileHovered))
        {
            indicator.StartDeselectBuffer();
        }

    }

    protected void RangeCheck()
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
