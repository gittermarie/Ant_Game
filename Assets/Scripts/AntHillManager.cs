using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntHillManager : MonoBehaviour
{
    [SerializeField] private GameObject BrownAnt;
    private bool unpaused = true;
    private float _delay = 3f;
    private List<Anthill> anthills = new List<Anthill>();

    public void AddAnthill(Vector3Int position, int color)
    {
        Vector3Int pos = position;
        int col = color;
        Anthill myAnthill = new Anthill(pos, col);
        Instantiate(BrownAnt, pos + Vector3Int.back, Quaternion.identity);
        BrownAnt.GetComponent<Ant>().position = pos;
        BrownAnt.GetComponent<Ant>().color = col;
        myAnthill.PrintThis();
        anthills.Add(myAnthill);
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
                //check if ant has arrived (antcounter)
                //if no ant has arrived start countdown 
                //if countdown has reached a certain number
                //GAME OVER
                anthill.PrintThis();
            }
            yield return new WaitForSeconds(_delay);
        }
        
    }
}
