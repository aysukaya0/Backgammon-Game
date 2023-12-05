using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using SNG.Save;

public class Pieces : MonoBehaviour
{
    public static bool Collidable = false;

    public static Action<Draggable> PieceRemoved;
    public static Action<int,int> DiceRolled;
    public static Action<int> PieceMoved;

    public List<Draggable> PiecesInSlot = new List<Draggable>();
    
    public MiddleSlots WhiteMiddle;
    public MiddleSlots BlackMiddle;

    public DiceController DiceController;
    public PlayerController Player;
    public BotController Bot;
    public CollectionController CollectionController;
    public EndGameController EndGame;

    public int FirstNumber = 0;
    public int SecondNumber = 0;
    public int Count = 0;

    public bool FirstMoved = false;
    public bool SecondMoved = false;
    public bool DoubleDice;
    

    private void OnEnable()
    {
        PieceRemoved += PieceRemovedHandler;
        DiceRolled += DetermineDiceNumbers;
        PieceMoved += CountDecreaser;
    }
    private void OnDisable()
    {
        PieceRemoved -= PieceRemovedHandler;
        DiceRolled -= DetermineDiceNumbers;
        PieceMoved -= CountDecreaser;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        int length = PiecesInSlot.Count;
        Draggable collidedDraggable = other.GetComponent<Draggable>();
        int formerSlot = collidedDraggable.Slot_number;
        

        if (collidedDraggable != null && Collidable)
        {
            if (collidedDraggable.LastPosition == collidedDraggable.transform.position)
            {
                collidedDraggable.Collider.enabled = true;
                return;
            }

            if (PiecesInSlot.Contains(collidedDraggable))
            {
                collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                    () => {
                        collidedDraggable.Collider.enabled = true;
                    });
                return;

            }
            if (collidedDraggable.IsBroken && ((int.Parse(this.name) + 1 == FirstNumber) && !FirstMoved || (int.Parse(this.name) + 1 == SecondNumber && !SecondMoved)))
            {
                if (length == 0)
                {
                    BlackMiddle.PiecesInMiddle.Remove(collidedDraggable);
                    collidedDraggable.Collider.enabled = false;
                    collidedDraggable.transform.DOMove(new Vector3(transform.position.x, 4.05f, transform.position.z), 0.5f).OnComplete(
                        () =>
                        {
                            collidedDraggable.IsBroken = false;
                            collidedDraggable.IsDraggable = true;
                            collidedDraggable.LastPosition = collidedDraggable.transform.position;
                            collidedDraggable.Collider.enabled = true;
                            PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                        });
                    PiecesInSlot.Add(collidedDraggable);
                    collidedDraggable.Slot_number = int.Parse(this.name);
                }
                else if (length == 1)
                {
                    BlackMiddle.PiecesInMiddle.Remove(collidedDraggable);
                    if (PiecesInSlot[0].CompareTag("White")) //different tags
                    {
                        collidedDraggable.IsBroken = false;
                        collidedDraggable.IsDraggable = true;
                        Draggable broken = PiecesInSlot[0];
                        PiecesInSlot.Remove(broken);
                        broken.IsBroken = true;
                        broken.IsDraggable = false;
                        WhiteMiddle.MiddleHandler(broken);

                        collidedDraggable.Collider.enabled = false;
                        collidedDraggable.transform.DOMove(new Vector3(transform.position.x, 4.05f, transform.position.z), 0.5f).OnComplete(
                        () =>
                        {
                            collidedDraggable.LastPosition = collidedDraggable.transform.position;
                            collidedDraggable.Collider.enabled = true;
                            PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                        });
                    }
                    else //same tags
                    {
                        collidedDraggable.Collider.enabled = false;
                        collidedDraggable.transform.DOMove(new Vector3(transform.position.x, 4.05f - 0.80f, transform.position.z), 0.5f).OnComplete(
                        () => {
                            collidedDraggable.IsBroken = false;
                            collidedDraggable.IsDraggable = true;
                            PiecesInSlot[0].IsDraggable = false;
                            PiecesInSlot[0].Collider.enabled = false;
                            collidedDraggable.LastPosition = collidedDraggable.transform.position;
                            collidedDraggable.Collider.enabled = true;
                            PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                        });

                    }
                    PiecesInSlot.Add(collidedDraggable);
                    BlackMiddle.PiecesInMiddle.Remove(collidedDraggable);
                    collidedDraggable.Slot_number = int.Parse(this.name);
                }
                else if (length > 1) //slot has more than one pieces
                {
                    //check the tags
                    if (PiecesInSlot[0].CompareTag("White")) //different: piece returns to the last position
                    {
                        collidedDraggable.Collider.enabled = false;
                        collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                            () => {
                                collidedDraggable.Collider.enabled = true;
                            });
                    }
                    else //same: remove from its former slot
                    {
                        BlackMiddle.PiecesInMiddle.Remove(collidedDraggable);
                        float yOffset;
                        if (length < 5)
                        {
                            PiecesInSlot[PiecesInSlot.Count - 1].IsDraggable = false;
                            PiecesInSlot[PiecesInSlot.Count - 1].Collider.enabled = false;
                            yOffset = PiecesInSlot.Count * 0.80f;
                            collidedDraggable.Collider.enabled = false;
                            collidedDraggable.transform.DOMove(new Vector3(transform.position.x, 4.05f - yOffset, transform.position.z), 0.5f).OnComplete(
                            () => {
                                collidedDraggable.IsBroken = false;
                                collidedDraggable.IsDraggable = true;
                                collidedDraggable.LastPosition = collidedDraggable.transform.position;
                                collidedDraggable.Collider.enabled = true;
                                PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                            });

                        }
                        else
                        {
                            yOffset = (PiecesInSlot.Count - 5) * 0.80f;
                            collidedDraggable.Collider.enabled = false;
                            collidedDraggable.transform.DOMove(new Vector3(transform.position.x, 3.65f - yOffset, transform.position.z), 0.5f).OnComplete(
                            () => {
                                collidedDraggable.IsBroken = false;
                                collidedDraggable.IsDraggable = true;
                                collidedDraggable.LastPosition = collidedDraggable.transform.position;
                                PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                            });

                        }
                        PiecesInSlot.Add(collidedDraggable);
                        collidedDraggable.Slot_number = int.Parse(this.name);
                    }
                }
            }

