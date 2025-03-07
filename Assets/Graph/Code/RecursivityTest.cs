using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursivityTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int accumulator = 0;
        for (int i = 0; i <= 4; i++)
        {
            accumulator++;
        }
        Debug.Log("Acummulation" + accumulator);

        Debug.Log("Recursivity acummulation" + Accumulation(0));
    }

    protected int Accumulation(int value)
    {
        if (value <= 4)
        {
            return Accumulation(++value);
        }
        else
        {
            return value;
        }
    }

}
