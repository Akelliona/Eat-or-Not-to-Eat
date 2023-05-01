using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Create settings/Game Settings", order = 1)]
public class GameSettings : ScriptableObject
{
    public uint MaxLife;
    public float TimeForChoose;

}
