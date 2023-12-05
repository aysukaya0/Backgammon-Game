using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShopAnimation : MonoBehaviour
{
    public CanvasGroup BackgroundPanel;
    public GameObject ShopPopup;

    public void OpenShopPopUp()
    {
        ShopPopup.SetActive(true);
        ShopPopup.transform.DOScale(1f, .5f);
        BackgroundPanel.gameObject.SetActive(true);
        BackgroundPanel.DOFade(1f, 0.5f);
    }

    public void CloseShopPopUp()        
    {
        ShopPopup.transform.DOScale(0, .5f).OnComplete(() =>
        {
            ShopPopup.SetActive(false);
        });
        BackgroundPanel.DOFade(0, 0.5f).OnComplete(() =>
        {
            BackgroundPanel.gameObject.SetActive(false);
        });

    }
}
