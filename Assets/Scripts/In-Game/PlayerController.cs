using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public DiceController DiceController;
    public BotController Bot;
    public CollectionController CollectionController;

    public MiddleSlots MiddleBlack;
    public Pieces[] Slots;



    public IEnumerator PlayBlack()
    {
        DiceController.DiceObjects = DiceController.RollDices();
        int firstNum = int.Parse(DiceController.DiceObjects[0].tag);
        int secondNum = int.Parse(DiceController.DiceObjects[1].tag);
        Pieces.DiceRolled?.Invoke(firstNum, secondNum);
        yield return new WaitForSeconds(2.3f);
        IsBlackCollectable();

        if (!IsThereAnyMove(firstNum) && !IsThereAnyMove(secondNum))
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
        else
        {
            DiceController.BlacksTurn = true;
        }

    }

    public bool IsThereAnyMove(int diceNumber)
    {
        if (MiddleBlack.PiecesInMiddle.Count > 0)
        {
            for (int i = 0; i < 6; i++)
            {
                if (i == diceNumber - 1)
                {
                    if (Slots[i].PiecesInSlot.Count == 0 || Slots[i].PiecesInSlot.Count == 1 || (Slots[i].PiecesInSlot.Count > 1 && Slots[i].PiecesInSlot[0].CompareTag("Black")))
                    {
                        MiddleBlack.PiecesInMiddle[MiddleBlack.PiecesInMiddle.Count - 1].Collider.enabled = true;
                        return true;
                    }
                }
            }
            return false;
        }
        for (int i = 0; i < 24; i++)
        {
            if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("Black"))
            {
                int newSlotNum = i + diceNumber;
                if (newSlotNum <= 23
                    && (Slots[newSlotNum].PiecesInSlot.Count == 0
                        || (Slots[newSlotNum].PiecesInSlot.Count > 0 && Slots[newSlotNum].PiecesInSlot[0].CompareTag("Black"))
                        || (Slots[newSlotNum].PiecesInSlot.Count == 1)))
                {
                    return true;
                }
            }
        }
        if (IsBlackCollectable())
        {
            int slotNumber = SlotNumber(diceNumber);
            if (Slots[slotNumber].PiecesInSlot.Count > 0 && Slots[slotNumber].PiecesInSlot[0].CompareTag("Black"))
            {
                return true;
            }
            else
            {
                for (int i = slotNumber - 1; i > 17; i--)
                {
                    if (CollectionController.Collectable)
                    {
                        if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("Black"))
                        {
                            return false;
                        }
                    }
                }
                for (int i = slotNumber + 1; i < 24; i++)
                {
                    if (CollectionController.Collectable)
                    {
                        if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("Black"))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool IsBlackCollectable()
    {
        if (MiddleBlack.PiecesInMiddle.Count > 0)
        {
            CollectionController.Collectable = false;
            return false;
        }

        for (int i = 0; i<18; i++)
        {
            if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("Black"))
            {
                CollectionController.Collectable = false;
                return false;
            }
        }
        CollectionController.Collectable = true;
        return true;
    }

    public int SlotNumber (int slotOrder)
    {
        if (slotOrder == 1)
        {
            return 23;
        }
        else if (slotOrder == 2)
        {
            return 22;
        }
        else if (slotOrder == 3)
        {
            return 21;
        }
        else if (slotOrder == 4)
        {
            return 20;
        }
        else if (slotOrder == 5)
        {
            return 19;
        }
        else 
        {
            return 18;
        }
    }

    public bool NoBlackPieces()
    {
        if (MiddleBlack.PiecesInMiddle.Count > 0)
        {
            return false;
        }

        for (int i = 0; i < 24; i++)
        {
            if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("Black"))
            {
                return false;
            }
        }
        return true;
    }

}
