using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SNG.Save;
using System;
using UnityEngine.UI;

public class ExperienceDisplay : MonoBehaviour
{
    public Image Mask;
    [SerializeField] TextMeshProUGUI ExperienceLevel;
    [SerializeField] TextMeshProUGUI ExperienceRatio;

    private float _newFillAmount;
    private float timeLimit = 1.5f;
    private bool isRunning = false;
    private bool isRunning2 = false;

    private void Start()
    {
        Mask.fillAmount = SaveGame.Instance.PlayerData.FillAmount;
        if (SaveGame.Instance.PlayerData.Experience >= SaveGame.Instance.PlayerData.NecessaryExperience)
        {
            ExperienceLevel.text = SaveGame.Instance.PlayerData.PlayerLevel.ToString();
            ExperienceRatio.text = SaveGame.Instance.PlayerData.NecessaryExperience.ToString() + "/" + SaveGame.Instance.PlayerData.NecessaryExperience.ToString();
            LevelChange();
        }
        else
        {
            ExperienceLevel.text = SaveGame.Instance.PlayerData.PlayerLevel.ToString();
            ExperienceRatio.text = SaveGame.Instance.PlayerData.Experience.ToString() + "/" + SaveGame.Instance.PlayerData.NecessaryExperience.ToString();
            BarChange();
        }
    }

    private void LevelChange()
    {
        if (!isRunning)
        {
            StartCoroutine(ProgressBarFull());
        }
    }

    private void BarChange()
    {
        if (!isRunning2)
        {
            StartCoroutine(ProgressBarChange());
        }
    }

    private IEnumerator ProgressBarFull()
    {
        isRunning = true;
        float startTime = Time.time;

        while (Time.time - startTime < timeLimit)
        {
            Mask.fillAmount += 1.0f * Time.deltaTime;
            yield return null;
        }
        Mask.fillAmount = 1.0f;

        SaveGame.Instance.PlayerData.Experience -= SaveGame.Instance.PlayerData.NecessaryExperience;
        SaveGame.Instance.PlayerData.PlayerLevel++;
        SaveGame.Instance.PlayerData.ChangeNecessaryExperience(SaveGame.Instance.PlayerData.PlayerLevel);

        _newFillAmount = (float)SaveGame.Instance.PlayerData.Experience / (float)SaveGame.Instance.PlayerData.NecessaryExperience;

        Mask.fillAmount = 0f;

        startTime = Time.time;

        while (Time.time - startTime < timeLimit)
        {
            if (Mask.fillAmount > _newFillAmount - 0.01f)
            {
                yield return null;
                break;
            }
            Mask.fillAmount += _newFillAmount * Time.deltaTime;
            yield return null;
        }
        Mask.fillAmount = _newFillAmount;
        ExperienceLevel.text = SaveGame.Instance.PlayerData.PlayerLevel.ToString();
        ExperienceRatio.text = SaveGame.Instance.PlayerData.Experience.ToString() + "/" + SaveGame.Instance.PlayerData.NecessaryExperience.ToString();
        SaveGame.Instance.PlayerData.FillAmount = Mask.fillAmount;

        isRunning = false;
    }

    private IEnumerator ProgressBarChange()
    {
        isRunning2 = true;
        _newFillAmount = (float)SaveGame.Instance.PlayerData.Experience / (float)SaveGame.Instance.PlayerData.NecessaryExperience;

        float startTime = Time.time;

        while (Time.time - startTime < timeLimit)
        {
            if (Mask.fillAmount > _newFillAmount - 0.01f)
            {
                yield return null;
                break;
            }
            Mask.fillAmount += _newFillAmount * Time.deltaTime;
            yield return null;
        }
        Mask.fillAmount = _newFillAmount;
        SaveGame.Instance.PlayerData.FillAmount = Mask.fillAmount;

        isRunning2 = false;
    }
}
