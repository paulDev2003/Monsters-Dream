using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
using TMPro;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class Monster : MonoBehaviour
{
    public string monsterName = "name";
    public MonsterSO monsterSO;
    public MonsterData monsterData;
    public bool enemie;
    #region Stats
    public int level = 1;
    public float health;
    public float physicalDamage;
    public float speedAttack;
    public float defense;
    public float evasion;
    public float magicalDamage;
    public float magicalDefense;
    public float shield;
    #endregion
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
    [HideInInspector] public NavMeshAgent agent;
    public Animator animator;
    public int valueI;
    public GameObject areaAttack;
    [HideInInspector]public Image circleAttacksToSkill;
    public Transform positionAttacksToSkill;
    public Collider targetCollider;
    private GameObject objCircleAttacks;
    public bool shieldActivated = false;

    [Space(10)]
    #region Buffs
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
    #endregion
    #region StatesAndEffects
    public List<Sprite> spriteStates = new List<Sprite>();
    public List<int> intStates = new List<int>();
    public List<int> intStatesEffects = new List<int>();
    public List<StatusEffect> activeEffects = new List<StatusEffect>();
    public StatusEffect monsterEffect;
    #endregion
    public bool specialAttack = false;
    [HideInInspector] public bool awaitToSpecialAttack = false;
    private int enemieRandomSpecialAttack;
    [HideInInspector] public int skillCount = 0;
    [HideInInspector] public bool notShowInterface = false;
    public bool underControl = false;

    #region StateMachineStates
    public MonsterStateMachine monsterStateMachine;
    public MonsterChaseState monsterChaseState;
    public MonsterBasicAttackState monsterBasicAttackState;
    public MonsterSpecialAttackState monsterSpecialAttackState;
    public MonsterDeadState monsterDeadState;
    #endregion
    #region StateMachineSO
    [SerializeField] private MonsterChaseSO monsterChaseSO;
    [SerializeField] private MonsterBasicAttackSO monsterBasicAttackSO;
    [SerializeField] private MonsterSpecialAttackSO monsterSpecialAttackSO;
    [SerializeField] private MonsterDeadSO monsterDeadSO;

    [HideInInspector] public MonsterChaseSO monsterChaseInstance;
    [HideInInspector] public MonsterBasicAttackSO monsterBasicAttackInstance;
    [HideInInspector] public MonsterSpecialAttackSO monsterSpecialAttackInstance;
    [HideInInspector] public MonsterDeadSO monsterDeadInstance;

    #endregion
    private void Awake()
    {
        monsterChaseInstance = Instantiate(monsterChaseSO);
        monsterBasicAttackInstance = Instantiate(monsterBasicAttackSO);
        monsterSpecialAttackInstance = Instantiate(monsterSpecialAttackSO);
        monsterDeadInstance = Instantiate(monsterDeadSO);

        monsterStateMachine = new MonsterStateMachine();

        monsterChaseState = new MonsterChaseState(this, monsterStateMachine);
        monsterBasicAttackState = new MonsterBasicAttackState(this, monsterStateMachine);
        monsterSpecialAttackState = new MonsterSpecialAttackState(this, monsterStateMachine);
        monsterDeadState = new MonsterDeadState(this, monsterStateMachine);
    }
    protected virtual void Start()
    {       
        SetupCore();
        InitializeStateMachines();
        SetupLists();
        SetupUI();
        SetupTarget();
        SetupExtras();       
    }

    protected virtual void SetupCore()
    {
        currentTimeRegeneration = regenerationTime;
        inUse = true;
        agent = GetComponent<NavMeshAgent>();
        maxExp = level * 50;
        agent.avoidancePriority = Random.Range(30, 60);
        gameManager = FindAnyObjectByType<GameManager>();
        Assert.IsNotNull(gameManager, "No encuentra gameManager");
    }

    protected virtual void InitializeStateMachines()
    {
        monsterChaseInstance.Initialize(gameObject, this);
        monsterBasicAttackInstance.Initialize(gameObject, this);
        monsterSpecialAttackInstance.Initialize(gameObject, this);
        monsterDeadInstance.Initialize(gameObject, this);
        monsterStateMachine.Initialize(monsterChaseState);
        
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
        }
    }

    protected virtual void SetupUI()
    {
        if (!notShowInterface)
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
    }

    protected virtual void SetupTarget()
    {
        if (!notShowInterface)
        {
            target = ChooseTarget(oppositeList);
        }      
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
        monsterStateMachine.currentState.FrameUpdate();
        if (positionAttacksToSkill == null && !notShowInterface)
        {
            objCircleAttacks.transform.position = transform.position + Vector3.up * 1.5f;
        }
        else if(!notShowInterface)
        {
            objCircleAttacks.transform.position = positionAttacksToSkill.position;
        }
        //Dejar o Sacar?
        if (gameManager.finish)
        {
            stopFigth = true;
        }
    }

    private void FixedUpdate()
    {
        monsterStateMachine.currentState.PhysicsUpdate();
    }

    public void UpdateStatsEffects()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].Tick(this, Time.deltaTime, intStatesEffects[i]);
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

    public void RegenerateUpdate()
    {
        if (currentTimeRegeneration > 0)
        {
            currentTimeRegeneration -= Time.deltaTime;
        }
        else if (healthRegeneration > 0)
        {
            RegenerateHealth(healthRegeneration);
            currentTimeRegeneration = regenerationTime;
        }
    }
    public void ShootSpecialAttack()
    {
        circleAttacksToSkill.fillAmount = 0;
        if (!enemie)
        {
            skillDrop.cooldownImage.fillAmount = 0;
        }
        skillCount++;
        monsterSO.skill.ShootSkill(this);
    }

    protected virtual float CalculateDamage()
    {
        
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

    public void RotateTowardsTarget()
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

    public void RegenerateHealth(int healthUp)
    {
        healthFigth += healthUp;
        if (healthFigth > health)
        {
            healthFigth = health;
        }
        GameObject textInstanced = Instantiate(gameManager.textInfoPrefab, transform.position + Vector3.up * 2, Quaternion.identity, gameManager.canvasWorld.transform);
        TextMeshProUGUI textComponent = textInstanced.GetComponent<TextMeshProUGUI>();
        textComponent.text = $"+{healthUp}";
        textComponent.color = Color.green;
        lifeBar.UpdateFill(this);
    }

    public void ReloadAttackToSkill(int attacksToSkill)
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

    public void BasicAttackDamage()
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
        if (lifeBar != null)
        {
            UpdateBar();
        }      
    }

    public void TakeDamageHealth(int damage)
    {  
        healthFigth -= damage;
        AttackScreenInfo(damage, gameObject);
        if (lifeBar!=null)
        {
            lifeBar.UpdateFill(this);
        }      
    }

    public void RunSkillCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    public void ReloadNotInstanceAttack()
    {
        circleAttacksToSkill.fillAmount = 0;
        skillDrop.cooldownImage.fillAmount = 0;
    }

    public void FillCircleSkillAttack()
    {
        if (!notShowInterface)
        {
            circleAttacksToSkill.fillAmount += totalAmountToSkill;
        }
        if (!enemie && !notShowInterface)
        {
            skillDrop.cooldownImage.fillAmount = circleAttacksToSkill.fillAmount;
        }
    }
}
