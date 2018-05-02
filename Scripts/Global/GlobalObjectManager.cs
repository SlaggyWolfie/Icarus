using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObjectManager : MonoBehaviour
{
    // Declare any public variables that you want to be able 
    // to access throughout the scene
    public GameObject player;
    public GameObject HUDCanvas;

    private void Start()
    {
        if (player == null) Debug.LogError("WARNING: Player reference missing!");
        if (HUDCanvas == null) Debug.LogError("WARNING: HUD Object reference missing!");
    }

    #region Implementation 1 - Current
    public static GlobalObjectManager Instance
    {
        get;
        private set;
    }

    private void OnEnable()
    {
        //Do not use with Awake (they do the same sort of)
        Instance = this;
    }

    //private void Awake()
    //{
    //    //Do not use with OnEnable (they do the same)
    //    if (Instance == null) Instance = this;
    //    else Destroy(gameObject); //remove duplicates
    //}
    #endregion

    #region Implementation 2
    //Change Inheritance Parent to Gameobject

    //private static GameObject _manager;
    //public static GameObject manager
    //{
    //    get
    //    {
    //        return _manager ??
    //            (_manager = FindObjectOfType<GlobalObjectManager>);
    //    }
    //}

    //private void OnEnable()
    //{
    //    _manager = this;
    //}
    #endregion Implementation 2
}
