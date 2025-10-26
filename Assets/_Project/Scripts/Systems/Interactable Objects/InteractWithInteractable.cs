using System.Linq;
using Player;
using UnityEngine;

public class InteractWithInteractable : MonoBehaviour
{
    [Header("Configurações de Pegar Objeto")]
    public float maxPickupDistance = 20f;
    public LayerMask pickupMask = ~0;

    private void Awake()
    {
        ActionsManager.Instance.Interact += Interact;
    }
    public void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, maxPickupDistance, pickupMask);
        Debug.Log($"[Pickup] {colliders.Length} objeto(s) detectado(s) no raio.");


        var grabbables = colliders
            .Where(c => c.CompareTag("Interactable"))
            .OrderBy(c => Vector3.Distance(this.transform.position, c.transform.position))
            .ToArray();

        if (grabbables.Length > 0)
        {
            Collider nearest = grabbables.First(); // agora só pega de objetos válidos
            Debug.Log($"[Pickup] Objeto mais próximo (Grabbable): {nearest.name}");
            Debug.Log($"[Pickup] Pegando objeto: {nearest.name}");
            nearest.gameObject.GetComponent<IInteractable>().Interaction();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, maxPickupDistance);
    }
}


