using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Transform _spawnPoint;
    
    private void Start()
    {
        _spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _spawnPoint.position = transform.position;
        }
    }
}
