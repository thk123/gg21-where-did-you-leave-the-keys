using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public AudioSource DoorKnock;
    KeySpawnSystem KeySpawnSystem;
    public float Timeout = 60.0f;
    public UnlockableDoor DoorToUnlock;
    public UIController UIController;

    private int DoorsUnlocked;
    private float StartTime;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(DoorKnock != null, "No door knock set");
        Debug.Assert(UIController != null, "No UI Controller set");
        KeySpawnSystem = GetComponent<KeySpawnSystem>();
        DoorsUnlocked = 0;
        StartTime = Time.time;
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
            DoorKnock.Play();
            float timeStarted = Time.time;
            yield return WaitUntil(() => DoorToUnlock.IsUnlocked, Timeout);
            if (DoorToUnlock.IsUnlocked)
            {
                ++DoorsUnlocked;
                UIController.ShowSuccess(Time.time - timeStarted);
                DoorToUnlock.Lock();
            }
            else
            {
                UIController.ShowFailure(DoorsUnlocked, Time.time - StartTime);
            }
        }
        UIController.ShowCompletion(DoorsUnlocked, Time.time - StartTime);

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
