using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoCase : MonoBehaviour
{
    public Text Name;
    public Text Score;
    public Text Date;
    public Text ChallSender;
    private string _senderFormat = "from  {0}";
    private string _receiverFormat = "to  {0}";

    public GameObject SwordPicto;

    public Color PendingColor;
    public Color WonColor;
    public Color LoseColor;

    public delegate void HistoCaseEvent();
    public HistoCaseEvent OnClicked;

    public void Init(string name, string score, string nbQuestions, string date, string otherName, bool pending, bool receiving, bool won = false)
	{
        Init(name, score, nbQuestions, date);

        SwordPicto.SetActive(pending);
        ChallSender.color = pending ? PendingColor : won ? WonColor : LoseColor;

        if (!string.IsNullOrEmpty(otherName))
        {
            if (receiving) ChallSender.text = string.Format(_senderFormat, otherName);
            else ChallSender.text = string.Format(_receiverFormat, otherName);
        }
        else
        {
            ChallSender.text = "";
        }
    }

    public void Init(string name, string score, string nbQuestions, string date)
	{
        Name.text = name;
        Score.text = $"{score}/{nbQuestions}";
        Date.text = date;
        ChallSender.text = "";
    }

    public void OnCaseClicked()
	{
        OnClicked?.Invoke();
    }
}
