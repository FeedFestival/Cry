using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class FieldOfView : MonoBehaviour
{
    private UnitInteligence _unitInteligence;

    public void Initialize(UnitInteligence unitInteligence)
    {
        _unitInteligence = unitInteligence;
        gameObject.AddComponent<TriggerExtension>();
        gameObject.GetComponent<TriggerExtension>().ShowMesh = true;
    }

    void OnTriggerEnter(Collider unitObject)
    {
        if (unitObject.CompareTag("Player") && unitObject.name == "FeetCollider")
        {
            if (_unitInteligence.Player == null)
                _unitInteligence.Player = unitObject.transform.parent.GetComponent<Unit>();
            _unitInteligence.PlayerInFOV = true;
        }
    }
    void OnTriggerExit(Collider unitObject)
    {
        if (unitObject.CompareTag("Player") && unitObject.name == "FeetCollider")
        {
            _unitInteligence.RemoveAlert(Alert.Seeing);
            _unitInteligence.PlayerInFOV = false;

            if (_unitInteligence.MainAction == MainAction.MoveTowardsPlayer && _unitInteligence.AlertLevel == AlertLevel.Talkative)
                _unitInteligence.SetAggressive();
        }
    }
}