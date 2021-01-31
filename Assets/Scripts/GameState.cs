using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameState : MonoBehaviour
{
    public DoorKnocker DoorKnock;
    KeySpawnSystem KeySpawnSystem;
    public float Timeout = 60.0f;
    public UnlockableDoor DoorToUnlock;
    public UIController UIController;
    public Fade Fade;
    public MusicManager MusicManager;
    public RigidbodyFirstPersonController Player;
    public AudioClip MotorbikeAway;
    public AudioClip MotorbikeArrived;

    private int DoorsUnlocked;
    private float LevelStartTime;

   // public CentralDoor CentralDoor;
    public GameObject CentralDoor;
    public int KeysForCentralDoor = 3;

    public Transform[] DifficultyObjects;
    public int[] DifficultyThresholds;
    int CurrentDifficulty = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(DoorKnock != null, "No door knock set");
        Debug.Assert(UIController != null, "No UI Controller set");
        Debug.Assert(Player != null, "No player set");
        Debug.Assert(MotorbikeAway != null, "No motorbike away sound");

        Fade = Fade ?? UIController.GetComponent<Fade>();
        Debug.Assert(Fade != null, "Could not find fade component");

        KeySpawnSystem = GetComponent<KeySpawnSystem>();
        DoorsUnlocked = 0;
        CurrentDifficulty = 0;
        LevelStartTime = Time.time;
        CentralDoor.SetActive(true);

        for (int i=0; i < DifficultyObjects.Length; i++) {
            foreach (Transform SomeObject in DifficultyObjects[i]) {
                SomeObject.gameObject.SetActive(false);
            }
        }

        StartCoroutine(GameSequence());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator GameSequence()
    {
        // Skip one frame to ensure the key system is set up
        yield return null;
        Player.mouseLook.SetCursorLock(true);
        Player.enabled = false;
        yield return UIController.ShowTutorial_Iter();
        Player.enabled = true;
        while (KeySpawnSystem.AnyMoreKeys)
        {            
            KeySpawnSystem.SpawnNextKey();
            AudioSource.PlayClipAtPoint(MotorbikeArrived, DoorKnock.transform.position);
            DoorKnock.Knock(0);
            float timeStartedKey = Time.time;
            yield return WaitUntil(() => DoorToUnlock.IsUnlocked, Timeout / 3.0f);
            if (DoorToUnlock.IsUnlocked)
            {
                yield return CompleteKey(timeStartedKey);
                continue;
            }
            DoorKnock.Knock(1);
            MusicManager.SetMusicIntensity(1);
            yield return WaitUntil(() => DoorToUnlock.IsUnlocked, Timeout / 3.0f);
            if (DoorToUnlock.IsUnlocked)
            {
                yield return CompleteKey(timeStartedKey);
                continue;
            }
            DoorKnock.Knock(2);
            MusicManager.SetMusicIntensity(2);
            yield return WaitUntil(() => DoorToUnlock.IsUnlocked, Timeout / 3.0f);
            if (DoorToUnlock.IsUnlocked)
            {
                yield return CompleteKey(timeStartedKey);
                continue;
            }
            else
            {

                ShowEndScreen(false, DoorsUnlocked, Time.time - LevelStartTime);
                yield break;
            }
        }
        ShowEndScreen(true, DoorsUnlocked, Time.time - LevelStartTime);
    }

    private void ShowEndScreen(bool win, int doors, float time)
    {
        if(!win)
        {
            AudioSource.PlayClipAtPoint(MotorbikeAway, Camera.main.transform.position);
        }
        MusicManager.SetMusicIntensity(0);
        Player.mouseLook.SetCursorLock(false);
        Player.enabled = false;
        UIController.ShowEndGame(win, doors, time);
    }

    private IEnumerator CompleteKey(float timeStartedKey)
    {
        ++DoorsUnlocked;
        MusicManager.SetMusicIntensity(0);
        yield return UIController.ShowSuccess(Time.time - timeStartedKey);
        DoorToUnlock.Lock();

        yield return Fade.FadeToBlack(1.0f);

       // Debug.Log(DoorsUnlocked);
        if (DoorsUnlocked == KeysForCentralDoor) {
           // CentralDoor.isUnlocked();
           CentralDoor.SetActive(false);
        }

        if (DoorsUnlocked >= DifficultyThresholds[CurrentDifficulty]) {
            //move up a difficulty and unlock stuff
            CurrentDifficulty = Mathf.Clamp(CurrentDifficulty + 1, 0, DifficultyObjects.Length - 1);
            foreach (Transform SomeObject in DifficultyObjects[CurrentDifficulty]) {
                SomeObject.gameObject.SetActive(true);
            }

        }

        yield return Fade.FadeToClear(1.0f);
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator WaitUntil(Func<bool> condition, float timeout)
    {
        float timeRemaining = timeout;
        while(!condition.Invoke())
        {
            if(timeRemaining > 0.0f)
            {
                timeRemaining -= Time.deltaTime;
                yield return null;
            }
            else
            {
                break;
            }
        }
    }
}
