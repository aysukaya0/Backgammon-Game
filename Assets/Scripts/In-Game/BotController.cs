using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SNG.Save;

public class BotController : MonoBehaviour
{
    public DiceController DiceController;
    public Pieces[] Slots;
    public MiddleSlots MiddleWhites;
    public MiddleSlots MiddleBlacks;
    public PlayerController Player;
    public EndGameController EndGame;

    public void PlayBot()
    {
        DiceController.BlacksTurn = false;
        StartCoroutine(PlayingBot());
        Player.IsBlackCollectable();
    }

    private IEnumerator PlayingBot()
    {
        yield return new WaitForSeconds(1f);
        DiceController.DiceObjects = DiceController.RollDices();
        yield return new WaitForSeconds(2.3f);
        int firstNumber = int.Parse(DiceController.DiceObjects[0].tag);
        int secondNumber = int.Parse(DiceController.DiceObjects[1].tag);
        if (firstNumber != secondNumber)
        {
            if (MiddleWhites.PiecesInMiddle.Count > 0)
            {
                int randomNum = Random.Range(0, 2);
                if (randomNum == 0)
                {
                    bool success = MoveTheMiddlePiece(firstNumber);
                    if (success && MiddleWhites.PiecesInMiddle.Count > 0)
                    {
                        yield return new WaitForSeconds(1f);
                        MoveTheMiddlePiece(secondNumber);
                    }
                    else if (success && MiddleWhites.PiecesInMiddle.Count == 0)
                    {
                        yield return new WaitForSeconds(1f);
                        MoveOneWhitePiece(secondNumber);
                    }
                    else if (!success)
                    {
                        MoveTheMiddlePiece(secondNumber);
                        if (MiddleWhites.PiecesInMiddle.Count == 0)
                        {
                            yield return new WaitForSeconds(1f);
                            MoveOneWhitePiece(firstNumber);
                        }
                    }
                }
                else if (randomNum == 1)
                {
                    bool success = MoveTheMiddlePiece(secondNumber);
                    if (success && MiddleWhites.PiecesInMiddle.Count > 0)
                    {
                        yield return new WaitForSeconds(1f);
                        MoveTheMiddlePiece(firstNumber);
                    }
                    else if (success && MiddleWhites.PiecesInMiddle.Count == 0)
                    {
                        yield return new WaitForSeconds(1f);
                        MoveOneWhitePiece(firstNumber);
                    }
                    else if (!success)
                    {
                        MoveTheMiddlePiece(firstNumber);
                        if (MiddleWhites.PiecesInMiddle.Count == 0)
                        {
                            yield return new WaitForSeconds(1f);
                            MoveOneWhitePiece(secondNumber);
                        }
                    }
                }
            }
            else
            {
                int randomNum = Random.Range(0, 2);

                if (randomNum == 0)
                {
                    StartCoroutine(MoveTwoWhitePiece(firstNumber, secondNumber));
                }
                else if (randomNum == 1)
                {
                    StartCoroutine(MoveTwoWhitePiece(secondNumber, firstNumber));
                }
                yield return new WaitForSeconds(1.5f);
            }
            bool noRemain = NoWhitePieces();
            if (noRemain)
            {
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

                // YOU LOSE
                SaveGame.Instance.PlayerData.ChangeExperience(20);
                EndGame.OpenLoseScreen();

            }
            else
            {
                DiceController.DiceObjects[0].transform.DOScale(0, 0.4f).OnComplete(
                () => {
                    Destroy(DiceController.DiceObjects[0]);
                });

                DiceController.DiceObjects[1].transform.DOScale(0, 0.4f).OnComplete(
                    () => {
                        Destroy(DiceController.DiceObjects[1]);
                    });
                yield return new WaitForSeconds(1.2f);
                StartCoroutine(Player.PlayBlack());
            }
        }
        else
        {
            int slotNum = Player.SlotNumber(firstNumber);
            if (MiddleWhites.PiecesInMiddle.Count > 0 && Slots[slotNum].PiecesInSlot.Count > 1 && Slots[slotNum].PiecesInSlot[0].CompareTag("Black"))
            {
                //
            }
            else
            {
                int i = 0;
                while (i < 4)
                {
                    if (MiddleWhites.PiecesInMiddle.Count > 0)
                    {
                        MoveTheMiddlePiece(firstNumber);
                    }
                    else if (IsWhiteCollectable() && (IsCollectableWithDiceNumber(firstNumber) || SmallerThanDice(firstNumber)))
                    {
                        CollectWhite(firstNumber);
                    }
                    else
                    {
                        MoveOneWhitePiece(firstNumber);
                    }
                    yield return new WaitForSeconds(0.5f);
                    i++;
                }
                yield return new WaitForSeconds(1f);
            }
            bool noRemaining = NoWhitePieces();
            if (noRemaining)
            {
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
                yield return new WaitForSeconds(1.2f);

                // YOU LOSE

                EndGame.OpenLoseScreen();

            }
            else
            {
                DiceController.DiceObjects[0].transform.DOScale(0, 0.4f).OnComplete(
                () => {
                    Destroy(DiceController.DiceObjects[0]);
                });

                DiceController.DiceObjects[1].transform.DOScale(0, 0.4f).OnComplete(
                    () => {
                        Destroy(DiceController.DiceObjects[1]);
                    });
                yield return new WaitForSeconds(1.2f);
                StartCoroutine(Player.PlayBlack());
            }
        }
    }

