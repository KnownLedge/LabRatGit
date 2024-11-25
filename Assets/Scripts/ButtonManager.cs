using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
	// really simple script to move the game to different scenes with mouse clicks
	// create a bool for your scene and add a unique if(is____) block to link to a new scene. 
	// change the selected bool in the inspector to make the prefab go to different screens

	public bool isPuzzle;

	public bool isTailtest;

	public bool isTopdown;

	public bool isTitle;

	public bool isQuit;

	// Start is called before the first frame update
	void OnMouseUp()
	{

		if (isPuzzle)
		{
			SceneManager.LoadScene("PuzzleScene");
		}

		if (isTailtest)
		{
			SceneManager.LoadScene("TailTestRoom");
		}

		if (isTopdown)
		{
			SceneManager.LoadScene("TopDownExample");
		}

		if (isTitle)
		{
			SceneManager.LoadScene("Title");
		}

		// If the quit button is clicked
		if (isQuit)
		{
			// Quit the application
			Application.Quit();
		}

		Time.timeScale = 1f;
	}
}
