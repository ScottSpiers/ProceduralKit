using System.Collections.Generic;

public class MetricCounter
{
    private int numRooms = 0;
    private int numCorridors = 0;
    private int numTurns = 0;

    private int[,] levelCount = new int[11,11];

    public MetricCounter()
    {

    }

    public void MetricCount(string s)
    {
        while( s.IndexOf("RR") != -1)
            s = s.Replace("RR", "R");

        while (s.IndexOf("RA") != -1)
            s = s.Replace("RA", "R");

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
        numTurns = (numPlus + numMinus) - (numCorridors);

        return numTurns;
    }

    public void UpdateLevelCount()
    {
        if(numRooms > 10)
            numRooms = 10;

        if (numCorridors > 10)
            numCorridors = 10;

        levelCount[numRooms, numCorridors] += 1;
    }

    public void ResetMetrics()
    {
        numRooms = 0;
        numCorridors = 0;
        numTurns = 0;
    }

    public string GetLevelCount()
    {
        string s = "";
        for(int i = 0; i < 11; ++i)
        {
            for (int j = 0; j < 11; ++j)
            {
                s += levelCount[i, j] + "\t";
            }
            s += "\n\n";
        }
        return s;
    }

    public void ShapeCount(List<Module> mods)
    {
        Dictionary<System.Tuple<int, int>, bool> cellMap = new Dictionary<System.Tuple<int, int>, bool>();

        foreach(Module m in mods)
        {
            if(m.sym == 'F')
            {

            }
        }
    }
}
