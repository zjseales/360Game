using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointMaster : MonoBehaviour
{
    public static CheckPointMaster instance;
    public static Vector2 lastCheckPointPos;

    public static Vector2 firstPosition;

    void Awake()
    {
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        } else
        {
            Destroy(gameObject);
        }
    }

}
