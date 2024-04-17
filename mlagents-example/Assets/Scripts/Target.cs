using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Renderer _renderer;

    private int _counter = 0;
    // private Vector3 _currentTarget;
    void Start()
    {

        _renderer = GetComponent<Renderer>();
        // _currentTarget = RandomWorldPoint();
    }
    Vector3 RandomWorldPoint()
    {
        Vector3 randomPoint = new(Random.Range(0f, 1f), Random.Range(0f, 1f),0);
        return Camera.main.ViewportToWorldPoint(randomPoint);
    }

    // Update is called once per frame
    void Update()
    {
        _counter++;
        if (!_renderer.isVisible)
        {
            transform.position = RandomWorldPoint();
            // _currentTarget = RandomWorldPoint();
        } else if (_counter > 1000)
        {
            transform.position = RandomWorldPoint();
            _counter = 0;
        }
        // else
        // {
        //     transform.position = Vector3.Lerp(transform.position, _currentTarget, 0.1f * Time.deltaTime);
        //     if (Vector2.Distance(transform.position, _currentTarget) < 1f)
        //     {
        //         transform.position = RandomWorldPoint();
        //         _currentTarget = Vector3.zero;  //RandomWorldPoint();
        //     }
        // } 
    }
}
