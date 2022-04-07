using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDInfo : MonoBehaviour
{
    private GameObject player;
    private GameObject txtPoints;
    private GameObject lifes;

    public Sprite lifes3;
    public Sprite lifes2;
    public Sprite lifes1;
    public Sprite lifes0;
    void Start()
    {
        player = GameObject.Find("Player");
        txtPoints = GameObject.Find("txtPoints");
        lifes = GameObject.Find("lifes");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        switch (player.GetComponent<PlayerManager>().life)
        {
            case 3:
                lifes.GetComponent<Image>().sprite = lifes3;
                break;
            case 2:
                lifes.GetComponent<Image>().sprite = lifes2;
                break;
            case 1:
                lifes.GetComponent<Image>().sprite = lifes1;
                break;
            case 0:
                lifes.GetComponent<Image>().sprite = lifes0;
                break;
        }

        txtPoints.GetComponent<Text>().text = "Points: " + player.GetComponent<PlayerManager>().points;
    }
}
