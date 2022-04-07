using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private bool dir;
    private float speed = 0.2f;
    private float initY;
    private float distance = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        dir = false;
        initY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dir) 
            if ((transform.position.y - initY) < distance)
                transform.position = new Vector3(transform.position.x,transform.position.y + (speed * Time.deltaTime), transform.position.z);
            else
                dir = !dir;
        else
        if ((transform.position.y - initY) > 0)
            transform.position = new Vector3(transform.position.x,transform.position.y - (speed * Time.deltaTime), transform.position.z);
        else
            dir = !dir;
    }
}