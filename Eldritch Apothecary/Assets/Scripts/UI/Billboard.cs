using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class Billboard : MonoBehaviour
{
    [Tooltip("Wether to fully follow camera or just in Y axis")]
    [SerializeField] bool freezeXZAxis = false;

    void Update()
    {
        if (freezeXZAxis)
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        else
            transform.rotation = Camera.main.transform.rotation;
    }
}
