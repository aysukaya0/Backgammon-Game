using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BotCollector : MonoBehaviour
{
    public Pieces[] Slots;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Draggable collidedDraggable = other.GetComponent<Draggable>();

        if (collidedDraggable != null)
        {
            if (collidedDraggable.CompareTag("Black"))
            {
                collidedDraggable.Collider.enabled = false;
                collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                    () => {
                        collidedDraggable.Collider.enabled = true;
                    });
            }
            else
            {
                collidedDraggable.transform.DOScale(0, 0.4f).OnComplete(
                        () => {
                            Destroy(other.gameObject);
                        });
            }
        }
    }
}
