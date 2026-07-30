using System.Collections.Generic;
using System.Linq;
using Scripts.Injector;
using Scripts.Injector.Managers;
using Scripts.UI.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Popup
{
    public class LeaderBoardPopup : BasePopup
    {
        [SerializeField] Button _cancelButton;
        [SerializeField] Transform _content;
        [SerializeField] PlayerResultView _playerResultsPrefab;
        [SerializeField] List<PlayerResultView> _playerResults;
        
        LeaderBoardManager _leaderBoardManager;

        void Awake()
        {
            _leaderBoardManager = ServiceLocator.Get<LeaderBoardManager>();
        }

        void OnEnable()
        {
            _cancelButton.onClick.AddListener(CancelButtonHandler);
            LoadResults();
        }
        
        void OnDisable()
        {
            _cancelButton.onClick.RemoveListener(CancelButtonHandler);
        }
        void CancelButtonHandler()
        {
           Hide();
        }

        void LoadResults()
        {
            foreach (var resultData in _leaderBoardManager.GetResultsData())
            {
                var currPlayerView = _playerResults.FirstOrDefault(p => !p.gameObject.activeInHierarchy);
                
                if (currPlayerView == null)
                {
                    currPlayerView = Instantiate(_playerResultsPrefab, _content);
                    _playerResults.Add(currPlayerView);
                }
                
                currPlayerView.Show(resultData.Name, resultData.Score);
            }
        }
    }
}