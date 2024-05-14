using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class FansManager : MonoBehaviour
{
    public List<FanScriptr> allFans = new List<FanScriptr>();
    [SerializeField] TMP_Text _attendanceTxt;
    [SerializeField] TMP_Text _fansCount;

    private void Start()
    {
        var redFans = allFans.Where(x => x.fanColor == Color.red).ToList();
        var blueFans = allFans.Where(x => x.fanColor == Color.blue).ToList();
        var cyanFans = allFans.Where(x => x.fanColor == Color.cyan).ToList();
        var yellowFans = allFans.Where(x => x.fanColor == Color.yellow).ToList();
        var magentaFans = allFans.Where(x => x.fanColor == Color.magenta).ToList();
        var greenFans = allFans.Where(x => x.fanColor == Color.green).ToList();

        var F1 = redFans.Concat(cyanFans);
        var nascar = blueFans.Concat(greenFans);
        var motorbike = yellowFans.Concat(magentaFans);

        var attendance = F1.Concat(nascar).Concat(motorbike);
        _attendanceTxt.text = "Attendance: " + attendance.Count().ToString();

        int F1Count = F1.Count();
        int nascarCount = nascar.Count();
        int motorbikeCount = motorbike.Count();

        var attendanceCounts = new List<(string, int)>
        {
            ("F1", F1Count),
            ("Nascar", nascarCount),
            ("Motorbike", motorbikeCount)
        };
        attendanceCounts = attendanceCounts.OrderByDescending(tuple => tuple.Item2).ToList();

        string attendanceText = "MOST FANS\n";
        foreach (var tuple in attendanceCounts)
        {
            attendanceText += $"{tuple.Item1}: {tuple.Item2}\n";
        }
        _fansCount.text = attendanceText;

        var emptySeats = allFans.Where(x => x.fanColor == Color.gray)
                                .Select(x => x.gameObject)
                                .ToList();
        foreach (var seat in emptySeats) { seat.SetActive(false); }
    }
}
