using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeNode nodePrefab;
    [SerializeField] private Vector2Int size;

    private readonly List<MazeNode> nodes = new();
    private readonly List<int> exitNodesIds = new();
    private MazeNode enterNode;

    public MazeNode EnterNode
    {
        get => enterNode;
        private set => enterNode = value;
    }
    
    private void Start()
    {
        // CreateMaze(size);
    }

    public void CreateMaze(Vector2Int mazeSize)
    {
        GenerateMazeInstant(mazeSize);

        EnterNode = nodes.FirstOrDefault(node => node.canBeEnter);
        MazeNode exitNode = nodes[exitNodesIds[Random.Range(0, exitNodesIds.Count)]];

        if (EnterNode != null)
        {
            EnterNode.SetState(NodeState.Enter);
            EnterNode.isEnter = true;
        }

        if (exitNode != null)
        {
            exitNode.SetState(NodeState.Exit);
            exitNode.isExit = true;
        }
    }

    //For step by step creation remove comments on coroutine logic 
    private void /*IEnumerator*/ GenerateMazeInstant(Vector2Int size)
    {
        // Create nodes
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 nodePos = new(x - (size.x / 2f), 0, y - (size.y / 2f));
                MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);

                newNode.SetNodeParams(nodes.Count, new Vector2Int(x, y), IsOnCenter(x, y, size),
                    IsOnBorder(x, y, size));

                if (newNode.canBeExit)
                {
                    exitNodesIds.Add(newNode.nodeId);
                }

                nodes.Add(newNode);
            }
        }

        List<MazeNode> currentPath = new();
        List<MazeNode> completedNodes = new();

        // Choose starting node
        currentPath.Add(nodes[Random.Range(0, nodes.Count)]);
        currentPath.FirstOrDefault()?.SetState(NodeState.Current);

        while (completedNodes.Count < nodes.Count)
        {
            // Check nodes next to the current node
            List<int> possibleNextNodes = new();
            List<NodeSide> possibleDirections = new();

            GatherPossibleDirections(currentPath, completedNodes, size, possibleDirections, possibleNextNodes);
            ChooseNextNode(currentPath, completedNodes, possibleDirections, possibleNextNodes);

            // yield return new WaitForSeconds(0.05f);
        }
    }

    private bool IsOnBorder(int xPosition, int yPosition, Vector2Int size)
    {
        return xPosition == 0 || yPosition == 0 || xPosition == size.x - 1 || yPosition == size.y - 1;
    }

    private bool IsOnCenter(int xPosition, int yPosition, Vector2Int size)
    {
        return xPosition == size.x / 2 && yPosition == size.y / 2;
    }

    private void GatherPossibleDirections(List<MazeNode> currentPath, List<MazeNode> completedNodes, Vector2Int size,
        List<NodeSide> possibleDirections, List<int> possibleNextNodes)
    {
        int currentNodeIndex = currentPath[currentPath.Count - 1].nodeId;
        int currentNodeX = currentNodeIndex / size.y;
        int currentNodeY = currentNodeIndex % size.y;

        if (currentNodeX < size.x - 1)
        {
            // Check node to the right of the current node
            if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
                !currentPath.Contains(nodes[currentNodeIndex + size.y]))
            {
                possibleDirections.Add(NodeSide.Right);
                possibleNextNodes.Add(currentNodeIndex + size.y);
            }
        }

        if (currentNodeX > 0)
        {
            // Check node to the left of the current node
            if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                !currentPath.Contains(nodes[currentNodeIndex - size.y]))
            {
                possibleDirections.Add(NodeSide.Left);
                possibleNextNodes.Add(currentNodeIndex - size.y);
            }
        }

        if (currentNodeY < size.y - 1)
        {
            // Check node above the current node
            if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                !currentPath.Contains(nodes[currentNodeIndex + 1]))
            {
                possibleDirections.Add(NodeSide.Up);
                possibleNextNodes.Add(currentNodeIndex + 1);
            }
        }

        if (currentNodeY > 0)
        {
            // Check node below the current node
            if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                !currentPath.Contains(nodes[currentNodeIndex - 1]))
            {
                possibleDirections.Add(NodeSide.Down);
                possibleNextNodes.Add(currentNodeIndex - 1);
            }
        }
    }

    private void ChooseNextNode(List<MazeNode> currentPath, List<MazeNode> completedNodes, List<NodeSide> possibleDirections,
        List<int> possibleNextNodes)
    {
        if (possibleDirections.Count > 0)
        {
            int chosenDirection = Random.Range(0, possibleDirections.Count);
            MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

            switch (possibleDirections[chosenDirection])
            {
                case NodeSide.Right:
                    currentPath[currentPath.Count - 1].RemoveWall(NodeSide.Right);
                    chosenNode.RemoveWall(NodeSide.Left);
                    break;
                case NodeSide.Left:
                    currentPath[currentPath.Count - 1].RemoveWall(NodeSide.Left);
                    chosenNode.RemoveWall(NodeSide.Right);
                    break;
                case NodeSide.Up:
                    currentPath[currentPath.Count - 1].RemoveWall(NodeSide.Up);
                    chosenNode.RemoveWall(NodeSide.Down);
                    break;
                case NodeSide.Down:
                    currentPath[currentPath.Count - 1].RemoveWall(NodeSide.Down);
                    chosenNode.RemoveWall(NodeSide.Up);
                    break;
            }

            currentPath.Add(chosenNode);
            chosenNode.SetState(NodeState.Current);
        }
        else
        {
            completedNodes.Add(currentPath[currentPath.Count - 1]);
            currentPath[currentPath.Count - 1].SetState(NodeState.Completed);
            currentPath.RemoveAt(currentPath.Count - 1);
        }
    }

    public void Clear()
    {
        nodes.Clear();
        exitNodesIds.Clear();
        enterNode = null;
        
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
