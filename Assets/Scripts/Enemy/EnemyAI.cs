using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack, Rest }
    public State currentState;

    public Transform[] patrolPoints;
    public float AgentSpeed = 5;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float restDuration = 5f;
    public Renderer EnemyRender;


    private int currentPatrolIndex;
    private Transform player;
    private NavMeshAgent agent; 
    private float stateTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = State.Patrol;
        currentPatrolIndex = 0;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Rest:
                Rest();
                break;
        }
    }

    private void Patrol()
    {
        EnemyRender.material.color = Color.green;

        // Si llega al destino, pasa al siguiente punto de patrullaje
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }

        // Si el jugador está en rango, cambia a Persecución
        if (Vector3.Distance(player.position, transform.position) <= chaseRange)
        {
            currentState = State.Chase;
        }
    }

    private void Chase()
    {
        agent.SetDestination(player.position);

        EnemyRender.material.color = Color.red;

        // Si el jugador está en rango de ataque, cambia a Ataque
        if (Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            currentState = State.Attack;
        }
        // Si el jugador sale del rango de persecución, vuelve a Patrullaje
        else if (Vector3.Distance(player.position, transform.position) > chaseRange)
        {
            currentState = State.Rest;
            GoToNextPatrolPoint();
        }
    }

    private void Attack()
    {
        // Detiene el movimiento para atacar
        agent.speed = AgentSpeed;

        // Lógica de ataque (por ejemplo, reducir la salud del jugador)
        Debug.Log("Atacando al jugador!");

        // Si el jugador se aleja del rango de ataque, vuelve a Persecución
        if (Vector3.Distance(player.position, transform.position) > attackRange)
        {
            agent.speed = AgentSpeed;
            currentState = State.Chase;
        }
        // Si el jugador no está en el rango de visión, vuelve a Patrullaje
        else if (Vector3.Distance(player.position, transform.position) > chaseRange)
        {
            agent.speed = AgentSpeed;
            currentState = State.Patrol;
            GoToNextPatrolPoint();
        }
    }

    private void Rest()
    {
        EnemyRender.material.color = Color.yellow;
        // Detiene el movimiento durante el descanso
        agent.speed = 0;
        // Cronómetro para el descanso
        
        if (restDuration < 0)
        {
            currentState = State.Patrol;
            agent.speed = AgentSpeed;
            GoToNextPatrolPoint();
            restDuration = 2.5f;
        }
        else
        {
            restDuration -= 1 * Time.deltaTime;
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RestPoint"))
        {
            currentState = State.Rest;
        }
    }
}
