using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ant : MonoBehaviour
{
    private bool moveForward = true;
    private bool foundBestRoute = false;
    private bool foundtrajectory = false;
    private bool atGoal = false;
    private bool returning = false;
    public Vector3Int position;
    private Vector3Int nextposition;
    private List<Vector3Int> trajectory = new List<Vector3Int>();
    private int stepCounter;
    private float _speed = 1f;
    public int color;
    // List of all Lists of possible roads(List<Vector3Int>) to take at specific node
    private List<List<List<Vector3Int>>> openList = new List<List<List<Vector3Int>>>();
    
    private readonly Vector3Int[] neighbourPositions = 
    {
        Vector3Int.up,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.left,
    };
    
    [SerializeField] private Tilemap roadmap;
    
    [SerializeField] private Tilemap hillsandleafsmap;
    
    [SerializeField] private Tile[] leafs;
    
    [SerializeField] private RoadManagerScript roadmanager;

    private void Update()
    {
        if (!foundtrajectory)
        {
            if (!foundBestRoute)
            {
                Explore(position);
            }
        }
        else
        {
            // move towards current goal
            Vector3 move = position - nextposition;
            transform.position += (move * _speed);
            CheckMovementConditions();
        }
        
    }
    
    
    private void Explore(Vector3Int pos)
    {
        List<List<Vector3Int>> nodeList = new List<List<Vector3Int>>();
        int minlength = 500;
        List<Vector3Int> nextroad = new List<Vector3Int>();
        // add neighbour roads to the open list
        foreach (Vector3Int neighbour in neighbourPositions)
        {
            if (hillsandleafsmap.GetTile(pos + neighbour) == leafs[color])
            {
                atGoal = true;
                foundBestRoute = true;
                foundtrajectory = true;
                foundBestRoute = true;
                moveForward = false;
                return;
            }
            // add all Roads to open list
            // !!! still need to add tajectory check
            if (roadmap.GetTile(pos + neighbour) != null)
            {
                nodeList.Add(roadmanager.GetRoad(pos + neighbour)); 
            }
        }
        
        openList.Add(nodeList);
        foreach (List<Vector3Int> road in nodeList)
        {
            if (road.Count < minlength)
            {
                minlength = road.Count;
                nextroad = road;
            }
        }
        trajectory = nextroad;
        foundtrajectory = true;
    }

    private void CheckMovementConditions()
    {
        // if the ant has reached the next position and it is still within
        // the trajectory and it is moving forward:
        // set the next position to the next position in the trajectory
        if (position == nextposition && stepCounter < trajectory.Count && moveForward)
        {
            position = nextposition;
            nextposition = trajectory[stepCounter];
            stepCounter += 1;
        }
        
        // if the ant has reached the next position and it is still within
        // the trajectory and it is moving backward:
        // set the next position to the previous position in the trajectory
        else if (position == nextposition && stepCounter > 0 && !moveForward)
        {
            position = nextposition;
            nextposition = trajectory[stepCounter];
            stepCounter -= 1;
        }
        
        // if the Ant has reached the end of the trajectory
        // reverse the movement direction
        else if (stepCounter == 0 || stepCounter == trajectory.Count)
        {
            moveForward = !moveForward;
        }
    }
}
