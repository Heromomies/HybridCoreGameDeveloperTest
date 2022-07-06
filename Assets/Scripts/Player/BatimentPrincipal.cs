using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BatimentPrincipal : MonoBehaviour
{
    [SerializeField] private LayerMask layerBatPrincipal;
    [SerializeField] private LayerMask layerOtherBat;
    [SerializeField] private GameObject circleToSpawnUnderBatPrincipal, circleToSpawnUnderOtherBat;
    
    private GameObject _goBatPrincipal;
    private bool _isBatPrincipalTouched;
    private Camera _cam;
    
    private void Start()
    {
        _cam = Camera.main;
        var position = transform.position;
        circleToSpawnUnderBatPrincipal = Instantiate(circleToSpawnUnderBatPrincipal, position, Quaternion.Euler(-90,0,0));
        circleToSpawnUnderBatPrincipal.SetActive(false);
        circleToSpawnUnderOtherBat = Instantiate(circleToSpawnUnderOtherBat, position, Quaternion.Euler(-90,0,0));
        circleToSpawnUnderOtherBat.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerBatPrincipal))
            {
                var hitPos = hit.transform.position;
                circleToSpawnUnderBatPrincipal.transform.position = new Vector3(hitPos.x, 0.01f, hitPos.z);
                circleToSpawnUnderBatPrincipal.SetActive(true);
                _goBatPrincipal = hit.collider.gameObject;
                _isBatPrincipalTouched = true;  
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (circleToSpawnUnderBatPrincipal != null)
            {
                circleToSpawnUnderBatPrincipal.SetActive(false);
            }
        }

        if (_isBatPrincipalTouched)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerOtherBat))
                {
                    var hitPos = hit.transform.position;
                    circleToSpawnUnderOtherBat.transform.position = new Vector3(hitPos.x, 0.01f, hitPos.z); 
                    circleToSpawnUnderOtherBat.SetActive(true);
                }
                else
                {
                    if (circleToSpawnUnderOtherBat != null)
                    {
                        circleToSpawnUnderOtherBat.SetActive(false);
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (circleToSpawnUnderOtherBat != null)
                {
                    circleToSpawnUnderOtherBat.SetActive(false);
                }   
            }
        }
        
    }
    
}
