using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasObjectsSwitch : MonoBehaviour
{
    public List<GameObject> canvaObjects;
    public bool SetCanvaObjectsAutomatically = false;
    [Header("Hand switch should equal canvaObjects. It switch in START only")]
    public bool turnOnHandSwitch = false;
    [Tooltip("True = object On, False = object Off")]
    public bool[] handSwitch;

    private void Awake()
    {
        if (SetCanvaObjectsAutomatically) SetList();
    }
    private void Start()
    {

        if (turnOnHandSwitch) HandSwitch();

    }

    private void SetList()
    {
        var children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            canvaObjects.Add(child.gameObject);
        }
    }

    private void HandSwitch()
    {
        if (canvaObjects.Count <= 0)
        {
            Debug.LogError("The is no CanvaObjects to set");
            return;
        }
        else if (handSwitch.Length <= 0)
        {
            Debug.LogError("The is no HandSwitch to know what to set");
            return;
        }

        for (int index = 0; index < canvaObjects.Count; index++)
        {

            canvaObjects[index].SetActive(handSwitch[index]);
        }
    }

    public void SetActiveCanvaObjectNr(int index, bool state)
    {
        if (canvaObjects.Count <= 0)
        {
            Debug.LogError("The is no CanvaObjects to set");
            return;
        }

        canvaObjects[index].SetActive(state);
    }

    public void SetActiveCanvaObjectNr(int index, bool state, int index_2, bool state_2)
    {
        if (canvaObjects.Count == 0)
        {
            Debug.LogError("The is no CanvaObjects to set");
            return;
        }
        canvaObjects[index].SetActive(state);
        canvaObjects[index_2].SetActive(state_2);
    }

    public void SetActiveCanvaObjectNr(string all, bool state)
    {
        if (all != "all")
        {
            Debug.LogError("I dont't understand this commend: " + all);
            return;
        }

        if (canvaObjects.Count == 0)
        {
            Debug.LogError("The is no CanvaObjects to set");
            return;
        }

        foreach (GameObject obj in canvaObjects)
        {
            obj.SetActive(state); 
        }

    }

}
