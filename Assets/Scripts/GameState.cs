using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public DoorKnocker DoorKnock;
    KeySpawnSystem KeySpawnSystem;
    public float Timeout = 60.0f;
    public UnlockableDoor DoorToUnlock;
    public UIController UIController;
    public MusicManager MusicManager;

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
        while (KeySpawnSystem.AnyMoreKeys)
        {
            yield return new WaitForSeconds(2.0f);
            KeySpawnSystem.SpawnNextKey();
            DoorKnock.Knock(0);
            float timeStartedKey = Time.time;
            yield return WaitUntil(() => DoorToUnlock.IsUnlocked, Timeout / 3.0f);
            if (DoorToUnlock.IsUnlocked)
            {
                CompleteKey(timeStartedKey);
                continue;
            }
            DoorKnock.Knock(1);
            MusicManager.SetMusicIntensity(1);
            yield return WaitUntil(() => DoorToUnlock.IsUnlocked, Timeout / 3.0f);
            if (DoorToUnlock.IsUnlocked)
            {
                CompleteKey(timeStartedKey);
                continue;
            }
            DoorKnock.Knock(2);
            MusicManager.SetMusicIntensity(2);
            yield return WaitUntil(() => DoorToUnlock.IsUnlocked, Timeout / 3.0f);
            if (DoorToUnlock.IsUnlocked)
            {
                CompleteKey(timeStartedKey);
                continue;
            }
            else
            {
                MusicManager.SetMusicIntensity(0);
                UIController.ShowFailure(DoorsUnlocked, Time.time - LevelStartTime);
                yield break;
            }
        }
        UIController.ShowCompletion(DoorsUnlocked, Time.time - LevelStartTime);

    }

    private void CompleteKey(float timeStartedKey)
    {
        ++DoorsUnlocked;
        UIController.ShowSuccess(Time.time - timeStartedKey);
        DoorToUnlock.Lock();
        MusicManager.SetMusicIntensity(0);


       // Debug.Log(DoorsUnlocked);
        if (DoorsUnlocked == KeysForCentralDoor) {
           // CentralDoor.isUnlocked();
           CentralDoor.SetActive(false);
        }

        if (DoorsUnlocked >= DifficultyThresholds[CurrentDifficulty]) {
            //move up a difficulty and unlock stuff
            CurrentDifficulty++;
            foreach (Transform SomeObject in DifficultyObjects[CurrentDifficulty]) {
                SomeObject.gameObject.SetActive(true);
            }

        }

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
