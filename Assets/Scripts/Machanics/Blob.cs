using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    public enum BlobTypes { Spawner, Killer, Thunder, FishBomb, Poison, Slug, Electricity };
    public BlobTypes blobType;    
    public bool negative = true;
    [Tooltip("Only negative effects. Points for Munchie that touched Blob. If it's still alive...")]
    public int expPointsForBrave = 5;
    [Tooltip("Only negative effects. Points for Everyone")]
    public int expPointsForEveryone = 1;
    public int incubPointsFromNegative = 1;
    public int incubPointsFromPositive = 2;

    [Header("Settings")]
    public int amountToSpawn = 1;
    public int damageOfKiller = 100;
    public int damageOfThunder = 100;
    public GameObject fishBomb;
    public int poisonAmount = 5;
    public GameObject slugTrail;
    public float slugTrailOffset = 0.9f;
    public float slugRadius = 3f;
    public float slugValue = 4f;
    public float slugTime = 3f;
    public GameObject lightningBolt;
    public float electricTime = 4f;
    public int electricDMG = 4;


    [Header("Audio Effect")]
    public AudioClip effectClip;
    [SerializeField] [Range(0, 1)] float effectClipVolume = 0.2f;

 
    private void OnTriggerEnter(Collider other)
    {
        var munchie = other.GetComponent<Munchie>();
        if (!munchie) { return; }        

        if (blobType == BlobTypes.Spawner)
        {
            for (int toSpawn = amountToSpawn; toSpawn > 0; toSpawn--)
            {
                FindObjectOfType<MunchieSpawner>().SpawnNewMunchie(transform.position);
            }
        }
        else if (blobType == BlobTypes.Killer)
        {
            if (munchie) munchie.AttackMunchie(blobType.ToString(), damageOfKiller);
        }
        else if (blobType == BlobTypes.Thunder)
        {
            FindObjectOfType<Munchie>().AttackMunchie(blobType.ToString(), damageOfThunder);

        }
        else if (blobType == BlobTypes.FishBomb)
        {
            var newFishBomb = Instantiate(fishBomb, transform.position, Quaternion.identity);
            var time = newFishBomb.GetComponent<FishBomb>().timeToDisapear;
            Destroy(newFishBomb, time);
        }
        else if (blobType == BlobTypes.Poison)
        {
            var virusNumber = Time.time;
            if (munchie) munchie.Poison(poisonAmount, virusNumber);
        }
        else if (blobType == BlobTypes.Slug)
        {
            Debug.Log("SLUG TIME");
            Collider[] colliders = Physics.OverlapSphere(transform.position, slugRadius);
            foreach (Collider hit in colliders)
            {
                if (munchie != null)
                {
                    munchie.Slug(slugValue, slugTime);
                    munchie.SlugTrail(slugTrail, slugTrailOffset);
                }
            }
        }
        else if (blobType == BlobTypes.Electricity)
        {
            var electricNumber = Time.time;
            if (munchie) munchie.Electrocute(electricTime, electricDMG, electricNumber);
        }

        StatisticsAndGains(munchie);
        PlaySFX();
        Destroy(gameObject);
    }

    private void StatisticsAndGains(Munchie munchie)
    {
        if (!munchie) return;
        var statistic = FindObjectOfType<StatisticGatherer>();
        if (negative)
        {
            
            statistic.ChangeExpBy(expPointsForBrave + DictoBonus(statistic, true));
            munchie.AddExpToMunchie(expPointsForBrave + DictoBonus(statistic, true), "exp for brave + dicto bonus: " + DictoBonus(statistic, true).ToString());
            ExperienveForEveryone(statistic);
            statistic.ChangeIncubPointsBy(incubPointsFromNegative);
        }
        else
        {
            statistic.ChangeIncubPointsBy(incubPointsFromPositive + statistic.incubBonus);
        }
    }

    private int DictoBonus(StatisticGatherer statistic, bool isForBrave)
    {
        var dictos = statistic.aliveClass_Dicto;
        if (isForBrave)
        {
            //if (dictos>0)Debug.Log("brave exp bonus: " + dictos);
            return dictos;
        }
        else
        {
            var everyoneBonus = Mathf.FloorToInt(dictos / 2);
            //if (everyoneBonus > 0)Debug.Log("every exp bonus: " + everyoneBonus);
            return everyoneBonus;
        }
    }

    private void ExperienveForEveryone(StatisticGatherer statistic)
    {
        Munchie[] allMunchies = FindObjectsOfType<Munchie>();

        foreach(Munchie munchie in allMunchies)
        {
            if (!munchie) return;
            munchie.AddExpToMunchie(expPointsForEveryone + DictoBonus(statistic, false), "exp for everyone + dicto bonus: " + DictoBonus(statistic, false).ToString());
            statistic.ChangeExpBy(expPointsForEveryone + DictoBonus(statistic, false));
        }
    }

    private void PlaySFX()
    {
        if (effectClip) AudioSource.PlayClipAtPoint(effectClip, Camera.main.transform.position, effectClipVolume);  
    }

}
