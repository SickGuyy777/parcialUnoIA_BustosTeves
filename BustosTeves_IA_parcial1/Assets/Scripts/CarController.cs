using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] float _movementSpeed;
    [SerializeField] float _arriveWayRadius;
    [SerializeField] GameObject[] _waypoints;
    [SerializeField] GameObject[] _positionCheck;
    [SerializeField] PositionChecker _checkerManager;
    public float distToNextChecker;

    GameObject _currentWay;
    [HideInInspector] public int currentCheck = 0;
    [HideInInspector] public int currentLap = 0;
    [HideInInspector] public GameObject currentChecker;
    int _wayIndex, _chekcerIndex = 0;

    private void Start()
    {
        _currentWay = _waypoints[_wayIndex];
        currentChecker = _positionCheck[_chekcerIndex];
        _checkerManager.finishMsg.SetActive(false);
        _checkerManager.positionsMsg.SetActive(true);
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

        if (currentLap > _checkerManager.lapsToFinish)
        {
            if (!_checkerManager.finishPositions.Contains(this)) _checkerManager.finishPositions.Add(this);

            if(_checkerManager.finishPositions.Count == _checkerManager.distances.Length)
            {
                _checkerManager.finishMsg.SetActive(true);
                _checkerManager.positionsMsg.SetActive(false);
            }

            _movementSpeed -= 2f * Time.deltaTime;

            if (_movementSpeed <= 0.3f) _movementSpeed = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PosChecker")
        {
            _chekcerIndex++;
            if (_chekcerIndex > _positionCheck.Length - 1) _chekcerIndex = 0;
            currentCheck++;
            currentChecker = _positionCheck[_chekcerIndex];
        }
        if (other.tag == "FinishLine") currentLap++;
    }
}
