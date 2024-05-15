using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class PositionChecker : MonoBehaviour
{
    public CarController[] distances;
    public TMP_Text[] texts;
    public List<CarController> finishPositions = new List<CarController>();
    public int lapsToFinish;
    public GameObject finishMsg;
    public GameObject positionsMsg;
    public GameObject timerEndRace;
    
    [SerializeField] TMP_Text _1rstPos;
    [SerializeField] TMP_Text _last5;
    [SerializeField] TMP_Text _timerRaceEnd;

    public float cooldownEndRace = 5;

    List<CarController> _positions = new List<CarController>();
    int _indexText;

    private void Update()
    {
        foreach (var car in distances)
        {
            Vector3 dist = car.currentChecker.transform.position - car.transform.position;
            car.distToNextChecker = dist.magnitude;
        }

        _positions = distances.OrderByDescending(x => x.currentLap)
                     .ThenByDescending(x => x.currentCheck)
                     .ThenBy(x => x.distToNextChecker)
                     .ToList();

        if (finishPositions.Any())
        {
            _1rstPos.text = "1st: " + finishPositions.Where(x => x.currentLap > lapsToFinish).First().gameObject.name.ToString();
            if (finishPositions.Count >= distances.Length)
            {
                var lastFive = finishPositions.Skip(1).ToList();

                var lastFiveText = lastFive.Aggregate("", (current, car) =>
                {
                    int position = lastFive.IndexOf(car) + 2;
                    return $"{current}\n{position}{GetPositionSuffix(position)}: {car.gameObject.name}";
                });

                _last5.text = lastFiveText;
            }

            if(finishPositions.Count >= distances.Length / 2)
            {
                var carsNoFinish = distances
                    .SelectMany(x => x._allCars)
                    .TakeWhile(x => x.currentLap < lapsToFinish)
                    .ToList();

                timerEndRace.SetActive(true);
                cooldownEndRace -= Time.deltaTime;
                _timerRaceEnd.text = cooldownEndRace.ToString();
                if (cooldownEndRace <= 0 || finishPositions.Count >= distances.Length)
                {
                    timerEndRace.SetActive(false);
                }

                if (cooldownEndRace <= 0)
                {
                    timerEndRace.SetActive(false);
                    foreach (var item in carsNoFinish)
                    {
                        item.EndRace();
                    }
                }
            }
        }

        foreach (var text in texts)
        {
            text.text = _positions[_indexText].gameObject.name;
            _indexText++;
            if (_indexText > texts.Length - 1) _indexText = 0;
        }
    }

    private string GetPositionSuffix(int position)
    {
        if (position % 10 == 1 && position != 11) return "st";
        else if (position % 10 == 2 && position != 12) return "nd";
        else if (position % 10 == 3 && position != 13) return "rd";
        else return "th";
    }
}
