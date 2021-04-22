using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoCase : MonoBehaviour
{
    public Text Name;
    public Text Score;
    public Text Date;

    public void Init(string name, string score, string nbQuestions, string date)
	{
        Name.text = name;
        Score.text = $"{score}/{nbQuestions}";
        Date.text = date;
    }
}
