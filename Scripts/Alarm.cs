using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Alarm : MonoBehaviour
{
    [SerializeField] private Light _lights;
    [SerializeField] private Door _door;

    private float _soundValueRate = 0.1f;
    private float _soundValueSmoothness = 0.03f;

    private Coroutine _soundCoroutine;

    private WaitForSeconds _rate;
    private AudioSource _audioSource;
    private Animator _lightsAnimator;

    private void Awake()
    {   
        _rate = new WaitForSeconds(_soundValueRate);
        _audioSource = GetComponent<AudioSource>();
        _lightsAnimator = _lights.GetComponent<Animator>();

        ToggleLights(false);

        _door.Entered += Trigger;
        _door.CameOut += Stop;
    }

    private void OnDisable()
    {
        _door.Entered -= Trigger;
        _door.CameOut -= Stop;
    }

    public void Trigger(Door door)
    {
        ToggleLights(true);

        if (_soundCoroutine != null)
            StopCoroutine(_soundCoroutine);

        _soundCoroutine = StartCoroutine(FadeInSound());
    }

    public void Stop(Door door)
    {
        ToggleLights(false);

        if (_soundCoroutine != null)
            StopCoroutine(_soundCoroutine);

        _soundCoroutine = StartCoroutine(FadeOutSound());
    }

    private void ToggleLights(bool value)
    {       
        if(_lights != null)
            _lights.enabled = value;

        if(_lightsAnimator != null)
            _lightsAnimator.enabled = value;
    }

    private IEnumerator FadeInSound()
    {        
        float minVolume = 0;
        float maxVolume = 0.5f;

        _audioSource.volume = minVolume;
        _audioSource.Play();

        while (_audioSource.volume < maxVolume)
        {
            _audioSource.volume += _soundValueSmoothness;
            yield return _rate;
        }
    }    

    private IEnumerator FadeOutSound()
    {
        float minVolume = 0;

        while (_audioSource.volume > minVolume)
        {
            _audioSource.volume -= _soundValueSmoothness;
            yield return _rate;
        }
    }        
}
