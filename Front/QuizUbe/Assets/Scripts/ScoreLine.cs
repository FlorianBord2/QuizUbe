using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLine : MonoBehaviour
{
    public Text Username;
    public Text Score;

    public void Init(string username, string score)
	{
        Username.text = username;
        Score.text = score;
    }
}
