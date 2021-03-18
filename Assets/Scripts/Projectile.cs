using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 target;
    [SerializeField]
    private float permipermissibleDistance;
    public Rigidbody rigid;
    public float speed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target) < permipermissibleDistance)
            Destroy(this.gameObject);
    }
}
