using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Smunchie : MonoBehaviour
{
    public enum SmunchieClass {Random, Moather, Dicto, Sacri, Seeer, Whipy } // adding new class 1/3
    public SmunchieClass smunchieClass;

    //  ------------------------------------------------------------ adding new class 2/3

    [Header("Class Attributes")]
    public GameObject moatherAttribute;
    public GameObject dictoAttribute;
    public GameObject sacriAttribute;
    public GameObject seeerAttribute;
    public GameObject whipyAttribute;
    public int moaAb = 1;
    public float seerAb = 0.1f;

    [Header("Debug Only")]
    public int advanceLevel = 0;
    public List<SmunchieClass> listOfAvaibleClasses;
 
    LevelSettings levelSettings;
    StatisticGatherer statisticGatherer;
    NavMeshAgent agent;
    bool imSmunchie = false;
    bool hasAttribute = false;

    bool counted = false;
    bool falling = true;

    private void Awake()
    {
        statisticGatherer = FindObjectOfType<StatisticGatherer>();
        levelSettings = FindObjectOfType<LevelSettings>();
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //ebug.Log("SSSSSSMUUUUUNCHIEE!");
        var smunchieContainer = GameObject.FindGameObjectWithTag("SmunchieContainer");
        transform.parent = smunchieContainer.transform;
        statisticGatherer.ChangeSmunchiesNumberBy(1);
        imSmunchie = true;
        SpawnClassAttribute();
        IncubationCostCalculation();

        FindObjectOfType<SoundController>().SmunchieSpawnSound(smunchieClass);
        // if (chosenClass = SmunchieClass.Random)
    }

    private static void IncubationCostCalculation()
    {
        IncubButtonController[] ibcArray = FindObjectsOfType<IncubButtonController>();
        foreach (IncubButtonController ibc in ibcArray)
        {
            ibc.CostCalculation();
        }
    }

    private void Update()
    {
        if (!counted && agent.isActiveAndEnabled) // when touch a ground
        {
           // Debug.Log("I'm counting!");
            counted = true;
            falling = false;            
            statisticGatherer.ChangeNumberOfSmunchieClass(smunchieClass, 1);
            if (smunchieClass == SmunchieClass.Sacri) gameObject.tag = "Sacri";
        }
    }

    public void Uncount()
    {
        counted = false;
        falling = true;
        statisticGatherer.ChangeNumberOfSmunchieClass(smunchieClass, -1);
        if (smunchieClass == SmunchieClass.Sacri) gameObject.tag = "Untagged";
    }



    private void OnDestroy()
    {
        if (imSmunchie)
        {
            statisticGatherer.ChangeSmunchiesNumberBy(-1);
            statisticGatherer.ChangeDeathsBy(1);
            if (!falling) statisticGatherer.ChangeNumberOfSmunchieClass(smunchieClass, -1);

            if (smunchieClass == SmunchieClass.Seeer) SeeerAbility(-seerAb);
        }
    }


    public void Advance()
    {
        advanceLevel++;
    }

    public SmunchieClass GetSmunchieClass()
    {
        return smunchieClass;
    }

    private void CountAvaibleClasses() //  ---------------------------------------------- adding new class 3/3 and then go to the settings
    {
        listOfAvaibleClasses = new List<SmunchieClass>();

        if (levelSettings.moatherClassEnabled)
        {
            listOfAvaibleClasses.Add(SmunchieClass.Moather);
        }
        if (levelSettings.dictoClassEnabled)
        {
            listOfAvaibleClasses.Add(SmunchieClass.Dicto);
        }
        if (levelSettings.sacriClassEnabled)
        {
            listOfAvaibleClasses.Add(SmunchieClass.Sacri);
        }
        if (levelSettings.seeerClassEnabled)
        {
            listOfAvaibleClasses.Add(SmunchieClass.Seeer);
        }
        if (levelSettings.whipyClassEnabled)
        {
            listOfAvaibleClasses.Add(SmunchieClass.Whipy);
        }
    }

    public void SetSmunchieClass(SmunchieClass s_class)
    {
        smunchieClass = s_class;
        SpawnClassAttribute();
    }

    public void SetRandomClass() 
    {
        CountAvaibleClasses();
        Debug.Log("Random class");
        if (listOfAvaibleClasses.Count <= 0) return;
        var result = Random.Range(0, listOfAvaibleClasses.Count);
        //Debug.Log("setting a class");
        SetSmunchieClass(listOfAvaibleClasses[result]);
        smunchieClass = listOfAvaibleClasses[result];
        SpawnClassAttribute();
    }

    private void SpawnClassAttribute()
    {
        if (hasAttribute) return;
        else hasAttribute = true;

        if (smunchieClass == SmunchieClass.Moather)
        {
            var newAttribute = Instantiate(moatherAttribute, transform.position, transform.rotation);
            newAttribute.transform.parent = transform;

            MoatherAbility(moaAb);
        }
        else if (smunchieClass == SmunchieClass.Dicto)
        {

            var newAttribute = Instantiate(dictoAttribute, transform.position, transform.rotation);
            newAttribute.transform.parent = transform;
        }
        else if (smunchieClass == SmunchieClass.Sacri)
        {

            var newAttribute = Instantiate(sacriAttribute, transform.position, transform.rotation);
            newAttribute.transform.parent = transform;            
        }
        else if (smunchieClass == SmunchieClass.Seeer)
        {
            var newAttribute = Instantiate(seeerAttribute, transform.position, transform.rotation);
            newAttribute.transform.parent = transform;

            SeeerAbility(seerAb);
        }
        else if (smunchieClass == SmunchieClass.Whipy)
        {

            var newAttribute = Instantiate(whipyAttribute, transform.position, transform.rotation);
            newAttribute.transform.parent = transform;
        }

        else
        {
            Debug.Log("else");
            hasAttribute = false;
        }        
    }

    // SMUNCHIES ABILITIES

    private void MoatherAbility(int amount) // one time
    {
        statisticGatherer.ChangeNumberOfIncubBonus(amount);    
    }

    public void SacriAbility() // triggered by dying Munchie
    {
        //Debug.Log("SACRIFICATORIO");
        var munchieSpawner = FindObjectOfType<MunchieSpawner>();
        munchieSpawner.SpawnNewMunchie(transform.position);
        munchieSpawner.SpawnNewMunchie(transform.position);
        munchieSpawner.SpawnNewMunchie(transform.position);
        Destroy(gameObject);
    }

    public void SeeerAbility(float amount) // one time
    {
        levelSettings.ChangeTimeOfHint(amount);
    }
    

}
