using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Position", menuName = "ScriptableObject/Position")]
public class Position : ScriptableObject
{
    public Vector3[] PositionsOfWhites;
    public Vector3[] PositionsOfBlacks;
}
