using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetricCounter
{
    private int numRooms = 0;
    private int numCorridors = 0;
    private int numTurns = 0;

    public void MetricCount(string s)
    {
        while( s.IndexOf("RR") != -1)
            s = s.Replace("RR", "R");

        string temp = s.Replace("R", "");
        numRooms += s.Length - temp.Length;

        temp = s.Replace("C", "");
        numCorridors += s.Length - temp.Length;


    } 

    public int GetRooms()
    {
        return numRooms;
    }

    public int GetCorridors()
    {
        return numCorridors;
    }

    public int GetTurns(string s)
    {
        string temp = s.Replace("+", "");
        int numPlus = s.Length - temp.Length;

        temp = s.Replace("-", "");
        int numMinus = s.Length - temp.Length;
        numTurns = (numPlus + numMinus) - (numCorridors * 2);

        return numTurns;
    }
}
