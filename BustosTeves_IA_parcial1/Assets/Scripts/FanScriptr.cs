using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanScriptr : MonoBehaviour
{
    public Color fanColor;

    FansManager _manager;
    Renderer _fanRenderer;

    private void Awake()
    {
        _manager = GetComponentInParent<FansManager>();
        _fanRenderer = GetComponent<Renderer>();

        fanColor = GetColor();
        _fanRenderer.material.color = fanColor;
        if (!_manager.allFans.Contains(this)) _manager.allFans.Add(this);
    }

    public Color GetColor()
    {
        Color[] _colors = { Color.red, Color.yellow, Color.blue, Color.green, Color.cyan, Color.magenta, Color.grey };
        int randomIndex = Random.Range(0, _colors.Length);
        return _colors[randomIndex];
    }
}
