using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Munchie : MonoBehaviour
{
    public GameObject deathVFX;
    public Color sickColor = new Color(154, 205, 50); // sickGreen
    public GameObject expEffectObj;
    public float exp_timeToDestroy = 2f;
    public GameObject levelUpEffectObj;
    public float lvl_timeToDestroy = 2f;

    [Header("Debug Only")]
    public int health;
    public int bonusHealth;
    public string resistance;
    public int experience;
    public int level;
    public int teachingLimit;
    public bool speeding;
    public float defaultSpeed = 8;
    public float speedCap = 8;
    public List<float> virusDefenseSystem = new List<float> ();
    public List<float> electroDefenseSystem = new List<float>();
    public float currentVirusNumber;
    public float currentElectroNumber;

    //Event System
    int dice;
    int eventProbability;
    int teachingProbability;
    int notMoreThan;
    bool teachExpEqualToLevel;
    int expToTeachIfFixed;

    NavMeshAgent agent;
    Rigidbody m_RigidBody;
    StatisticGatherer statisticGatherer;
    Renderer m_Renderer;
    Smunchie smunchie;
    LevelSettings levelSettings;
    GlobalSettings globalSettings;
    SoundController soundController;

    Color baseColor;
    bool falling = false;
    bool poisoned = false;
    bool slugged = false;
    bool electrocuted = false;
    GameObject trail;
    int poisonLeft;
    float electricTime;
    int electricDamage;
    //GameObject lightningBolt;
    float timeOfLightningEffect;
    int lightningDamage;
    bool damageOnNextFall;
    Vector3 target_1;
    Vector3 target_2;
    bool target_1_reached = false;


    Collider previousTouch;


    private void Awake()
    {
        statisticGatherer = FindObjectOfType<StatisticGatherer>();
        statisticGatherer.ChangeMunchiesNumberBy(1);
    }

    private void Start()
    {
        levelSettings = FindObjectOfType<LevelSettings>();
        globalSettings = FindObjectOfType<GlobalSettings>();
        smunchie = GetComponent<Smunchie>();
        agent = GetComponent<NavMeshAgent>();
        m_RigidBody = GetComponent<Rigidbody>();
        m_Renderer = GetComponent<Renderer>();
        soundController = FindObjectOfType<SoundController>();
        baseColor = m_Renderer.material.color;
        level = levelSettings.startingLevel;
        health = levelSettings.baseHealth;

        //event values
        dice = levelSettings.dice;
        eventProbability = levelSettings.eventProbability;
        teachingProbability = levelSettings.teachingProbability;
        notMoreThan = levelSettings.notMoreThan;
        teachExpEqualToLevel = globalSettings.teachExpEqualToLevel;
        expToTeachIfFixed = levelSettings.expToTeachIfFixed;
        teachingLimit = levelSettings.baseTeachingLimit;

        if (!smunchie) soundController.MunchieSpawnSound();
    }

    private void Update()
    {
        if (electrocuted)
        {
            ElectroVisual();
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shredder")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "Mesh" && falling)
        {
            TurnOnAgent();
            if (damageOnNextFall) AttackMunchie("falling damage", levelSettings.fallingDamage);
            falling = false;
            m_RigidBody.useGravity = false;
            m_RigidBody.velocity = new Vector3(0, 0, 0) ;
        }
        else if (other.tag == "Munchie")
        {
            if (poisoned) other.GetComponent<Munchie>().Poison(poisonLeft, currentVirusNumber);
            //if (electrocuted) ElectroVFX(other);
            if (electrocuted) other.GetComponent<Munchie>().Electrocute(electricTime, electricDamage, currentElectroNumber);

            if (!poisoned && !electrocuted) EventSystem(other);
        }


    }    

    private void OnDestroy()
    {
        statisticGatherer.ChangeMunchiesNumberBy(-1);
        statisticGatherer.ChangeDeathsBy(1);
    }

    // --------------------------------------------  MY METHODS

    // BASIC - moving, resistance, being attacked, health, death, effects

    public void MoveToPoint(Vector3 point)
    {
        if (!agent.isActiveAndEnabled) return;
        agent.SetDestination(point);
    }

    public void SetResistance(string type)
    {
        resistance = type;
        //Debug.Log("new resistance! " + resistance);
    }

    public void AttackMunchie(string byWhat, int damage)
    {
        if (byWhat == "Thunder" && resistance == "Thunder")
        {
            Debug.Log("Thunder RESISTED");
            return;
        }

        if(statisticGatherer.aliveClass_Sacri > 0 && (!smunchie | smunchie.smunchieClass != Smunchie.SmunchieClass.Sacri))
        {
            Debug.Log("I was saved!");
            GameObject.FindGameObjectWithTag("Sacri").GetComponent<Smunchie>().SacriAbility();
            return;
        }
        ChangeHealthBy(-damage);        
    }

    public void ChangeBonusHealthBy(int amount)
    {
        bonusHealth += amount;
    }
    public void ChangeHealthBy(int amount)
    {
        health += amount;
        if (health+ bonusHealth <= 0) Death();
    }

    public void ChangeDefaultSpeed(float amount)
    {
        defaultSpeed += amount;
    }

    private void Death()
    {
        Destroy(gameObject);
        DeathFX();
    }

    private void DeathFX()
    {
        soundController.DeathSound();

        if (!deathVFX) { return; }
        // var spawnVFXpostion = new Vector2(transform.position.x + vfxOffsset.x, transform.position.y + vfxOffsset.y);
        var particleEffect = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(particleEffect, 1.5f);
    }

    // experience, leveling, advancing

    public void AddExpToMunchie(int expAmount, string source)
    {
        if (!levelSettings) return;
        if (levelSettings.turnOffExperience || expAmount <= 0) return;
        if (health < 0) return;
        Debug.Log("Exp up! from: " + source);
        experience += expAmount;        
        if (experience > 0 && Mathf.FloorToInt(experience / levelSettings.levelThreshold) > level -1) // bo na 1 zaczynaja
        {
            LevelUp();
        }
        else
        {
            txtEffect(expEffectObj);
        }

    }

    private void txtEffect(GameObject effect)
    {
        var newExpEffect = Instantiate(effect, transform.position, Quaternion.identity);
        newExpEffect.transform.parent = transform;
        Destroy(newExpEffect, exp_timeToDestroy);
    }

    private void LevelUp()
    {
        var lvlToUp = Mathf.FloorToInt(experience / levelSettings.levelThreshold) - level - 1;

        //Debug.Log("Level up!");
        level += lvlToUp;
        txtEffect(levelUpEffectObj);
        ChangeHealthBy(1);
        //ChangeTeachingProbabilityBy(1);
        if (level > 1 && Mathf.FloorToInt(level / levelSettings.advancingThreshold) > 0)
        {
            Advance();
        }
    }

    private void Advance()
    {
        //Debug.Log("Advance!");
        if (smunchie.isActiveAndEnabled)
        {
           // Debug.Log("Second Advance!");
            smunchie.Advance();
        }
        else
        {
            CreateNewSmunchie(smunchie);
        }
    }

    private void CreateNewSmunchie(Smunchie who)
    {
        who.enabled = true;
        who.SetRandomClass();        
    }

    private void CreateNewSmunchie(Smunchie who, Smunchie.SmunchieClass chosenClass)
    {
        who.enabled = true;
        who.SetSmunchieClass(chosenClass);
    }

    // event system

    private void EventSystem(Collider otherMunchie)
    {
        if (previousTouch == otherMunchie)
        {
            //Debug.Log("Hey we touched last time!! No more touchy touchy");
            return;
        }
        //Debug.Log("IT'S A TOUCH!");
        var result = RollADice(dice);

        if (result <= eventProbability)
        {
            if (levelSettings.turnOffRandomEvents) return;
            Event(otherMunchie);
        }
        else if (result <= teachingProbability)
        {
            if (levelSettings.turnOffTeaching) return;
            Teaching(otherMunchie);
        }

        previousTouch = otherMunchie;
    }

    private void Teaching(Collider otherMunchie)
    {
        var otherMunchieSmunchie = otherMunchie.GetComponent<Smunchie>(); // hahahahahah
     
        if (!levelSettings.turnOffPassingSmunchieClass 
            && smunchie.isActiveAndEnabled 
            && !otherMunchieSmunchie.isActiveAndEnabled 
            && teachingLimit > 0)
        { 
            Debug.Log("Munchie make another Munchie a true Smunchie!");
            var myClass = smunchie.GetSmunchieClass();
            CreateNewSmunchie(otherMunchieSmunchie, myClass);
            teachingLimit--;
        }
        else 
        {
            var otherMainComponent = otherMunchie.GetComponent<Munchie>();
            if (otherMainComponent.experience < experience)
            {
                //Debug.Log("Munchie is teaching a Munchie: " + level);
                if (teachExpEqualToLevel) otherMainComponent.AddExpToMunchie(level, "teachingLevel");
                else otherMainComponent.AddExpToMunchie(expToTeachIfFixed, "teachingFixed");
            }
        }
    }

    private void Event(Collider otherMunchie)
    {
        Debug.Log("EVENT!");
    }

    private int RollADice(int dice)
    {
        return UnityEngine.Random.Range(1, dice+1);
    }

    private void ChangeTeachingProbabilityBy(int amount)
    {
        if (teachingProbability + amount <= notMoreThan)
        {
            teachingProbability += amount;
        }
    }

    // falling, spawning from heaven

    public void Falling(float timeAfterFallingStarts, bool damage)
    {
        if (damage) smunchie.Uncount(); // when damage, so NOT spawn, ten uncount Smunchie for purpose of skills etc.
        damageOnNextFall = damage;
        StartCoroutine(Wait(timeAfterFallingStarts));
    }

    private IEnumerator Wait(float timeAfterFallingStarts)
    {
        yield return new WaitForSeconds(timeAfterFallingStarts);
        m_RigidBody.useGravity = true;
        falling = true;
    }

    private void TurnOnAgent()
    {
        agent.enabled = true;
    }

    // slug blob

    public void Slug(float slowingValue, float slowingTime)
    {
        Debug.Log("Munchie Sluggged :(");
        if (resistance == "Slug")
        { 
            Debug.Log("Slug RESISTED!"); 
            return; 
        }
        if (slugged) { return; }
        slugged = true;
        StartCoroutine(SlowingDown(slowingValue, slowingTime));
     
    }

    public void SlugTrail(GameObject slugTrail, float offset)
    {
        if (!slugged) { return; }
        if (trail) { return; }
        trail = Instantiate(slugTrail, transform.position - new Vector3(0, offset, 0), Quaternion.identity);
        trail.transform.parent = transform;
    }

    private IEnumerator SlowingDown(float slowingValue, float slowingTime)
    {
        if (slugged == true)
        {           
            agent.speed -= slowingValue; 
            yield return new WaitForSecondsRealtime(slowingTime);
            agent.speed += slowingValue;
            slugged = false;
            Destroy(trail);
        }

    }

    // electric blob

    public void Electrocute(float time, int damage, float electroNumber) //GameObject effect
    {        
        if (resistance == "Electricity") { Debug.Log("Electricity RESISTED!"); return; }
        if (electroDefenseSystem.Contains(electroNumber)) return;
        virusDefenseSystem.Add(electroNumber);
        if (electrocuted) { return; }
        
        electrocuted = true;
        currentElectroNumber = electroNumber;
        electricTime = time;
        electricDamage = damage;
        //lightningBolt = effect;
        target_1 = transform.position + new Vector3(-0.5f, 0, 0);
        target_2 = transform.position + new Vector3(0.5f, 0, 0);
        target_1_reached = false;
 
        StartCoroutine(BeingElectrocuted(time, damage));       
    }

    private IEnumerator BeingElectrocuted(float time, int damage)
    {
        timeOfLightningEffect = time;
        lightningDamage = damage;

        agent.enabled = false;
        AttackMunchie("Electro", lightningDamage);
        yield return new WaitForSeconds(timeOfLightningEffect);
        electrocuted = false;
        agent.enabled = true;

    }

    private void ElectroVisual()
    {
        var speed = 20 * Time.deltaTime;



        Vector3 target = new Vector3(0, 0);        
         
         if (!target_1_reached)
         {
            target = target_1;
         } else
        {
            target = target_2;
        }
    

        transform.position = Vector3.MoveTowards(transform.position, target, speed);

        if (transform.position == target_1)
        {
            target_1_reached = true;
        }

        if (transform.position == target_2)
        {
            target_1_reached = false;
        }

    }

    /*
    public void ElectroVFX(Collider otherMunchie)
    {
        if (otherMunchie)
        {
            var newlightning = Instantiate(lightningBolt, transform.position, Quaternion.identity);
            newlightning.transform.parent = transform;
            newlightning.GetComponent<LightningBoltScript>().StartObject = gameObject;
            otherMunchie.GetComponent<Munchie>().ElectroVFX(newlightning);
            otherMunchie.GetComponent<Munchie>().Electrocute(newlightning, timeOfLightningEffect, lightningDamage);
            Destroy(newlightning, timeOfLightningEffect);
        }
    }
    public void ElectroVFX(GameObject newlightning)
    {
        newlightning.GetComponent<LightningBoltScript>().EndObject = gameObject;        
    }*/


    // poison blob

    public void Poison(int poisonStartingAmount, float virusNumber)
    {
        if (resistance == "Poison") { Debug.Log("Poison RESISTED!"); return; }
        if (virusDefenseSystem.Contains(virusNumber)) return;
        virusDefenseSystem.Add(virusNumber);
        if (poisoned) { return; }
        
        poisoned = true;
        currentVirusNumber = virusNumber;
        poisonLeft = poisonStartingAmount;
        m_Renderer.material.SetColor("_Color", sickColor);
        StartCoroutine(BeingSick());

    }

    private IEnumerator BeingSick()
    {
        var time = levelSettings.timeBetweenPoisonDamage;
        do
        {
            //Debug.Log("Health: " + health + "poison: " + poisonLeft);
            AttackMunchie("poison", poisonLeft);
            if (poisonLeft >= 1) poisonLeft--;
            yield return new WaitForSeconds(time);
            if (poisonLeft <= 0)
            {
                poisoned = false;
                m_Renderer.material.SetColor("_Color", baseColor);
            }
        } while (poisoned);

    }
   
    public void SetSpeeding(bool state)
    {
        speeding = state;
    }

  
}
