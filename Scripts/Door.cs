using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{ 
    private WaitForSeconds _rate;
    private int _checkRate = 2;
    private bool _hasEntered = false;

    public event Action<Door> Entered;
    public event Action<Door> CameOut;

    private void Awake()
    {
        _rate = new WaitForSeconds(_checkRate);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_hasEntered == false)
        {
            Entered?.Invoke(this);
            _hasEntered = true;
            StartCoroutine(Await());
        }
        else
        {
            CameOut?.Invoke(this);
            _hasEntered = false;
            StartCoroutine(Await());
        }
    }

    private IEnumerator Await()
    {
        while (true)
        {
            yield return _rate;
        }
    }
}
