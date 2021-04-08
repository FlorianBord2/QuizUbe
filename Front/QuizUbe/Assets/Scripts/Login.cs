using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login
{
	public class LoginData
	{
		public string localId;
	}

    public class LoginResponse
	{
		public string displayName;
		public string email;
		public string expiresIn;
		public string idToken;
		public string king;
		public string localId;
		public string refreshToken;
		public bool registered;
	}

	public class RegisterErrorResponse
	{
		public struct ErrorBody
		{
			public string domain;
			public string message;
			public string reason;
		}

		public int code;
		public ErrorBody[] errors;
		public string message;
	}

	public class RegisterResponse
	{
		public string email;
		public string expiresIn;
		public string idToken;
		public string kind;
		public string localId;
		public string refreshToken;
	}
}
