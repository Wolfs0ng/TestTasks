using System;
using System.Collections;
using Scripts.Data;
using Scripts.Utilities;
using TMPro;
using UnityEngine;

namespace Scripts.Level.Controller
{
    public class TimerController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _leftTimeText;
        
        int _secondsPerAnswer;
        float _time;
        bool _isRunning;
        Coroutine _timerCounter;

        public Action OnTimeIsOut;

        public void SetData(TimerData data)
        {
            _time = data.RoundDuration;
            _secondsPerAnswer = data.SecondPerAnswer;
        }

        public void StartTimer()
        {
            _isRunning = true;
            _timerCounter = StartCoroutine(TimerCounter());
        }

        public void StopTimer()
        {
            _isRunning = false;
            StopCoroutine(_timerCounter);
            _timerCounter = null;
        }
        
        public void ResumeTimer()
        {
            _isRunning = true;
            _timerCounter = StartCoroutine(TimerCounter());
        }

        public void AddTime()
        {
            _time += _secondsPerAnswer;
            _leftTimeText.text = Mathf.RoundToInt(_time).ToString();
        }

        void TimeIsOut()
        {
            OnTimeIsOut?.Invoke();
        }

        IEnumerator TimerCounter()
        {
            while (_isRunning)
            {
                _leftTimeText.text = Mathf.RoundToInt(_time).ToString();

                if (_time <= 0f)
                    TimeIsOut();
                else
                    yield return Awaiters.Seconds(1f);

                _time--;
            }
        }
    }
}
