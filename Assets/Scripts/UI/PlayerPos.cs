using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPos : MonoBehaviour
{
    public CheckPointMaster cpm;

    // Start is called before the first frame update
    void Start()
    {
        cpm = GameObject.FindGameObjectWithTag("CheckPointMaster").GetComponent<CheckPointMaster>();

        transform.position = CheckPointMaster.lastCheckPointPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            transform.position = CheckPointMaster.lastCheckPointPos;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
