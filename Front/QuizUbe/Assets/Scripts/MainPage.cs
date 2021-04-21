using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : Page
{

	public void OnPlayButtonClicked()
	{
		Close();
		Program.SearchPage.Open(null);
	}

	public void OnFriendsButtonClicked()
	{
		Close();
		Program.FriendsPage.Open(null);
	}

	public void OnLeaderboardButtonClicked()
	{
		Close();
		Program.LeaderboardPage.Open(null);
	}


	public override void Open(string value)
	{
        gameObject.SetActive(true);
	}

}
