using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{ 
    private bool _hasEntered = false;

    public event Action<Door> Entered;
    public event Action<Door> CameOut;
        
    private void OnCollisionEnter(Collision collision)
    {
        if (_hasEntered == false)
        {
            Entered?.Invoke(this);
            _hasEntered = true;
        }
        else
        {
            CameOut?.Invoke(this);
            _hasEntered = false;
        }
    }
}
