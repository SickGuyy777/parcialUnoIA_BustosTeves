using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] float _movementSpeed;
    [SerializeField] float _arriveWayRadius;
    [SerializeField] GameObject[] _waypoints;

    GameObject _currentWay;
    int _wayIndex = 0;

    private void Start()
    {
        _currentWay = _waypoints[_wayIndex];
        _movementSpeed = Random.Range(6, 11);
        _arriveWayRadius = Random.Range(1f, 3f);
    }

    private void Update()
    {
        Vector3 dist = _currentWay.transform.position - transform.position;
        if(dist.magnitude < _arriveWayRadius)
        {
            _wayIndex++;
            if (_wayIndex > _waypoints.Length - 1) _wayIndex = 0;
            _currentWay = _waypoints[_wayIndex];
            _movementSpeed = Random.Range(6, 11);
            _arriveWayRadius = Random.Range(1f, 3f);
        }

        transform.position += dist.normalized * _movementSpeed * Time.deltaTime;
        transform.forward = dist;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_currentWay.transform.position, _arriveWayRadius);
    }
}
