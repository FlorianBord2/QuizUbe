using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoginPage : Page
{
	public Text Title;

	//Login
	public InputField UserInputField;
	public InputField PasswordInputField;

	//Register
	public InputField RUserInputField;
	public InputField RUsernameInputField;
	public InputField RPasswordInputField;

	public Text ErrorMsg;

	private const string LOGIN_URL = @"http://127.0.0.1:65000/users/login";
	private const string REGISTER_URL = @"http://127.0.0.1:65000/users/register?";

	private enum SelectedTab
	{
		Login,
		Register
	}

	public GameObject[] Tabs;

	public void OnTryRegisterButtonClicked()
	{
		OpenTab(SelectedTab.Register);
		ErrorMsg.text = "";
	}

	public void OnTryLoginButtonClicked()
	{
		OpenTab(SelectedTab.Login);
	}

	public void OnRegisterButtonClicked()
	{
		bool valid = true;
		if (string.IsNullOrWhiteSpace(RUserInputField.text) || string.IsNullOrWhiteSpace(RUsernameInputField.text) || string.IsNullOrWhiteSpace(RPasswordInputField.text))
		{
			valid = false;
		}

		if (valid)
		{
			string requestResp = WebUtility.Instance.Get(REGISTER_URL, ("username", RUsernameInputField.text.Trim(' ')), ("email", RUserInputField.text.Trim(' ')), ("password", RPasswordInputField.text.Trim(' ')));

			StringReader reader = new StringReader(requestResp);
			JsonSerializer ser = JsonSerializer.Create(new JsonSerializerSettings());

			if (requestResp.Contains("errors"))
			{
				Login.RegisterErrorResponse error = (Login.RegisterErrorResponse)ser.Deserialize(reader, typeof(Login.RegisterErrorResponse));
				ErrorMsg.text = error.message;
			}
			else
			{
				ErrorMsg.text = "";

				Login.RegisterResponse response = (Login.RegisterResponse)ser.Deserialize(reader, typeof(Login.RegisterResponse));


				Program.LoginData.localId = response.localId;
				LoadNextPage();
			}
		}
	}

	public void OnLoginButtonClicked()
	{
		bool valid = true;
		if (string.IsNullOrWhiteSpace(UserInputField.text) || string.IsNullOrWhiteSpace(PasswordInputField.text))
		{
			valid = false;
		}

		if (valid)
		{
			Title.text = "Waiting for response";
			string requestResp = WebUtility.Instance.Get(LOGIN_URL, ("email", UserInputField.text.Trim(' ')), ("password", PasswordInputField.text.Trim(' ')));

			StringReader reader = new StringReader(requestResp);
			JsonSerializer ser = JsonSerializer.Create(new JsonSerializerSettings());
			Login.LoginResponse response = (Login.LoginResponse)ser.Deserialize(reader, typeof(Login.LoginResponse));

			Title.text = requestResp;
			if (response.registered)
			{
				Program.LoginData.localId = response.localId;
				LoadNextPage();
			}
		}
	}

	private void LoadNextPage()
	{
		Close();
		Program.MainPage.Open(null);
	}


	public override void Open(string value)
	{
		gameObject.SetActive(true);
		OpenTab(SelectedTab.Login);
	}

	private void OpenTab(SelectedTab tabToOpen)
	{
		for (int i = 0; i < Tabs.Length; i++)
		{
			Tabs[i].SetActive(i == (int)tabToOpen);
		}
	}
}
