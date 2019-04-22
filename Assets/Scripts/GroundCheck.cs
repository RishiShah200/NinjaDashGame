﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private Player player;
    private void Start()
    {
        player = gameObject.GetComponentInParent<Player>();        
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        player.grounded = true;   
    }
    void OnTriggerStay2D(Collider2D col)
    {
        player.grounded = true;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        player.grounded = false;
    }
}
