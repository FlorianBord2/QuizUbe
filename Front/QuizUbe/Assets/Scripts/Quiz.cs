using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz
{
	public class Question
	{
		/// <summary>
		/// Good answer
		/// </summary>
		public string Response;

		public string Title;
		public string Type;

		/// <summary>
		/// ??????????????
		/// </summary>
		public string UserResponse;

		public Video[] Videos;
	}

	public class Video
	{
		/// <summary>
		/// Amount of comments
		/// </summary>
		public ulong Comment;

		public string Desc;

		/// <summary>
		/// Amount of dislikes
		/// </summary>
		public ulong Dislike;

		/// <summary>
		/// Amount of likes
		/// </summary>
		public ulong Like;

		public string Title;

		/// <summary>
		/// URL of the video picture
		/// </summary>
		public string URL;

		/// <summary>
		/// Amount of views
		/// </summary>
		public ulong View;
	}

	public Question[] Questions;
	
	public Quiz(Question[] questions)
	{
		Questions = questions;
	}
}


