using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ant1 : MonoBehaviour
{
    [SerializeField] private Tilemap roadmap;
    [SerializeField] private Tilemap hillsandleafsmap;
    [SerializeField] private Tile[] leafs;
    [SerializeField] private RoadManagerScript roadmanager;
    
    private bool explore = true;
    private bool moveForward = true;
    private bool start = true;
    private Vector3Int position;
    private Vector3Int nextposition;
    private Vector3Int lastposition;
    private Queue<Vector3Int> trajectory;
    private Stack<Vector3Int> reversetrajectory;
    public int color;
    
    private readonly Vector3Int[] neighbourPositions = 
    {
        Vector3Int.up,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.left,
    };

    void Start()
    {
        ExploreMove();
        start = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (moveForward)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 move = position - nextposition;
        transform.position += (move);
        lastposition = position;
        position = nextposition;
        nextposition = trajectory.Dequeue();
        reversetrajectory.Push(position);
        if (explore)
        {
            ExploreMove();
        }
        if (trajectory.Count == 0)
        {
            moveForward = !moveForward;
        }
    }

    private void ExploreMove()
    {
        foreach (Vector3Int neighbour in neighbourPositions)
        {
            if (hillsandleafsmap.GetTile(position + neighbour) == leafs[color])
            {
                explore = false;
                moveForward = false;
                trajectory.Clear();
                return;
            }
            // add all Roads to open list
            // !!! still need to add tajectory check
            if (roadmap.GetTile(position + neighbour) != null && 
                position + neighbour != lastposition)
            {
                List<Vector3Int> road = roadmanager.GetRoad(position + neighbour);
                // if the neighbouring road is shorter than the rest of the trajectory
                // walk the shorter road instead
                if (road.Count < trajectory.Count || start)
                {
                    trajectory.Clear();
                    foreach (Vector3Int step in road)
                    {
                        trajectory.Enqueue(step);
                    }
                }
            }
        }
    }
}
