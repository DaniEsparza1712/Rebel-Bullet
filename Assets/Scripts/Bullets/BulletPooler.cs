using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private int poolSize;

    private List<GameObject> _bullets = new();

    public string bulletKey;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var bull = Instantiate(bullet);
            _bullets.Add(bull);
            bull.SetActive(false);
        }
    }

    public GameObject PullBullet(Vector3 pos, Vector3 forward)
    {
        foreach(var bull in _bullets)
        {
            if (!bull.activeSelf)
            {
                bull.transform.position = pos;
                bull.transform.forward = forward;
                bull.GetComponent<Rigidbody>().velocity = Vector3.zero;
                bull.SetActive(true);
                
                return bull;
            }
        }
        return null;
    }
}
