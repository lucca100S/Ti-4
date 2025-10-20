using UnityEngine;

/// <summary>
/// Representa um coletável no cenário.
/// Notifica observadores quando é coletado.
/// </summary>
public class Collectables : OptionalInteractableObjects
{
    public override void Interaction()
    {
     
           
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CollectableObservable.Instance?.NotifyListeners(this);
            this.gameObject.SetActive(false);
        }
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(SphereOrigin, Radius);
    }
}
