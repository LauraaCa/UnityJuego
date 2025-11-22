using UnityEngine;

public class ZonaGanadoraDebug : MonoBehaviour
{
    [Header("UI (opcional)")]
    public GameObject pantallaGanaste;

    [Header("Explosiones (opcional)")]
    public GameObject explosionPrefab;
    public float explosionRadius = 50f;
    public float explosionForce = 800f;
    public LayerMask explosionMask;
    public LayerMask ignoreRoomLayer;

    bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("[ZonaGanadora] OnTriggerEnter detectado con: " + other.name + " (tag: " + other.tag + ", layer: " + LayerMask.LayerToName(other.gameObject.layer) + ")");


        if (!other.CompareTag("Player"))
        {
            Debug.Log("[ZonaGanadora] El objeto no tiene tag Player. Tag actual: " + other.tag);
            return;
        }

        if (triggered)
        {
            Debug.Log("[ZonaGanadora] Ya se ha activado antes, ignorando.");
            return;
        }

        triggered = true;

        Debug.Log("¡¡ GANASTE !! - activando lógica de victoria");

        if (pantallaGanaste != null)
            pantallaGanaste.SetActive(true);
        else
            Debug.Log("[ZonaGanadora] pantallaGanaste es NULL");

     
        EliminarEnemigos();
    }

   

    private void EliminarEnemigos()
    {
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("[ZonaGanadora] Enemigos encontrados con tag 'Enemy': " + enemigos.Length);

        foreach (GameObject e in enemigos)
        {
            Debug.Log("[ZonaGanadora] Destruyendo enemigo: " + e.name);
            Destroy(e);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

        Collider c = GetComponent<Collider>();
        if (c != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.25f);
            Gizmos.matrix = transform.localToWorldMatrix;
            if (c is BoxCollider box)
            {
                Gizmos.DrawCube(box.center, box.size);
            }
            else if (c is SphereCollider sphere)
            {
                Gizmos.DrawSphere(sphere.center, sphere.radius);
            }
        }
    }
}
