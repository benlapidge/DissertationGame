using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : Shootable
{
    [Header("Shootable Object Properties")] [SerializeField]
    private float health;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Audio")] 
    [SerializeField] private AudioSource voiceBox = default;
    [SerializeField] private AudioClip[] hitClips = default;
    [SerializeField] private AudioClip[] walkClip = default;
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
    private ParticleSystem blood;
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
        blood = gameObject.GetComponentInChildren<ParticleSystem>();
        voiceBox = GetComponent<AudioSource>();
        InitialiseLaser();
    }

    private void InitialiseLaser()
    {
        laser = gameObject.AddComponent<LineRenderer>();
        laser.enabled = false;
        laser.startColor = Color.red;
        laser.endColor = Color.red;
        laser.startWidth = 0.2f;
        laser.endWidth = 0.2f;
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (alive && !playerInSightRange && !playerInAttackRange)
            Patrolling(); //note this cool syntax for further if statements
        if (alive && playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (alive && playerInSightRange && playerInAttackRange) AttackPlayer();
    }


    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        var distanceToWalkPoint = transform.position - walkPoint;
        //Walkpoint Reached
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        var randomZ = Random.Range(-walkPointRange, walkPointRange);
        var randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position); //stops enemy from moving
        transform.LookAt(player);

        if (!alreadyAttacked)
        {

            // only if raycast is player
            RaycastHit hit;
            if (Physics.Raycast(agent.transform.position, agent.transform.forward, out hit))
            {
                if (hit.transform == player)
                {
                    laser.enabled = true;
                    laser.SetPosition(0, agent.transform.position);
                    laser.SetPosition(1, player.transform.position);
                    HealthSystem.OnTakeDamage(20);
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
        if (health <= 0) Death();
    }

    public override void Death()
    {
        agent.enabled = false;
        alive = false;
        blood.Play();
        voiceBox.PlayOneShot(deathClip);
        animation.Play("DeathAnim");
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
}