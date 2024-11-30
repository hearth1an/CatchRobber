using System.Collections;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    [SerializeField] private Light _lights;

    private float _soundValueRate = 0.1f;
    private float _soundValueSmoothness = 0.03f;

    private Coroutine _soundFadeIn;
    private Coroutine _soundFadeOut;    

    private AudioSource _audioSource => GetComponent<AudioSource>();
    private Animator _lightsAnim => _lights.gameObject.GetComponent<Animator>();    

    private void Awake()
    {   
        ToggleLights(false);
    }

    public void TriggerAlarm()
    {
        ToggleLights(true);

        if (_soundFadeOut != null)
        {
            StopCoroutine(_soundFadeOut);
        }

        _soundFadeIn = StartCoroutine(FadeInSound());
    }

    public void StopAlarm()
    {
        ToggleLights(false);

        if (_soundFadeIn != null)
        {
            StopCoroutine(_soundFadeIn);
        }

        _soundFadeOut = StartCoroutine(FadeOutSound());
    }

    private void ToggleLights(bool value)
    {
        _lights.enabled = value;
        _lightsAnim.enabled = value;
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
            yield return new WaitForSeconds(_soundValueRate);
        }
    }    

    private IEnumerator FadeOutSound()
    {
        float minVolume = 0;

        while (_audioSource.volume > minVolume)
        {
            _audioSource.volume -= _soundValueSmoothness;
            yield return new WaitForSeconds(_soundValueRate);
        }
    }        
}
