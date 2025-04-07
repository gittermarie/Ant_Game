using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private GameObject filler;
    private bool unpaused = true;
    private Vector3 end = new Vector3(0.01f, 0, 0);
    private Vector3 endscale = new Vector3(0.02f, 0, 0);
    
    // Update is called once per frame
    private void Start()
    {
        filler = transform.GetChild (0).gameObject;
        filler.transform.position = transform.position + new Vector3(-0.5f, 0, 0);
        filler.transform.localScale = new Vector3(0, 1, 0);
        StartCoroutine(SpawnSystem());
    }

    IEnumerator SpawnSystem()
    {
        int counter = 0;
        while (unpaused)
        {
            counter++;
            filler.transform.position += end;
            filler.transform.localScale += endscale;

            if (counter >= 50)
            {
                SceneManager.LoadScene(5);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
