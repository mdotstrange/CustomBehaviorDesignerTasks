using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
using BehaviorDesigner.Runtime.Tasks.Movement;
using System.Collections.Generic;
using System.Collections;

//Task by M dot Strange
public class generateWaypoints : Action
{
    public string waypointTargetTag;
    public SharedInt desiredPointCount;
    public SharedFloat minDistance;
    public SharedFloat maxDistance;
    public SharedBool useAgentYPosition;
    public SharedGameObjectList wayPoints;
    public SharedBool useNavMeshEdges;

    GameObject objToSpawn;
    GameObject wpParent;
    int retries;
    int remainderWaypointCount;
    List<Collider> colliderList = new List<Collider>();
    int mainIndex;
    NavMeshHit hitto;
    NavMeshHit hitto1;
    bool goodPointFound;

    public override TaskStatus OnUpdate()
    {
        GetInterestingObjects();

        if (AllPointsDone() == true)
        {
            ParentWayPoints();
            return TaskStatus.Success;
        } else
        {
            remainderWaypointCount = GetRemainderPointCount();

            for (mainIndex = 0; mainIndex < remainderWaypointCount; mainIndex++)
            {
                var posTotest = MakeRandoSpherePos();
                while (SamplePosition(posTotest) != true)
                {
                    posTotest = MakeRandoSpherePos();
                }
            }
            if (useAgentYPosition.Value == true)
            {
                LevelWayPointYPosition();
            }

            ParentWayPoints();
            return TaskStatus.Success;
        }
    }

    public void GetInterestingObjects()
    {
        if (waypointTargetTag != null && waypointTargetTag != "")
        {
            var colliderArray = Physics.OverlapSphere(transform.position, maxDistance.Value);
            if (colliderArray.Length != 0)
            {
                colliderList = new List<Collider>(colliderArray);

                for (int index = 0; index < colliderList.Count; index++)
                {
                    var i = colliderList[index];

                    if (i.gameObject.CompareTag(waypointTargetTag) && wayPoints.Value.Contains(i.gameObject) != true)
                    {
                        var waypoint = MakeWayPoint(i.transform.position);
                        AddWayPointToList(waypoint);
                    }
                }

            }
        }
    }

    public bool AllPointsDone()
    {
        if (wayPoints.Value.Count == desiredPointCount.Value)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public int GetRemainderPointCount()
    {
        return desiredPointCount.Value - wayPoints.Value.Count;
    }

    public void LevelWayPointYPosition()
    {
        for (int index1 = 0; index1 < wayPoints.Value.Count; index1++)
        {
            var i = wayPoints.Value[index1];

            if (i.transform.position.y != transform.position.y)
            {
                i.transform.position = new Vector3(i.transform.position.x, transform.position.y, i.transform.position.z);
            }

        }
    }

    public void AddWayPointToList(GameObject obj)
    {
        wayPoints.Value.Add(obj);
    }

    public bool IsPositionTooClose(Vector3 pos)
    {
        if (wayPoints.Value.Count == 0)
        {
            return false;
        } else
        {
            for (int index = 0; index < wayPoints.Value.Count; index++)
            {
                var i = wayPoints.Value[index].transform.position;
                Vector3 offset = i - pos;
                float sqrLen = offset.sqrMagnitude;


                if (sqrLen < (maxDistance.Value * 0.5f) * (maxDistance.Value * 0.5f))
                {
                    return true;
                }

            }

            return false;
        }


    }

    public bool SamplePosition(Vector3 pos)
    {

        retries = 0;
        var posTotest = pos;

        if (useNavMeshEdges.Value == false)
        {
            while (NavMesh.SamplePosition(posTotest, out hitto, maxDistance.Value, NavMesh.AllAreas) != true || IsPositionTooClose(hitto.position) == true || retries < 50)
            {
                retries++;
                posTotest = MakeRandoSpherePos();
            }

            var waypoint = MakeWayPoint(hitto.position);
            AddWayPointToList(waypoint);

            if (retries < 50)
            {
                return false;
            } else
            {
                return true;
            }
        } else
        {
            goodPointFound = false;

            while (retries < 100 && goodPointFound == false)
            {
                retries++;
                posTotest = MakeRandoSpherePos();
                var sampleHit = NavMesh.SamplePosition(posTotest, out hitto, maxDistance.Value, NavMesh.AllAreas);
                var edgeHit = NavMesh.FindClosestEdge(posTotest, out hitto1, NavMesh.AllAreas);

                if (sampleHit == true)
                {
                    if (IsPositionTooClose(hitto.position) == false)
                    {
                        var waypoint = MakeWayPoint(hitto.position);
                        AddWayPointToList(waypoint);
                        goodPointFound = true;
                        return true;
                    }

                } else if (edgeHit == true)
                {
                    if (IsPositionTooClose(hitto1.position) == false)
                    {
                        var waypoint = MakeWayPoint(hitto1.position);
                        AddWayPointToList(waypoint);
                        goodPointFound = true;
                        return true;
                    }

                }

            }
            Debug.Log("FINISHED");


            if (retries < 100)
            {
                return false;
            } else
            {
                return true;
            }
        }



    }

    public Vector3 MakeRandoSpherePos()
    {
        if (useAgentYPosition.Value == false)
        {

            var direction = transform.forward;
            var destination = transform.position;
            var randoSphere = Random.insideUnitSphere * maxDistance.Value;
            randoSphere = new Vector3(randoSphere.x, randoSphere.y, randoSphere.z);
            direction = direction + randoSphere * maxDistance.Value;
            destination = transform.position + direction.normalized * Random.Range(minDistance.Value, maxDistance.Value);
            return new Vector3(destination.x, randoSphere.y, destination.z);
        } else
        {
            var agentYPosition = transform.position.y;
            var direction = transform.forward;
            var destination = transform.position;
            var randoSphere = Random.insideUnitSphere * maxDistance.Value;
            randoSphere = new Vector3(randoSphere.x, agentYPosition, randoSphere.z);
            direction = direction + randoSphere * maxDistance.Value;
            destination = transform.position + direction.normalized * Random.Range(minDistance.Value, maxDistance.Value);
            return new Vector3(destination.x, agentYPosition, destination.z);
        }

    }

    public GameObject MakeWayPoint(Vector3 pos)
    {
        objToSpawn = new GameObject();
        objToSpawn.name = "WayPoint " + mainIndex.ToString();
        objToSpawn.transform.position = pos;    
        return objToSpawn;

    }

    public void ParentWayPoints()
    {
        wpParent = new GameObject();
        wpParent.name = gameObject.name + "_Waypoints";
        wpParent.transform.position = transform.position;

        for (int index = 0; index < wayPoints.Value.Count; index++)
        {
            var i = wayPoints.Value[index].transform;

            i.SetParent(wpParent.transform);
        }
    }
}