using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class SpawnManager : MonoBehaviour
{
    private Random rand = new Random();
    private bool unpaused = true;
    private int[] hillcounter = {0, 0, 0, 0};
    private int[] leafcounter = {0, 0, 0, 0};
    
    [SerializeField]
    private Tile[] antHills;

    [SerializeField]
    private Tile[] leafs;

    [SerializeField] 
    public Tilemap roadmap;
    
    [SerializeField] 
    public Tilemap hillsandleafsmap;

    [SerializeField] 
    public AntHillManager anthillmanager;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(SpawnSystem());
    }

    IEnumerator SpawnSystem()
    {
        while (unpaused)
        {
            int color = rand.Next(0, 4);
            Vector3Int random = RandomTileGenerator();
            int i = 0;
            while (!CheckTileStatus(random) && i < 100)
            {
                random = RandomTileGenerator();
                i++;
            }
            if (hillcounter[color] == 0 || leafcounter[color] > 4)
            {
                hillsandleafsmap.SetTile(random, antHills[color]);
                anthillmanager.AddAnthill(random, color);
                hillcounter[color] += 1;
                leafcounter[color] = 0;
            }
            else 
            {
                hillsandleafsmap.SetTile(random, leafs[color]);
                leafcounter[color] += 1;
            }
            yield return new WaitForSeconds(rand.Next(3, 10));
        }
        
    }

    private Vector3Int RandomTileGenerator()
    {
        int x = rand.Next(-10, 10);
        int y = rand.Next(-5, 5);
        return new Vector3Int(x, y, 0);
    }

    private bool CheckTileStatus(Vector3Int position)
    {
        if (roadmap.GetTile(position) == null 
            && hillsandleafsmap.GetTile(position) == null)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}