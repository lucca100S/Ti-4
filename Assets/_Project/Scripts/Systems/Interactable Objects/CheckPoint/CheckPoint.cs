using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Representa um checkpoint interagível no cenário.
/// </summary>
public class CheckPoint : OptionalInteractableObjects
{
    [SerializeField]
    public GameObject SpawnPoint;
    public override void Interaction()
    {
#if UNITY_EDITOR
        base.Interaction();
#endif
        /* Lógica de checkpoint */
        if (!PreviouslyInteracted)
        {
            PlayerCheckpoint player = GameObject.FindFirstObjectByType<PlayerCheckpoint>();
            if (player != null)
            {
                player.SetCheckpoint(this);
                //Teste
                this.gameObject.GetComponent<Renderer>().material.color = Color.cyan;
            }
        }
        else 
        {
            //Abrir fast travel
        }
    }

    public override void InitializeInteractableState()
    {
        base.InitializeInteractableState();
        switch (PreviouslyInteracted)
        {
            case true:
                Debug.Log("Checkpoint em estado ativado");
                //Teste
                this.gameObject.GetComponent<Renderer>().material.color = Color.cyan;
                break;
            case false:
                Debug.Log("Checkpoint em estado não ativado");
                //Teste
                this.gameObject.GetComponent<Renderer>().material.color = Color.green;
                break;
        }
    }
}
