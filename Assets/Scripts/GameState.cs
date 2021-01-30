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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(DoorKnock != null, "No door knock set");
        KeySpawnSystem = GetComponent<KeySpawnSystem>();
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
            KeySpawnSystem.SpawnNextKey();
            DoorKnock.Play();
            yield return WaitUntil(() => DoorToUnlock.IsUnlocked, Timeout);
            if (DoorToUnlock.IsUnlocked)
            {
                DoorToUnlock.Lock();
                if (KeySpawnSystem.AnyMoreKeys)
                {
                    yield return new WaitForSeconds(2.0f);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        }
        Debug.Log("Complete");

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
