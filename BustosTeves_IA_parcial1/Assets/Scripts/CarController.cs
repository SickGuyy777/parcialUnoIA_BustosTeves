using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CarController : MonoBehaviour
{
    public float movementSpeed;
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

    public List<CarController> _allCars = new List<CarController>();
    List<F1Car> _f1;
    List<Nascar> _nascar;
    List<Motorbike> _motorbike;

    private void Start()
    {
        if (!_allCars.Contains(this)) _allCars.Add(this);

        _f1 = _allCars.OfType<F1Car>().ToList();
        _nascar = _allCars.OfType<Nascar>().ToList();
        _motorbike = _allCars.OfType<Motorbike>().ToList();

        _currentWay = _waypoints[_wayIndex];
        currentChecker = _positionCheck[_chekcerIndex];
        _checkerManager.finishMsg.SetActive(false);
        _checkerManager.positionsMsg.SetActive(true);

        SetStats();
    }

    private void Update()
    {
        Vector3 dist = _currentWay.transform.position - transform.position;

        if (dist.magnitude < _arriveWayRadius)
        {
            _wayIndex++;
            if (_wayIndex > _waypoints.Length - 1) _wayIndex = 0;
            _currentWay = _waypoints[_wayIndex];

            SetStats();
        }

        transform.position += dist.normalized * movementSpeed * Time.deltaTime;
        transform.forward = dist;

        if (currentLap > _checkerManager.lapsToFinish)
        {
            if (!_checkerManager.finishPositions.Contains(this)) _checkerManager.finishPositions.Add(this);

            if(_checkerManager.finishPositions.Count == _checkerManager.distances.Length)
            {
                _checkerManager.finishMsg.SetActive(true);
                _checkerManager.positionsMsg.SetActive(false);
            }

            movementSpeed -= 2f * Time.deltaTime;

            if (movementSpeed <= 0.3f) movementSpeed = 0f;
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

    void SetStats()
    {
        foreach (var f1 in _f1)
        {
            f1.movementSpeed = Random.Range(8, 13);
            f1._arriveWayRadius = Random.Range(.2f, 1.6f);
        }

        foreach (var nascar in _nascar)
        {
            nascar.movementSpeed = Random.Range(6, 12);
            nascar._arriveWayRadius = Random.Range(1f, 2.5f);
        }

        foreach (var bike in _motorbike)
        {
            bike.movementSpeed = Random.Range(6, 9);
            bike._arriveWayRadius = Random.Range(2f, 4f);
        }
    }

    public void EndRace()
    {
        _checkerManager.finishMsg.SetActive(true);
        _checkerManager.positionsMsg.SetActive(false);

        movementSpeed -= 2f * Time.deltaTime;

        if (movementSpeed <= 0.3f) movementSpeed = 0f;
    }
}
