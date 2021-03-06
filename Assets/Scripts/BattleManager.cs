using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, обеспечивающий бой
/// </summary>
public class BattleManager : MonoBehaviour
{
    public static BattleManager S;
    /// <summary>
    /// Текущее поле боя
    /// </summary>
    [SerializeField]
    private Battlefield _bf;
    /// <summary>
    /// Текущий юнит
    /// </summary>
    [SerializeField]
    private Unit _crntUnit;
    /// <summary>
    /// Кнопки ходьбы
    /// </summary>
    [SerializeField]
    private List<Button> walkButtons = new List<Button>();
    [SerializeField]
    private Transform enemyPanel;
    /// <summary>
    ///  Шаблон кнопки вврага
    /// </summary>
    [SerializeField]
    private GameObject enemyBtn;
    public Enemy selectedEnemy;
    private List<Unit> initiativeList = new List<Unit>();
    private int crntUnitId;
    // Start is called before the first frame update
    void Awake()
    {
        S = this;
        //bf = gameObject.GetComponent<Battlefield>();
        Bf = _bf;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Battlefield Bf
    {
        get { return _bf; }
        set
        {
            setupBattle();
        }
    }

    private void setupBattle()
    {
        initiativeList.Add(Bf.hero);
        foreach(Enemy enemy in Bf.enemies)
        {
            initiativeList.Add(enemy);
        }
        initiativeList.OrderByDescending(unit => unit.initiative);
        crntUnitId = 0;
        CrntUnit = initiativeList[crntUnitId];
    }

    /// <summary>
    /// Ходьба союзного юнита
    /// </summary>
    /// <param name="dir">Направление</param>
    public void walkBtnClick(int dir)
    {
        direction crntDir = (direction)dir;
        Vector2 pos = _crntUnit.Position;
        Cell target = new Cell();
        switch (crntDir)
        {
            case direction.left:
                target = _bf.FindCell(pos.x - 1, pos.y + 1);
                break;
            case direction.leftBack:
                target = _bf.FindCell(pos.x, pos.y + 1);
                break;
            case direction.leftForward:
                target = _bf.FindCell(pos.x - 1, pos.y);
                break;
            case direction.Right:
                target = _bf.FindCell(pos.x + 1, pos.y - 1);
                break;
            case direction.RightBack:
                target = _bf.FindCell(pos.x + 1, pos.y);
                break;
            case direction.RightForward:
                target = _bf.FindCell(pos.x, pos.y - 1);
                break;
        }
        _crntUnit.GoToCell(target);
        UnshowInterface();
        enemyPanel.parent.parent.gameObject.SetActive(false);
        //SwitchUnit();
    }
    /// <summary>
    /// Атака текущим юнитом другого
    /// </summary>
    /// <param name="attackedUnit">Юнит, которого атакуют</param>
    public void AttackUnit(Unit attackedUnit)
    {
        if (attackedUnit is Enemy)
            (attackedUnit as Enemy).Selected = false;
        float hitChance = CalculateHitChance(_crntUnit.weapon.hitChance, _crntUnit.Position, attackedUnit.Position);
        UnshowInterface();      
        if (Random.value > hitChance)
        {
            SwitchUnit();
            return;
        }
        _crntUnit.weapon.transform.LookAt(attackedUnit.transform);
        Projectile proj = Instantiate(_crntUnit.weapon.projectilePrefab,
            _crntUnit.weapon.shootPoint.transform.position,
            _crntUnit.transform.rotation,
            _crntUnit.weapon.transform).GetComponent<Projectile>();
        proj.target = attackedUnit.transform.position;
        proj.rigid.velocity = _crntUnit.weapon.transform.forward * proj.speed;
        attackedUnit.Health -= _crntUnit.weapon.damage;
        SwitchUnit();
    }

    /// <summary>
    /// Текущий юнит, изменеие свойства приводит к перерасчету интерфейса
    /// </summary>
    public Unit CrntUnit
    {
        get { return _crntUnit;  }
        set
        {
            _crntUnit = value;
            var pos = _crntUnit.Position;
            foreach (Cell cell in _bf.recoloredCells)
                cell.gameObject.GetComponent<Renderer>().
                    material.color = Color.white;
            foreach (Button wb in walkButtons)
                wb.gameObject.SetActive(false);
            if (_crntUnit is Ally)
            {

                // Левый передний
                Cell lf = _bf.FindCell(pos.x - 1, pos.y);
                if (!(lf == null || 
                    lf.unit != null ||
                    lf.ostacle != null))
                {
                    var cell = _bf.FindCell(pos.x - 1, pos.y);
                    cell.gameObject.GetComponent<Renderer>().
                        material.color = Color.green;
                    _bf.recoloredCells.Add(cell);
                    walkButtons[0].gameObject.SetActive(true);
                }

                // Левый
                Cell l = _bf.FindCell(pos.x - 1, pos.y + 1);
                if (!(l == null ||
                    l.unit != null ||
                    l.ostacle != null))
                {
                    var cell = _bf.FindCell(pos.x - 1, pos.y + 1);
                    cell.gameObject.GetComponent<Renderer>().
                        material.color = Color.green;
                    _bf.recoloredCells.Add(cell);
                    walkButtons[1].gameObject.SetActive(true);
                }

                // Левый задний
                Cell lb = _bf.FindCell(pos.x, pos.y + 1);
                if (!(lb == null || 
                    lb.unit != null ||
                    lb.ostacle != null))
                {
                    var cell = _bf.FindCell(pos.x, pos.y + 1);
                    cell.gameObject.GetComponent<Renderer>().
                        material.color = Color.green;
                    _bf.recoloredCells.Add(cell);
                    walkButtons[2].gameObject.SetActive(true);
                }

                // Правый передний
                Cell rf = _bf.FindCell(pos.x, pos.y - 1);
                if (!(rf == null || 
                    rf.unit != null ||
                    rf.ostacle != null))
                {
                    var cell = _bf.FindCell(pos.x, pos.y - 1);
                    cell.gameObject.GetComponent<Renderer>().
                        material.color = Color.green;
                    _bf.recoloredCells.Add(cell);
                    walkButtons[3].gameObject.SetActive(true);
                }

                // Правый
                Cell r = _bf.FindCell(pos.x + 1, pos.y - 1);
                if (!(r == null || 
                    r.unit != null ||
                    r.ostacle != null))
                {
                    var cell = _bf.FindCell(pos.x + 1, pos.y - 1);
                    cell.gameObject.GetComponent<Renderer>().
                        material.color = Color.green;
                    _bf.recoloredCells.Add(cell);
                    walkButtons[4].gameObject.SetActive(true);
                }

                // Правый задний
                Cell rb = _bf.FindCell(pos.x + 1, pos.y);
                if (!(rb == null || 
                    rb.unit != null ||
                    rb.ostacle != null))
                {
                    var cell = _bf.FindCell(pos.x + 1, pos.y);
                    cell.gameObject.GetComponent<Renderer>().
                        material.color = Color.green;
                    _bf.recoloredCells.Add(cell);
                    walkButtons[5].gameObject.SetActive(true);
                }


                var children = enemyPanel.GetComponentsInChildren<Transform>();
                for (int i = 1; i < children.Length; i++)
                    Destroy(children[i].gameObject);
                enemyPanel.parent.parent.gameObject.SetActive(false);
                List<Enemy> enemiesInRange = new List<Enemy>();
                foreach(Enemy enemy in _bf.enemies)
                {
                    if (enemy.Position.x <= _crntUnit.Position.x + _crntUnit.weapon.range &&
                        enemy.Position.x >= _crntUnit.Position.x - _crntUnit.weapon.range &&
                        enemy.Position.y <= _crntUnit.Position.y + _crntUnit.weapon.range &&
                        enemy.Position.y >= _crntUnit.Position.y - _crntUnit.weapon.range)
                        enemiesInRange.Add(enemy);
                }
                if (enemiesInRange.Count != 0)
                    enemyPanel.parent.parent.gameObject.SetActive(true);
                foreach (Enemy e in enemiesInRange)
                {
                    EnemyButton go = Instantiate(enemyBtn, enemyPanel).GetComponent<EnemyButton>();
                    string chance = CalculateHitChance(_crntUnit.weapon.hitChance, _crntUnit.Position, e.Position) * 100 + "%";
                    go.text.text = chance;
                    go.enemy = e;
                }
            }
            else if (_crntUnit is Enemy)
            {
                (_crntUnit as Enemy).MakeMove();
            }
        }
    }

    public void SwitchUnit()
    {
        crntUnitId++;
        if (crntUnitId >= initiativeList.Count)
            crntUnitId = 0;
        CrntUnit = initiativeList[crntUnitId];
    }

    public float CalculateHitChance(float baseChance, Vector2 start, Vector2 finish)
    {
        List<Cell> traectory = Cell.CreateTraectory(
            _bf.FindCell(start),
            _bf.FindCell(finish), _bf);
        float hitChance = baseChance;
        foreach (Cell cell in traectory)
        {
            if (cell.ostacle != null)
            {
                hitChance *= cell.ostacle.hitChance;
            }
        }
        return hitChance;
    }

    private void UnshowInterface()
    {
        foreach (Button wb in walkButtons)
            wb.gameObject.SetActive(false);
        var children = enemyPanel.GetComponentsInChildren<Transform>();
        for (int i = 1; i < children.Length; i++)
            Destroy(children[i].gameObject);
    }
}
