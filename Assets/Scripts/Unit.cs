using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для работы с юнитами
/// </summary>
public class Unit : MonoBehaviour
{
    /// <summary>
    /// Координаты юнита
    /// </summary>
    [SerializeField]
    private Vector2 _position;
    [SerializeField]
    private float height;
    public float speed;
    [SerializeField]
    private int _health;
    public float initiative;
    /// <summary>
    /// Текущее оружие
    /// </summary>
    public Weapon weapon;
    public float permissibleDistance; //Плавность движения
    private bool moving;
    public Vector3 movingTarget;
    public Battlefield bf;
    public float showDamageDelay;
    [SerializeField]
    private Color originalColor;

    private void Awake()
    {
        var u = this;
    }

    protected void Update()
    {
        // Движение юнита
        if (moving)
        {
            Vector3 pos = new Vector3();
            pos.x = Mathf.Lerp(transform.position.x, movingTarget.x, Time.deltaTime * speed);
            pos.y = Mathf.Lerp(transform.position.y, movingTarget.y, Time.deltaTime * speed);
            pos.z = Mathf.Lerp(transform.position.z, movingTarget.z, Time.deltaTime * speed);
            transform.position = pos;
            if (Vector3.Distance(transform.position, movingTarget) < permissibleDistance)
            {
                BattleManager.S.SwitchUnit();
                //transform.position = movingTarget;
                moving = false;
            }
        }
    }

    /// <summary>
    /// Координаты юнита
    /// </summary>
    public Vector2 Position
    {
        get { return _position; }
        set {  _position = value; }
    }

    /// <summary>
    /// Перемещение юнита на другую клетку
    /// </summary>
    /// <param name="target">Клетка, на которую надо переместиться</param>
    public void GoToCell(Cell target)
    {

        movingTarget = new Vector3(target.transform.position.x,
            target.transform.position.y + height,
            target.transform.position.z);
        Cell previousCell = bf.FindCell(Position.x, Position.y);
        previousCell.unit= null;
        target.unit = this;
        Position = target.position;
        moving = true;
    }

    /// <summary>
    /// Здоровье юнита
    /// </summary>
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
            bf.FindCell(Position).gameObject.GetComponent<Renderer>().
                        material.color = Color.yellow;
            Invoke("ResetMaterial", showDamageDelay);
        }
    }

    private void ResetMaterial()
    {
        bf.FindCell(Position).gameObject.GetComponent<Renderer>().
            material.color = originalColor;
    }
}
