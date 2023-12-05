using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndGameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject WinScreen;
    public GameObject LoseScreen;

    public void OpenWinScreen()
    {
        WinScreen.gameObject.SetActive(true);
        WinScreen.transform.DOScale(1, 1.0f);
    }

    public void OpenLoseScreen()
    {
        LoseScreen.gameObject.SetActive(true);
        LoseScreen.transform.DOScale(1, 1.0f);
    }
}
