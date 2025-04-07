using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadManagerScript : MonoBehaviour
{
    
    
    private List<List<Vector3Int>> _roadArray = new List<List<Vector3Int>>();

    public void AddRoad(List<Vector3Int> road)
    {
        _roadArray.Add(road);
    }

    public void RemoveRoad(List<Vector3Int> road)
    {
        _roadArray.Remove(road);
    }


    public List<Vector3Int> GetRoad(Vector3Int tile)
        // search for tile vector in road array (road array contains all previously added road vectors)
    {
        List<Vector3Int> returnList = new List<Vector3Int>();
        foreach (List<Vector3Int> roadPos in _roadArray)
        {
            foreach (Vector3Int tilePos in roadPos)
            {
                if (tilePos.Equals(tile))
                {
                    returnList = roadPos;
                }
            }
        }

        return returnList;
    }
}
