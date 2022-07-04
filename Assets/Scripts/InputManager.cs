using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private RuleTile roadTile;
    
    [SerializeField]
    private RuleTile transparentroadTile;
    
    [SerializeField] 
    private Camera mainCamera;

    // Tilemaps
    [SerializeField] 
    public Tilemap roadmap;
    
    [SerializeField] 
    public Tilemap hillsandleafsmap;
    
    [SerializeField]
    public LayerMask groundMask;

    [SerializeField] 
    public RoadManagerScript roadManager;

    private bool _buildingMode = true;
    private bool _deletionMode = false;
    private List<Vector3Int> road = new List<Vector3Int>();
    private bool _builtRoad = false;
    private readonly Vector3Int[] neighbourPositions = 
    {
        Vector3Int.up,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.left,
    };
    public float zoomSpeed = 1;
    public float targetOrtho;
    public float smoothSpeed = 2.0f;

    private void Start()
    {
        targetOrtho = mainCamera.orthographicSize;
    }

    void Update()
    {
        if (_buildingMode)
        {
            RoadBuilder();
        }
        
        else if (_deletionMode)
        {
            RoadDeleter();
        }
        
        float scroll = Input.GetAxis ("Mouse ScrollWheel");
        if (scroll != 0.0f) {
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp (targetOrtho, 4f, 10f);
        }
         
        mainCamera.orthographicSize = Mathf.MoveTowards (mainCamera.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
    }

    public void RoadBuildingClick()
    {
        _deletionMode = false;
        _buildingMode = true;
    }
    
    public void RoadDeletionClick()
    {
        _buildingMode = false;
        _deletionMode = true;
    }
    
    private void RoadBuilder()
    // while the mouse is held down each mouse position is converted to 
    // a tile position and a road tile is placed
    // An array which keeps track of all tile position is passed 
    // to the roadmanger after the whileloop ends
    {
        // if the mouse is held down append the mouseposition and set tile
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(
                    Input.mousePosition),Vector2.zero,Mathf.Infinity, groundMask);
            if (hit.collider != null)
            {
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int clickPos = roadmap.WorldToCell(mouseWorldPos);
                if (!roadmap.HasTile(clickPos))
                {
                    roadmap.SetTile(clickPos, transparentroadTile);
                    road.Add(clickPos);
                    _builtRoad = true;
                }
                
            }
        } 
        else
        {
            if (_builtRoad)
            {
                if (CheckLegality(road))
                {
                    roadManager.AddRoad(new List<Vector3Int>(road));
                }
                else
                {
                    DeleteRoad(road);
                }
                road.Clear();
                _builtRoad = false;
            }
        }
    }

    private void RoadDeleter()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
                Mathf.Infinity, groundMask);
            if (hit.collider != null)
            {
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int clickPos = roadmap.WorldToCell(mouseWorldPos);
                List<Vector3Int> deletedRoad = roadManager.GetRoad(clickPos);
                DeleteRoad(deletedRoad);
                roadManager.RemoveRoad(deletedRoad);
            }
        }
    }

    private void DeleteRoad(List<Vector3Int> roadList)
    {
        if (roadList != null)
        {
            foreach (Vector3Int tilePos in roadList)
            {
                roadmap.SetTile(tilePos, null);
            }
        }
       
    }

    private bool CheckLegality(List<Vector3Int> roadList)
    {
        foreach (Vector3Int tilePos in roadList)
        {
            if (hillsandleafsmap.HasTile(tilePos))
            {
                return false;
            }
            roadmap.SetTile(tilePos, roadTile);
        }

        if (HasNeighbours(roadList[0]) > 1 && HasNeighbours(roadList[roadList.Count - 1]) > 1)
        {
            return true;
        }

        return false;
    }

    private int HasNeighbours(Vector3Int tile)
    {
        int counter = 0;
        foreach (Vector3Int neighbour in neighbourPositions)
        {
            if (hillsandleafsmap.HasTile(tile + neighbour) ||
                (roadmap.HasTile(tile + neighbour)))
            {
                counter++;
            }
        }

        return counter;
    }
}