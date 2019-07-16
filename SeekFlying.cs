using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using MEC;
using DG.Tweening;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

//Requires DoTween(free) and More Effective Coroutines(free)

public class SeekFlying : Action
{
    [BehaviorDesigner.Runtime.Tasks.TooltipAttribute("If target is null target position will be used")]
    public SharedGameObject Target;
    public SharedVector3 TargetPosition;
    public LayerMask SurfaceLayer;
    public LayerMask CanSeeTargetLayers;
    public SharedString CanSeeTargetTag;
    //[Range(0.5f, 2f)]
    public SharedFloat MoveSpeed = 0.25f;
    //[Range(1, 30)]
    [BehaviorDesigner.Runtime.Tasks.TooltipAttribute("The distance the agent will detect surfaces in front of it")]
    public SharedFloat CastForwardDistance = 8f;
    //[Range(1, 30)]
    [BehaviorDesigner.Runtime.Tasks.TooltipAttribute("This number is used when the agent CAN SEE the target otherwise it will use this number * 0.25f")]
    public SharedFloat AboveFloorDistance = 15f;
    //[Range(1, 30)]
    public SharedFloat BelowCeilingDistance = 4f;
    [BehaviorDesigner.Runtime.Tasks.TooltipAttribute("If the agent cannot find the target by flying it will get on the Navmesh, be sure to use a big enough offset so your agent is still off the ground")]
    NavMeshAgent NavMeshAgent;
    //[Range(2, 10)]
    public SharedFloat AgentActiveTime = 4f;
    public SharedFloat AgentSpeed;
    public SharedFloat AgentAngularSpeed;

    //priv
    Vector3 XZMoveDirection;
    Vector3 YMoveDirection;
    int randomSideBoostCount;
    bool NavMeshAgentActive;
    Vector3 activeTarget;
    CoroutineHandle waiter;
    CoroutineHandle navDisabler;
    bool moveForwardToTarget;
    RaycastHit forwardHitto;
    Rigidbody _rigidbody;
    Transform _transform;
    Tween FlyToGround;

    RaycastHit[] downHitResults = new RaycastHit[1];
    RaycastHit[] upHitResults = new RaycastHit[1];
    RaycastHit[] castForwardResults = new RaycastHit[1];
    RaycastHit[] castRightResults = new RaycastHit[1];
    RaycastHit[] castLeftResults = new RaycastHit[1];
    RaycastHit[] surfaceBelowResults = new RaycastHit[1];
    RaycastHit[] canSeeResults = new RaycastHit[1];


    public bool DebuggingActive;

    public override void OnReset()
    {
        base.OnReset();
        MoveSpeed.Value = 0.25f;
        CastForwardDistance.Value = 8f;
        AboveFloorDistance.Value = 15f;
        BelowCeilingDistance.Value = 4;
        AgentActiveTime.Value = 4f;
        AgentSpeed.Value = 4f;
        AgentAngularSpeed.Value = 400f;
    }

    public override void OnStart()
    {
        if (NavMeshAgent == null)
        {
            NavMeshAgent = Owner.gameObject.GetComponent<NavMeshAgent>();
        }

        NavMeshAgent.speed = AgentSpeed.Value;
        NavMeshAgent.angularSpeed = AgentAngularSpeed.Value;

        _rigidbody = Owner.gameObject.GetComponent<Rigidbody>();
        _transform = Owner.transform;
        YMoveDirection = Vector3.zero;
        XZMoveDirection = Vector3.zero;
        moveForwardToTarget = true;
        randomSideBoostCount = 0;
        NavMeshAgent.enabled = false;
        NavMeshAgentActive = false;
        _rigidbody.isKinematic = false;

        if (Target != null)
        {
            TargetPosition = Target.Value.transform.position;
        }

        activeTarget = TargetPosition.Value;
    }

