using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DiceController : MonoBehaviour
{
    public GameObject[] Dices;
    public bool BlacksTurn = false;
    public BotController Bot;
    public PlayerController Player;
    public List<GameObject> DiceObjects;


    private Vector3 _firstDicePosition = new Vector3(-1.45f, 0f, 0f);
    private Vector3 _secondDicePosition = new Vector3(0.09f, 0f, 0f);
    private int _firstDiceNumber = 0;
    private int _secondDiceNumber = 0;
    [SerializeField] GameObject WhiteBegins;
    [SerializeField] GameObject BlackBegins;



    public void DetermineFirstPlayer()
    {
        while (_firstDiceNumber == _secondDiceNumber)
        {
            _firstDiceNumber = UnityEngine.Random.Range(1, 7);
            _secondDiceNumber = UnityEngine.Random.Range(1, 7);
        }

        GameObject firstDice = Instantiate(Dices[_firstDiceNumber - 1], new Vector3(-3.5f, 2.3f, 0f), Quaternion.identity);
        GameObject secondDice = Instantiate(Dices[_secondDiceNumber - 1], new Vector3(-3.5f, -2.3f, 0f), Quaternion.identity);

        firstDice.transform.DOScale(1.2f, 0.4f).SetLoops(4, LoopType.Yoyo);
        secondDice.transform.DOScale(1.2f, 0.4f).SetLoops(4, LoopType.Yoyo).OnComplete(
            () =>
            {
                Destroy(firstDice);
                Destroy(secondDice);
                if (_firstDiceNumber > _secondDiceNumber)
                {
                    BlacksTurn = false;
                    WhiteBegins.SetActive(true);
                    WhiteBegins.transform.DOScale(0.6f, 0.8f).OnComplete(
                        () =>
                        {
                            WhiteBegins.transform.DOScale(0, 0.8f).OnComplete(
                                () =>
                                {
                                    WhiteBegins.SetActive(false);

                                }).OnComplete(
                            () =>
                            {
                                Bot.PlayBot();
                            });
                        });
                }
                else
                {
                    BlackBegins.SetActive(true);
                    BlackBegins.transform.DOScale(0.6f, 0.8f).OnComplete(
                        () =>
                        {
                            BlackBegins.transform.DOScale(0, 0.8f).OnComplete(
                                () =>
                                {
                                    BlackBegins.SetActive(false);
                                }).OnComplete(
                            () =>
                            {
                                StartCoroutine(Player.PlayBlack());
                            });
                        });

                }
            });
    }

    public List<GameObject> RollDices()
    {
        int firstDiceNumber = UnityEngine.Random.Range(1, 7);
        int secondDiceNumber = UnityEngine.Random.Range(1, 7);
        List<GameObject> result = new List<GameObject>();

        GameObject firstDice = Instantiate(Dices[firstDiceNumber-1], _firstDicePosition, Quaternion.identity);
        GameObject secondDice = Instantiate(Dices[secondDiceNumber-1], _secondDicePosition, Quaternion.identity);

        result.Add(firstDice);
        result.Add(secondDice);

        firstDice.transform.DOScale(1.2f, 0.4f).SetLoops(4, LoopType.Yoyo).OnComplete(
            () =>
            {
                firstDice.transform.DOScale(0.82f, 0.8f);
                firstDice.transform.DOMove(new Vector3(5.657f, 0.52f, 0f), 0.8f);
            });

        secondDice.transform.DOScale(1.2f, 0.4f).SetLoops(4, LoopType.Yoyo).OnComplete(
            () =>
            {
                secondDice.transform.DOScale(0.82f, 0.8f);
                secondDice.transform.DOMove(new Vector3(5.657f, -0.52f, 0f), 0.8f);
            });

        return result;
    }


}
