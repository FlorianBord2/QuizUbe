using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLine : MonoBehaviour
{
    public Text UsernameText;
    public Text ScoreText;
    public Text QuizCountText;

    public string Username;
    public int Score;
    public int QuizCount;

    public void Init(string username, int score, int quizCount)
	{
        Username = username;
        UsernameText.text = username;
        Score = score;
        ScoreText.text = score.ToString();
        QuizCount = quizCount;
        QuizCountText.text = quizCount.ToString();
    }
}
