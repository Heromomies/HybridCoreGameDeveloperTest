using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllieManager : MonoBehaviour
{
    public float speed;
    public int damage;

    [HideInInspector] public Vector3 batToGoPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BatManager batManager))
        {
            batManager.numberOfAlliesInBat++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, batToGoPos, Time.deltaTime * speed);
    }
}
