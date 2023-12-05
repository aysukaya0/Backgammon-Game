using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Draggable : MonoBehaviour
{
    public BoxCollider2D Collider;
    public SpriteRenderer Sprite;
    public bool IsDragging;
    public int Slot_number;
    public bool IsDraggable;
    public bool IsBroken = false;
    public Vector3 LastPosition;
    
}