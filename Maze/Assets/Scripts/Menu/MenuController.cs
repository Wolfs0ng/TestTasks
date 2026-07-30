using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject gameItems;
    
    [SerializeField] private GameObject menuGameObject;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button setupGameButton;

    [SerializeField] private GameObject setupGameObject;
    [SerializeField] private TMP_InputField xSizeInputField;
    [SerializeField] private TMP_InputField ySizeInputField;
    [SerializeField] private Button setupDoneButton;

    [SerializeField] private GameObject gameEndGameObject;
    [SerializeField] private TextMeshProUGUI timerLabel;
    [SerializeField] private TextMeshProUGUI distanceLabel;
    [SerializeField] private Button returnToMenuButton;

    private void Start()
    {
        RegisterListeners();
    }

    private void RegisterListeners()
    {
        startGameButton.onClick.AddListener(OnStartButtonClick);
        setupGameButton.onClick.AddListener(OnSetupGameClick);
        setupDoneButton.onClick.AddListener(OnSetupDoneClick);
        returnToMenuButton.onClick.AddListener(OnReturnToMenuClick);
    }

    private void UnregisterListeners()
    {
        startGameButton.onClick.RemoveListener(OnStartButtonClick);
        setupGameButton.onClick.RemoveListener(OnSetupGameClick);
        setupDoneButton.onClick.RemoveListener(OnSetupDoneClick);
        returnToMenuButton.onClick.RemoveListener(OnReturnToMenuClick);
    }
    
    private void OnStartButtonClick()
    {
        menuGameObject.SetActive(false);
        gameItems.SetActive(true);
        gameController.StartGame();
    }

    private void OnSetupGameClick()
    {
        menuGameObject.SetActive(false);
        setupGameObject.SetActive(true);
    }

    private void OnSetupDoneClick()
    {
        gameController.SetMazeSize(new(int.Parse(xSizeInputField.text), int.Parse(ySizeInputField.text)));
        
        menuGameObject.SetActive(true);
        setupGameObject.SetActive(false);
    }

    public void ShowEndGame(int time, float distance)
    {
        gameItems.SetActive(false);
        gameEndGameObject.SetActive(true);
        
        timerLabel.text = time.ToString();
        distanceLabel.text = distance.ToString();
    }

    private void OnReturnToMenuClick()
    {
        gameEndGameObject.SetActive(false);
        menuGameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        UnregisterListeners();
    }
}
