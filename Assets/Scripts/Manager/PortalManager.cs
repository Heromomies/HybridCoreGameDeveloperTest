using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private bool addSpeed;
    [SerializeField] private bool addDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AllieManager allieManager))
        {
            if (addDamage)
            {
                allieManager.damage++;
            }
            else if (addSpeed)
            {
                allieManager.speed++;
            }
        }
    }
}
