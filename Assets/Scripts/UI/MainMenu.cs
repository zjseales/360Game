using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject ControlsUI;
    private bool openControls;
    private float quitTimer;

    void Update(){
        if(Input.GetKey(KeyCode.W) && !openControls){
            CheckPointMaster.firstPosition = new Vector2(-17.39f, -2.58f);
            CheckPointMaster.lastCheckPointPos = CheckPointMaster.firstPosition;
            Play();
        } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) && !openControls){
            ControlsOpen();
        }
        if(Input.GetKeyDown(KeyCode.S) && openControls){
            ControlsClose();
        } else if (Input.GetKeyDown(KeyCode.S) && !openControls){
            quitTimer = 0;
        }
        if (Input.GetKey(KeyCode.S)){
            quitTimer += Time.deltaTime;
        } else {
            quitTimer = 0;
        }
        if(quitTimer >= 3){
            QuitGame();
        }
    }
    
    public void QuitGame(){
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    // Update is called once per frame
    public void Play()
    {

        // Start the new game
        //Load cutscene
        SceneManager.LoadScene("OpeningScene2");
        // Load the first level
        //SceneManager.LoadScene("Level1_ArmadilloOnly");
        
    }

    public void ControlsOpen()
    {
        openControls = true;
        ControlsUI.SetActive(true);

    }

    public void ControlsClose()
    {
        openControls = false;
        ControlsUI.SetActive(false);

    }
}
