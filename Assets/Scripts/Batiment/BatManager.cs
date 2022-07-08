using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BatManager : MonoBehaviour
{
    [SerializeField] private float timeBeforeSpawnAllies;
    [SerializeField] private bool increaseAllieAtStart;
    [SerializeField] private Material blueMaterial;
    [HideInInspector] public int numberOfAlliesInBat;
    [HideInInspector] public bool batCorrupted;

    private int _possibleLinks = 1;
    private int _pathLinked;

    private MeshRenderer _meshRenderer;
    private TextMeshPro _textDisplayNumberOfAllies;
    [HideInInspector] public List<Vector3> batLinked;

    // Start is called before the first frame update
    void Start()
    {
        _textDisplayNumberOfAllies = GetComponentInChildren<TextMeshPro>();
        _meshRenderer = GetComponent<MeshRenderer>();

        if (increaseAllieAtStart)
        {
            batCorrupted = true;
            InvokeRepeating(nameof(IncreaseNumberAllies), 0f, timeBeforeSpawnAllies);
        }
    }

    public void IncreaseNumberAllies()
    {
        if (!batCorrupted)
        {
            batCorrupted = true;
        }
        
        numberOfAlliesInBat ++;
        _textDisplayNumberOfAllies.text = $"{numberOfAlliesInBat}";

        if (numberOfAlliesInBat > 10)
        {
            _possibleLinks++;
        }
        if (numberOfAlliesInBat > 30)
        {
            _possibleLinks++;
        }
    }

    public void ChangeMaterial()
    {
        _meshRenderer.material = blueMaterial;
    }
    
    public void CanLaunchAllie()
    {
        if (_pathLinked < _possibleLinks)
        {
            InvokeRepeating(nameof(LaunchAllies), 0f, timeBeforeSpawnAllies);
            _pathLinked++;
        }
    }
    void LaunchAllies()
    {
        if (numberOfAlliesInBat > 0)
        {
            var allie =PoolManager.Instance.SpawnObjectFromPool("Allie", transform.position, Quaternion.identity, null);
            if(allie.TryGetComponent(out AllieManager allieManager))
            {
                allieManager.goDontCollideWith = gameObject;
                for (int i = 0; i < batLinked.Count; i++)
                {
                    var randomNumber = Random.Range(0, batLinked.Count);
                    allieManager.batToGoPos = batLinked[randomNumber];
                }
            }

            numberOfAlliesInBat--;
            _textDisplayNumberOfAllies.text = $"{numberOfAlliesInBat}";
        }
    }
}
