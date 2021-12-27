using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MunchieSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject munchiePrefab;
    public int diceToRollForRandomTreat = 12;
    public Color[] colorsOfVariations;
    [Tooltip("The same as colors! Thunder / Poison / Slug / Electricity")]
    public string[] resistances;
    public float sizeVariationMin = 0.5f;
    public float sizeVariationMax = 1.5f;

    LevelSettings levelSettings;
    private void Start()
    {
        levelSettings = FindObjectOfType<LevelSettings>();
    }
    public void SpawnNewMunchie(Vector3 postion)
    {
       // var player = FindObjectOfType<Munchie>();
        var newMunchie = Instantiate(munchiePrefab, postion, Quaternion.identity) as GameObject;
        newMunchie.transform.parent = transform;

        if (levelSettings.colorVariation && RollADice(diceToRollForRandomTreat) == 1)
        {
            var random = Random.Range(0, colorsOfVariations.Length);
            newMunchie.GetComponent<Renderer>().material.color = colorsOfVariations[random];
            newMunchie.GetComponent<Munchie>().SetResistance(resistances[random]); // has to be tied to colors
        } // color (resistance)
        if (levelSettings.sizeVariation && RollADice(diceToRollForRandomTreat) == 1)
        {
            var random = Random.Range(sizeVariationMin, sizeVariationMax);
            if (random != 1)
            {
                if (random > 1) // big 
                {
                    newMunchie.GetComponent<Munchie>().ChangeDefaultSpeed(-random * 3); // slow 
                    newMunchie.GetComponent<Munchie>().ChangeBonusHealthBy(Mathf.FloorToInt(random * 6)); // healthy                   
                }
                else //small
                {
                    newMunchie.GetComponent<Munchie>().ChangeDefaultSpeed(random * 3); // fast 
                    newMunchie.GetComponent<Munchie>().ChangeBonusHealthBy(Mathf.FloorToInt(-random * 6)); // less healthy                    
                }
                newMunchie.GetComponent<Transform>().localScale = new Vector3(random, random, random);
            }

        } // size (speed&health)
        if (levelSettings.uniqueVariation && RollADice(diceToRollForRandomTreat*2) == 2)
        {
            newMunchie.GetComponent<Renderer>().material.SetFloat("_Glossiness", 1f);
        } // shiny

    }

    private int RollADice(int randomExcl)
    {
        return Random.Range(1, randomExcl+1);
    }
}
