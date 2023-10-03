using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberManager : MonoBehaviour
{


    public static string DisplayNumber(float num)
    {
        float value = num;
        float remainder = 0;
        string unit;
        if(num >= 1000)
        {
            value = Mathf.Floor(num / 1000);
            remainder = Mathf.Floor((num % 1000) / 100);
            unit = "k";
            return new string(value.ToString() + "." + remainder.ToString() + unit);
        }

        return new string(value.ToString());

        

    }

}
