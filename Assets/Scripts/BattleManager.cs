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
    public Battlefield bf;
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
    // Start is called before the first frame update
    void Awake()
    {
        S = this;
        //bf = gameObject.GetComponent<Battlefield>();
        CrntUnit = _crntUnit;
    }

    // Update is called once per frame
    void Update()
    {

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
                target = bf.FindCell(pos.x - 1, pos.y + 1);
                break;
            case direction.leftBack:
                target = bf.FindCell(pos.x, pos.y + 1);
                break;
            case direction.leftForward:
                target = bf.FindCell(pos.x - 1, pos.y);
                break;
            case direction.Right:
                target = bf.FindCell(pos.x + 1, pos.y - 1);
                break;
            case direction.RightBack:
                target = bf.FindCell(pos.x + 1, pos.y);
                break;
            case direction.RightForward:
                target = bf.FindCell(pos.x, pos.y - 1);
                break;
        }
        _crntUnit.bf.FindCell(_crntUnit.Position.x, _crntUnit.Position.y).unit = null;
        _crntUnit.Position = target.position;
        _crntUnit.GoToCell(target);
        CrntUnit = _crntUnit;
    }
    /// <summary>
    /// Атака текущим юнитом другого
    /// </summary>
    /// <param name="attackedUnit">Юнит, которого атакуют</param>
    public void AttackUnit(Unit attackedUnit)
    {
        if (attackedUnit is Enemy)
            (attackedUnit as Enemy).Selected = false;
        List<Cell> traectory = Cell.CreateTraectory(
            bf.FindCell(_crntUnit.Position.x, _crntUnit.Position.y),
            bf.FindCell(attackedUnit.Position.x, attackedUnit.Position.y), bf);
        float hitChance = _crntUnit.weapon.hitChance;
        foreach(Cell cell in traectory)
        {
            if (cell.ostacle != null)
            {
                hitChance *= cell.ostacle.hitChance;
            }
        }
        if (Random.value > hitChance)
            return;
        _crntUnit.weapon.transform.LookAt(attackedUnit.transform);
        Projectile proj = Instantiate(_crntUnit.weapon.projectilePrefab,
            _crntUnit.weapon.shootPoint.transform.position, 
            _crntUnit.transform.rotation, 
            _crntUnit.weapon.transform).GetComponent<Projectile>();
        proj.target = attackedUnit.transform.position;
        proj.rigid.velocity = _crntUnit.weapon.transform.forward * proj.speed;
        attackedUnit.Health -= _crntUnit.weapon.damage;
        CrntUnit = _crntUnit;
    }

    /// <summary>
    /// Текущий юнит, изменеие свойства приводит к перерасчету интерфейса
    /// </summary>
    public Unit CrntUnit
    {
        get { return CrntUnit;  }
        set
        {
            _crntUnit = value;
            if (_crntUnit is Ally)
            {
                var pos = _crntUnit.Position;
                foreach (Cell cell in bf.recoloredCells)
                    cell.gameObject.GetComponent<Renderer>().
                        material.color = Color.white;
                foreach (Button wb in walkButtons)
                    wb.interactable = true;

                // Левый передний
                Cell lf = bf.FindCell(pos.x - 1, pos.y);
                if (lf == null || 
                    lf.unit != null ||
                    lf.ostacle != null)
                    walkButtons[0].interactable = false;
                else
                {
                    var cell = bf.FindCell(pos.x - 1, pos.y);
                    cell.gameObject.GetComponent<Renderer>().
                        material.color = Color.green;
                    bf.recoloredCells.Add(cell);
                }

                // Левый
                Cell l = bf.FindCell(pos.x - 1, pos.y + 1);
                if (l == null ||
                    l.unit != null ||
                    l.ostacle != null)
                    walkButtons[1].interactable = false;
                else
                {
                    var cell = bf.FindCell(pos.x - 1, pos.y + 1);
                    cell.gameObject.GetComponent<Renderer>().
                        material.color = Color.green;
                    bf.recoloredCells.Add(cell);
                }

                // Левый задний
                Cell lb = bf.FindCell(pos.x, pos.y + 1);
                if (lb == null || 
                    lb.unit != null ||
                    lb.ostacle != null)
                    walkButtons[2].interactable = false;
                else
                {
                    var cell = bf.FindCell(pos.x, pos.y + 1);
                    cell.gameObject.GetComponent<Renderer>().
                        material.color = Color.green;
                    bf.recoloredCells.Add(cell);
                }

                // Правый передний
                Cell rf = bf.FindCell(pos.x, pos.y - 1);
                if (rf == null || 
                    rf.unit != null ||
                    rf.ostacle != null)
                    walkButtons[3].interactable = false;
                else
                {
                    var cell = bf.FindCell(pos.x, pos.y - 1);
                    cell.gameObject.GetComponent<Renderer>().
                        material.color = Color.green;
                    bf.recoloredCells.Add(cell);
                }

                // Правый
                Cell r = bf.FindCell(pos.x + 1, pos.y - 1);
                if (r == null || 
                    r.unit != null ||
                    r.ostacle != null)
                    walkButtons[4].interactable = false;
                else
                {
                    var cell = bf.FindCell(pos.x + 1, pos.y - 1);
                    cell.gameObject.GetComponent<Renderer>().
                        material.color = Color.green;
                    bf.recoloredCells.Add(cell);
                }

                // Правый задний
                Cell rb = bf.FindCell(pos.x + 1, pos.y);
                if (rb == null || 
                    rb.unit != null ||
                    rb.ostacle != null)
                    walkButtons[5].interactable = false;
                else
                {
                    var cell = bf.FindCell(pos.x + 1, pos.y);
                    cell.gameObject.GetComponent<Renderer>().
                        material.color = Color.green;
                    bf.recoloredCells.Add(cell);
                }


                var children = enemyPanel.GetComponentsInChildren<Transform>();
                for (int i = 1; i < children.Length; i++)
                    Destroy(children[i].gameObject);
                enemyPanel.parent.parent.gameObject.SetActive(false);
                List<Enemy> enemiesInRange = new List<Enemy>();
                foreach(Enemy enemy in bf.enemies)
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
                    go.enemy = e;
                }
            }
            
        }
    }
}
