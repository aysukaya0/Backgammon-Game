using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SideSlots : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Draggable collidedDraggable = other.GetComponent<Draggable>();

        if (collidedDraggable != null)
        {
            collidedDraggable.Collider.enabled = false;
            collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                () => {
                    collidedDraggable.Collider.enabled = true;
                    collidedDraggable.LastPosition = collidedDraggable.transform.position;
                });
        }
    }
}
