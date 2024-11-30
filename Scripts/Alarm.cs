using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Alarm : MonoBehaviour
{
    [SerializeField] private Light _lights;
    [SerializeField] private Door _door;

    private float _soundValueRate = 0.1f;
    private float _soundValueSmoothness = 0.03f;

    private Coroutine _soundFadeIn;
    private Coroutine _soundFadeOut;

    private WaitForSeconds _rate;

    private AudioSource _audioSource => GetComponent<AudioSource>();   

    private void Awake()
    {   
        _rate = new WaitForSeconds(_soundValueRate);

        ToggleLights(false);
        _door.Entered += TriggerAlarm;
        _door.CameOut += StopAlarm;
    }

    public void TriggerAlarm(Door door)
    {
        ToggleLights(true);

        if (_soundFadeOut != null)
            StopCoroutine(_soundFadeOut);

        _soundFadeIn = StartCoroutine(FadeInSound());
    }

    public void StopAlarm(Door door)
    {
        ToggleLights(false);

        if (_soundFadeIn != null)
            StopCoroutine(_soundFadeIn);

        _soundFadeOut = StartCoroutine(FadeOutSound());
    }

    private void ToggleLights(bool value)
    {
        var lightsAnim = _lights.GetComponent<Animator>();

        if (_lights != null)
            _lights.enabled = value;
        
        if(lightsAnim != null)
            lightsAnim.enabled = value;
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
