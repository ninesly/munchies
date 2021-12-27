using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MotionController : MonoBehaviour
{
    public Munchie munchie;
    public NavMeshAgent agent;
    Vector3 destination;

    float timeToIncrease;
    float amountToIncrease;
    float startTime;

    [Header("Debug only")]
    public bool speeding = false;

    StatisticGatherer statisticGatherer;

    private void Start()
    {
        statisticGatherer = FindObjectOfType<StatisticGatherer>();
    }
    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0)&& !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Sllower");
            Move();
        }

        if (speeding && Input.GetMouseButton(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("speeder");
            SpeedingUp();
            Move();
        }*/

        if (statisticGatherer.aliveClass_Whipy > 0) speeding = true;
        else speeding = false;

        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (speeding)
            {
                timeToIncrease = 0.5f;
                amountToIncrease = 1f;
                startTime = Time.time;
            }
            else Move(); 

        }
        if (Input.GetMouseButton(0) && speeding && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) // && Time.time - timer > windUpTime)
        {
                if (Time.time - startTime > timeToIncrease)
                {
                    if (agent.speed + amountToIncrease < munchie.defaultSpeed + statisticGatherer.aliveClass_Whipy) agent.speed += amountToIncrease;
                    timeToIncrease -= .2f; //Lowers amount of time till next speed boost
                    startTime = Time.time;
                }
                Move();
        }
        if (Input.GetMouseButtonUp(0) && speeding && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            agent.speed = munchie.defaultSpeed;
        }

    }


    private void Move()
    {      

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
            munchie.MoveToPoint(destination);
        }
    }


}
