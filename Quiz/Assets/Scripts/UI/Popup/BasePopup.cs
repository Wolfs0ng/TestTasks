using Scripts.Injector.Managers;
using UnityEngine;

namespace Scripts.UI.Popup
{
    public class BasePopup : MonoBehaviour
    {
        [SerializeField] PopUp _name;
        
        public PopUp Name => _name;
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}