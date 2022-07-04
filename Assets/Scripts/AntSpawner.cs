using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AntSpawner : MonoBehaviour
{
    [SerializeField] private GameObject MenuAnt;
    bool unpaused = true;
    private System.Random rand = new System.Random();
    void Start()
    {
        StartCoroutine(SpawnSystem());
    }

    IEnumerator SpawnSystem()
    {
        while (unpaused)
        {
            int randint = rand.Next(1, 5);
            for (int i = 0; i < randint; i++)
            {
                Instantiate(MenuAnt,
                    new Vector3(Random.Range(-10, 10), 6, 0),
                    Quaternion.Euler(0, 0, Random.Range(0, 360))); 
            }
            yield return new WaitForSeconds(1f);
        }
        
    }
}
