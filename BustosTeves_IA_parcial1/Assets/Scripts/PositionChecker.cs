using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChecker : MonoBehaviour
{
    public List<CarController> distances = new List<CarController>();

    private void Update()
    {
        foreach (var car in distances)
        {
            Vector3 dist = car.currentChecker.transform.position - car.transform.position;
            car.distToNextChecker = dist.magnitude;
        }
    }
}
