using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.isTrigger!= true)
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponent<Player>().damage(1);
            }
          //  Destroy(GameObject);
        }
    }
}
