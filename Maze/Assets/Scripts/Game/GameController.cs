using System.Collections;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private MenuController menuController;
    [SerializeField] private MazeGenerator mazeGenerator;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private TextMeshProUGUI timerLabel;
    [SerializeField] private TextMeshProUGUI distanceLabel;

    [SerializeField] private LeaderboardManager leaderboardManager;

    private Vector2Int mazeSize = new(10,10);
    private PlayerController player;
    
    private Vector3 previousPlayerPosition;
    private float traveledDistance;
    private int timer;

    private bool isGameStarted;
    
    public void SetMazeSize(Vector2Int mazeSize)
    {
        this.mazeSize = mazeSize;
    }
    
    public void StartGame()
    {
        mazeGenerator.Clear();
        if (player != null)
        {
            Destroy(player.gameObject);
        }
        
        isGameStarted = true;
        
        GenerateMaze();
        SpawnPlayer();

        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timer++;
            timerLabel.text = timer.ToString();
        }
    }

    private void Update()
    {
        if (!isGameStarted)
        {
            return;
        }
        
        traveledDistance += Vector3.Distance( previousPlayerPosition, player.transform.position ) ;
        previousPlayerPosition = player.transform.position ;

        distanceLabel.text = (Mathf.Round(traveledDistance * 10) / 10).ToString();
    }

    private void GenerateMaze()
    {
        mazeGenerator.CreateMaze(mazeSize);
    }
    
    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, mazeGenerator.EnterNode.transform.position, Quaternion.identity)
            .GetComponent<PlayerController>();
        
        player.OnExitTriggered += OnExitTriggered;
    }

    private void OnExitTriggered()
    {
        isGameStarted = false;
        
        StopCoroutine(StartTimer());
        
        player.OnExitTriggered -= OnExitTriggered;
        player.enabled = false;
        float distanceTraveled = Mathf.Round(traveledDistance * 10) / 10;
        menuController.ShowEndGame(timer, distanceTraveled);
        leaderboardManager.AddNewResult(mazeSize, timer, distanceTraveled);
    }
}
