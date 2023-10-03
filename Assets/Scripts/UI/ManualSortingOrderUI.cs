using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualSortingOrderUI : MonoBehaviour
{

    [SerializeField] private Canvas canvas;
    private bool sorted = false;

    void Update()
    {
        float playerY = Player.Instance.transform.position.y;

        float elementY = canvas.transform.position.y;


        if(playerY > elementY && !sorted)
        {
            canvas.sortingOrder = 1;
            sorted = true;
        }
        if(playerY <= elementY && sorted)
        {
            canvas.sortingOrder = 0;
            sorted = false;
        }
    }
}
