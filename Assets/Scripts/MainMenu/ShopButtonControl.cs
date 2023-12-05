using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SNG.Save;

public class ShopButtonControl : MonoBehaviour
{
    public ShopItem[] ShopItems;
    public ShopItemInfos PieceImagesAndMoney;

    private long _money;

    private void Start()
    {
        InteractableButtons();

        long currentPiece = SaveGame.Instance.PieceData.CurrentPieceIndex;
        ShopItems[currentPiece].ButtonText.text = "IN USE";
        for (int i =0; i<4; i++)
        {
            if (i != currentPiece)
            {
                if (SaveGame.Instance.PieceData.Purchased != null)
                {
                    if (SaveGame.Instance.PieceData.Purchased[i] == true)
                    {
                        ShopItems[i].ButtonText.text = "USE";
                    }
                    else
                    {
                        ShopItems[i].ButtonText.text = "500<sprite name=\"Coin\">";
                    }
                }
            }
        }

    }

    private void OnEnable()
    {
        InteractableButtons();
    }

    public void OnButtonClick()
    {
        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        if (clickedButton != null)
        {
            int clickedIndex = int.Parse(clickedButton.name);
            int currentPiece = SaveGame.Instance.PieceData.CurrentPieceIndex;
            if (clickedIndex == currentPiece)
            {
                return;
            }
            else
            {
                ShopItems[currentPiece].ButtonText.text = "USE";
                ShopItems[clickedIndex].ButtonText.text = "IN USE";
                if (SaveGame.Instance.PieceData.Purchased[clickedIndex] == true)
                {
                    SaveGame.Instance.PieceData.ChangeSpriteIndex(clickedIndex);

                }
                else
                {
                    SaveGame.Instance.PieceData.ChangePurchaseStatus(clickedIndex, true);
                    SaveGame.Instance.PieceData.ChangeSpriteIndex(clickedIndex);
                    SaveGame.Instance.PlayerData.ChangeMoney(-PieceImagesAndMoney.PieceMoney[clickedIndex]);
                    InteractableButtons();
                }
            }
        }
    }

    public void InteractableButtons()
    {
        _money = SaveGame.Instance.PlayerData.Money;

        for (int i = 0; i < 4; i++)
        {
            if (SaveGame.Instance.PieceData.Purchased != null)
            {
                if (SaveGame.Instance.PieceData.Purchased[i] == false)
                {
                    if (_money >= PieceImagesAndMoney.PieceMoney[i])
                    {
                        ShopItems[i].ButtonComp.interactable = true;
                    }
                    else
                    {
                        ShopItems[i].ButtonComp.interactable = false;
                    }
                }
            }
        }
    }
}
