using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Animator an;
    private NavMeshAgent nav;
    public float damage = 20f;
    
    private void Awake() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        nav.destination = player.transform.position;
        if(nav.velocity.magnitude > 1)
        {
            an.SetBool("isRunning" , true);
        }
        else
        {
            an.SetBool("isRunning" , false);
        }
    }

    //ZombieAttack
    private void OnTriggerEnter(Collider collider) 
    {
        if(collider.gameObject == player)
        {
            an.SetBool("isAttacking" , true);
        }
    }
    private void OnTriggerExit(Collider collider) 
    {
        if(collider.gameObject == player)
        {
            an.SetBool("isAttacking" , false);
        }
    }
}
