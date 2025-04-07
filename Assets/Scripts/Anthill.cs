using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Anthill
{
    public int returncounter;
    public Ant1[] ants = new Ant1[10];
    public int antcount = 0;
    public GameObject counter;
    public bool counterset = false;
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

    public void AddAnt(Ant1 ant)
    {
        ants[antcount] = ant;
        antcount++;
    }
}
