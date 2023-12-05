using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/ShopItemInfos")]
public class ShopItemInfos : ScriptableObject
{
    public Sprite[] PieceImages;
    public int[] PieceMoney;

}
