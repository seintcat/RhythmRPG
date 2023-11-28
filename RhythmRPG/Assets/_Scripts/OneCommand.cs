using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneCommand
{
    public int groundIndex;
    public int chargeCount;

    public OneCommand(int _groundIndex)
    {
        groundIndex = _groundIndex;
        chargeCount = 0;
    }
}
