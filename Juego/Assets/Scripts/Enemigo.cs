using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    [Header("Componentes")]
    public NavMeshAgent agent;
    public Animator animator;
    public string paramCorrer;
    public string paramAtacar;

    [Header("Jugador")]
    public Transform jugador;

    [Header("Percepción")]
    public float radioDeteccion;
    public float radioAtaque;

    [Header("Patrulla")]
    public float radioPatrulla;
    public float tiempoIdle;

    private float idleTimer;
    private bool tieneDestino = false;
    private bool persiguiendo = false;
    private bool atacando = false;

    [Header("Daño")]
    public float tiempoImpacto;    // tiempo exacto del golpe
    public float duracionAtaque;   // duración total de la animación
    public int daño = 20;

    [Header("Cooldown de daño")]
    public float tiempoEntreDaños;   // cada cuánto puede hacer daño
    private float ultimoDaño;


    void Start()
    {
        NuevoDestino();
    }


    void Update()
    {
        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (atacando)
        {
            agent.ResetPath();
            return;
        }

        // Detectar jugador
        if (distancia <= radioDeteccion && distancia > radioAtaque)
            persiguiendo = true;
        else if (distancia > radioDeteccion + 2f)
            persiguiendo = false;

        // Atacar
        if (distancia <= radioAtaque)
        {
            StartCoroutine(Atacar());
            return;
        }

        // Perseguir
        if (persiguiendo)
        {
            agent.speed=8;
            agent.SetDestination(jugador.position);
            animator.SetBool(paramCorrer, true);
            return;
        }

        // Patrulla
        animator.SetBool(paramCorrer, agent.velocity.magnitude > 0.1f);

        if (!tieneDestino)
        {
            NuevoDestino();
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= tiempoIdle)
            {
                idleTimer = 0f;
                tieneDestino = false;
            }
        }
    }



    private System.Collections.IEnumerator Atacar()
    {
        atacando = true;

        agent.ResetPath();
        agent.speed = 0;
        animator.SetBool(paramCorrer, false);

        animator.SetTrigger(paramAtacar);

        // Esperar al frame del golpe
        yield return new WaitForSeconds(tiempoImpacto);

        // SOLO HACER DAÑO SI YA PASÓ EL COOLDOWN
        if (Time.time - ultimoDaño >= tiempoEntreDaños)
        {
            if (Vector3.Distance(transform.position, jugador.position) <= radioAtaque)
            {
                jugador.GetComponent<VidaJugador>().RecibirDaño(daño);
                ultimoDaño = Time.time;
            }
        }

        // Esperar resto animación
        yield return new WaitForSeconds(duracionAtaque - tiempoImpacto);

        atacando = false;
    }



    void NuevoDestino()
    {
        Vector3 randomDir = Random.insideUnitSphere * radioPatrulla + transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, radioPatrulla, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            tieneDestino = true;
        }
        else
        {
            tieneDestino = false;
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioAtaque);
    }
}
