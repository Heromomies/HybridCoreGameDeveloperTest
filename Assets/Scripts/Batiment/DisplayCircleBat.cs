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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerBatPrincipal))
            {
                var hitPos = hit.transform.position;
                circleToSpawnUnderBatPrincipal.transform.position = new Vector3(hitPos.x, 0.01f, hitPos.z);
                circleToSpawnUnderBatPrincipal.SetActive(true);
                _firstBatGo = hit.collider.gameObject;
                _isBatPrincipalTouched = true;  
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (_secondBatTouched && _firstBatGo != null)
            {
                _firstBatGo.TryGetComponent(out BatManager spawnAlliesInBat);
                spawnAlliesInBat.batLinked.Add(_secondBatGo.transform.position);
                spawnAlliesInBat.CanLaunchAllie();
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
        }

        if (_isBatPrincipalTouched)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hitBat, Mathf.Infinity, layerBatPrincipal))
                {
                    var hitPos = hitBat.transform.position;
                    _circleToSpawnUnderOtherBat.transform.position = new Vector3(hitPos.x, 0.01f, hitPos.z);
                    _circleToSpawnUnderOtherBat.SetActive(true);
                    _secondBatGo = hitBat.collider.gameObject;
                    _secondBatTouched = true;
                }
                else if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
                {
                    var worldPos = hit.point;
                    _circleToSpawnUnderOtherBat.transform.position = worldPos + _addHeightOnYAxis; 
                    _circleToSpawnUnderOtherBat.SetActive(true);
                    _secondBatTouched = false;
                }
            }
        }
        
    }
    
}
