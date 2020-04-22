using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        enabled = false;
        Destroy(this);        
    }
}
