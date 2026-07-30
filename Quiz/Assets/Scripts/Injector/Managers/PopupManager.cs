using Scripts.UI.Popup;
using UnityEngine;

namespace Scripts.Injector.Managers
{
    public enum PopUp
    {
        GameOver,
        LeaderBoard
    }
    
    public class PopupManager : MonoBehaviour
    {
        const string CANVAS = "Canvas";
        
        [SerializeField] Transform _root;
        [SerializeField] GameObject _gameOverPopupPrefab;
        [SerializeField] GameObject _leaderBoardPopupPrefab;

        GameOverPopup _currGameOverPopup;
        LeaderBoardPopup _currLeaderBoardPopup;
        
        public void ShowPopup(PopUp name)
        {
            switch (name)
            {
                case PopUp.GameOver:
                    CreatePopUp<GameOverPopup>(_currGameOverPopup, _gameOverPopupPrefab);
                    break;
                case PopUp.LeaderBoard:
                    CreatePopUp<LeaderBoardPopup>(_currLeaderBoardPopup, _leaderBoardPopupPrefab);
                    break;
            }
        }

        void CreatePopUp<T>(BasePopup popup, GameObject prefab) where T : BasePopup
        {
            if (_root == null)
                _root = GameObject.FindGameObjectWithTag(CANVAS).transform;

            if (popup == null)
                popup = Instantiate(prefab, _root).GetComponent<T>();
                
            popup.Show();
        }
    }
}