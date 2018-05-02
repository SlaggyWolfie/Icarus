using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryComponent : BaseAction
{
    public static bool puzzleCompleted = false;

    public static GameObject buttonPressed;
    public List<GameObject> previewLights;
    [Header("Highlight Settings")]
    [Tooltip("Indicates for how long the highlighting will last for each preview.")]
    public float highlightTime;
    [Tooltip("Indicates what the delay will be between each preview highlight.")]
    public float highlightDelay;

    public float completionTime = 10f;
    //public int endTime;
    [Header("Subtitles")]
    public Subtitles[] _subtitles;

    private UI_Subtitles _ui_subtitles;
    private GameObject[] _previewLightsRandomSequence;
    private GameObject _currentPreviewLight;
    private GameObject _startLight;
    private int[] _previewLightsNameIntegers;
    private bool _puzzleIsStarted = false;

    private void Start ()
    {
        _startLight = GetComponentInChildren<TriggerGameObjectActivation>().gameObject;
        _ui_subtitles = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_Subtitles>();
        if ( _ui_subtitles == null )
            Debug.LogError("WARNING: UI_Subtitles reference missing from MemoryComponent");
    }

    public override void Action ()
    {
        if ( !_puzzleIsStarted )
        {
            Debug.Log("Puzzle started!");
            _ui_subtitles.SetUpSingleSub(_subtitles[0]);    //Subtitles for Sequence Started!
            InitializeSequence();
        }
        else
        {
            RestartSequence();
        }
    }

    //----------------------------------------------------------------------------------------------------
    //-------------------------------------         START        -----------------------------------------
    //----------------------------------------------------------------------------------------------------
    private void InitializeSequence ()
    {
        _previewLightsRandomSequence = new GameObject[previewLights.Count];
        _previewLightsNameIntegers = new int[previewLights.Count];
        List<GameObject> previewLightsClone = new List<GameObject>(previewLights);  //prevents loss of original list in the inspector and allows for easy restarting
        for ( int i = 0; i < previewLights.Count; i++ )
        {
            _previewLightsRandomSequence[i] = previewLightsClone[UnityEngine.Random.Range(0, previewLightsClone.Count)];    //randomizes the sequence
            ExtractIntegersFromName(_previewLightsRandomSequence[i].name, out _previewLightsNameIntegers[i]);
            previewLightsClone.Remove(_previewLightsRandomSequence[i]);
        }
        _puzzleIsStarted = true;
        PuzzleStart();
    }

    private void PuzzleStart ()
    {
        DisableLights();
        StartCoroutine(InitiationHighlights(highlightTime, highlightDelay));    //starts the preview lights
    }

    //----------------------------------------------------------------------------------------------------
    //------------------------------------        HIGHLIGHT        ---------------------------------------
    //----------------------------------------------------------------------------------------------------

    IEnumerator InitiationHighlights ( float pHighlightTime, float pDelay )
    {
        for ( int i = 0; i < _previewLightsRandomSequence.Length; i++ )
        {
            _currentPreviewLight = _previewLightsRandomSequence[i];
            StartCoroutine(HighlightPreview(pHighlightTime));
            yield return new WaitForSeconds(pHighlightTime + pDelay);
        }
        EnableLights();
        StartCoroutine(CooldownTimer(completionTime));  //start the puzzle timer after the highlighting has stopped
        StartCoroutine(SequenceUpdate());               //starts searching for input from the player
    }
    /// <summary>
    /// Highlights the current preview light for a set amount of time.
    /// </summary>
    /// <param name="pWaitTime">How long the highlighting lasts.</param>
    /// <returns></returns>
    IEnumerator HighlightPreview ( float pWaitTime )
    {
        _currentPreviewLight.GetComponent<TriggerGameObjectActivation>().Activate();
        yield return new WaitForSeconds(pWaitTime);
        _currentPreviewLight.GetComponent<TriggerGameObjectActivation>().Deactivate();
    }

    private void EnableLights ()
    {
        foreach ( GameObject _object in _previewLightsRandomSequence )
        {
            _object.GetComponent<TriggerGameObjectActivation>().Activate();
        }
        _startLight.GetComponentInChildren<TriggerGameObjectActivation>().Activate();
    }

    private void DisableLights ()
    {
        foreach ( GameObject _object in _previewLightsRandomSequence )
        {
            _object.GetComponent<TriggerGameObjectActivation>().Deactivate();
        }
        _startLight.GetComponentInChildren<TriggerGameObjectActivation>().Deactivate();
    }


    //----------------------------------------------------------------------------------------------------
    //------------------------------------         UPDATE        -----------------------------------------
    //----------------------------------------------------------------------------------------------------

    IEnumerator SequenceUpdate ()
    {
        for ( int i = 0; i < _previewLightsRandomSequence.Length; i++ )
        {
            int currentNumber = 0;
            buttonPressed = null;
            while ( !buttonPressed )    //waits for input before checking if the input is correct
            {
                yield return null;
            }
            ExtractIntegersFromName(buttonPressed.name, out currentNumber);
            CheckInput(_previewLightsRandomSequence[i], currentNumber);     //checks if the input is the correct input from the sequence
        }
        FinalizePuzzle();
    }

    void CheckInput ( GameObject pButton, int pCurrentSequenceNumber )
    {
        int buttonNumber = 0;
        ExtractIntegersFromName(pButton.name, out buttonNumber);
        Debug.Log("Comparing " + buttonNumber + " with " + pCurrentSequenceNumber + " = " + (buttonNumber == pCurrentSequenceNumber));
        if ( buttonNumber != pCurrentSequenceNumber )   //restarts puzzle if the input is different than the answer
        {
            Debug.Log("Wrong sequence! Returning...");
            RestartSequence();
        }
        else
        {
            Debug.Log("Input was correct...");
            _ui_subtitles.SetUpSingleSub(_subtitles[3]);    //Subtitles for Input Correct!

        }
    }

    private void ExtractIntegersFromName ( string pName, out int pInteger )
    {
        pInteger = 0;
        string[] extractInt = pName.Split(' ');
        foreach ( string s in extractInt )
        {
            int.TryParse(s, out pInteger);
        }
        //Debug.Log(pInteger);
    }

    //----------------------------------------------------------------------------------------------------
    //------------------------------------         RESTART        ----------------------------------------
    //----------------------------------------------------------------------------------------------------

    private void RestartSequence ()
    {
        StopAllCoroutines();
        Debug.Log("All coroutines stopped and highlights reset!");
        _ui_subtitles.SetUpSingleSub(_subtitles[1]);    //Subtitles for Sequence Ended!
        GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_TimerOverlay>().SetTimer(0);
        _puzzleIsStarted = false;
        puzzleCompleted = false;
        EnableLights();
    }

    IEnumerator CooldownTimer ( float pCooldownTime )
    {
        GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_TimerOverlay>().SetTimer((int)pCooldownTime);
        yield return new WaitForSeconds(pCooldownTime);
        RestartSequence();
    }

    //----------------------------------------------------------------------------------------------------
    //------------------------------------           END          ----------------------------------------
    //----------------------------------------------------------------------------------------------------

    private void FinalizePuzzle ()
    {
        StopAllCoroutines();
        GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_TimerOverlay>().SetTimer(0);
        Debug.Log("Sequence was correct! Congratulations!");
        _ui_subtitles.SetUpSingleSub(_subtitles[2]);    //Subtitles for Sequence Correct!

        puzzleCompleted = true;
    }

}