            else
            {
                if (!DoubleDice)
                {
                    if (int.Parse(this.name) - collidedDraggable.Slot_number != FirstNumber && int.Parse(this.name) - collidedDraggable.Slot_number != SecondNumber)
                    {
                        collidedDraggable.Collider.enabled = false;
                        collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                            () => {
                                collidedDraggable.Collider.enabled = true;
                            });
                        return;
                    }
                    else if (FirstMoved && int.Parse(this.name) - collidedDraggable.Slot_number == FirstNumber)
                    {
                        collidedDraggable.Collider.enabled = false;
                        collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                            () => {
                                collidedDraggable.Collider.enabled = true;
                            });
                        return;
                    }
                    else if (SecondMoved && int.Parse(this.name) - collidedDraggable.Slot_number == SecondNumber)
                    {
                        collidedDraggable.Collider.enabled = false;
                        collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                            () => {
                                collidedDraggable.Collider.enabled = true;
                            });
                        return;
                    }
                }
                else
                {
                    if (int.Parse(this.name) - collidedDraggable.Slot_number != FirstNumber)
                    {
                        collidedDraggable.Collider.enabled = false;
                        collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                            () => {
                                collidedDraggable.Collider.enabled = true;
                            });
                        return;
                    }
                }

                //check if the piece is moving in correct direction: e.g, black pieces can only go to the slots with bigger number, otherwise return to the last position
                if (collidedDraggable.CompareTag("Black") && collidedDraggable.Slot_number >= int.Parse(this.name))
                {
                    collidedDraggable.Collider.enabled = false;
                    collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                        () => {
                            collidedDraggable.Collider.enabled = true;
                        });
                }
                else if (collidedDraggable.CompareTag("White") && collidedDraggable.Slot_number <= int.Parse(this.name))
                {
                    collidedDraggable.Collider.enabled = false;
                    collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                        () => {
                            collidedDraggable.Collider.enabled = true;
                        });
                }
                else
                {
                    if (length == 0) //slot is empty: remove piece from its former slot and place it to the new one
                    {
                        PieceRemoved?.Invoke(collidedDraggable); //removing process
                        if (transform.position.y > 0)
                        {
                            collidedDraggable.Collider.enabled = false;
                            collidedDraggable.transform.DOMove(new Vector3(transform.position.x, 4.05f, transform.position.z), 0.5f).OnComplete(
                                () =>
                                {
                                    collidedDraggable.LastPosition = collidedDraggable.transform.position;
                                    collidedDraggable.Collider.enabled = true;
                                    PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                                });
                        }
                        else
                        {
                            collidedDraggable.Collider.enabled = false;
                            collidedDraggable.transform.DOMove(new Vector3(transform.position.x, -4.05f, transform.position.z), 0.5f).OnComplete(
                                () =>
                                {
                                    collidedDraggable.LastPosition = collidedDraggable.transform.position;
                                    collidedDraggable.Collider.enabled = true;
                                    PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                                });
                        }
                        PiecesInSlot.Add(collidedDraggable);
                        collidedDraggable.Slot_number = int.Parse(this.name);
                    }
                    else if (length == 1) //slot has one piece whether black or white: check if the tags suit with each other
                    {
                        //remove the piece from its former slot
                        PieceRemoved?.Invoke(collidedDraggable);
                        if ((collidedDraggable.CompareTag("Black") && PiecesInSlot[0].CompareTag("White")) || (collidedDraggable.CompareTag("White") && PiecesInSlot[0].CompareTag("Black"))) //different tags
                        {
                            Draggable broken = PiecesInSlot[0];
                            PiecesInSlot.Remove(broken);
                            broken.IsBroken = true;
                            broken.IsDraggable = false;
                            if (broken.CompareTag("White"))
                            {
                                WhiteMiddle.MiddleHandler(broken);
                            }
                            else
                            {
                                BlackMiddle.MiddleHandler(broken);
                            }
                            if (transform.position.y > 0)
                            {
                                collidedDraggable.Collider.enabled = false;
                                collidedDraggable.transform.DOMove(new Vector3(transform.position.x, 4.05f, transform.position.z), 0.5f).OnComplete(
                                () =>
                                {
                                    collidedDraggable.LastPosition = collidedDraggable.transform.position;
                                    PiecesInSlot.Add(collidedDraggable);
                                    collidedDraggable.Collider.enabled = true;
                                    PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                                });
                            }
                            else
                            {
                                collidedDraggable.Collider.enabled = false;
                                collidedDraggable.transform.DOMove(new Vector3(transform.position.x, -4.05f, transform.position.z), 0.5f).OnComplete(
                                () =>
                                {
                                    collidedDraggable.LastPosition = collidedDraggable.transform.position;
                                    PiecesInSlot.Add(collidedDraggable);
                                    collidedDraggable.Collider.enabled = true;
                                    PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                                });
                            }
                        }
                        else //same tags
                        {
                            PiecesInSlot[0].IsDraggable = false;
                            PiecesInSlot[0].Collider.enabled = false;
                            if (transform.position.y > 0)
                            {
                                collidedDraggable.Collider.enabled = false;
                                collidedDraggable.transform.DOMove(new Vector3(transform.position.x, 4.05f - 0.80f, transform.position.z), 0.5f).OnComplete(
                                () => {
                                    collidedDraggable.LastPosition = collidedDraggable.transform.position;
                                    collidedDraggable.Collider.enabled = true;
                                    PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                                });
                            }
                            else
                            {
                                collidedDraggable.Collider.enabled = false;
                                collidedDraggable.transform.DOMove(new Vector3(transform.position.x, -4.05f + 0.80f, transform.position.z), 0.5f).OnComplete(
                                () => {
                                    collidedDraggable.LastPosition = collidedDraggable.transform.position;
                                    collidedDraggable.Collider.enabled = true;
                                    PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                                });
                            }
                            PiecesInSlot.Add(collidedDraggable);
                        }
                        collidedDraggable.Slot_number = int.Parse(this.name);
                    }
                    else if (length > 1) //slot has more than one pieces
                    {
                        //check the tags
                        if ((collidedDraggable.CompareTag("Black") && PiecesInSlot[0].CompareTag("White")) || (collidedDraggable.CompareTag("White") && PiecesInSlot[0].CompareTag("Black"))) //different: piece returns to the last position
                        {
                            collidedDraggable.Collider.enabled = false;
                            collidedDraggable.transform.DOMove(collidedDraggable.LastPosition, 0.5f).OnComplete(
                                () => {
                                    collidedDraggable.Collider.enabled = true;
                                });
                        }
                        else //same: remove from its former slot
                        {
                            PieceRemoved?.Invoke(collidedDraggable);
                            float yOffset;
                            if (length < 5)
                            {
                                PiecesInSlot[PiecesInSlot.Count - 1].IsDraggable = false;
                                PiecesInSlot[PiecesInSlot.Count - 1].Collider.enabled = false;
                                yOffset = PiecesInSlot.Count * 0.80f;
                                if (transform.position.y > 0)
                                {
                                    collidedDraggable.Collider.enabled = false;
                                    collidedDraggable.transform.DOMove(new Vector3(transform.position.x, 4.05f - yOffset, transform.position.z), 0.5f).OnComplete(
                                    () => {
                                        collidedDraggable.LastPosition = collidedDraggable.transform.position;
                                        collidedDraggable.Collider.enabled = true;
                                        PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                                    });
                                }
                                else
                                {
                                    collidedDraggable.Collider.enabled = false;
                                    collidedDraggable.transform.DOMove(new Vector3(transform.position.x, -4.05f + yOffset, transform.position.z), 0.5f).OnComplete(
                                    () => {
                                        collidedDraggable.LastPosition = collidedDraggable.transform.position;
                                        collidedDraggable.Collider.enabled = true;
                                        PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                                    });
                                }
                            }
                            else
                            {
                                yOffset = (PiecesInSlot.Count - 5) * 0.80f;
                                if (transform.position.y > 0)
                                {
                                    collidedDraggable.Collider.enabled = false;
                                    collidedDraggable.transform.DOMove(new Vector3(transform.position.x, 3.65f - yOffset, transform.position.z), 0.5f).OnComplete(
                                    () => {
                                        collidedDraggable.LastPosition = collidedDraggable.transform.position;
                                        PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                                    });
                                }
                                else
                                {
                                    collidedDraggable.Collider.enabled = false;
                                    collidedDraggable.transform.DOMove(new Vector3(transform.position.x, -3.65f + yOffset, transform.position.z), 0.5f).OnComplete(
                                    () => {
                                        collidedDraggable.LastPosition = collidedDraggable.transform.position;
                                        PieceMoved?.Invoke(int.Parse(this.name) - formerSlot);
                                    });
                                }
                            }
                            PiecesInSlot.Add(collidedDraggable);
                            collidedDraggable.Slot_number = int.Parse(this.name);
                        }
                    }
                }
            }
        }
        Player.IsBlackCollectable();
    }
    void PieceRemovedHandler(Draggable draggable)
    {
        if (PiecesInSlot.Contains(draggable))
        {
            if (PiecesInSlot.Count > 5)
            {
                PiecesInSlot.Remove(draggable);
                if (transform.position.y > 0)
                {
                    PiecesInSlot[PiecesInSlot.Count - 1].transform.DOMove(new Vector3(transform.position.x, 0.85f, transform.position.z), 0.5f).OnComplete(
                        () => {
                            PiecesInSlot[PiecesInSlot.Count - 1].IsDraggable = true;
                            PiecesInSlot[PiecesInSlot.Count - 1].Collider.enabled = true;
                            PiecesInSlot[PiecesInSlot.Count - 1].LastPosition = PiecesInSlot[PiecesInSlot.Count - 1].transform.position;
                        });
                }
                else
                {
                    PiecesInSlot[PiecesInSlot.Count - 1].transform.DOMove(new Vector3(transform.position.x, -0.85f, transform.position.z), 0.5f).OnComplete(
                        () => {
                            PiecesInSlot[PiecesInSlot.Count - 1].IsDraggable = true;
                            PiecesInSlot[PiecesInSlot.Count - 1].Collider.enabled = true;
                            PiecesInSlot[PiecesInSlot.Count - 1].LastPosition = PiecesInSlot[PiecesInSlot.Count - 1].transform.position;
                        });
                }
            }
            else
            {
                PiecesInSlot.Remove(draggable);
                if (PiecesInSlot.Count == 0)
                {
                    return;
                }
                PiecesInSlot[PiecesInSlot.Count - 1].LastPosition = PiecesInSlot[PiecesInSlot.Count - 1].transform.position;
                PiecesInSlot[PiecesInSlot.Count - 1].IsDraggable = true;
                PiecesInSlot[PiecesInSlot.Count - 1].Collider.enabled = true;
            }
        }
    }

    void DetermineDiceNumbers(int firstNum, int secondNum)
    {
        FirstNumber = CollectionController.FirstNumber = firstNum;
        SecondNumber = CollectionController.SecondNumber = secondNum;
        FirstMoved = SecondMoved = CollectionController.FirstMoved = CollectionController.SecondMoved = false;
        if (FirstNumber == SecondNumber)
        {
            Count = 4;
            DoubleDice = CollectionController.DoubleDice = true;
        }
        else
        {
            Count = 2;
            DoubleDice = CollectionController.DoubleDice = false;
        }
    }

    void CountDecreaser(int difference)
    {
        bool noRemaining = Player.NoBlackPieces();
        if (noRemaining && this.name == "0")
        {
            DiceController.BlacksTurn = false;
            DiceController.DiceObjects[0].transform.DOScale(0, 0.4f).OnComplete(
            () => {
                DiceController.DiceObjects[0].transform.DOKill();
                Destroy(DiceController.DiceObjects[0]);
            });

            DiceController.DiceObjects[1].transform.DOScale(0, 0.4f).OnComplete(
                () => {
                    DiceController.DiceObjects[1].transform.DOKill();
                    Destroy(DiceController.DiceObjects[1]);
                });

            SaveGame.Instance.PlayerData.ChangeMoney(200);
            SaveGame.Instance.PlayerData.ChangeExperience(60);
            EndGame.OpenWinScreen();
            return;

        }
        Player.IsBlackCollectable();

        if (CollectionController.FirstMoved && CollectionController.SecondMoved && this.name == "0")
        {
            DiceController.BlacksTurn = false;
            DiceController.DiceObjects[0].transform.DOScale(0, 0.4f).OnComplete(
            () => {
                Destroy(DiceController.DiceObjects[0]);
            });

            DiceController.DiceObjects[1].transform.DOScale(0, 0.4f).OnComplete(
                () => {
                    Destroy(DiceController.DiceObjects[1]);
                });
            Bot.PlayBot();
            return;
        }

        else if (difference == FirstNumber)
        {
            FirstMoved = CollectionController.FirstMoved = true;
            if (!Player.IsThereAnyMove(SecondNumber) && this.name == "0")
            {
                DiceController.BlacksTurn = false;
                DiceController.DiceObjects[0].transform.DOScale(0, 0.4f).OnComplete(
                () => {
                    Destroy(DiceController.DiceObjects[0]);
                });

                DiceController.DiceObjects[1].transform.DOScale(0, 0.4f).OnComplete(
                    () => {
                        Destroy(DiceController.DiceObjects[1]);
                    });
                Bot.PlayBot();
                return;
            }
        }
        else if (difference == SecondNumber)
        {
            SecondMoved = CollectionController.SecondMoved = true;
            if (!Player.IsThereAnyMove(FirstNumber) && this.name == "0")
            {
                DiceController.BlacksTurn = false;
                DiceController.DiceObjects[0].transform.DOScale(0, 0.4f).OnComplete(
                () => {
                    Destroy(DiceController.DiceObjects[0]);
                });

                DiceController.DiceObjects[1].transform.DOScale(0, 0.4f).OnComplete(
                    () => {
                        Destroy(DiceController.DiceObjects[1]);
                    });
                Bot.PlayBot();
                return;
            }
        }

        Count--;
        
        if (Count == 0 && this.name == "0")
        {
            DiceController.BlacksTurn = false;
            DiceController.DiceObjects[0].transform.DOScale(0, 0.4f).OnComplete(
            () => {
                Destroy(DiceController.DiceObjects[0]);
            });

            DiceController.DiceObjects[1].transform.DOScale(0, 0.4f).OnComplete(
                () => {
                    Destroy(DiceController.DiceObjects[1]);
                });
            Bot.PlayBot();
        }
    }
}