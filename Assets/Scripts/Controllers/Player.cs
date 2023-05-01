using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public uint StarsAmount;
    public uint MaxHeart;
    public uint Hearts;

    public Player(uint maxHearts)
    {
        StarsAmount = 0;
        MaxHeart = maxHearts;
        Hearts = MaxHeart;
    }
}
