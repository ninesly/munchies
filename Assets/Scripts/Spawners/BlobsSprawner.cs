using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobsSprawner : MonoBehaviour
{
    [Header("Blobs have to be in type order to not mess random colors!")]
    public GameObject[] blobPrefabs;
    public GameObject[] spawnPoints;
    public GameObject hintPrefab;

    LevelSettings levelSettings;
    
    GameObject newBlob;
    GameObject newHint;
    int spawnIndex = 0;
    int blobIndex = 0;
    bool spawnAnotherHint = true;
    bool spawnAnotherBlob = false;

    private void Start()
    {
        levelSettings = FindObjectOfType<LevelSettings>();
    }

    void Update()
    {
        if (!newHint && !newBlob && spawnAnotherHint) StartCoroutine(SpawnHint());
        if (!newHint && !newBlob && spawnAnotherBlob) StartCoroutine(SpawnBlob());
    }

    private IEnumerator SpawnHint()
    {
        spawnAnotherHint = false;
        spawnAnotherBlob = true;
        newHint = Instantiate(hintPrefab, spawnPoints[spawnIndex].transform.position, Quaternion.identity);
        Destroy(newHint, levelSettings.timeOfHint);
        yield return new WaitForSeconds (levelSettings.timeOfHint);    

    }


    private IEnumerator SpawnBlob()
    {
        spawnAnotherHint = true;
        spawnAnotherBlob = false;
        newBlob = Instantiate(blobPrefabs[blobIndex], spawnPoints[spawnIndex].transform.position, Quaternion.identity);
        RandomType();
        RandomPlace();
        Destroy(newBlob, levelSettings.timeOfRemainig);
        yield return new WaitForSeconds(levelSettings.timeOfRemainig);
    }

    private void RandomPlace()
    {
        if (!levelSettings.randomPlaceSpawn)
        {
            spawnIndex++;
            if (spawnIndex >= spawnPoints.Length)
            {
                spawnIndex = 0;
            }
        }
        else
        {
            spawnIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
        }
    }

    private void RandomType()
    {
        if (!levelSettings.randomBlobTypeSpawn)
        {
            blobIndex++;
            if (blobIndex >= blobPrefabs.Length)
            {
                blobIndex = 0;
            }
        }
        else
        {
            blobIndex = UnityEngine.Random.Range(0, blobPrefabs.Length);
        }
    }
}
