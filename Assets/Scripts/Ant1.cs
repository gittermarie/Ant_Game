using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Ant1 : MonoBehaviour
{
    
    [SerializeField] public ScoreManager scoremanager;
    [SerializeField] public GameObject pointText;
    [SerializeField] public Tilemap roadmap;
    [SerializeField] public Tilemap hillsandleafsmap;
    [SerializeField] private Tile[] leafs;
    [SerializeField] public RoadManagerScript roadmanager;
    [SerializeField] private RuleTile roadtile;

    private Vector3 shift = new Vector3(0.5f, 0.5f, 0f);
    private float speed = 0.5f;
    private bool explore = true;
    private bool moveForward = true;
    private bool start = true;
    private Vector3 anthill;
    public Vector3Int position = new Vector3Int(-30, -10, 0);
    public Vector3Int nextposition; 
    private Stack<Vector3Int> trajectory = new Stack<Vector3Int>();
    private Stack<Vector3Int> reversetrajectory = new Stack<Vector3Int>();
    public int color;
    private Vector3 move;
    public bool hasreturned = true;
    private System.Random rand = new System.Random();
    
    private readonly Vector3Int[] neighbourPositions = 
    {
        Vector3Int.up,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.left,
    };

    void Start()
    {
        transform.position = (position + shift);
        anthill = transform.position;
        nextposition = position;
        reversetrajectory.Push(position);
        move = nextposition - position;
        hasreturned = true;
    }
    void FixedUpdate()
    {
        // until we have roads we can walk on we simply look for them
        if (start)
        {
            ExploreMove();
        }
        // transform the position until we have reached the next position 
        // then update next position to be the next step on our path
        else
        {
            transform.Translate(move * speed * Time.fixedDeltaTime, 0);
            if (Vector3.SqrMagnitude(transform.position - (nextposition + shift)) < 0.0002)
            {
                UpdateMovement();
                if (!explore && Vector3.SqrMagnitude(transform.position - anthill) < 0.9)
                {
                    Instantiate(pointText, transform.position, Quaternion.identity);
                    scoremanager.AddPoint();
                    hasreturned = true;
                    CheckRoads();
                }
            }
        }
    }

    private void UpdateMovement()
    {
        if (explore)
        {
            ExploreMove();
        }
        
        if (moveForward)
        {
            Move(trajectory, reversetrajectory);
        }
        else
        {
            Move(reversetrajectory, trajectory);
        }
    }
    
    private void Move(Stack<Vector3Int> path, Stack<Vector3Int> trail)
    {
        position = nextposition;
        nextposition = path.Pop();
        trail.Push(nextposition);
        move = nextposition - position;
        Quaternion toRoation = Quaternion.LookRotation(Vector3.forward, move);
        transform.rotation = toRoation;
        speed = getSpeed();
        if (path.Count == 0)
        {
            moveForward = !moveForward;
        }
    }

    private void ExploreMove()
    {
        foreach (Vector3Int neighbour in neighbourPositions)
        {
            // check if any neighbour fulfills the goal criteria
            if (hillsandleafsmap.GetTile(nextposition + neighbour) == leafs[color])
            {
                explore = false;
                moveForward = false;
                reversetrajectory.Push(nextposition+neighbour);
                trajectory.Clear();
                return;
            }
            
            // check if any neighbour is part of a road we could be walking on
            if (roadmap.GetTile(nextposition + neighbour) == roadtile && 
                !reversetrajectory.Contains(nextposition + neighbour) &&
                !trajectory.Contains(nextposition + neighbour))
            {
                Stack<Vector3Int> helperstack = new Stack<Vector3Int>();
                List<Vector3Int> road = roadmanager.GetRoad(nextposition + neighbour);
                
                // random chance
                int chance = rand.Next(0, 100);
                
                // are we connecting with the beginning of the road?
                if (Vector3.SqrMagnitude(nextposition - road[0]) <= 1.5f)
                {
                    // if the neighbouring road is shorter than the rest of the trajectory
                    // (or our random chance is bigger than 80 (20% chance))
                    // walk the other road instead
                    if (road.Count < trajectory.Count || start || chance > 80)
                    {
                        trajectory.Clear();
                        foreach (Vector3Int step in road)
                        {
                            helperstack.Push(step);
                        }
                        // fill our trajectory
                        for(int i = 0; i < road.Count; i++)
                        {
                            Vector3Int step = helperstack.Pop();
                            trajectory.Push(step);
                        }
                    }
                }
                // are we connecting with the end of the road?
                else if (Vector3.SqrMagnitude(nextposition - road[road.Count - 1]) <= 1.5f)
                {
                    if (road.Count < trajectory.Count || start || chance > 80)
                    {
                        // here we don't need a helper
                        trajectory.Clear();
                        foreach (Vector3Int step in road)
                        {
                            trajectory.Push(step);
                        }
                    }
                }
                // what are we connecting with???
                else
                {
                    int index = road.IndexOf((nextposition + neighbour));
                    if ((road.Count - index) < trajectory.Count || start || chance > 80)
                    {
                        trajectory.Clear();
                        for (int i = index; i < road.Count; i++)
                        {
                            helperstack.Push(road[i]);
                        }

                        for(int i = 0; i < (road.Count - index); i++)
                        {
                            trajectory.Push(helperstack.Pop());
                        }
                    }
                    else if (index < trajectory.Count || start || chance > 80)
                    {
                        trajectory.Clear();
                        for (int i = 0; i <= index; i++)
                        {
                            trajectory.Push(road[i]);
                        }
                    }
                }

                if (trajectory.Count > 0)
                {
                    moveForward = true;
                    start = false;
                }
            }
        }
    }

    private float getSpeed()
    {
        // slow down for a junction
        foreach (Vector3Int neighbour in neighbourPositions)
        {
            if (roadmap.GetTile(nextposition + neighbour) == roadtile &&
                !reversetrajectory.Contains(nextposition + neighbour) &&
                !trajectory.Contains(nextposition + neighbour))
            {
                return 0.2f;
            }
        }

        return 0.5f;
    }

    
    // check if the road has been deleted
    private void CheckRoads()
    {
        if (roadmap.HasTile(trajectory.Peek()) == false)
        {
            trajectory.Clear();
            start = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {//
        
    }

    public Vector3 getPos()
    {
        return transform.position;
    }
}
