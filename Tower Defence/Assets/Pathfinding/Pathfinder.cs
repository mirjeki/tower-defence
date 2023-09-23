using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }
    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

    Node startSearchNode;
    Node destinationSearchNode;
    Node currentSearchNode;

    Dictionary<Vector2Int, Node> searchedNodes = new Dictionary<Vector2Int, Node>();

    Queue<Node> nodesToSearch = new Queue<Node>();
    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();

        if (gridManager != null)
        {
            grid = gridManager.Grid;
            startSearchNode = grid[startCoordinates];
            destinationSearchNode = grid[destinationCoordinates];
        }


    }

    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }

    private void ExploreNeighbours()
    {
        List<Node> neighbours = new List<Node>();

        foreach (var direction in directions)
        {
            Vector2Int neighbourCoords = currentSearchNode.coordinates + direction;

            if (grid.ContainsKey(neighbourCoords))
            {
                neighbours.Add(gridManager.GetNode(neighbourCoords));
            }
        }

        foreach (var neighbour in neighbours)
        {
            if (!searchedNodes.ContainsKey(neighbour.coordinates) && neighbour.isWalkable)
            {
                neighbour.connectedTo = currentSearchNode;
                searchedNodes.Add(neighbour.coordinates, neighbour);
                nodesToSearch.Enqueue(neighbour);
            }
        }
    }

    private void BreadthFirstSearch(Vector2Int coordinates)
    {
        startSearchNode.isWalkable = true;
        destinationSearchNode.isWalkable = true;

        nodesToSearch.Clear();
        searchedNodes.Clear();

        bool isRunning = true;

        nodesToSearch.Enqueue(grid[coordinates]);
        searchedNodes.Add(coordinates, grid[coordinates]);
        startSearchNode.isExplored = true;

        while (nodesToSearch.Count > 0 && isRunning)
        {
            currentSearchNode = nodesToSearch.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbours();
            if (currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationSearchNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;
            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
