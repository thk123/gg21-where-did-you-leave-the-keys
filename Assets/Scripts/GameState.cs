using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public AudioSource DoorKnock;
    KeySpawnSystem KeySpawnSystem;

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
        if(DoorToUnlock.IsUnlocked)
        {
            if (KeySpawnSystem.AnyMoreKeys)
            {
                KeySpawnSystem.SpawnNextKey();
                DoorToUnlock.Lock();
            }
            else
            {
                Debug.Log("Complete");
            }
        }
    }

    IEnumerator GameSequence()
    {
        yield return new WaitForSeconds(2.0f);
        DoorKnock.Play();
    }
}
