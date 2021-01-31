using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawnSystem : MonoBehaviour
{
    public List<GameObject> KeySpawnPositions;
    public GameObject KeyPrefab;

    public Queue<GameObject> RemainingSpots;
    Transform LastSpawnPoint;
    GameObject NextKey;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(KeyPrefab != null, "No key prefab set");

        //RemainingSpots = new Queue<GameObject>(Shuffle(KeySpawnPositions));
        RemainingSpots = new Queue<GameObject>(KeySpawnPositions);
    }

    // Update is called once per frame
    void Update()
    {
        if(NextKey != null && NextKey.transform.position.y < -10.0f)
        {
            // Respawn the key
            GameObject.Destroy(NextKey);
            NextKey = GameObject.Instantiate(KeyPrefab, LastSpawnPoint);
        }
    }

    public bool AnyMoreKeys
    {
        get => RemainingSpots.Any();
    }

    public void SpawnNextKey()
    {
        LastSpawnPoint = RemainingSpots.Dequeue().transform;
        NextKey = GameObject.Instantiate(KeyPrefab, LastSpawnPoint);
    }

    private IEnumerable<GameObject> Shuffle(List<GameObject> objects)
    {
        while(objects.Any())
        {
            int indexToRemove = Random.Range(0, objects.Count);
            yield return objects[indexToRemove];
            objects.RemoveAt(indexToRemove);
        }
    }
}
