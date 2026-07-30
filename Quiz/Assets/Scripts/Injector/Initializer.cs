using Scripts.Injector.Managers;
using UnityEngine;

namespace Scripts.Injector
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] PopupManager _popupManager;
        [SerializeField] DifficultyManager _difficultyManager;
        [SerializeField] LeaderBoardManager _leaderBoardManager;
        
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            ServiceLocator.Register(new SceneManager());
            ServiceLocator.Register(_popupManager);
            ServiceLocator.Register(_difficultyManager);
            ServiceLocator.Register(_leaderBoardManager);
            
            
            ServiceLocator.Get<SceneManager>().LoadMenuScene();
        }
    }
}
