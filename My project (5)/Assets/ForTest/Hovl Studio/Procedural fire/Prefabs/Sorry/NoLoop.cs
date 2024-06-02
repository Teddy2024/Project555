using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoLoop : MonoBehaviour
{
    [SerializeField] private float stayTime = 0.5f;
    // Start is called before the first frame update
    private void Awake() 
    {
        Destroy(gameObject , stayTime);
    }
}
