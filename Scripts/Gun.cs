using UnityEngine;

public class Gun : MonoBehaviour
{
    //reference
    //https://www.youtube.com/watch?v=THnivyG0Mvo&t=734s
    // Start is called before the first frame update
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public ParticleSystem muzzleFlash;
    [SerializeField] private Camera fpsCam;
    private Animator shoot;
    [SerializeField] AudioSource gunSound = default;
    [SerializeField] private AudioClip shot = default;

    private void Start()
    {
        shoot = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) Shoot();
    }

    private void Shoot()
    {
        muzzleFlash.Play();
        shoot.Play("BarrelShot");
        shoot.Play("idle");
        gunSound.PlayOneShot(shot);
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            var target = hit.transform.GetComponent<Shootable>();
            if (target != null) target.OnDamage(damage);
        }
    }
}