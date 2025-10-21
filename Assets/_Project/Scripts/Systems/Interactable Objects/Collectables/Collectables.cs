using UnityEngine;

/// <summary>
/// Representa um colet�vel no cen�rio.
/// Notifica observadores quando � coletado.
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
