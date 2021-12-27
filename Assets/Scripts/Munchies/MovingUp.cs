using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUp : MonoBehaviour
{
    public float speed = 8f;

    void Update()
    {
        if (Time.timeScale > 0) MoveUp();
        else Time.timeScale = 1;
    }

    private void MoveUp()
    {
        transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
    }

}
