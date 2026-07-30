using TMPro;
using UnityEngine;

namespace Scripts.UI.Items
{
   public class PlayerResultView : MonoBehaviour
   {
      [SerializeField] TextMeshProUGUI _name;
      [SerializeField] TextMeshProUGUI _result;

      public void Show(string name, int result)
      {
         _name.text = name;
         _result.text = result.ToString();
         gameObject.SetActive(true);
      }
   }
}
