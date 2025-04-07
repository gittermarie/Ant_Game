using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AntHillManager : MonoBehaviour
{
    [SerializeField] private GameObject timer;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private GameObject BrownAnt;
    [SerializeField] private RoadManagerScript roadmanager;
    [SerializeField] private Tilemap hillsandleafsmap;
    [SerializeField] private Tilemap roadmap;
    private bool unpaused = true;
    private float _delay = 3f;
    private List<Anthill> anthills = new List<Anthill>();
    private int rounds = 0;

    public void AddAnthill(Vector3Int position, int color)
    {
        Vector3Int pos = position;
        int col = color;
        Anthill myAnthill = new Anthill(pos, col);
        anthills.Add(myAnthill);
        GameObject ant = GameObject.Instantiate(BrownAnt) as GameObject;
        ant.GetComponent<Ant1>().position = myAnthill.getPos();
        ant.GetComponent<Ant1>().color = myAnthill.getColor();
        ant.GetComponent<Ant1>().roadmanager = roadmanager;
        ant.GetComponent<Ant1>().roadmap = roadmap;
        ant.GetComponent<Ant1>().hillsandleafsmap = hillsandleafsmap;
        ant.GetComponent<Ant1>().scoremanager = _scoreManager;
        myAnthill.AddAnt(ant.GetComponent<Ant1>());
    }
    
    void Start()
    {
        StartCoroutine(HillSystem());
    }
    
    IEnumerator HillSystem()
    // checks and manages the countdown for all anthills
    {
        while (unpaused)
        {
            foreach (Anthill anthill in anthills)
            {
                // if there is no Ant at the anthill and there are less ants than the limit of 5,
                // add a new Ant to the Anthill
                if (anthill.antcount < 10)
                {
                    if (Vector3.SqrMagnitude(anthill.ants[anthill.antcount - 1].getPos() - anthill.getPos()) > 1)
                    {
                        GameObject ant = GameObject.Instantiate(BrownAnt) as GameObject;
                        ant.GetComponent<Ant1>().position = anthill.getPos();
                        ant.GetComponent<Ant1>().color = anthill.getColor();
                        ant.GetComponent<Ant1>().roadmanager = roadmanager;
                        ant.GetComponent<Ant1>().roadmap = roadmap;
                        ant.GetComponent<Ant1>().hillsandleafsmap = hillsandleafsmap;
                        ant.GetComponent<Ant1>().scoremanager = _scoreManager;
                        anthill.AddAnt(ant.GetComponent<Ant1>());
                    }
                }

                if (rounds > 30)
                {
                    // Check how many ants have returned since the last update
                    for (int i = 0; i < anthill.antcount; i++)
                    {
                        if (anthill.ants[i].hasreturned)
                        {
                            anthill.returncounter++;
                            anthill.ants[i].hasreturned = false;
                        }
                    }
                    
                    // if no ants have returned
                    // start gameover counter
                    if (anthill.returncounter < 1 && anthill.counterset == false)
                    {
                        anthill.counter = Instantiate(timer, anthill.getPos(), Quaternion.identity);
                        anthill.counterset = true;
                    }
                    else if (anthill.returncounter >= 1 && anthill.counterset)
                    {
                        Destroy(anthill.counter);
                        anthill.counterset = false;
                    }
                    anthill.returncounter = 0;
                }
            }
            rounds++;
            yield return new WaitForSeconds(_delay);
        }
        
    }
}
