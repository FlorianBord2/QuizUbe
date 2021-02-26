using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Page : MonoBehaviour
{
	[HideInInspector] public MainProgram Program;

	public abstract void Open(string value);

	public virtual void Close()
	{
		gameObject.SetActive(false);
	}
}
