using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Classe responsável pelo gerenciamento das colisões por trigger do player
/// </summary>
public class PlayerTriggerCollsion : MonoBehaviour, IColliderEnterCollision
{
    public HashSet<string> CollisionTags { get; set; }
    [SerializeField]
    [Tooltip("Exposição de CollisionTags herdado por interface")]
    public List<string> tags;

    private void Awake()
    {
        CollisionTags = new HashSet<string>(tags);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (CollisionTags.Contains(other.tag))
            {
                switch (other.tag)
                {
                    case "Poison":
                        Debug.Log("Morto");
                        //this.gameObject.TryGetComponent<T>().DeathRoutine();
                        break;
                    default: 
                        Debug.Log($"Exceção encontrada no método OnTriggerEnter() de PlayerTriggerCollision, tag: {other.tag} definida em CollisionTags porém sem comportamento definido");
                        break;
                }
            }
        }
    }
}
