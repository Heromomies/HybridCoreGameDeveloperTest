using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllieManager : MonoBehaviour
{
    public float speed;
    public int damage;

    private float _startSpeed;
    private int _startDamage;
    
    [HideInInspector] public Vector3 batToGoPos;

    [HideInInspector] public GameObject goDontCollideWith;

    private void Start()
    {
        _startDamage = damage;
        _startSpeed = speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != goDontCollideWith)
        {
            if (other.TryGetComponent(out BatManager batManager))
            {
                for (int i = 0; i < damage; i++)
                {
                    batManager.IncreaseNumberAllies();
                }
                speed = _startSpeed;
                damage = _startDamage;
                gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, batToGoPos, Time.deltaTime * speed);
    }
}
