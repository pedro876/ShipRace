using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UsefulFuncs : MonoBehaviour
{
    public static float SmoothStep(float edge0, float edge1, float value)
    {
        value = Mathf.Clamp((value - edge0) / (edge1 - edge0), 0.0f, 1.0f);
        return value * value * (3f - 2f * value);
    }

    /*public static string GetCurrentDate()
    {
        System.DateTime now = System.DateTime.Now;
        return "" + now.Day + "_" + now.Month + "_" + now.Year;
    }*/

    /*public static string GetCurrentInstant()
    {
        System.DateTime now = System.DateTime.Now;
        return GetCurrentDate() + "_" + now.Hour + "_" + now.Minute + "_" + now.Second;
    }*/

    /*public static string InstantToDate(string instant)
    {
        string[] dateInfo = Split(instant, '_');
        return dateInfo[0] + "_" + dateInfo[1] + "_" + dateInfo[2];
    }*/

    /*public DateTime GetDateTimeFromInstant(string instant)
    {
        string[] dateInfo = Split(instant, '_');
        DateTime date = new DateTime(
            int.Parse(dateInfo[2]),
            int.Parse(dateInfo[1]),
            int.Parse(dateInfo[0]),
            int.Parse(dateInfo[3]),
            int.Parse(dateInfo[4]),
            int.Parse(dateInfo[5])
        );
        return date;
    }*/

    /*public static string[] Split(string original, char splitChar = ' ')
    {
        List<string> allStrings = new List<string>();
        string current = "";
        char c;
        for(int i = 0; i < original.Length; i++)
        {
            c = original[i];
            if(c == splitChar)
            {
                if(current != "")
                {
                    allStrings.Add(current);
                    current = "";
                }
            } else
            {
                current += c;
            }
        }
        if(current != "")
        {
            allStrings.Add(current);
        }

        return allStrings.ToArray();
    }*/

    /*public static string FormatDate(string date)
    {
        string[] dateArr = UsefulFuncs.Split(date, '_');
        if (GameManager.english)
        {
            return "" + dateArr[1] + "/" + dateArr[0] + "/" + dateArr[2];
        }
        else
        {
            return "" + dateArr[0] + "/" + dateArr[1] + "/" + dateArr[2];
        }
    }*/

    /*public static string FormatPlayTime(string playTime)
    {
        int seconds = int.Parse(playTime);
        int mins = seconds / 60;
        int hours = mins / 60;
        mins -= hours * 60;
        return "" + hours + "h " + mins + "m";
    }*/
}
