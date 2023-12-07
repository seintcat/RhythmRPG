using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterGroup 
{
    public void MoveClockwise();
    public void MoveCounterClockwise();

    public void SwapUpSideDown();
    public void SwapLeftSideRight();

    public void Move(PositionChangeMethod method)
    {
        switch (method)
        {
            case PositionChangeMethod.Clockwise:
                MoveClockwise();
                break;
            case PositionChangeMethod.CounterClockwise:
                MoveCounterClockwise();
                break;
            case PositionChangeMethod.FrontBack:
                SwapLeftSideRight();
                break;
            case PositionChangeMethod.UpDown:
                SwapUpSideDown();
                break;
            default:
                break;
        }
    }
}
