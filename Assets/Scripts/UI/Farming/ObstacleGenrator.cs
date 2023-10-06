using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenrator : MonoBehaviour
{
    [Range(1, 100)]
    public int percentFilled;

    public void GenerateObstacles(List<Land> landPlots)
    {
        int plotsToFill = Mathf.RoundToInt((float)percentFilled / 100 * landPlots.Count);

        List<int> shuffeldList = ShuffleLandIndexes(landPlots.Count);

        for(int i=0; i<plotsToFill;i++)
        {
            int index = shuffeldList[i];

            Land.FarmObstacleStatus status = (Land.FarmObstacleStatus)Random.Range(1, 3);

            landPlots[index].SetObstacleStatus(status);
        }
    }

    List<int> ShuffleLandIndexes(int count)
    {
        List<int> listToReturn = new List<int>();
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, i + 1);
            listToReturn.Insert(index, i); 
        }

        return listToReturn;
    }
}

