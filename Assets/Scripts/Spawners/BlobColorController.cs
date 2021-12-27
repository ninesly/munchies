using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobColorController : MonoBehaviour
{
    public Material bombMaterial;
    public Material electricMaterial;
    public Material killerMaterial;
    public Material poisonMaterial;
    public Material slugMaterial;
    public Material spawnerMaterial;
    public Material thunderMaterial;

    public GameObject[] blobPrefabs;

    [Header("Debug Only")]
    public List<Material> defaultBlobMaterials; 
    public List<Material> randomBlobMaterials;
    public int fuse;

    void Start()
    {
        defaultBlobMaterials = new List<Material> { bombMaterial, electricMaterial, killerMaterial, poisonMaterial, slugMaterial, spawnerMaterial, thunderMaterial };
        if (FindObjectOfType<LevelSettings>().randomColorOfBlob)
        {
            ShuffleBlobMaterials();
            ColloringBlobs(true);
        }
        else ColloringBlobs(false);
    }

    public void ShuffleBlobMaterials()
    {
        do
        {
            var random = Random.Range(0, defaultBlobMaterials.Count);
            randomBlobMaterials.Add(defaultBlobMaterials[random]);
            defaultBlobMaterials.RemoveAt(random);
            fuse++;
        } while (defaultBlobMaterials.Count > 0 && fuse <= 15);
    }

    public void ColloringBlobs(bool random)
    {
        if (random)
        {
            for (int current = 0; current < blobPrefabs.Length; current++)
            {              
                blobPrefabs[current].GetComponentInChildren<Renderer>().material = randomBlobMaterials[current];
            }
        }
        else
        {
            for (int current = 0; current < blobPrefabs.Length; current++)
            {
                blobPrefabs[current].GetComponentInChildren<Renderer>().material = defaultBlobMaterials[current];
            }
        }
    }

}


/*
 *   if (current > 0)
                {
                    if (blobPrefabs[current] != blobPrefabs[current - 1])
                    {
                        var previous = current - 1;
                        Debug.Log("Blob #" + current + " is different than Blob#" + previous);
                        colorIndex++;
                    }
                    else { var previous = current - 1; Debug.Log("Blob #" + current + " is the same as Blob#" + previous); }
                }
*/