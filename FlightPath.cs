using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;

public class FlightPath : Action
{
    public SharedGameObject flyer;
    public SharedVector3List castDirections;
    public SharedFloat rayLength;
    public SharedFloat forceMultiplier;
    public SharedFloat lookSpeed;
    public SharedGameObject destination;
    public SharedVector3 destinationV3;
    public SharedFloat arrivedDistance;
    public SharedFloat desiredGroundDist;
    public ForceMode forceMode;
    public LayerMask layerMask;
    public SharedInt retries;
    Transform flyerTrans;
    Vector3 targetDirection;
    Rigidbody rb;
    SharedVector3List tempList;
    SharedVector3List newList;
    SharedVector3List regList;
    SharedVector3List shortList;
    private bool arrived;
    private float YAdd;
    int loops;

    private Vector3 direction;

    public override void OnStart()
    {
        arrived = false;
        rb = flyer.Value.gameObject.GetComponent<Rigidbody>();
        flyerTrans = flyer.Value.gameObject.transform;
        loops = 0;
    }

    public override TaskStatus OnUpdate()
    {

        if(destination.Value != null)
        {
            destinationV3.Value = destination.Value.gameObject.transform.position;
        }
     


        newList = castDirections.Value;
        tempList = new List<Vector3>();
        regList = new List<Vector3>();
        shortList = new List<Vector3>();

        DoTheLook();

        targetDirection = destinationV3.Value - flyerTrans.position;

        var closestDist = Mathf.Infinity;
        Vector3 closestObj = Vector3.zero;

        for (int index = 0; index < castDirections.Value.Count; index++)
        {
            var i = castDirections.Value[index];

            if (Physics.Raycast(flyer.Value.gameObject.transform.position, i, rayLength.Value * 2, layerMask, QueryTriggerInteraction.Ignore) != true)
            {
                var dist = Vector3.Angle(i, targetDirection);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    // i is the closest cast
                    tempList.Value.Add(i);
                }


            }
        }

        if (tempList.Value.Count != 0)
        {

            closestObj = tempList.Value[0];

        } else
        {
            for (int index = 0; index < newList.Value.Count; index++)
            {
                var i = newList.Value[index];

                if (Physics.Raycast(flyer.Value.gameObject.transform.position, i, rayLength.Value, layerMask, QueryTriggerInteraction.Ignore) != true)
                {
                    var dist = Vector3.Angle(i, targetDirection);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        // i is the closest cast
                        regList.Value.Add(i);


                    }

                }

            }


        }
        if (regList.Value.Count != 0)
        {
            closestObj = regList.Value[0];
        } else
        {
            for (int index = 0; index < castDirections.Value.Count; index++)
            {
                var i = castDirections.Value[index];

                if (Physics.Raycast(flyer.Value.gameObject.transform.position, i, rayLength.Value / 2, layerMask, QueryTriggerInteraction.Ignore) != true)
                {
                    var dist = Vector3.Angle(i, targetDirection);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        // i is the closest cast
                        shortList.Value.Add(i);
                    }
                }
            }

        }
        if (shortList.Value.Count != 0)
        {
            closestObj = shortList.Value[0];
        } else
        {
            loops++;
        }





        RaycastHit hit;
        bool castHit = Physics.Raycast(flyer.Value.gameObject.transform.position, Vector3.down, out hit, desiredGroundDist.Value, layerMask, QueryTriggerInteraction.Ignore);


        if (castHit)
        {
            //Debug.Log("HIT FLOOR");
            float floorDistance = hit.distance;
            YAdd = desiredGroundDist.Value - floorDistance;

            if (Physics.Raycast(flyer.Value.gameObject.transform.position, Vector3.up, out hit, rayLength.Value / 3, layerMask, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("HIT Celing");
                YAdd = 0;
            }
        } else
        {
            YAdd = 0;
        }




        rb.AddRelativeForce(new Vector3(closestObj.x, closestObj.y + YAdd, closestObj.z) * forceMultiplier.Value * Time.deltaTime, forceMode);

        float distance = (Vector3.Distance(flyer.Value.gameObject.transform.position, destinationV3.Value));

        //Debug.Log("Is this " + distance + desiredGroundDist.Value + " less than this " + arrivedDistance.Value);


        if (distance - desiredGroundDist.Value <= arrivedDistance.Value)
        {

            //rb.Sleep();
            arrived = true;

        } else
        {
            arrived = false;
        }




        if (arrived == true)
        {
            return TaskStatus.Success;
        }

        if (loops == retries.Value)
        {

            rb.AddRelativeForce(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));
        }



        return TaskStatus.Running;
    }

    public override void OnReset()
    {
        targetDirection = Vector3.zero;
        arrived = false;
    }

    public void DoTheLook()
    {



        direction = Vector3.Normalize(destinationV3.Value - flyer.Value.gameObject.transform.position + Vector3.up);
        float step = lookSpeed.Value * Time.deltaTime;

        Vector3 rotateDir = Vector3.RotateTowards(flyer.Value.gameObject.transform.forward, direction, step, 0.0f);
        flyer.Value.gameObject.transform.rotation = Quaternion.LookRotation(rotateDir);
        direction = Vector3.Normalize(destinationV3.Value - flyer.Value.gameObject.transform.position);

    }
}