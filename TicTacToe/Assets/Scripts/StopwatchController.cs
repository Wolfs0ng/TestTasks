using TMPro;
using UnityEngine;

namespace Scripts.Level.Controller
{
    public class StopwatchController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _timeText;
        
        private float startTime;
        private float elapsedTime;
        private bool isRunning;

        private void Update()
        {
            if (isRunning)
            {
                elapsedTime = Time.time - startTime;
                _timeText.text = FormatTime(elapsedTime);
            }
        }

        public void StartTimer()
        {
            startTime = Time.time;
            isRunning = true;
        }

        public void StopTimer()
        {
            isRunning = false;
        }

        public void ResetTimer()
        {
            elapsedTime = 0;
            startTime = Time.time;
            _timeText.text = FormatTime(0);
        }

        string FormatTime(float time)
        {
            int minutes = (int)(time / 60);
            int seconds = (int)(time % 60);

            string formattedTime = $"{minutes:00}:{seconds:00}";
            return formattedTime;
        }
    }
}
