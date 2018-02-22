﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour {

    private string _dataFileName;
    private string _gameDataFilePath;


    public void Init()
    {
        _dataFileName = "data.txt";
        _gameDataFilePath = "/Data/data.txt";
    }

    public DateTime GetAppLastClosingTime()
    {
        var filePath = Application.dataPath + _gameDataFilePath;
        DateTime appLastClosingTime = DateTime.MinValue;

        if (File.Exists(filePath))
        {
            var dataString = File.ReadAllText(filePath);
            var loadedData = JsonUtility.FromJson<GameData>(dataString);

            appLastClosingTime = new DateTime(
                loadedData.Year, 
                loadedData.Mounth, 
                loadedData.Day, 
                loadedData.Hour, 
                loadedData.Minute, 
                loadedData.Second
            );
        }

        return appLastClosingTime;
    }

    public void SaveAppLastClosingTime()
    {
        var gameData = new GameData();
        var dateTime = NtpServerConnectionManager.Instance.GetTime();

        gameData.SetDate(
            dateTime.Year, 
            dateTime.Month, 
            dateTime.Day,
            dateTime.Hour,
            dateTime.Minute,
            dateTime.Second
        ); 

        Debug.Log(gameData.GetDate());

        var gameDataString = JsonUtility.ToJson(gameData);
        Debug.Log(gameDataString);

        var filePath = Application.dataPath + _gameDataFilePath;
        File.WriteAllText(filePath, gameDataString);

        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(gameDataString);
            }

        }
    }
}

[Serializable]
public class GameData
{

    public int Year;
	public int Mounth;
	public int Day;
    public int Hour;
    public int Minute;
    public int Second;

    public void SetDate(int year , int month, int day, int hour, int minute, int second)
    {
        Day = day;
        Mounth = month;
        Year = year;
        Hour = hour;
        Minute = minute;
        Second = second;
    }

    public DateTime GetDate()
    {
        var date = new DateTime(Year,Mounth,Day,Hour,Minute,Second);
        return date;
    }
}
