using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float health = 100f;
   
    public void Hit(float damage)
    {
        health -= damage;
    }
}