    public bool MoveOneWhitePiece(int diceNumber)
    {
        List<int> fromSlots = new List<int>();
        for (int i=0; i<24; i++)
        {
            if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("White"))
            {
                int newSlotNum = i - diceNumber;
                if (newSlotNum >= 0
                    && (Slots[newSlotNum].PiecesInSlot.Count == 0
                        || (Slots[newSlotNum].PiecesInSlot.Count > 0 && Slots[newSlotNum].PiecesInSlot[0].CompareTag("White"))
                        || (Slots[newSlotNum].PiecesInSlot.Count == 1)))
                {
                    fromSlots.Add(i);
                }
            }
        }
        if (fromSlots.Count == 0)
        {
            return false;
        }
        bool success = false;
        while (!success)
        {
            int index;
            int fromSlotNum;
            int newSlotNum;
            index = UnityEngine.Random.Range(0, fromSlots.Count);
            fromSlotNum = fromSlots[index];
            newSlotNum = fromSlotNum - diceNumber;
            Draggable pieceToBeMoved = Slots[fromSlotNum].PiecesInSlot[Slots[fromSlotNum].PiecesInSlot.Count - 1];
            Vector3 newPosition;
            if (Slots[newSlotNum].PiecesInSlot.Count == 0 || (Slots[newSlotNum].PiecesInSlot.Count > 0 && Slots[newSlotNum].PiecesInSlot[0].CompareTag("White")))
            {
                if (Slots[newSlotNum].PiecesInSlot.Count < 5)
                {
                    float yOffset = Slots[newSlotNum].PiecesInSlot.Count * 0.80f;
                    if (Slots[newSlotNum].transform.position.y > 0)
                    {
                        newPosition = new Vector3(Slots[newSlotNum].transform.position.x, 4.05f - yOffset, Slots[newSlotNum].transform.position.z);
                    }
                    else
                    {
                        newPosition = new Vector3(Slots[newSlotNum].transform.position.x, -4.05f + yOffset, Slots[newSlotNum].transform.position.z);
                    }
                    if (Slots[newSlotNum].PiecesInSlot.Count != 0)
                    {
                        Slots[newSlotNum].PiecesInSlot[Slots[newSlotNum].PiecesInSlot.Count - 1].IsDraggable = false;
                        Slots[newSlotNum].PiecesInSlot[Slots[newSlotNum].PiecesInSlot.Count - 1].Collider.enabled = false;
                    }
                    Slots[fromSlotNum].PiecesInSlot.Remove(pieceToBeMoved);
                    if (Slots[fromSlotNum].PiecesInSlot.Count > 0)
                    {
                        Slots[fromSlotNum].PiecesInSlot[Slots[fromSlotNum].PiecesInSlot.Count - 1].IsDraggable = true;
                        Slots[fromSlotNum].PiecesInSlot[Slots[fromSlotNum].PiecesInSlot.Count - 1].Collider.enabled = true;
                    }
                    pieceToBeMoved.Collider.enabled = false;
                    pieceToBeMoved.transform.DOMove(newPosition, 0.5f).OnComplete(
                                    () => {
                                        pieceToBeMoved.LastPosition = pieceToBeMoved.transform.position;
                                        pieceToBeMoved.Collider.enabled = true;
                                        pieceToBeMoved.Slot_number = newSlotNum;
                                        Slots[newSlotNum].PiecesInSlot.Add(pieceToBeMoved);
                                    });
                    return true;
                }
                else
                {
                    float yOffset = (Slots[newSlotNum].PiecesInSlot.Count - 5) * 0.80f;
                    if (Slots[newSlotNum].transform.position.y > 0)
                    {
                        newPosition = new Vector3(Slots[newSlotNum].transform.position.x, 3.65f - yOffset, Slots[newSlotNum].transform.position.z);
                    }
                    else
                    {
                        newPosition = new Vector3(Slots[newSlotNum].transform.position.x, -3.65f + yOffset, Slots[newSlotNum].transform.position.z);
                    }
                    if (Slots[newSlotNum].PiecesInSlot.Count != 0)
                    {
                        Slots[newSlotNum].PiecesInSlot[Slots[newSlotNum].PiecesInSlot.Count - 1].IsDraggable = false;
                        Slots[newSlotNum].PiecesInSlot[Slots[newSlotNum].PiecesInSlot.Count - 1].Collider.enabled = false;
                    }
                    Slots[fromSlotNum].PiecesInSlot.Remove(pieceToBeMoved);
                    if (Slots[fromSlotNum].PiecesInSlot.Count > 0)
                    {
                        Slots[fromSlotNum].PiecesInSlot[Slots[fromSlotNum].PiecesInSlot.Count - 1].IsDraggable = true;
                        Slots[fromSlotNum].PiecesInSlot[Slots[fromSlotNum].PiecesInSlot.Count - 1].Collider.enabled = true;
                    }
                    pieceToBeMoved.Collider.enabled = false;
                    pieceToBeMoved.transform.DOMove(newPosition, 0.5f).OnComplete(
                                    () => {
                                        pieceToBeMoved.LastPosition = pieceToBeMoved.transform.position;
                                        pieceToBeMoved.Collider.enabled = true;
                                        pieceToBeMoved.Slot_number = newSlotNum;
                                        Slots[newSlotNum].PiecesInSlot.Add(pieceToBeMoved);
                                    });
                    return true;
                }
            }
            else if (Slots[newSlotNum].PiecesInSlot.Count == 1 && Slots[newSlotNum].PiecesInSlot[0].CompareTag("Black"))
            {
                Draggable broken = Slots[newSlotNum].PiecesInSlot[0];

                if (Slots[newSlotNum].transform.position.y > 0)
                {
                    newPosition = new Vector3(Slots[newSlotNum].transform.position.x, 4.05f, Slots[newSlotNum].transform.position.z);
                }
                else
                {
                    newPosition = new Vector3(Slots[newSlotNum].transform.position.x, -4.05f, Slots[newSlotNum].transform.position.z);
                }
                Slots[fromSlotNum].PiecesInSlot.Remove(pieceToBeMoved);
                if (Slots[fromSlotNum].PiecesInSlot.Count > 0)
                {
                    Slots[fromSlotNum].PiecesInSlot[Slots[fromSlotNum].PiecesInSlot.Count - 1].IsDraggable = true;
                    Slots[fromSlotNum].PiecesInSlot[Slots[fromSlotNum].PiecesInSlot.Count - 1].Collider.enabled = true;
                }
                Slots[newSlotNum].PiecesInSlot.Remove(broken);
                float yOffset = MiddleBlacks.PiecesInMiddle.Count * 0.80f;
                Vector3 blacksPosition = new Vector3(MiddleBlacks.transform.position.x, -0.86f - yOffset, 0f);
                broken.Collider.enabled = false;
                broken.Slot_number = -1;
                broken.transform.DOMove(blacksPosition, 0.5f).OnComplete(
                    () => {
                        MiddleBlacks.PiecesInMiddle.Add(broken);
                        broken.LastPosition = blacksPosition;
                        broken.IsDraggable = false;
                        broken.IsBroken = true;
                    });
                pieceToBeMoved.Collider.enabled = false;
                pieceToBeMoved.transform.DOMove(newPosition, 0.5f).OnComplete(
                                () => {
                                    pieceToBeMoved.LastPosition = pieceToBeMoved.transform.position;
                                    pieceToBeMoved.Collider.enabled = true;
                                    pieceToBeMoved.Slot_number = newSlotNum;
                                    Slots[newSlotNum].PiecesInSlot.Add(pieceToBeMoved);
                                });
                return true;
            }
            fromSlots.RemoveAt(index);
            if (fromSlots.Count == 0)
            {
                return false;
            }
        }
        return false;
    }

    private IEnumerator MoveTwoWhitePiece(int firstDice, int secondDice)
    {
        bool collectable = IsWhiteCollectable();
        bool success;
        if (collectable)
        {
            if (firstDice != secondDice)
            {
                bool firstSuccess = IsCollectableWithDiceNumber(firstDice);
                bool secondSuccess = IsCollectableWithDiceNumber(secondDice);

                if (firstSuccess && secondSuccess)
                {
                    CollectWhite(firstDice);
                    yield return new WaitForSeconds(0.5f);
                    CollectWhite(secondDice);
                }
                else if (firstSuccess && !secondSuccess)
                {
                    CollectWhite(firstDice);
                    yield return new WaitForSeconds(0.5f);
                    secondSuccess = SmallerThanDice(secondDice);
                    if (secondSuccess)
                    {
                        CollectWhite(secondDice);
                    }
                    else
                    {
                        MoveOneWhitePiece(secondDice);
                    }
                }
                else if (secondSuccess && !firstSuccess)
                {
                    CollectWhite(secondDice);
                    yield return new WaitForSeconds(0.5f);
                    firstSuccess = SmallerThanDice(firstDice);
                    if (firstSuccess)
                    {
                        CollectWhite(firstDice);
                    }
                    else
                    {
                        MoveOneWhitePiece(firstDice);
                    }
                }
                else
                {
                    firstSuccess = SmallerThanDice(firstDice);
                    secondSuccess = SmallerThanDice(secondDice);
                    if (!firstSuccess && !secondSuccess)
                    {
                        int index = Random.Range(0, 1);
                        int dice1;
                        int dice2;
                        if (index == 0)
                        {
                            dice1 = firstDice;
                            dice2 = secondDice;
                        }
                        else
                        {
                            dice2 = firstDice;
                            dice1 = secondDice;
                        }
                        MoveOneWhitePiece(dice1);
                        yield return new WaitForSeconds(0.5f);
                        if (IsCollectableWithDiceNumber(dice2) || SmallerThanDice(dice2))
                        {
                            CollectWhite(dice2);
                        }
                        else 
                        {
                            MoveOneWhitePiece(dice2);
                        }
                    }
                    else if (firstSuccess && !secondSuccess)
                    {
                        CollectWhite(firstDice);
                        yield return new WaitForSeconds(0.5f);
                        MoveOneWhitePiece(secondDice);
                    }
                    else if (secondSuccess && !firstSuccess)
                    {
                        CollectWhite(secondDice);
                        yield return new WaitForSeconds(0.5f);
                        MoveOneWhitePiece(firstDice);
                    }
                    else
                    {
                        CollectWhite(firstDice);
                        yield return new WaitForSeconds(0.5f);
                        CollectWhite(secondDice);
                    }
                }
            }
        }
        else
        {
            success = MoveOneWhitePiece(firstDice);
            if (success)
            {
                yield return new WaitForSeconds(0.5f);
                MoveOneWhitePiece(secondDice);
            }
            else
            {
                MoveOneWhitePiece(secondDice);
                yield return new WaitForSeconds(0.5f);
                MoveOneWhitePiece(firstDice);
            }
        }
    }

    public bool MoveTheMiddlePiece(int diceNum)
    {
        int newSlotNum = -1;

        if (diceNum == 1
            && (Slots[23].PiecesInSlot.Count == 0
                || (Slots[23].PiecesInSlot.Count > 0 && Slots[23].PiecesInSlot[0].CompareTag("White"))
                || (Slots[23].PiecesInSlot.Count == 1 && Slots[23].PiecesInSlot[0].CompareTag("Black"))))
        {
            newSlotNum = 23;
        }
        else if (diceNum == 2
            && (Slots[22].PiecesInSlot.Count == 0
                || (Slots[22].PiecesInSlot.Count > 0 && Slots[22].PiecesInSlot[0].CompareTag("White"))
                || (Slots[22].PiecesInSlot.Count == 1 && Slots[22].PiecesInSlot[0].CompareTag("Black"))))
        {
            newSlotNum = 22;
        }
        else if (diceNum == 3
            && (Slots[21].PiecesInSlot.Count == 0
                || (Slots[21].PiecesInSlot.Count > 0 && Slots[21].PiecesInSlot[0].CompareTag("White"))
                || (Slots[21].PiecesInSlot.Count == 1 && Slots[21].PiecesInSlot[0].CompareTag("Black"))))
        {
            newSlotNum = 21;
        }
        else if (diceNum == 4
            && (Slots[20].PiecesInSlot.Count == 0
                || (Slots[20].PiecesInSlot.Count > 0 && Slots[20].PiecesInSlot[0].CompareTag("White"))
                || (Slots[20].PiecesInSlot.Count == 1 && Slots[20].PiecesInSlot[0].CompareTag("Black"))))
        {
            newSlotNum = 20;
        }
        else if (diceNum == 5
            && (Slots[19].PiecesInSlot.Count == 0
                || (Slots[19].PiecesInSlot.Count > 0 && Slots[19].PiecesInSlot[0].CompareTag("White"))
                || (Slots[19].PiecesInSlot.Count == 1 && Slots[19].PiecesInSlot[0].CompareTag("Black"))))
        {
            newSlotNum = 19;
        }
        else if (diceNum == 6
            && (Slots[18].PiecesInSlot.Count == 0
                || (Slots[18].PiecesInSlot.Count > 0 && Slots[18].PiecesInSlot[0].CompareTag("White"))
                || (Slots[18].PiecesInSlot.Count == 1 && Slots[18].PiecesInSlot[0].CompareTag("Black"))))
        {
            newSlotNum = 18;
        }

        if (newSlotNum != -1)
        {
            Draggable pieceToBeMoved = MiddleWhites.PiecesInMiddle[MiddleWhites.PiecesInMiddle.Count - 1];
            Vector3 newPosition;
            MiddleWhites.PiecesInMiddle.Remove(pieceToBeMoved);
            if (Slots[newSlotNum].PiecesInSlot.Count == 0 || (Slots[newSlotNum].PiecesInSlot.Count > 0 && Slots[newSlotNum].PiecesInSlot[0].CompareTag("White")))
            {
                float yOffset = Slots[newSlotNum].PiecesInSlot.Count * 0.80f;
                newPosition = new Vector3(Slots[newSlotNum].transform.position.x, -4.05f + yOffset, Slots[newSlotNum].transform.position.z);
                pieceToBeMoved.Collider.enabled = false;
                pieceToBeMoved.transform.DOMove(newPosition, 0.5f).OnComplete(
                                    () => {
                                        pieceToBeMoved.Slot_number = newSlotNum;
                                        pieceToBeMoved.LastPosition = pieceToBeMoved.transform.position;
                                        pieceToBeMoved.Collider.enabled = true;
                                        pieceToBeMoved.IsBroken = false;
                                        pieceToBeMoved.IsDraggable = true;
                                        Slots[newSlotNum].PiecesInSlot.Add(pieceToBeMoved);
                                    });
            }
            else if (Slots[newSlotNum].PiecesInSlot.Count == 1 && Slots[newSlotNum].PiecesInSlot[0].CompareTag("Black"))
            {
                newPosition = new Vector3(Slots[newSlotNum].transform.position.x, -4.05f, Slots[newSlotNum].transform.position.z);
                Draggable broken = Slots[newSlotNum].PiecesInSlot[0];
                Slots[newSlotNum].PiecesInSlot.Remove(broken);
                float yOffset = MiddleBlacks.PiecesInMiddle.Count * 0.80f;
                Vector3 blacksPosition = new Vector3(MiddleBlacks.transform.position.x, -0.86f - yOffset, 0f);
                broken.Collider.enabled = false;
                broken.Slot_number = -1;
                broken.transform.DOMove(blacksPosition, 0.5f).OnComplete(
                    () => {
                        MiddleBlacks.PiecesInMiddle.Add(broken);
                        broken.LastPosition = broken.transform.position;
                        broken.IsDraggable = false;
                        broken.IsBroken = true;
                    });
                pieceToBeMoved.Collider.enabled = false;
                pieceToBeMoved.transform.DOMove(newPosition, 0.5f).OnComplete(
                                () => {
                                    pieceToBeMoved.LastPosition = pieceToBeMoved.transform.position;
                                    pieceToBeMoved.Collider.enabled = true;
                                    pieceToBeMoved.Slot_number = newSlotNum;
                                    Slots[newSlotNum].PiecesInSlot.Add(pieceToBeMoved);
                                });
            }
            
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsWhiteCollectable()
    {
        if (MiddleWhites.PiecesInMiddle.Count > 0)
        {
            return false;
        }
        for (int i=6; i < 24; i++)
        {
            if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("White"))
            {
                return false;
            }
        }
        return true;
    }

    public bool IsCollectableWithDiceNumber(int diceNumber)
    {
        int slotNumber = diceNumber - 1;
        if (Slots[slotNumber].PiecesInSlot.Count > 0 && Slots[slotNumber].PiecesInSlot[0].CompareTag("White"))
        {
            return true;
        }

        return false;
    }

    public bool SmallerThanDice(int diceNumber)
    {
        int slotNumber = diceNumber - 1;
        for (int i = slotNumber + 1; i < 6; i++)
        {
            if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("White"))
            {
                return false;
            }
        }
        for (int i = slotNumber - 1; i >= 0; i--)
        {
            if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("White"))
            {
                return true;
            }
        }
        return false;
    }

    public void CollectWhite(int diceNumber)
    {
        int slotNumber = diceNumber - 1;
        if (Slots[slotNumber].PiecesInSlot.Count > 0 && Slots[slotNumber].PiecesInSlot[0].CompareTag("White"))
        {
            Vector3 targetPosition = new Vector3(5.6482f, 2.48f, 0f);
            Draggable draggable = Slots[slotNumber].PiecesInSlot[Slots[slotNumber].PiecesInSlot.Count - 1];
            draggable.Collider.enabled = false;
            draggable.transform.DOMove(targetPosition, 0.5f).OnComplete(
                        () => {
                            Slots[slotNumber].PiecesInSlot.Remove(draggable);
                            if (Slots[slotNumber].PiecesInSlot.Count > 0)
                            {
                                Slots[slotNumber].PiecesInSlot[Slots[slotNumber].PiecesInSlot.Count - 1].IsDraggable = true;
                                Slots[slotNumber].PiecesInSlot[Slots[slotNumber].PiecesInSlot.Count - 1].Collider.enabled = true;
                            }
                            draggable.Collider.enabled = true;
                        });

        }
        else
        {
            for (int i = slotNumber - 1; i >= 0; i--)
            {
                if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("White"))
                {
                    Vector3 targetPosition = new Vector3(5.6482f, 2.48f, 0f);
                    Draggable draggable = Slots[i].PiecesInSlot[Slots[i].PiecesInSlot.Count - 1];
                    draggable.Collider.enabled = false;
                    draggable.transform.DOMove(targetPosition, 0.5f).OnComplete(
                        () => {
                            Slots[i].PiecesInSlot.Remove(draggable);
                            if (Slots[i].PiecesInSlot.Count > 0)
                            {
                                Slots[i].PiecesInSlot[Slots[i].PiecesInSlot.Count - 1].IsDraggable = true;
                                Slots[i].PiecesInSlot[Slots[i].PiecesInSlot.Count - 1].Collider.enabled = true;
                            }
                            draggable.Collider.enabled = true;
                        });
                    return;
                }
            }
        }
    }

    bool NoWhitePieces()
    {
        for (int i = 0; i < 24; i++)
        {
            if (Slots[i].PiecesInSlot.Count > 0 && Slots[i].PiecesInSlot[0].CompareTag("White"))
            {
                return false;
            }
        }
        return true;
    }
}
