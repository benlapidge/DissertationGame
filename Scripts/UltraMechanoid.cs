using UnityEngine;
using UnityEngine.AI;

public class UltraMechanoid : Shootable
{
   
    [Header("Shootable Object Properties")] [SerializeField]
    private float health;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    [SerializeField] private float DamageAmount = default;

    [Header("Audio")] 
    [SerializeField] private AudioSource voiceBox = default;
    [SerializeField] private AudioClip hitClip = default;
    [SerializeField] private AudioClip shotClip = default;
    [SerializeField] private AudioClip deathClip = default;
    
    

    //patrolling
    public Vector3 walkPoint;
    public float walkPointRange;

    //attacking
    public float timeBetweenAttacks;
    public float sightRange, attackRange;

    //https://www.youtube.com/watch?v=UjkSFoLxesw&t=55s
    // Lifestyle
    private bool alive;
    private bool alreadyAttacked;

    private Animator animation;
    
    private bool playerInSightRange, playerInAttackRange;
    private bool walkPointSet;
    
    //laser effect
    private LineRenderer laser;

    private void Awake()
    {
        
        alive = true;
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animation = gameObject.GetComponent<Animator>();
        voiceBox = GetComponent<AudioSource>();
        InitialiseLaser();
    }

    private void InitialiseLaser()
    {
        laser = gameObject.AddComponent<LineRenderer>();
        laser.enabled = false;
        laser.startColor = Color.red;
        laser.endColor = Color.red;
        laser.startWidth = 0.4f;
        laser.endWidth = 0.4f;
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (alive && !playerInSightRange && !playerInAttackRange)
        {
            Idle();
            Debug.Log("player not in sight or attack range");
        }

        if (alive && playerInSightRange && !playerInAttackRange)
        {
            Idle();
            Debug.Log("player seen but not in attack range");
        }

        if (alive && playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
            Debug.Log("ATTACK");
        }
    }


    private void Idle()
    {
       animation.Play("UM_idle");
       Debug.Log("UM Idle Method");
    }
    
    

    private void AttackPlayer()
    {
        Debug.Log("UM Attack Player");
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        Debug.Log("UM Lookat Player");

        if (!alreadyAttacked)
        {
            Debug.Log("UM Attack");
            animation.Play("UM_Attack");
            // only if raycast is player
            RaycastHit hit;
            if (Physics.Raycast(agent.transform.position, agent.transform.forward, out hit))
            {
                if (hit.transform == player)
                {
                    laser.enabled = true;
                    laser.SetPosition(0, agent.transform.position);
                    laser.SetPosition(1, player.transform.position);
                    voiceBox.PlayOneShot(shotClip);
                    HealthSystem.OnTakeDamage(DamageAmount);
                    //end attack
                    alreadyAttacked = true;
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }
            }

            
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public override void OnDamage(float amount)
    {
        health -= amount;
        voiceBox.PlayOneShot(hitClip);
        if (health <= 0) Death();
    }

    public override void Death()
    {
        agent.enabled = false;
        alive = false;
        voiceBox.PlayOneShot(deathClip);
        animation.Play("UM_death");
        animation.StopPlayback();
        Destroy(gameObject, 3f);
    }

    public override void OnFocus()
    {
        Debug.Log("Looking at " + gameObject.name);
    }

    public override void OnFocusLost()
    {
        Debug.Log("Stopped looking at " + gameObject.name);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
