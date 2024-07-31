using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    //[SerializeField] ScoreData scoreData;

    public void SaveToJson(ScoreData scoreToSave)
    {
        string json = JsonUtility.ToJson(scoreToSave, true);
        File.WriteAllText(Application.dataPath + "/ScoreDataFile.json", json);
    }

    public ScoreData LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/ScoreDataFile.json");
        ScoreData data = JsonUtility.FromJson<ScoreData>(json);
        
        return data;
    }

    public bool CheckForSavedData()
    {
        return File.Exists(Application.dataPath + "/ScoreDataFile.json");
    }

    public ScoreData CheckForBestScore(ScoreData currentScore)
    {
        ScoreData bestScore = LoadFromJson();

        float currentKillCount = int.Parse(currentScore.killCount);
        float bestKillCount = int.Parse(bestScore.killCount);

        if (currentKillCount > bestKillCount) return currentScore;
        else
        if (currentKillCount == bestKillCount)
        {
            float currentTimeAlive = TextToNumber(currentScore.timeAlive);
            float bestTimeAlive = TextToNumber(bestScore.timeAlive);

            if (currentTimeAlive > bestTimeAlive) return currentScore;
            else return bestScore;
        }
        return bestScore;
    }

    //---------- UTILITIES -----------------------------------------------------------------------------------------------------------------//
    
    public string NumberToText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public float TextToNumber(string text)
    {
        string[] parts = text.Split(':');
        int minutes = int.Parse(parts[0]);
        int seconds = int.Parse(parts[1]);
        return minutes * 60 + seconds;
    }

}
