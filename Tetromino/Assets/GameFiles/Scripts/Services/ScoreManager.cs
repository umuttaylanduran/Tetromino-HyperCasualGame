using UnityEngine;
using System;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

	public int Score { get; private set; }
	public int HighScore { get; private set; }
	public bool HasNewHighScore { get; private set; }

	public static event Action<int> ScoreUpdated = delegate {};
    public static event Action<int> HighscoreUpdated = delegate {};
	
	private const string HIGHSCORE = "HIGHSCORE";	

    void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        Reset();
    }

	public void Reset()
	{
		
		Score = 0;

		
		HighScore = PlayerPrefs.GetInt(HIGHSCORE, 0);
		HasNewHighScore = false;
	}

	public void AddScore(int amount)
	{
		Score += amount;

        
        ScoreUpdated(Score);

        
        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt(HIGHSCORE, HighScore);
            HasNewHighScore = true;

            HighscoreUpdated(HighScore);
        }
	}
}