    public override TaskStatus OnUpdate()
    {
        if (NavMeshAgent.enabled == false && NavMeshAgentActive == false)
        {
            if (Target != null)
            {
                TargetPosition = Target.Value.transform.position;
            }

            activeTarget = TargetPosition.Value;
            CastForward();
            CastDownAndUp();
            SetDirectionToTarget();
            LookAtMoveDirection();
        } else
        {
            YMoveDirection = Vector3.zero;
            XZMoveDirection = Vector3.zero;
        }


        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        base.OnEnd();

        CleanUpWhenFinished();

    }

    public override void OnBehaviorComplete()
    {
        base.OnBehaviorComplete();
        CleanUpWhenFinished();
    }

    public override void OnConditionalAbort()
    {
        base.OnConditionalAbort();
        CleanUpWhenFinished();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (XZMoveDirection + YMoveDirection != Vector3.zero && NavMeshAgent.enabled == false && NavMeshAgentActive == false)
        {
            _rigidbody.AddForce((XZMoveDirection + YMoveDirection) * MoveSpeed.Value, ForceMode.Impulse);
        }

    }

    void LookAtMoveDirection()
    {
        // _transform.LookAt(XZMoveDirection);

    }

    void SetDirectionToTarget()
    {
        if (moveForwardToTarget)
        {
            Vector3 activeTargetNoY = new Vector3(activeTarget.x, 0f, activeTarget.z);
            XZMoveDirection = (activeTargetNoY - _transform.position).normalized;
            XZMoveDirection = new Vector3(XZMoveDirection.x, 0f, XZMoveDirection.z);
        }


    }

    void CastDownAndUp()
    {
        int downHit = 0;

        if (CanSeeTarget())
        {
            downHit = Physics.RaycastNonAlloc(_transform.position, -Vector3.up, downHitResults, AboveFloorDistance.Value, SurfaceLayer, QueryTriggerInteraction.Ignore);
        } else
        {
            downHit = Physics.RaycastNonAlloc(_transform.position, -Vector3.up, downHitResults, AboveFloorDistance.Value * 0.5f, SurfaceLayer, QueryTriggerInteraction.Ignore);
        }

        var upHit = Physics.RaycastNonAlloc(_transform.position, Vector3.up, upHitResults, BelowCeilingDistance.Value, SurfaceLayer, QueryTriggerInteraction.Ignore);



        if (downHit > 0)
        {
            if (DebuggingActive)
            {
                Debug.DrawRay(_transform.position, -Vector3.up * AboveFloorDistance.Value, Color.red, 0.1f);
            }

            YMoveDirection.y = 1;
        } else if (upHit > 0)
        {
            if (DebuggingActive)
            {

                Debug.DrawRay(_transform.position, Vector3.up * BelowCeilingDistance.Value, Color.red, 0.1f);
            }
            YMoveDirection.y = -0.5f;
        } else
        {
            if (DebuggingActive)
            {

                Debug.DrawRay(_transform.position, -Vector3.up * AboveFloorDistance.Value, Color.yellow, 0.1f);
                Debug.DrawRay(_transform.position, Vector3.up * BelowCeilingDistance.Value, Color.green, 0.1f);
            }
            YMoveDirection.y = 0f;
        }

        YMoveDirection = YMoveDirection.normalized;
    }

    void CastForward()
    {
        Vector3 cleanedMoveDir = new Vector3(XZMoveDirection.x, 0f, XZMoveDirection.z);

        var forwardHit = Physics.RaycastNonAlloc(_transform.position, cleanedMoveDir, castForwardResults, CastForwardDistance.Value, SurfaceLayer, QueryTriggerInteraction.Ignore);
        forwardHitto = castForwardResults[0];

        if (forwardHit > 0)
        {
            if (DebuggingActive)
            {

                Debug.DrawRay(_transform.position, cleanedMoveDir * CastForwardDistance.Value, Color.red, 0.1f);
            }
            XZMoveDirection = Vector3.zero;
            moveForwardToTarget = false;

            RandomSideBoost();

        } else
        {
            if (DebuggingActive)
            {

                Debug.DrawRay(_transform.position, cleanedMoveDir * CastForwardDistance.Value, Color.blue, 0.1f);
            }
        }
    }

