using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private Vector2 _position;
    [SerializeField]
    private float height;
    public float speed;
    [SerializeField]
    private int _health;
    public Weapon weapon;
    public float permissibleDistance; //Плавность движения
    private bool moving;
    public Vector3 movingTarget;
    public Battlefield bf;
    public float showDamageDelay;

    private void Awake()
    {

    }

    protected void Update()
    {
        if (moving)
        {
            Vector3 pos = new Vector3();
            pos.x = Mathf.Lerp(transform.position.x, movingTarget.x, Time.deltaTime * speed);
            pos.y = Mathf.Lerp(transform.position.y, movingTarget.y, Time.deltaTime * speed);
            pos.z = Mathf.Lerp(transform.position.z, movingTarget.z, Time.deltaTime * speed);
            transform.position = pos;
        }
        if (Vector3.Distance(transform.position, movingTarget) < permissibleDistance)
        {
            //transform.position = movingTarget;
            moving = false;
        }
    }

    public Vector2 Position
    {
        get { return _position; }
        set {  _position = value; }
    }

    public void GoToCell(Cell target)
    {
        movingTarget = new Vector3(target.transform.position.x,
            target.transform.position.y + height,
            target.transform.position.z);
        target.unit = this;
        moving = true;
    }

    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                if (this is Enemy)
                {
                    bf.enemies.Remove(this as Enemy);
                }
                Destroy(this.gameObject);
            }
            this.gameObject.GetComponent<Renderer>().
                        material.color = Color.yellow;
            Invoke("ResetMaterial", showDamageDelay);
        }
    }

    private void ResetMaterial()
    {
        this.gameObject.GetComponent<Renderer>().
            material.color = Color.red;
    }
}
