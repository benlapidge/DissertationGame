using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class UltraMechanoid : Shootable
{

    [Header("Shootable Object Properties")] [SerializeField]
    private float health = default;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsPlayer;
    [SerializeField] private float DamageAmount = default;

    [Header("Audio")] 
    [SerializeField] private AudioSource voiceBox = default;
    [SerializeField] private AudioClip hitClip = default;
    [SerializeField] private AudioClip shotClip = default;
    [SerializeField] private AudioClip deathClip = default;
    [SerializeField] private GameObject helper = default;
    
// Spawn control
    private bool isCreated = true;

    private int counter = 0;
    //patrolling

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
        laser.startWidth = 0.6f;
        laser.endWidth = 0.6f;
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (alive && !playerInSightRange && !playerInAttackRange)
        {
            Idle();
            
        }

        if (alive && playerInSightRange && !playerInAttackRange)
        {
            Idle();

        }

        if (alive && playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
            
        }

        if (!isCreated)
        {
            SpawnHelper();
        }

        if (counter >= 15)
        {
            isCreated = false;
            counter = 0;
        }
        
    }


    private void Idle()
    {
        animation.Play("UM_idle");
    }

    private void AttackPlayer()
    {
        
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        //find eyeball for laser
        Vector3 eyeBall = GameObject.Find("Eye").transform.position;
        if (!alreadyAttacked)
        {
            int randomValue = Random.Range(0,3);
            animation.Play("UM_Attack");
            // only if raycast is player
            RaycastHit hit;
            
            if (Physics.Raycast(agent.transform.position, agent.transform.forward, out hit))
            {
                if (hit.transform == player && randomValue == 0)
                {
                    //laser attack
                    laser.enabled = true;
                        laser.SetPosition(0, eyeBall);
                        laser.SetPosition(1, player.transform.position);
                        voiceBox.PlayOneShot(shotClip);
                        HealthSystem.OnTakeDamage(DamageAmount);
                        //end attack
                        alreadyAttacked = true;
                        Invoke(nameof(ResetAttack), timeBetweenAttacks);
                } else if (hit.transform == player && randomValue == 1)
                {
                    //failed attack
                    laser.enabled = true;
                    laser.SetPosition(0, eyeBall);
                    laser.SetPosition(1, new Vector3(player.transform.position.x-3,player.transform.position.y+2,player.transform.position.z));
                    voiceBox.PlayOneShot(shotClip);
                    alreadyAttacked = true;
                    Invoke(nameof(ResetAttack), timeBetweenAttacks); 
                } else if (hit.transform == player && randomValue == 2)
                {
                    // spawn helper
                    isCreated = false;
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }
            }

            
        }
    }

    private void SpawnHelper()
    {
        isCreated = true;
        Debug.Log("SPAWN HELPER");
        int randomNumber = Random.Range(0, 2);
        Transform helperSpawnPoint = gameObject.transform.Find("SpawnPoints").GetChild(randomNumber);
        Instantiate(helper,
            new Vector3(helperSpawnPoint.position.x, helperSpawnPoint.position.y, helperSpawnPoint.position.z),
            helperSpawnPoint.rotation);
        
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public override void OnDamage(float amount)
    {
        counter++;
        animation.Play("UM_hit");
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
        // for visualising sight and attack ranges
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    
    // for adaptation
    public void SetHealth(int value)
    {
        switch (value)
        {
            case 2:
                health = 100f;
                break;
            case 3:
                health = 200f;
                break;
            case 4:
                health = 300f;
                break;
            case 5:
                health = 400f;
                break;
            case 6:
                health = 500f;
                break;
            
        }
    }

    public void SetDamagePower(int value)
    {
        switch (value)
        {
            case 2:
                DamageAmount = 10f;
                break;
            case 3:
                DamageAmount = 15f;
                break;
            case 4:
                DamageAmount = 20f;
                break;
            case 5:
                DamageAmount = 25f;
                break;
            case 6:
                DamageAmount = 30f;
                break;
        }
    }
}
