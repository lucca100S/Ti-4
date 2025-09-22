using System.Collections.Generic;
using UnityEngine;

public class InteractableAgent : MonoBehaviour
{
    //Lista de interativos no momento
    List<OptionalInteractableObjects> currentInteractables = new List<OptionalInteractableObjects>();

    public void AddInteractable(OptionalInteractableObjects interactable) 
    {
        if (interactable != null && !currentInteractables.Contains(interactable))
        {
            currentInteractables.Add(interactable);
            interactable.OnIsIntereactable();
        }
    }
    public void ExcludeInteractable(OptionalInteractableObjects interactable) 
    {
        if (interactable != null && currentInteractables.Contains(interactable))
        {
            currentInteractables.Remove(interactable);
            interactable.OnIsNotIntereactable();
        }
    }

    private void Interact() 
    {
        if(currentInteractables.Count > 0) 
        {
            OptionalInteractableObjects chosenInteractable = currentInteractables[0];
            if (currentInteractables.Count > 1)
            {
                float distanceToChosen;
                float distanceToCandidate;
                for (int i = 1; i < currentInteractables.Count; i++)
                {
                    distanceToChosen = Vector3.Distance(this.gameObject.transform.position, chosenInteractable.transform.position);
                    distanceToCandidate = Vector3.Distance(this.gameObject.transform.position, currentInteractables[i].transform.position);
                    if (distanceToChosen > distanceToCandidate)
                    {
                        chosenInteractable = currentInteractables[i];
                    }
                }
            }

            chosenInteractable.Interaction();
        }
    }
}
