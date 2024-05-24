using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public string targetTag;
    public Rigidbody rb;
    public GameObject hitFlash;
    public bool destroyable;
    [SerializeField]
    private float lifetime;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;
    private bool _isQuitting;

    private Manager _manager;
    // Start is called before the first frame update
    void Start()
    {
        _isQuitting = false;
        StartCoroutine(LifeCoroutine());
        _manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_manager.hasControl)
            rb.velocity = transform.forward * speed;
        else
            rb.velocity = Vector3.zero;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(targetTag))
        {
            LifeSystem enemyLife = other.GetComponent<LifeSystem>();
            enemyLife.ApplyDamage(damage);
        }
        if(other.CompareTag(targetTag) || other.CompareTag("Wall") || other.CompareTag("Floor")){
            gameObject.SetActive(false);
        }
    }
    private void OnApplicationQuit()
    {
        _isQuitting = true;
    }
    private void OnDisable()
    {
        if(!_isQuitting)
            Instantiate(hitFlash, transform.position, Quaternion.identity);
    }
    
    private IEnumerator LifeCoroutine()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);
    }
}
