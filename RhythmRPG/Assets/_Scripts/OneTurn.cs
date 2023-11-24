using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Turn", fileName = "Turn", order = 0)]
public class OneTurn : ScriptableObject
{
    public List<TurnOneTick> ticks;
}

[System.Serializable]
public struct TurnOneTick
{
    public AudioClip metronomeClip;
    public float metronomeWait;
}
