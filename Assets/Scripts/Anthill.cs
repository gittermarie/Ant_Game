using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Anthill
{
    private Ant[] ants;
    public int antcount;
    public int countdown;
    private Vector3Int pos;
    private int color;

    public Anthill(Vector3Int pos, int color)
    {
        this.pos = pos;
        this.color = color;
    }
    

    public Vector3Int getPos()
    {
        return this.pos;
    }
    
    public int getColor()
    {
        return this.color;
    }

    public void PrintThis()
    {
        Debug.Log(color);
        Debug.Log(pos);
    }
}
