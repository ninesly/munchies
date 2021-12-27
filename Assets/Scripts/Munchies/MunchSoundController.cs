using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MunchSoundController : MonoBehaviour
{
    public float volume = 1f;
    public AudioClip[] munchieSounds;

    [Header("Crowd Control - all values included. OrderSpawn:Leader,Quiet,Medium,Laud")]
    public int choirLeader = 1;
    public Vector2 clipsFromTo_Leader = new Vector2(1 , 3);
    public int laudMunchies = 1;
    public Vector2 clipsFromTo_laud = new Vector2(2 , 5);
    public int mediumMunchies = 3;
    public Vector2 clipsFromTo_medium = new Vector2(5 , 8);
    public int quietMunchies = 5;
    public Vector2 clipsFromTo_quiet = new Vector2(6 , 9);
    public Vector2 silentForTheRest = new Vector2(9, 9);

    StatisticGatherer statistic;



  
    [Header("Debug Only")]
    public string whoAmI;

    string leader = "choir leader";
    string laud = "laud Munchie";
    string medium = "medium Munchie";
    string quiet = "quiet Munchie";

    AudioSource audioSource;    
    Vector2 playlist;


    private void Start()
    {
        statistic = FindObjectOfType<StatisticGatherer>();


       audioSource = GetComponent<AudioSource>();
        playlist = CrowdControl();
    }
    void Update()
    {
        if (!audioSource.isPlaying) PlayFromArray(munchieSounds, playlist);

        /*
        if (statistic.aliveMunchies == 0) return;
        else
        {
            if (statistic.aliveMunchies <= choirLeader) StartCoroutine(PlayFromArray(munchieSounds, clipsFromTo_Leader));
            if (statistic.aliveMunchies <= quietMunchies) StartCoroutine(PlayFromArray(munchieSounds, clipsFromTo_quiet));
            if (statistic.aliveMunchies <= mediumMunchies) StartCoroutine(PlayFromArray(munchieSounds, clipsFromTo_medium));
            if (statistic.aliveMunchies > mediumMunchies) StartCoroutine(PlayFromArray(munchieSounds, clipsFromTo_laud));
        }*/
    }

     private Vector2 CrowdControl()
   {

       // crowd control when every Munchie is an audiosource

       if (statistic.choirLeader < choirLeader)
       {
           statistic.ChangeChoirLeader(1);
           whoAmI = leader;
           return clipsFromTo_Leader;
       }
       else if (statistic.quietMunchies < quietMunchies)
       {
           statistic.ChangeQuietMunchies(1);
           whoAmI = quiet;
           return clipsFromTo_quiet;
       }
       else if (statistic.mediumMunchies < mediumMunchies)
       {
           statistic.ChangeMediumMunchies(1);
           whoAmI = medium;
           return clipsFromTo_medium;
       }
       else if (statistic.laudMunchies < laudMunchies)
       {
           statistic.ChangeLaudMunchies(1);
           whoAmI = laud;
           return clipsFromTo_laud;
       }   
       else
       {
           whoAmI = "i'm silent :(";
           return silentForTheRest;
       } 

}

    public void PlayFromArray(AudioClip[] soundArray, Vector2 fromTo)
    {
        if (fromTo == silentForTheRest) return;
        if (fromTo == null)
        {
            whoAmI = "I'm broken :(";
            return;
        }
        int from = Mathf.FloorToInt(fromTo.x);
        int to = Mathf.FloorToInt(fromTo.y + 1);
        var random = UnityEngine.Random.Range(from,to);

        /* delete if going back to Munchies as a source
        yield return new WaitForSeconds(1.5f);
        AudioSource.PlayClipAtPoint(soundArray[random], Camera.main.transform.position, volume);*/
       

        if (random >= soundArray.Length)
        {
            whoAmI = "I'm broken :((";
            Debug.LogWarning("array length: " + soundArray.Length + "random: " + random);
            return;
        }
        
        audioSource.clip = soundArray[random];
        audioSource.Play();


    }

    
    private void OnDestroy()
    {
        if (whoAmI == leader) { statistic.ChangeChoirLeader(-1); }
        else if (whoAmI == laud) { statistic.ChangeLaudMunchies(-1); }
        else if (whoAmI == medium) { statistic.ChangeMediumMunchies(-1); }
        else if (whoAmI == quiet) { statistic.ChangeQuietMunchies(-1); }
    }

}
