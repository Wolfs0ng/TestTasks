using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Data
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
    
    public enum Operation
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    [Serializable]
    public class TimerData
    {
        public int RoundDuration = 60;
        public int SecondPerAnswer;
    }
    
    [Serializable]
    public class TaskData
    {
        public List<Operation> Operations;
        public int AnswerCount;
        [Range(0, 999)]
        public int MinValue;
        [Range(0, 999)]
        public int MaxValue;
    }
    
    [Serializable]
    public class BoosterData
    {
        public int InitialBoosterCount;
        public int AppearancePeriod;
        public int RemovedAnswersCount;
    }
    
    [CreateAssetMenu(fileName = "DifficultyData", menuName = "Difficulty")]
    public class DifficultyData : ScriptableObject
    {
        public Difficulty DifficultyType;
        [Header("Round settings")]
        public TimerData TimerData;
        public int PointsPerAnswer;
        
        [Header("Generation task settings")]
        public TaskData TaskData;
        
        [Header("Booster settings")]
        public BoosterData BoosterData;
    }
}