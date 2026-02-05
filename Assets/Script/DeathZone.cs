using System;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private GameObject _player;
    private GameObject _spawnPoint;
    
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.transform.position = _spawnPoint.transform.position;
        }
    }
}
