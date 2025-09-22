using System.Collections.Generic;
using UnityEngine;

public class InteractableAgent : MonoBehaviour
{
    //Lista de interativos no momento
    List<IInteractable> currentInteractables = new List<IInteractable>();

    public void AddInteractable(IInteractable interactable) 
    {
        if (interactable != null && !currentInteractables.Contains(interactable))
        {
            currentInteractables.Add(interactable);
            interactable.OnIsIntereactable();
        }
    }
    public void ExcludeInteractable(IInteractable interactable) 
    {
        if (interactable != null && currentInteractables.Contains(interactable))
        {
            currentInteractables.Remove(interactable);
            interactable.OnIsNotIntereactable();
        }
    }
}
