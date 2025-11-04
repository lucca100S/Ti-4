using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class InteractWithInteractable : MonoBehaviour
{
    [Header("Configura��es de Pegar Objeto")]
    public float maxPickupDistance = 20f;
    public LayerMask pickupMask = ~0;


    private void Update()
    {
        // Apenas para teste: pressionar a tecla E para interagir
        if (Keyboard.current.eKey.wasPressedThisFrame || Gamepad.current[GamepadButton.North].wasPressedThisFrame)
        {
            Interact();
        }
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
            Collider nearest = grabbables.First(); // agora s� pega de objetos v�lidos
            Debug.Log($"[Pickup] Objeto mais pr�ximo (Grabbable): {nearest.name}");
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


