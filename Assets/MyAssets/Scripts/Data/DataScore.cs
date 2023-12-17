using System;
using System.Linq;
using UnityEngine;

namespace MyAssets.Scripts.Data
{
    public class DataScore
    {
        private const string HighScoreKey = "Highscore";

        private int _currentScore;
        
        public int[] LastFiveScores { get; }
        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                OnScoreUpdated?.Invoke(value);
            }
        }

        
        public event Action<int> OnScoreUpdated;
        
        public DataScore()
        {
            LastFiveScores = new int[5];
            for (var i = 0; i < LastFiveScores.Length; i++)
                LastFiveScores[i] = PlayerPrefs.GetInt($"{HighScoreKey}{i}", 0);
            Array.Sort(LastFiveScores);
        }
        
        public void UpdateHighScore(int newScore)
        {
            Debug.Log("Updating high score");
            var isBetter = LastFiveScores.Any(scr => newScore > scr);
            if (isBetter)
            {
                LastFiveScores[0] = newScore;
                Array.Sort(LastFiveScores);
            }
            for (var i = 0; i < LastFiveScores.Length; i++)
                PlayerPrefs.SetInt($"{HighScoreKey}{i}", LastFiveScores[i]);
        }
    }
}