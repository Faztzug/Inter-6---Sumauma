using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))] 
public class EnemyIA : MonoBehaviour
{
    [SerializeField] [Range(0,10)] protected float[] updateRateRNG = new float[2];
    [HideInInspector] public Transform player;
    protected Vector3 pos;
    protected Vector3 playerPos;
    protected NavMeshAgent agent;
    protected Rigidbody rgbd;
    [SerializeField] protected float findPlayerDistance = 100f;
    [SerializeField] protected float minPlayerDistance = 10f;
    [Range(0,1)] protected float updateRate;
    protected float distance;
    [HideInInspector] public GameState state;
    //protected Animator anim;
    [HideInInspector] public bool alive = true;
    [SerializeField] protected int damageStunAsync = 3;
    protected int stunTimerAsync;
    protected float outlineMaxThickness;

    protected virtual void Start() 
    {
        if(updateRate == 0) updateRate = Random.Range(updateRateRNG[0], updateRateRNG[1]);
        state = GameState.GameStateInstance;
        player = GameState.PlayerTransform;
        agent = GetComponent<NavMeshAgent>();
        rgbd = GetComponent<Rigidbody>();
        rgbd.maxAngularVelocity = 0;
        distance = Mathf.Infinity;
        //anim = GetComponent<Animator>();
        StartCoroutine(CourotineAsyncUpdateIA());
    }

    protected virtual void Update() 
    {
        
    }

    protected IEnumerator CourotineAsyncUpdateIA()
    {
        yield return new WaitForSeconds(updateRate);

        rgbd.velocity = Vector3.zero;

        if(stunTimerAsync >= 0)
        {
            stunTimerAsync--;
        }
        else if(!GameState.onCutscene) AsyncUpdateIA();
        
        updateRate = Random.Range(updateRateRNG[0], updateRateRNG[1]);

        StartCoroutine(CourotineAsyncUpdateIA());
    }

    protected virtual void AsyncUpdateIA()
    {
        
    }

    public virtual void ForceUpdateIA()
    {
        updateRate = 0.020f;
    }
    
    protected bool IsPlayerAlive()
    {
        if(player != null && player.gameObject.activeSelf && state.isPlayerDead == false) return true;
        else return false;
    }
    public virtual void EnemyDeath()
    {
        //anim.SetTrigger("Die");

        if(agent.isOnNavMesh) agent.SetDestination(transform.position);

        alive = false;

        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        foreach (var script in GetComponentsInChildren<MonoBehaviour>())
        {
            if(script == this) continue;
            script.enabled = false;
        }
        this.StopAllCoroutines();
        Destroy(this.gameObject);
    }
    public virtual void Stun()
    {
        stunTimerAsync = damageStunAsync;
    }
    public virtual void UpdateHealth(float health, float maxHealth)
    {

    }

    protected virtual void OnValidate() { }
}
