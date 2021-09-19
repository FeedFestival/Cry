using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class FieldOfHearing : MonoBehaviour
{
    private UnitInteligence _unitInteligence;

    public void Initialize(UnitInteligence unitInteligence)
    {
        _unitInteligence = unitInteligence;
        gameObject.AddComponent<TriggerExtension>();
        gameObject.GetComponent<TriggerExtension>().ShowMesh = true;
    }

    private Unit _player;

    private bool _listenCarefully_running;
    IEnumerator ListenCarefully()
    {
        yield return new WaitForSeconds(1f);
        _listenCarefully_running = false;

        if (_unitInteligence.MainState == MainState.Aggressive)
            _unitInteligence.SoundPosition = _player.transform.position;
        else
            _unitInteligence.SetAlert(null, Alert.Hearing, _player.transform.position);
    }

    void OnTriggerEnter(Collider unitObject)
    {
        if (unitObject.CompareTag("Player") && unitObject.name == "FeetCollider")
        {
            if (_listenCarefully_running == false)
            {
                _player = unitObject.transform.parent.GetComponent<Unit>();
                _listenCarefully_running = true;
                StartCoroutine(ListenCarefully());
            }
        }
    }

    void OnTriggerStay(Collider unitObject)
    {
        if (_unitInteligence.MainState == MainState.Aggressive)
        {
            if (unitObject.CompareTag("Player") && unitObject.name == "FeetCollider")
                _unitInteligence.SoundPosition = unitObject.transform.position;
        }
    }

    void OnTriggerExit(Collider unitObject)
    {
        if (_listenCarefully_running)
        {
            StopCoroutine("ListenCarefully");
            _listenCarefully_running = false;
        }

        if (unitObject.CompareTag("Player") && unitObject.name == "FeetCollider")
        {
            _unitInteligence.SoundPosition = unitObject.transform.position;
            _unitInteligence.RemoveAlert(Alert.Hearing);
        }
    }
}