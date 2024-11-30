using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Robber _robber;
    [SerializeField] private Alarm _alarm;

    private int _checkRate = 2;

    private bool _hasEntered = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == _robber.gameObject)
        {
            if (_hasEntered == false)
            {
                _alarm.TriggerAlarm();
                _hasEntered = true;
                StartCoroutine(Await());
            }
            else
            {                
                _alarm.StopAlarm();
                _hasEntered = false;
                StartCoroutine(Await());
            }
        }
    }

    private IEnumerator Await()
    {
        while (true)
        {
            yield return new WaitForSeconds(_checkRate);
        }
    }
}
