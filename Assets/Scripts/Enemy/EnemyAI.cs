using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.PATROL;

    private Transform playerTr;

    public float attackDist = 5.0f;

    public float traceDist = 10.0f;

    //[HideInInspector]
    public bool isDie
    {
        get { return _isDie; }
        set { _isDie = value; }
    }
    private bool _isDie = false;

    private WaitForSeconds ws;
    private MoveAgent moveAgent;
    private Animator animator;
    private EnemyFire enemyFire;
    private EnemyFOV enemyFOV;

    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIndex = Animator.StringToHash("DieIndex");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");

    private void Awake()
    {
        moveAgent = GetComponent<MoveAgent>();
        animator = GetComponent<Animator>();
        enemyFire = GetComponent<EnemyFire>();
        enemyFOV = GetComponent<EnemyFOV>();

        animator.SetFloat(hashOffset, Random.Range(0, 1.0f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1.0f, 1.2f));
    }

    private void OnEnable()
    {
        isDie = false;
        animator.CrossFade("Idle", 0.1f);

        state = State.PATROL;
        GetComponent<CapsuleCollider>().enabled = true;

        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTr = player.transform;

        ws = new WaitForSeconds(0.3f);        

        Damage.OnPlayerDie += OnPlayerDie;
    }

    private IEnumerator CheckState()
    {
        while (!_isDie)
        {
            if(state == State.DIE)
                yield break;

            if (playerTr == null)
            {
                yield return null;
                continue;
            }

            float dist = Vector3.Distance(playerTr.position, transform.position);

            if (dist <= attackDist)
            {
                if (enemyFOV.IsViewPlayer())
                    state = State.ATTACK;
                else
                    state = State.TRACE;

            }else if (enemyFOV.IsTracePlayer())
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }

            yield return ws;
        }
    }

    private IEnumerator Action()
    {
        while (!_isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.PATROL:
                    moveAgent.patrolling = true;
                    //animator.SetBool("IsMove", true);
                    animator.SetBool(hashMove, true);
                    enemyFire.isFire = false;
                    break;
                case State.TRACE:
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    enemyFire.isFire = false;
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    if(!enemyFire.isFire)
                        enemyFire.isFire = true;
                    break;
                case State.DIE:
                    isDie = true;
                    enemyFire.isFire = false;
                    moveAgent.Stop();

                    animator.SetInteger(hashDieIndex, Random.Range(0, 3));
                    animator.SetTrigger(hashDie);

                    GetComponent<CapsuleCollider>().enabled = false;

                    StartCoroutine(Die());
                    break;
            }
        }
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(3.0f);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }

    public void OnPlayerDie()
    {
        moveAgent.Stop();
        enemyFire.isFire = false;
        StopAllCoroutines();

        animator.SetTrigger(hashPlayerDie);
    }
}
