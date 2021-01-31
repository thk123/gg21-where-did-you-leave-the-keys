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
    public MusicManager MusicManager;
    public RigidbodyFirstPersonController Player;

    private int DoorsUnlocked;
    private float LevelStartTime;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(DoorKnock != null, "No door knock set");
        Debug.Assert(UIController != null, "No UI Controller set");
        Debug.Assert(Player != null, "No player set");
        KeySpawnSystem = GetComponent<KeySpawnSystem>();
        DoorsUnlocked = 0;
        LevelStartTime = Time.time;
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

                ShowEndScreen(false, DoorsUnlocked, Time.time - LevelStartTime);
                yield break;
            }
        }
        ShowEndScreen(true, DoorsUnlocked, Time.time - LevelStartTime);
    }

    private void ShowEndScreen(bool win, int doors, float time)
    {
        MusicManager.SetMusicIntensity(0);
        Player.mouseLook.SetCursorLock(false);
        Player.enabled = false;
        UIController.ShowEndGame(win, doors, time);
    }

    private void CompleteKey(float timeStartedKey)
    {
        ++DoorsUnlocked;
        UIController.ShowSuccess(Time.time - timeStartedKey);
        DoorToUnlock.Lock();
        MusicManager.SetMusicIntensity(0);
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
