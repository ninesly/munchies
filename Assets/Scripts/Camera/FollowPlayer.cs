using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float clampMin_x = -5f;
    public float clampMax_x = 5f;
    public float clampMin_y = 5f;
    public float clampMax_y = 20f;
    public float clampMin_z = -15f;
    public float clampMax_z = 0f;
    public Vector2 resolution = new Vector2(1920, 1080);
    public bool diasbleMouseCamera = false;
    public float mouseMargin = 10f;
    public float mouseSensitivity = 0.1f;

    public bool disableAutoMove = true;
    Transform player;
    Vector3 offset;

    public static FollowPlayer instance;


    private void Awake()
    {
        Singleton();
    }

    private void Singleton()
    {
        if (instance != null) // if there are two of us
        {
            Destroy(gameObject); // destroy me
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Munchie").transform;
        offset = transform.position - player.transform.position;
    }

    void Update()
    {
        if(!disableAutoMove) transform.position = player.transform.position + offset;
        if (Time.timeScale > 0)
        {
            if (Input.mouseScrollDelta.y != 0) Scrolling();
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) MovingCameraByKeyboard();
            if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) && !diasbleMouseCamera) MovingCameraByMouse();
        }

        //Debug.Log(Input.mousePosition + " Xaxis: " + Input.GetAxis("Mouse X"));
    }

    private void MovingCameraByMouse()
    {
        float mouseChangeX = 0f;
        float mouseChangeY = 0f;

        

        if (Input.mousePosition.x > Screen.width - mouseMargin)
        {
            mouseChangeX = mouseSensitivity;
        } else if (Input.mousePosition.x < 0 + mouseMargin)
        {
            mouseChangeX = -mouseSensitivity;
        }

        if (Input.mousePosition.y > Screen.height - mouseMargin)
        {
            mouseChangeY = mouseSensitivity;
        }
        else if (Input.mousePosition.y < 0 + mouseMargin)
        {
            mouseChangeY = -mouseSensitivity;
        }
  


        transform.position += new Vector3(mouseChangeX, 0, mouseChangeY);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, clampMin_x, clampMax_x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, clampMin_z, clampMax_z));
    }

    private void MovingCameraByKeyboard()
    {
        var changeX = Input.GetAxis("Horizontal");
        var changeZ = Input.GetAxis("Vertical");   

        transform.position += new Vector3(changeX, 0, changeZ);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, clampMin_x, clampMax_x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, clampMin_z, clampMax_z));
    }

    private void Scrolling()
    {
        var changeY = Input.mouseScrollDelta.y;
        transform.position += new Vector3(0, -changeY, changeY);
        transform.position = new Vector3(
           transform.position.x,
           Mathf.Clamp(transform.position.y, clampMin_y, clampMax_y),
           //Mathf.Clamp(transform.position.z, -clampMin_z, -clampMax_z));
           transform.position.z);
    }
}
