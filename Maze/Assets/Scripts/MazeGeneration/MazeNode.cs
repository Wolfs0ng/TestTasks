using UnityEngine;

public enum NodeState
{
    Available,
    Current,
    Completed,
    Enter,
    Exit
}

public enum NodeSide
{
    Right,
    Left,
    Up,
    Down,
}

public class MazeNode : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;
    [SerializeField] private MeshRenderer floor;
    [SerializeField] private GameObject endGameCollider;

    public Vector2Int nodePos { get; private set; }
    public int nodeId { get; private set; }
    public bool canBeEnter { get; private set; }
    public bool canBeExit { get; private set; }
    public bool isEnter { get; set; }

    private bool isExitNode;

    public bool isExit
    {
        get => isExitNode;
        set
        {
            isExitNode = value;
            endGameCollider.SetActive(isExitNode);
        }
    }

    public void SetNodeParams(int nodeId, Vector2Int nodePos, bool canBeEnter, bool canBeExit)
    {
        this.nodeId = nodeId;
        this.nodePos = nodePos;
        this.canBeEnter = canBeEnter;
        this.canBeExit = canBeExit;
    }

    public void RemoveWall(NodeSide wallToRemove)
    {
        walls[(int)wallToRemove].gameObject.SetActive(false);
    }

    public void SetState(NodeState state)
    {
        switch (state)
        {
            case NodeState.Available:
                floor.material.color = Color.white;
                break;
            case NodeState.Current:
                floor.material.color = Color.yellow;
                break;
            case NodeState.Completed:
                floor.material.color = Color.blue;
                break;
            case NodeState.Enter:
                floor.material.color = Color.green;
                break;
            case NodeState.Exit:
                floor.material.color = Color.red;
                break;
        }
    }
}