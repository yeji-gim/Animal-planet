using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimeStamp 
{
    public int year;
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    public Season season;

    public enum DayOfTheWeek
    {
        Sat,
        Sun,
        Mon,
        Tue,
        Wed,
        Thu,
        Fri
    }
    public int day;
    public int hour;
    public int minute;

    public GameTimeStamp(int year, Season season, int day, int hour, int minute)
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }
    public GameTimeStamp(GameTimeStamp timestamp)
    {
        this.year = timestamp.year;
        this.season = timestamp.season;
        this.day = timestamp.day;
        this.hour = timestamp.hour;
        this.minute = timestamp.minute;
    }

    public void UpdateClock()
    {
        minute++;

        if(minute >= 60)
        {
            minute = 0;
            hour++;
        }
        if(hour>=24)
        {
            hour = 0;
            day++;
        }
        if(day >= 30)
        {
            day = 1;
            
            if(season == Season.Winter)
            {
                season = Season.Spring;
                year++;
            }
            else
            {
                season++;
            }
        }
    }

    public static int TimeStampToMinutes(GameTimeStamp timeStamp)
    {
        return HoursToMinute(DaysToHour(YearToDays(timeStamp.year)) + DaysToHour(SeasonToDays(timeStamp.season)) + DaysToHour(timeStamp.day) + timeStamp.hour) + timeStamp.minute;
    }

    public DayOfTheWeek GetDayOfTheWeek()
    {
        int dayPassed = YearToDays(year) + SeasonToDays(season) + day;

        int dayIndex = dayPassed % 7;

        return (DayOfTheWeek)dayIndex;
    }

    public static int HoursToMinute(int hour)
    {
        return hour * 60;
    }
    public static int get_hour(int hour)
    {
        return hour;
    }
    public static int DaysToHour(int days)
    {
        return days * 24;
    }

    public static int SeasonToDays(Season season)
    {
        int seasonIndex = (int)season;
        return seasonIndex * 30;
    }

    public static int YearToDays(int years)
    {
        return years * 4 * 30;
    }

    public static int CompareTimestamps(GameTimeStamp timestamp1, GameTimeStamp timestamp2)
    {
        int timestamp1Hours = DaysToHour(YearToDays(timestamp1.year)) + DaysToHour(SeasonToDays(timestamp1.season)) + DaysToHour(timestamp1.day) + timestamp1.hour;
        int timestamp2Hours = DaysToHour(YearToDays(timestamp2.year)) + DaysToHour(SeasonToDays(timestamp2.season)) + DaysToHour(timestamp2.day) + timestamp2.hour;
        int difference = timestamp2Hours - timestamp1Hours;
        return Mathf.Abs(difference);
    }
}