    bool CastRight()
    {
        if (DebuggingActive)
        {

            //Debug.Log("Cast right");
            Debug.DrawRay(_transform.position, _transform.InverseTransformDirection(Vector3.Cross(forwardHitto.normal, Vector3.up)) * (CastForwardDistance.Value * 0.5f), Color.cyan, 3f);
        }
        var hit = Physics.RaycastNonAlloc(_transform.position, _transform.InverseTransformDirection(Vector3.Cross(forwardHitto.normal, Vector3.up)), castRightResults, (CastForwardDistance.Value * 0.5f), SurfaceLayer, QueryTriggerInteraction.Ignore);

        if (hit > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    bool CastLeft()
    {
        if (DebuggingActive)
        {

            // Debug.Log("Cast left");
            Debug.DrawRay(_transform.position, _transform.InverseTransformDirection(Vector3.Cross(forwardHitto.normal, Vector3.down)) * (CastForwardDistance.Value * 0.5f), Color.magenta, 3f);
        }
        var hit = Physics.RaycastNonAlloc(_transform.position, _transform.InverseTransformDirection(Vector3.Cross(forwardHitto.normal, Vector3.down)), castLeftResults, (CastForwardDistance.Value * 0.5f), SurfaceLayer, QueryTriggerInteraction.Ignore);

        if (hit > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    void RandomSideBoost()
    {
        randomSideBoostCount += 1;
        bool useRandomDirection = true;


        var rando = Random.Range(0f, 1f);
        float weight = 0;

        if (CastRight())
        {
            if (DebuggingActive)
            {

                Debug.Log("--------HIT RIGHT");
            }
            weight = 0.5f;
        } else if (CastLeft())
        {
            if (DebuggingActive)
            {

                Debug.Log("--------HIT LEFT");
            }
            weight = -0.5f;
        } else
        {
            if (DebuggingActive)
            {

                Debug.Log("--------NO HITS");
            }
            useRandomDirection = false;
        }

        if (useRandomDirection)
        {
            rando += weight;
            rando = Mathf.Clamp(rando, 0, 1);

            if (rando <= 0.5f)
            {
                _rigidbody.AddForce(_transform.right * (MoveSpeed.Value * Random.Range(15f, 25f)), ForceMode.VelocityChange);
            } else
            {
                _rigidbody.AddForce(-_transform.right * (MoveSpeed.Value * Random.Range(15, 25f)), ForceMode.VelocityChange);
            }


        } else
        {
            var directionToTargetNoY = GetDirectionNoY(TargetPosition.Value, _transform.position);

            var dottoRight = Vector3.Dot(directionToTargetNoY, _transform.right);
            var dottoLeft = Vector3.Dot(directionToTargetNoY, -_transform.right);

            if (dottoRight > dottoLeft)
            {
                _rigidbody.AddForce(_transform.right * (MoveSpeed.Value * Random.Range(15, 25f)), ForceMode.VelocityChange);
            } else
            {
                _rigidbody.AddForce(-_transform.right * (MoveSpeed.Value * Random.Range(15f, 25f)), ForceMode.VelocityChange);
            }



        }

        if (waiter != null)
        {
            if (waiter.IsRunning != true)
            {
                waiter = Timing.RunCoroutine(_WaitThenContinue().CancelWith(gameObject));
            }
        } else
        {
            waiter = Timing.RunCoroutine(_WaitThenContinue().CancelWith(gameObject));
        }


        if (randomSideBoostCount >= 5)
        {
            if (DebuggingActive)
            {

                Debug.Log("CHECK IF CANT SEE TARGET");
            }

            randomSideBoostCount = 0;

            if (CanSeeTarget())
            {
                return;
            } else
            {
                if (DebuggingActive)
                {
                    Debug.Log("CANT SEE TARGET");
                }
                //run agent
                StartAgentProcess();
            }
        }

    }

    void StartAgentProcess()
    {
        var hitto = GetSurfaceBelowAgent();

        if (hitto.collider != null)
        {
            NavMeshHit hit;

            if (NavMesh.FindClosestEdge(hitto.point, out hit, NavMesh.AllAreas))
            {
                var moveToPoint = hit.position;
                moveToPoint.y += NavMeshAgent.baseOffset;

                NavMeshAgentActive = true;
                _rigidbody.isKinematic = true;
                FlyToGround = _transform.DOMove(moveToPoint, 2f).OnComplete(FinishedMoveToNavMeshPoint);
            }
        }
    }

    void FinishedMoveToNavMeshPoint()
    {
        if (DebuggingActive)
        {

            Debug.Log("FinishedMoveToNavMeshPoint");
        }
        NavMeshAgent.enabled = true;
        NavMeshAgent.SetDestination(TargetPosition.Value);
        navDisabler = Timing.RunCoroutine(_WaitThenDisableNavMesh().CancelWith(gameObject));
        // samplePath
    }

    RaycastHit GetSurfaceBelowAgent()
    {
        RaycastHit hitto;
        var hit = Physics.RaycastNonAlloc(_transform.position, Vector3.down, surfaceBelowResults, AboveFloorDistance.Value * 3f, SurfaceLayer, QueryTriggerInteraction.Ignore);
        hitto = surfaceBelowResults[0];

        if (hit > 0)
        {
            var surface = hitto.collider.GetComponent<NavMeshSurface>();

            if (surface != null)
            {
                return hitto;
            } else
            {
                return hitto;
            }
        } else
        {
            return hitto;
        }
    }

    IEnumerator<float> _WaitThenDisableNavMesh()
    {
        yield return Timing.WaitForSeconds(AgentActiveTime.Value);
        NavMeshAgent.enabled = false;
        _rigidbody.isKinematic = false;
        NavMeshAgentActive = false;
        yield break;
    }

    IEnumerator<float> _WaitThenContinue()
    {
        yield return Timing.WaitForSeconds(3f);
        moveForwardToTarget = true;
        yield break;
    }

    bool CanSeeTarget()
    {
        RaycastHit hitto;

        if (DebuggingActive)
        {
            Debug.DrawRay(_transform.position, GetDirection(_transform.position, TargetPosition.Value) * Mathf.Infinity, Color.green, 3f);
        }
        var hit = Physics.RaycastNonAlloc(_transform.position, GetDirection(_transform.position, TargetPosition.Value), canSeeResults, Mathf.Infinity, CanSeeTargetLayers, QueryTriggerInteraction.Ignore);
        hitto = canSeeResults[0];

        if (hit > 0)
        {
            if (hitto.collider.CompareTag(CanSeeTargetTag.Value))
            {
                return true;
            } else
            {
                return false;
            }
        } else
        {
            return false;
        }
    }

    void CleanUpWhenFinished()
    {
        _rigidbody.isKinematic = true;
        NavMeshAgent.enabled = false;

        if (FlyToGround != null)
        {
            FlyToGround.Kill();
        }

        if (waiter != null)
        {
            waiter.IsRunning = false;
        }

        if (navDisabler != null)
        {
            navDisabler.IsRunning = false;
        }
    }

    public  Vector3 GetDirectionNoY(Vector3 tr0, Vector3 tr1)
    {
        Vector3 rawDir = (tr0 - tr1).normalized;

        Vector3 dir = new Vector3(rawDir.x, 0f, rawDir.z);

        return dir;
    }

    public Vector3 GetDirection(Vector3 tr0, Vector3 tr1)
    {
        return (tr1 - tr0).normalized;
    }

}
