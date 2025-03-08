using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
	// really simple script to move the game to different scenes with mouse clicks
	// create a bool for your scene and add a unique if(is____) block to link to a new scene. 
	// change the selected bool in the inspector to make the prefab go to different screens

	public bool isLab1;

	public bool isLab2;

	public bool isLab3test2;

	public bool isLab3;

	public bool isTailtest;

	public bool isTopdown;

	public bool isTitle;

	public bool isQuit;

	// Start is called before the first frame update
	void OnMouseUp()
	{

		if (isLab1)
		{
			SceneManager.LoadScene("Lab1");
		}

		if (isLab2)
		{
			SceneManager.LoadScene("");
		}

		if (isLab3test2)
		{
			SceneManager.LoadScene("lab3_Test2");
		}

		if (isLab3)
		{
			SceneManager.LoadScene("Lab3_Test1");
		}

		if (isTailtest)
		{
			SceneManager.LoadScene("Lab2_Test");
		}

		if (isTopdown)
		{
			SceneManager.LoadScene("cbLab2");
		}

		if (isTitle)
		{
			Debug.Log("Button clicked!");
			SceneManager.LoadScene("Title");
		}

		
		if (isQuit)
		{
			
			Application.Quit();
		}

		Time.timeScale = 1f;
	}
}
