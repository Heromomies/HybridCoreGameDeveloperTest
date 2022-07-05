using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BatimentPrincipal : MonoBehaviour
{
    [SerializeField] private LayerMask layerBatPrincipal;

    private GameObject _goBatPrincipal;
    private bool _isBatPrincipalTouched;
    private Camera _cam;
    
    private void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit, Mathf.Infinity, layerBatPrincipal))
            {
                _goBatPrincipal = hit.collider.gameObject;
                _isBatPrincipalTouched = true;
            }
        }

        if (_isBatPrincipalTouched)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(_goBatPrincipal.transform.position, ray.direction, out var hit))
                {
                  Debug.Log(hit.collider.name);
                }
            }
        }
        
    }
    
}
