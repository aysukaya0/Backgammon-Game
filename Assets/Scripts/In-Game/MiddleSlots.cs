using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MiddleSlots : MonoBehaviour
{
    public List<Draggable> PiecesInMiddle = new List<Draggable>();

    public void MiddleHandler(Draggable other)
    {
        if (other != null && other.IsBroken)
        {
            other.Slot_number = -1;
            float yOffset = (PiecesInMiddle.Count) * 0.80f;
            other.IsDraggable = false;
            other.Collider.enabled = false;
            if (other.CompareTag("White"))
            {
                other.transform.DOMove(new Vector3(-0.670f, 0.86f + yOffset, 0f), 0.5f).OnComplete(
                    () => {
                        other.LastPosition = other.transform.position;
                        PiecesInMiddle.Add(other);
                    });
            }

            else
            {
                other.transform.DOMove(new Vector3(-0.670f, -0.86f - yOffset, 0f), 0.5f).OnComplete(
                    () => {
                        other.LastPosition = other.transform.position;
                        PiecesInMiddle.Add(other);
                    });
            }
            
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Draggable collidedDraggable = other.GetComponent<Draggable>();
        if (collidedDraggable != null && collidedDraggable.IsBroken == false)
        {
            collidedDraggable.Collider.enabled = false;
            collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                () =>
                {
                    collidedDraggable.Collider.enabled = true;
                    collidedDraggable.LastPosition = collidedDraggable.transform.position;
                });
        }
        else if (collidedDraggable != null && collidedDraggable.LastPosition != collidedDraggable.transform.position && collidedDraggable.IsBroken == true)
        {
            collidedDraggable.Collider.enabled = false;
            collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                () =>
                {
                    collidedDraggable.Collider.enabled = true;
                    collidedDraggable.LastPosition = collidedDraggable.transform.position;
                });
        }
    }
}