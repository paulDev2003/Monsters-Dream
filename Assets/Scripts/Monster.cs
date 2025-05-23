using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
using TMPro;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public string monsterName = "name";
    public MonsterSO monsterSO;
    public MonsterData monsterData;
    public bool enemie;
    public int level = 1;
    public float health;
    public float physicalDamage;
    public float speedAttack;
    public float defense;
    public float evasion;
    public float magicalDamage;
    public float magicalDefense;
    public float shield;
    public UILifeBar lifeBar;
    public UIShieldBar shieldBar;
    public SkillDrop skillDrop;
    public bool wasSpawned = false;
    public bool boss = false;
    public enum typeDamage
    {
        Physical,
        Magical
    };
    public typeDamage damageType;

    public enum typeSummon
    {
        Normal,
        Edge
    }
    public Vector2Int rangeAttacksToSkill = new Vector2Int();
    public int hitAcumulation;
    public int hitToEffect;
    public typeSummon summonType;
    [Space(10)]
    public float distanceAttack;
    public float speedMovement;
    public float rotationSpeed = 5;
    public float regenerationTime = 3.5f;
    private float currentTimeRegeneration;
    private float enemieDistance;
    private float attackTime;
    private int attacksToSkill;
    private float totalAmountToSkill;
    [Space(10)]
    public float healthFigth;
    public Monster target;
    public bool dead;
    public int exp;
    public int maxExp;
    [Tooltip("Para saber si está invocado o no")]
    public bool inUse = false;


    [HideInInspector] public GameManager gameManager;
    public List<GameObject> oppositeList;
    [HideInInspector] public List<GameObject> ownList;
    private bool stopFigth = false;
    private MonsterClass monsterClass;
    private NavMeshAgent agent;
    public int valueI;
    public GameObject areaAttack;
    private Image circleAttacksToSkill;
    public Transform positionAttacksToSkill;
    public Collider targetCollider;
    private GameObject objCircleAttacks;
    public bool shieldActivated = false;

    [Space(10)]
    public int damageBuff;
    public int basicDamageBuff;
    public int healthBuff;
    public int healthRegeneration;
    public int magicDamageBuff;
    public float decreasedCoolDown = 1;
    public float multiplierIncreasedSpeedAttack = 1;
    public int physicIncreaseDefense;
    public int magicIncreaseDefense;
    public int shieldIncrease;

    public List<Sprite> spriteStates = new List<Sprite>();
    public List<int> intStates = new List<int>();
    private List<int> intStatesEffects = new List<int>();
    public List<StatusEffect> activeEffects = new List<StatusEffect>();
    public StatusEffect monsterEffect;

    protected virtual void Start()
    {
        SetupCore();
        SetupLists();
        SetupUI();
        SetupTarget();
        SetupExtras();       
    }

    protected virtual void SetupCore()
    {
        ReloadAttackToSkill();
        currentTimeRegeneration = regenerationTime;
        inUse = true;
        agent = GetComponent<NavMeshAgent>();
        maxExp = level * 50;
        agent.avoidancePriority = Random.Range(30, 60);
        gameManager = FindAnyObjectByType<GameManager>();
        Assert.IsNotNull(gameManager, "No encuentra gameManager");
    }

    protected virtual void SetupLists()
    {
        if (enemie)
        {
            oppositeList = gameManager.friendsList;
            ownList = gameManager.enemieList;
            UpdateStats();
        }
        else
        {
            oppositeList = gameManager.enemieList;
            ownList = gameManager.friendsList;
            gameManager.monsterDrop[valueI].monsterScript = this;
        }
    }

    protected virtual void SetupUI()
    {
        if (positionAttacksToSkill == null)
        {
            objCircleAttacks = Instantiate(gameManager.circleAttacksToSkill, transform.position + Vector3.up, Quaternion.identity,
            gameManager.canvasWorld.transform);
        }
        else
        {
            objCircleAttacks = Instantiate(gameManager.circleAttacksToSkill, positionAttacksToSkill.position, Quaternion.identity,
            gameManager.canvasWorld.transform);
            Debug.Log("Se instancia en segundo");
        }
        circleAttacksToSkill = objCircleAttacks.GetComponent<Image>();
        circleAttacksToSkill.fillAmount = 0;
    }

    protected virtual void SetupTarget()
    {
        target = ChooseTarget(oppositeList);
    }

    protected virtual void SetupExtras()
    {
        if (areaAttack != null)
        {
            Destroy(areaAttack);
        }
    }
    protected virtual void Update()
    {
        if (agent == null)
        {
            Debug.Log("No encuentra el navMesh");
        }
        if (positionAttacksToSkill == null)
        {
            objCircleAttacks.transform.position = transform.position + Vector3.up * 1.5f;
        }
        else
        {
            objCircleAttacks.transform.position = positionAttacksToSkill.position;
        }

        if (gameManager.finish)
        {
            stopFigth = true;
        }
        if (!dead && !stopFigth)
        {
            UpdateStatsEffects();
            if (target != null)
            {
                if (targetCollider != null)
                {
                    Vector3 closestPoint = targetCollider.ClosestPoint(transform.position);
                    enemieDistance = Vector3.Distance(transform.position, closestPoint);
                }
                else
                {
                    enemieDistance = Vector3.Distance(transform.position, target.transform.position);
                }
               // RotateTowardsTarget();
                
            }
            if (attackTime > 0)
            {
                attackTime -= Time.deltaTime;
            }
            else
            {
                Attack();
            }
            if (currentTimeRegeneration > 0)
            {
                currentTimeRegeneration -= Time.deltaTime;
            }
            else if (healthRegeneration > 0)
            {
                RegenerateHealth();
                currentTimeRegeneration = regenerationTime;
            }
            if (healthFigth <= 0)
            {
                agent.isStopped = true;
                agent.enabled = false;
                healthFigth = 0;
                gameManager.RemoveFromList(ownList, this);
                dead = true;
                lifeBar.UpdateFill(this);
                if (!boss && enemie)
                {
                    Invoke("DesactivateMonster", 2.5f);
                }
                
                if (!enemie)
                {
                    gameManager.countMonsters--;
                }
            }
            else
            {
                if (target != null && !agent.isStopped)
                {
                    if (enemieDistance > distanceAttack)
                    {
                        // Ir hacia el objetivo (automáticamente esquiva obstáculos)
                        agent.SetDestination(target.transform.position);
                    }
                    else
                    {
                        // Ya está en rango de ataque, detener el agente
                        agent.ResetPath(); // o agent.isStopped = true;
                        RotateTowardsTarget();
                    }
                }
            }
        }
    }

    private void UpdateStatsEffects()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].Tick(this, Time.deltaTime, intStatesEffects[i]);
            //if (activeEffects[i].IsFinished)
            //{
            //    activeEffects.RemoveAt(i);
            //}
        }
    }
    public Monster ChooseTarget(List<GameObject> listOpposite)
    {
        GameObject _target = null;
        float distance = float.MaxValue;
        foreach (var enemie in listOpposite)
        {
            if (distance > Vector3.Distance(transform.position, enemie.transform.position))
            {
                distance = Vector3.Distance(transform.position, enemie.transform.position);
                _target = enemie;
            }
        }
        Monster scriptTarget = null;
        if (_target != null)
        {
            scriptTarget = _target.GetComponent<Monster>();
            if (scriptTarget == null)
            {
                scriptTarget = _target.GetComponentInChildren<Monster>();
            }
        }
        if (scriptTarget != null)
        {
            targetCollider = scriptTarget.GetComponentInChildren<Collider>();
        }

        return scriptTarget;
    }

    public Monster ChooseTarget(GameObject target)
    {
        Debug.Log("ChooseTarget");
        GameObject _target = target;
        
        
        Monster scriptTarget = null;

        scriptTarget = _target.GetComponent<Monster>();
        if (scriptTarget == null)
        {
            scriptTarget = _target.GetComponentInChildren<Monster>();
        }
        
        if (scriptTarget != null)
        {
            targetCollider = scriptTarget.GetComponentInChildren<Collider>();
        }

        return scriptTarget;
    }

    protected virtual void Attack()
    {
        if (enemieDistance > distanceAttack)
        {
            // Ya no se usa MoveTowards
            agent.SetDestination(target.transform.position);
        }
        else if (attackTime <= 0)
        {
            Debug.Log("Se detiene");
            agent.ResetPath(); // Detener el movimiento al atacar

            if (attacksToSkill > 0)
            {
                BasicAttackDamage();
                circleAttacksToSkill.fillAmount += totalAmountToSkill;
                if (!enemie)
                {
                    skillDrop.cooldownImage.fillAmount = circleAttacksToSkill.fillAmount;
                }              
                attacksToSkill -= 1;
            }
            else
            {
                ReloadAttackToSkill();
                monsterSO.skill.ShootSkill(target);
                circleAttacksToSkill.fillAmount = 0;
                if (!enemie)
                {
                    skillDrop.cooldownImage.fillAmount = 0;
                }                
                attackTime = 1 / speedAttack;
            }

            if (target.healthFigth <= 0)
            {
                target.healthFigth = 0;
                gameManager.CheckIfAnyAlive(oppositeList);
                target = ChooseTarget(oppositeList);
            }
        }
    }

    protected virtual float CalculateDamage()
    {
        attackTime = 1 / speedAttack;
        float randomChance = Random.Range(0f, 100f);
        if (randomChance < evasion)
        {
            Debug.Log("¡El objetivo esquivó el ataque!");
            return 0; // No hay daño si esquiva
        }
        float damageDone;
        if (damageType == typeDamage.Physical)
        {
            damageDone = physicalDamage - target.defense;
        }
        else
        {
            damageDone = magicalDamage - target.magicalDefense;
        }

        if (damageDone < 1)
        {
            return 1;
        }
        return damageDone;
    }

    private void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction = new Vector3(direction.x, 0, direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void AttackScreenInfo(float damage, GameObject monsterDamaged)
    {
        if (monsterDamaged != null)
        {
            GameObject textInstanced = Instantiate(gameManager.textInfoPrefab, monsterDamaged.transform.position + Vector3.up * 2, Quaternion.identity, gameManager.canvasWorld.transform);
            TextMeshProUGUI textComponent = textInstanced.GetComponent<TextMeshProUGUI>();
            if (damage == 0)
            {
                textComponent.text = "Evade";
            }
            else
            {
                textComponent.text = $"-{damage}";
            }
            Destroy(textInstanced, 1.2f);
        }      
    }

    public void UpdateStats()
    {
        monsterClass = new MonsterClass(monsterSO, monsterData.level);
        if (!enemie)
        {
            level = monsterData.level;
            exp = monsterData.currentXP;
        }

        health = monsterClass.Health + healthBuff;
        if (PlayerPrefs.GetInt("BattleNumber") == 0)
        {
            healthFigth = monsterClass.Health + healthBuff;
            Debug.Log("Entra en battleNumber 0");
        }
        else if (!enemie)
        {
            healthFigth = monsterData.currentHealth;
            Debug.Log("Entra en con la vida previa");
        }
        else
        {
            healthFigth = health;
        }
        physicalDamage = monsterClass.PhysicalDamage + damageBuff;
        speedAttack = monsterClass.SpeedAttack * multiplierIncreasedSpeedAttack;
        defense = monsterClass.Defense + physicIncreaseDefense;
        evasion = monsterClass.Evasion;
        magicalDamage = monsterClass.MagicalDamage + magicDamageBuff;
        magicalDefense = monsterClass.MagicalDefense + magicIncreaseDefense;
        shield = shieldIncrease;
        if (shield > 0)
        {
            shieldActivated = true;
            shieldBar.UpdateShield(this);
        }
        if (monsterData.isStarter)
        {

            lifeBar.UpdateFill(this);
        }
    }

    public void ChangeSelector()
    {
        if (!enemie)
        {
            gameManager.ChangeSelector(this.gameObject);
        }
    }

    public void ChangeToTarget()
    {
        if (enemie && gameManager.monsterSelected != null)
        {
            Monster friendMonster = gameManager.monsterSelected.GetComponent<Monster>();
            friendMonster.target = friendMonster.ChooseTarget(gameObject);
        }
    }

    public void ShowAreaDistanceAttack()
    {
        GameManager gameManagerNow = FindObjectOfType<GameManager>();
        float y = gameManagerNow.damageArea.transform.position.y;
        if (areaAttack != null)
        {
            DestroyImmediate(areaAttack);
        }
        Vector3 spawnArea = new Vector3(transform.position.x, y, transform.position.z);
        areaAttack = Instantiate(gameManagerNow.damageArea, spawnArea, Quaternion.identity);
        areaAttack.transform.localScale = Vector3.one * distanceAttack * 0.25f;
    }
    public void CleanAreaDistanceAttack()
    {
        DestroyImmediate(areaAttack);
    }

    private void RegenerateHealth()
    {
        healthFigth += healthRegeneration;
        if (healthFigth > health)
        {
            healthFigth = health;
        }
        GameObject textInstanced = Instantiate(gameManager.textInfoPrefab, transform.position + Vector3.up * 2, Quaternion.identity, gameManager.canvasWorld.transform);
        TextMeshProUGUI textComponent = textInstanced.GetComponent<TextMeshProUGUI>();
        textComponent.text = $"+{healthRegeneration}";
        textComponent.color = Color.green;
        lifeBar.UpdateFill(this);
    }

    private void ReloadAttackToSkill()
    {
        attacksToSkill = Random.Range(rangeAttacksToSkill.x, rangeAttacksToSkill.y);
        attacksToSkill = Mathf.RoundToInt(attacksToSkill / decreasedCoolDown);
        totalAmountToSkill = 1f / (float)attacksToSkill;
    }

    public void UpdateBar()
    {
        if (shieldActivated)
        {
            shieldBar.UpdateShield(this);
        }
        else
        {
            lifeBar.UpdateFill(this);
        }

    }

    private void BasicAttackDamage()
    {
        float damage = CalculateDamage() + basicDamageBuff;
        target.TakeDamage((int)damage);
        if (summonType == Monster.typeSummon.Edge)
        {
            if (hitAcumulation < hitToEffect)
            {
                hitAcumulation++;
            }
            else
            {
                target.AddStatusEffect(monsterEffect);
                hitAcumulation = 0;
            }
        }
    }

    public void AddStatusEffect(StatusEffect effect)
    {
        effect.ApplyEffect(this);
        ApplyEffectStatus(effect);
    }

    public void ApplyStatus(Sprite sprite)
    {
        int i = 0;
        foreach (var image in spriteStates)
        {
            if (image == sprite)
            {
                intStates[i] += 1;
                lifeBar.ShowStates(intStates, spriteStates);
                return;
            }
            i++;
        }
        intStates.Add(1);
        spriteStates.Add(sprite);
        lifeBar.ShowStates(intStates, spriteStates);
    }

    public void ApplyEffectStatus(StatusEffect statusEffect)
    {
        int i = 0;
        foreach (var effect in activeEffects)
        {
            if (effect == statusEffect)
            {
                intStatesEffects[i]++;
                return;
            }
            i++;
        }
        activeEffects.Add(statusEffect);
        intStatesEffects.Add(1);
    }

    private void OnDisable()
    {
        if (objCircleAttacks != null)
        {
            objCircleAttacks.SetActive(false);
        }       
    }
    private void OnEnable()
    {
        if (objCircleAttacks != null)
        {
            objCircleAttacks.SetActive(true);
        }      
    }

    public void TakeDamage(int damage)
    {
        if (shieldActivated)
        {
            shield -= damage;
            if (damage > shield)
            {
                float restDamage = damage - shield;
                healthFigth -= restDamage;
            }
        }
        else
        {
            healthFigth -= damage;
        }
        AttackScreenInfo(damage, gameObject);
        UpdateBar();
    }

    public void TakeDamageHealth(int damage)
    {  
        healthFigth -= damage;
        AttackScreenInfo(damage, gameObject);
        lifeBar.UpdateFill(this);
    }

    public void DesactivateMonster()
    {
        gameObject.SetActive(false);
    }
    //Los monstruos tienen su skill (Scriptable Object?), sus atributos (variables)
    //La lógica de movimiento (en teoría en este script), hay momentos el que el monstruo no ataca pero va a estar en la escena
    //
}
