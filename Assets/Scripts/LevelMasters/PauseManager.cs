using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
	GameObject[] pauseObjects;
    private CheckPointMaster cpm;

	public static bool showing;
	private float quitTimer;

    // Use this for initialization
    void Start()
	{
		quitTimer = 0;
		showing = false;
		Time.timeScale = 1;
		pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
		hidePaused();

        if (GameObject.FindGameObjectWithTag("CheckPointMaster") != null)
        {
            cpm = GameObject.FindGameObjectWithTag("CheckPointMaster").GetComponent<CheckPointMaster>();
        }
    }

	// Update is called once per frame
	void Update()
	{
		if (showing){
			Time.timeScale = 0;
			if(Input.GetKey(KeyCode.W)){
				hidePaused();
			} else if(Input.GetKey(KeyCode.R)){
				Reload();
			} else if (Input.GetKeyDown(KeyCode.S)){
           		quitTimer = 0;
			}
			if (Input.GetKey(KeyCode.S)){
            	quitTimer += Time.deltaTime;
        	} else {
            	quitTimer = 0;
        	}
        	if(quitTimer >= 3){
            	MainMenu();
        	}
        } else {
			Time.timeScale = 1;
		}

		//uses the p button to pause and unpause the game
		if (Input.GetKeyDown(KeyCode.P))
		{
			if (!showing)
			{
				showPaused();
			}
			else if (showing)
			{
				Debug.Log("turn off pause");
				hidePaused();
			}

	
		}
	}


	//Reloads the Level
	public void Reload()
	{
		CheckPointMaster.lastCheckPointPos = CheckPointMaster.firstPosition;
		// GameMaster.playerHealth = 3;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

	//controls the pausing of the scene
	public void pauseControl()
	{
		if (!showing)
		{
			showPaused();
		}
		else if (showing)
		{
			hidePaused();
		}
	}

	//shows objects with ShowOnPause tag
	public void showPaused()
	{
		showing = true;
		foreach (GameObject g in pauseObjects)
		{
			g.SetActive(true);
		}
	}

	//hides objects with ShowOnPause tag
	public void hidePaused()
	{
		showing = false;
		foreach (GameObject g in pauseObjects)
		{
			g.SetActive(false);
		}
	}

	//loads inputted level
	public void LoadLevel(string level)
	{
		SceneManager.LoadScene(level);
	}

	public void MainMenu()
    {
		SceneManager.LoadScene("MainMenu");

	}

}
