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
            _unitInteligence.Alert(unitObject.transform.parent.GetComponent<Unit>(), AlertType.InFieldOfView);
    }
    void OnTriggerExit(Collider unitObject)
    {
        if (unitObject.CompareTag("Player") && unitObject.name == "FeetCollider")
        {
            if (_unitInteligence.ActionTowardsAlert == ActionTowardsAlert.FacingAlertPosition)
                _unitInteligence.ActionTowardsAlert = ActionTowardsAlert.FacingAlertPosition;
            _unitInteligence.RemoveAlert(AlertType.InFieldOfView);
        }
    }
}