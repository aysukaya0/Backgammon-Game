using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SettingsAnimation : MonoBehaviour
{
    public CanvasGroup BackgroundPanel;
    public GameObject SettingsPopup;
    public GameObject FirstOn;
    public GameObject FirstOff;
    public RectTransform FirstButton;
    public GameObject SecondOn;
    public GameObject SecondOff;
    public RectTransform SecondButton;

    public void OpenSettingsPopUp()
    {
        SettingsPopup.SetActive(true);
        SettingsPopup.transform.DOScale(1f, .5f);
        BackgroundPanel.gameObject.SetActive(true);
        BackgroundPanel.DOFade(1f, 0.5f);
    }

    public void CloseSettingsPopUp()
    {
        SettingsPopup.transform.DOScale(0, .5f).OnComplete(() =>
        {
            SettingsPopup.SetActive(false);
        });
        BackgroundPanel.DOFade(0, 0.5f).OnComplete(() =>
        {
            BackgroundPanel.gameObject.SetActive(false);
        });

    }

    public void FirstSettingsOn()
    {
        FirstOff.SetActive(false);
        FirstOn.SetActive(true);
        FirstButton.DOAnchorPosX(365f, 0.1f);
    }

    public void FirstSettingsOff()
    {
        FirstOff.SetActive(true);
        FirstOn.SetActive(false);
        FirstButton.DOAnchorPosX(255f, 0.1f);
    }

    public void SecondSettingsOn()
    {
        SecondOff.SetActive(false);
        SecondOn.SetActive(true);
        SecondButton.DOAnchorPosX(365f, 0.1f);
    }

    public void SecondSettingsOff()
    {
        SecondOff.SetActive(true);
        SecondOn.SetActive(false);
        SecondButton.DOAnchorPosX(255f, 0.1f);
    }

}
