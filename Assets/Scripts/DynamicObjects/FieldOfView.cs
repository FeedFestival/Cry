using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

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
        if (unitObject.CompareTag("Player"))
            _unitInteligence.Alert(unitObject.transform.position, AlertType.PlayerInFieldOfView);
    }
    void OnTriggerExit(Collider unitObject)
    {
        //if (unitObject.CompareTag("Player"))
        //    Debug.Log("exit" + unitObject);
    }
}