using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectionController : MonoBehaviour
{
    public bool Collectable = false;
    public bool FirstMoved = false;
    public bool SecondMoved = false;
    public bool DoubleDice = false;
    public int FirstNumber;
    public int SecondNumber;
    public Pieces[] Slots;

    void OnTriggerEnter2D(Collider2D other)
    {
        Draggable collidedDraggable = other.GetComponent<Draggable>();
        if (collidedDraggable != null && Collectable)
        {
            int slotOrder = SlotOrder(collidedDraggable);
            if (!DoubleDice)
            {
                if (slotOrder == FirstNumber && !FirstMoved) //success
                {
                    collidedDraggable.transform.DOScale(0, 0.4f).OnComplete(
                        () => {
                            Slots[collidedDraggable.Slot_number].PiecesInSlot.Remove(collidedDraggable);
                            if (Slots[collidedDraggable.Slot_number].PiecesInSlot.Count > 0)
                            {
                                if (Slots[collidedDraggable.Slot_number].PiecesInSlot.Count >= 5)
                                {
                                    Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].transform.DOMove(collidedDraggable.LastPosition, 0.4f);
                                    Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].LastPosition = collidedDraggable.LastPosition;
                                }
                                Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].IsDraggable = true;
                                Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].Collider.enabled = true;
                            }
                            Destroy(other.gameObject);
                            Pieces.PieceMoved?.Invoke(FirstNumber);
                        });

                }
                else if (slotOrder == SecondNumber && !SecondMoved) //success
                {
                    collidedDraggable.transform.DOScale(0, 0.4f).OnComplete(
                        () => {
                            Slots[collidedDraggable.Slot_number].PiecesInSlot.Remove(collidedDraggable);
                            if (Slots[collidedDraggable.Slot_number].PiecesInSlot.Count > 0)
                            {
                                if (Slots[collidedDraggable.Slot_number].PiecesInSlot.Count >= 5)
                                {
                                    Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].transform.DOMove(collidedDraggable.LastPosition, 0.4f);
                                    Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].LastPosition = collidedDraggable.LastPosition;
                                }
                                Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].IsDraggable = true;
                                Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].Collider.enabled = true;
                            }
                            Destroy(other.gameObject);
                            Pieces.PieceMoved?.Invoke(SecondNumber);
                        });
                }
                else if (!SecondMoved && slotOrder < SecondNumber)
                {
                    for (int i = collidedDraggable.Slot_number - 1; i > 17; i--)
                    {
                        if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("Black"))
                        {
                            collidedDraggable.Collider.enabled = false;
                            collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                                () => {
                                    collidedDraggable.Collider.enabled = true;
                                });
                            return;
                        }
                    }
                    collidedDraggable.transform.DOScale(0, 0.4f).OnComplete(
                        () => {
                            Slots[collidedDraggable.Slot_number].PiecesInSlot.Remove(collidedDraggable);
                            if (Slots[collidedDraggable.Slot_number].PiecesInSlot.Count > 0)
                            {
                                if (Slots[collidedDraggable.Slot_number].PiecesInSlot.Count >= 5)
                                {
                                    Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].transform.DOMove(collidedDraggable.LastPosition, 0.4f);
                                    Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].LastPosition = collidedDraggable.LastPosition;
                                }
                                Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].IsDraggable = true;
                                Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].Collider.enabled = true;
                            }
                            Destroy(other.gameObject);
                            Pieces.PieceMoved?.Invoke(SecondNumber);
                        });
                }
                else if (!FirstMoved && slotOrder < FirstNumber)
                {
                    for (int i = collidedDraggable.Slot_number - 1; i > 17; i--)
                    {
                        if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("Black"))
                        {
                            collidedDraggable.Collider.enabled = false;
                            collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                                () => {
                                    collidedDraggable.Collider.enabled = true;
                                });
                            return;
                        }
                    }
                    collidedDraggable.transform.DOScale(0, 0.4f).OnComplete(
                        () => {
                            Slots[collidedDraggable.Slot_number].PiecesInSlot.Remove(collidedDraggable);
                            if (Slots[collidedDraggable.Slot_number].PiecesInSlot.Count > 0)
                            {
                                if (Slots[collidedDraggable.Slot_number].PiecesInSlot.Count >= 5)
                                {
                                    Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].transform.DOMove(collidedDraggable.LastPosition, 0.4f);
                                    Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].LastPosition = collidedDraggable.LastPosition;
                                }
                                Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].IsDraggable = true;
                                Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].Collider.enabled = true;
                            }
                            Destroy(other.gameObject);
                            Pieces.PieceMoved?.Invoke(FirstNumber);
                        });
                }
                else if (!FirstMoved && !SecondMoved && slotOrder < FirstNumber && slotOrder < SecondNumber)
                {
                    for (int i = collidedDraggable.Slot_number - 1; i > 17; i--)
                    {
                        if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("Black"))
                        {
                            collidedDraggable.Collider.enabled = false;
                            collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                                () => {
                                    collidedDraggable.Collider.enabled = true;
                                });
                            return;
                        }
                    }
                    collidedDraggable.transform.DOScale(0, 0.4f).OnComplete(
                        () => {
                            Slots[collidedDraggable.Slot_number].PiecesInSlot.Remove(collidedDraggable);
                            if (Slots[collidedDraggable.Slot_number].PiecesInSlot.Count > 0)
                            {
                                if (Slots[collidedDraggable.Slot_number].PiecesInSlot.Count >= 5)
                                {
                                    Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].transform.DOMove(collidedDraggable.LastPosition, 0.4f);
                                    Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].LastPosition = collidedDraggable.LastPosition;
                                }
                                Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].IsDraggable = true;
                                Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].Collider.enabled = true;
                            }
                            Destroy(other.gameObject);
                            if (FirstMoved)
                            {
                                Pieces.PieceMoved?.Invoke(SecondNumber);
                            }
                            else
                            {
                                Pieces.PieceMoved?.Invoke(FirstNumber);
                            }
                        });
                    
                }
                else
                {
                    collidedDraggable.Collider.enabled = false;
                    collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                        () => {
                            collidedDraggable.Collider.enabled = true;
                        });
                }
            }
            else
            {
                if (slotOrder == FirstNumber)
                {
                    collidedDraggable.transform.DOScale(0, 0.4f).OnComplete(
                        () => {
                            Slots[collidedDraggable.Slot_number].PiecesInSlot.Remove(collidedDraggable);
                            if (Slots[collidedDraggable.Slot_number].PiecesInSlot.Count > 0)
                            {
                                if (Slots[collidedDraggable.Slot_number].PiecesInSlot.Count >= 5)
                                {
                                    Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].transform.DOMove(collidedDraggable.LastPosition, 0.4f);
                                    Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].LastPosition = collidedDraggable.LastPosition;
                                }
                                Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].IsDraggable = true;
                                Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].Collider.enabled = true;
                            }
                            Destroy(other.gameObject);
                            Pieces.PieceMoved?.Invoke(FirstNumber);
                        });
                }
                else if (slotOrder < FirstNumber)
                {
                    for (int i = collidedDraggable.Slot_number - 1; i > 17; i--)
                    {
                        if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("Black"))
                        {
                            collidedDraggable.Collider.enabled = false;
                            collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                                () => {
                                    collidedDraggable.Collider.enabled = true;
                                });
                            return;
                        }
                    }
                    collidedDraggable.transform.DOScale(0, 0.4f).OnComplete(
                        () => {
                            Slots[collidedDraggable.Slot_number].PiecesInSlot.Remove(collidedDraggable);
                            if (Slots[collidedDraggable.Slot_number].PiecesInSlot.Count > 0)
                            {
                                if (Slots[collidedDraggable.Slot_number].PiecesInSlot.Count >= 5)
                                {
                                    Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].transform.DOMove(collidedDraggable.LastPosition, 0.4f);
                                    Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].LastPosition = collidedDraggable.LastPosition;
                                }
                                Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].IsDraggable = true;
                                Slots[collidedDraggable.Slot_number].PiecesInSlot[Slots[collidedDraggable.Slot_number].PiecesInSlot.Count - 1].Collider.enabled = true;
                            }
                            Destroy(other.gameObject);
                            Pieces.PieceMoved?.Invoke(FirstNumber);
                        });
                }
                else 
                {
                    collidedDraggable.Collider.enabled = false;
                    collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                        () => {
                            collidedDraggable.Collider.enabled = true;
                        });
                }
            }
        }
        else if (collidedDraggable != null && !Collectable)
        {
            collidedDraggable.Collider.enabled = false;
            collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                () => {
                    collidedDraggable.Collider.enabled = true;
                });
        }
    }
    int SlotOrder(Draggable draggable)
    {
        int slotNum = draggable.Slot_number;
        if (slotNum == 23)
        {
            return 1;
        }
        else if (slotNum == 22)
        {
            return 2;
        }
        else if (slotNum == 21)
        {
            return 3;
        }
        else if (slotNum == 20)
        {
            return 4;
        }
        else if (slotNum == 19)
        {
            return 5;
        }
        else if (slotNum == 18)
        {
            return 6;
        }
        return -1;
    }
}
