using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;

public class CharacterWaypointsHandler : MonoBehaviour
{

    private const float speed = 25f;

    [SerializeField] private List<Vector3> waypointList;
    [SerializeField] private List<float> waitTimeList;
    private int waypointIndex;

    [SerializeField] private string idleAnimation = "dZombie_Idle";
    [SerializeField] private string walkAnimation = "dZombie_Walk";

    [SerializeField] private float idleFrameRate = 1f;
    [SerializeField] private float walkFrameRate = 1f;
    [SerializeField] private Vector3 aimDirection;

    [SerializeField] private Player player;
    [SerializeField] private Transform pfFieldOfView;
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 50f;

    [SerializeField] private AudioSource audioSource; // 音效播放器
    [SerializeField] private AudioClip killSound; // 杀死主角的音效

    private FieldOfView fieldOfView;

    private V_UnitSkeleton unitSkeleton;
    private V_UnitAnimation unitAnimation;
    private AnimatedWalker animatedWalker;

    private enum State
    {
        Waiting,
        Moving,
        AttackingPlayer,
        Busy,
    }

    private State state;
    private float waitTimer;
    private UnitAnimType attackUnitAnim;
    private Vector3 lastMoveDir;

    public Animator spriteAnim;

    private void Start()
    {
        Transform bodyTransform = transform.Find("Body");
        unitSkeleton = new V_UnitSkeleton(1f, bodyTransform.TransformPoint, (Mesh mesh) => bodyTransform.GetComponent<MeshFilter>().mesh = mesh);
        unitAnimation = new V_UnitAnimation(unitSkeleton);
        animatedWalker = new AnimatedWalker(unitAnimation, UnitAnimType.GetUnitAnimType(idleAnimation), UnitAnimType.GetUnitAnimType(walkAnimation), idleFrameRate, walkFrameRate);
        state = State.Waiting;
        waitTimer = waitTimeList[0];
        lastMoveDir = aimDirection;
        attackUnitAnim = UnitAnimType.GetUnitAnimType("dMarine_Attack");

        fieldOfView = Instantiate(pfFieldOfView, null).GetComponent<FieldOfView>();
        fieldOfView.SetFoV(fov);
        fieldOfView.SetViewDistance(viewDistance);
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.Waiting:
            case State.Moving:
                HandleMovement();
                FindTargetPlayer();
                break;
            case State.AttackingPlayer:
                AttackPlayer();
                break;
            case State.Busy:
                break;
        }
        unitSkeleton.Update(Time.deltaTime);

        if (fieldOfView != null)
        {
            fieldOfView.SetOrigin(transform.position);
            fieldOfView.SetAimDirection(GetAimDir());
        }

        Debug.DrawLine(transform.position, transform.position + GetAimDir() * 10f);
    }

    private void FindTargetPlayer()
    {
        if (Vector3.Distance(GetPosition(), player.GetPosition()) < viewDistance)
        {
            Vector3 dirToPlayer = (player.GetPosition() - GetPosition()).normalized;
            if (Vector3.Angle(GetAimDir(), dirToPlayer) < fov / 2f)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(GetPosition(), dirToPlayer, viewDistance);
                if (raycastHit2D.collider != null)
                {
                    if (raycastHit2D.collider.gameObject.GetComponent<Player>() != null)
                    {
                        StartAttackingPlayer();
                    }
                }
            }
        }
    }

    public void StartAttackingPlayer()
    {
        AttackPlayer();
    }

    private void AttackPlayer()
    {
        state = State.Busy;

        Vector3 targetPosition = player.GetPosition();
        Vector3 dirToTarget = (targetPosition - GetPosition()).normalized;
        lastMoveDir = dirToTarget;

        unitAnimation.PlayAnimForced(attackUnitAnim, dirToTarget, 2f, (UnitAnim unitAnim) => {
            if (player.IsDead())
            {
                state = State.Moving;
            }
            else
            {
                state = State.AttackingPlayer;
            }
        }, (string trigger) => {
            // 造成伤害
            player.Damage(this);

            if (player.IsDead())
            {
                // 播放杀死主角的音效
                PlayKillSound();
            }
        }, null);

        Vector3 gunEndPointPosition = unitSkeleton.GetBodyPartPosition("MuzzleFlash");
        Shoot_Flash.AddFlash(gunEndPointPosition);
        WeaponTracer.Create(gunEndPointPosition, player.GetPosition());
    }

    private void PlayKillSound()
    {
        if (audioSource != null && killSound != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = killSound; // 设置音效
                audioSource.Play(); // 播放音效
            }
        }
        else
        {
            Debug.LogWarning("AudioSource or KillSound is not assigned!");
        }
    }

    private void HandleMovement()
    {
        switch (state)
        {
            case State.Waiting:
                spriteAnim.SetTrigger("Idle");
                waitTimer -= Time.deltaTime;
                animatedWalker.SetMoveVector(Vector3.zero);
                if (waitTimer <= 0f)
                {
                    state = State.Moving;
                }
                break;
            case State.Moving:
                Vector3 waypoint = waypointList[waypointIndex];

                Vector3 waypointDir = (waypoint - transform.position).normalized;
                lastMoveDir = waypointDir;

                float distanceBefore = Vector3.Distance(transform.position, waypoint);
                spriteAnim.SetTrigger("Walk");
                transform.position = transform.position + waypointDir * speed * Time.deltaTime;
                float distanceAfter = Vector3.Distance(transform.position, waypoint);

                float arriveDistance = .1f;
                if (distanceAfter < arriveDistance || distanceBefore <= distanceAfter)
                {
                    waitTimer = waitTimeList[waypointIndex];
                    waypointIndex = (waypointIndex + 1) % waypointList.Count;
                    state = State.Waiting;
                }
                break;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetAimDir()
    {
        return lastMoveDir;
    }

}

