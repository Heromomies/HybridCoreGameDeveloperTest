using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DisplayCircleBat : MonoBehaviour
{
    [SerializeField] private LayerMask layerBatPrincipal;
    [SerializeField] private LayerMask layerWall;
    [SerializeField] private GameObject circleToSpawnUnderBatPrincipal;
    [SerializeField] private GameObject wrongCircleToSpawnUnderBat;
    
    private GameObject _circleToSpawnUnderOtherBat;
    private GameObject _firstBatGo, _secondBatGo;
    private bool _isBatPrincipalTouched, _secondBatTouched;
    private Camera _cam;
    private readonly Vector3 _addHeightOnYAxis = new Vector3(0, 0.01f, 0);
    
    private void Start()
    {
        _cam = Camera.main;
        var position = transform.position;
        
        circleToSpawnUnderBatPrincipal = Instantiate(circleToSpawnUnderBatPrincipal, position, Quaternion.Euler(-90,0,0));
        circleToSpawnUnderBatPrincipal.SetActive(false);
        
        _circleToSpawnUnderOtherBat = Instantiate(circleToSpawnUnderBatPrincipal, position, Quaternion.Euler(-90,0,0));
        _circleToSpawnUnderOtherBat.SetActive(false);
        
        wrongCircleToSpawnUnderBat = Instantiate(wrongCircleToSpawnUnderBat, position, Quaternion.Euler(-90,0,0));
        wrongCircleToSpawnUnderBat.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // When the player click on his mouse
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerBatPrincipal))
            {
                if (hit.collider.TryGetComponent(out BatManager batManager))
                {
                    if (batManager.numberOfAlliesInBat > 0)
                    {
                        var hitPos = hit.transform.position;
                        circleToSpawnUnderBatPrincipal.transform.position = new Vector3(hitPos.x, 0.01f, hitPos.z);
                        circleToSpawnUnderBatPrincipal.SetActive(true);
                        _firstBatGo = hit.collider.gameObject;
                        _isBatPrincipalTouched = true;  
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0)) // When the player release his mouse
        {
            if (_secondBatTouched && _firstBatGo != null)
            {
                if(_firstBatGo.TryGetComponent(out BatManager batManager))
                {
                    if (batManager.numberOfAlliesInBat > 0)
                    {
                        var secondBatPos = _secondBatGo.transform.position;
                        batManager.batLinked.Add(secondBatPos);
                        batManager.CanLaunchAllie();
                    }
                }
            }
            
            if (circleToSpawnUnderBatPrincipal != null)
            {
                circleToSpawnUnderBatPrincipal.SetActive(false);
                _isBatPrincipalTouched = false; 
            }
            if (_circleToSpawnUnderOtherBat != null)
            {
                _circleToSpawnUnderOtherBat.SetActive(false);
            } 
            if (wrongCircleToSpawnUnderBat != null)
            {
                wrongCircleToSpawnUnderBat.SetActive(false);
            } 
        }

        if (_isBatPrincipalTouched) // When the player touch the first bat
        {
            if (Input.GetMouseButton(0)) 
            {
                Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(_firstBatGo.transform.position, ray.direction, Mathf.Infinity, layerWall))
                {
                    if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
                    {
                        _secondBatTouched = false;
                        var worldPos = hit.point;
                        wrongCircleToSpawnUnderBat.transform.position = worldPos + _addHeightOnYAxis;
                        wrongCircleToSpawnUnderBat.SetActive(true);
                        _circleToSpawnUnderOtherBat.SetActive(false);
                    }
                }
                else
                {
                    if (Physics.Raycast(ray, out var hitBat, Mathf.Infinity, layerBatPrincipal))
                    {
                        if (hitBat.collider.name != _firstBatGo.name)
                        {
                            var hitPos = hitBat.transform.position;
                            _circleToSpawnUnderOtherBat.transform.position = new Vector3(hitPos.x, 0.01f, hitPos.z);
                            _circleToSpawnUnderOtherBat.SetActive(true);
                            _secondBatGo = hitBat.collider.gameObject;
                            _secondBatTouched = true;
                        }
                    }
                    else if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
                    {
                        var worldPos = hit.point;
                        _circleToSpawnUnderOtherBat.transform.position = worldPos + _addHeightOnYAxis; 
                        _circleToSpawnUnderOtherBat.SetActive(true);
                        wrongCircleToSpawnUnderBat.SetActive(false);
                        _secondBatTouched = false;
                    }
                }
            }
        }
        
    }

    /*void CreatePathAndDisplayShader(Vector3 startingPos, Vector3 finalPos)
    {
        var distance = Vector3.Distance(startingPos, finalPos);
        distance = (int) distance;
        transformLookAt.position = startingPos;
        transformLookAt.LookAt(finalPos); 
        
        for (int i = 0; i < distance/2; i++)
        {
            var v = new Vector3(transformLookAt.localPosition.x - i -i, 0.01f, transformLookAt.localPosition.z - i);
            
            PoolManager.Instance.SpawnObjectFromPool("ArrowDirection", v, transformLookAt.localRotation, transformLookAt);
        }
    }*/
}
