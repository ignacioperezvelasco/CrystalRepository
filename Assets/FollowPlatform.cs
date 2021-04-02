using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlatform : MonoBehaviour
{


    [SerializeField] Transform platform;

    
    void Update()
    {
        this.transform.position = platform.position;
    }
}
