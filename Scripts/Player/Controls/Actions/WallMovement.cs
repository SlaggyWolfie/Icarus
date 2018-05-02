using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement
{
    public void WallMove ( float verticalInput, float horizontalInput, GameObject target, float wallRadius, float speed )
    {
        bool canMove = false;
        Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, wallRadius);
        //Collider closestCollider = null;

        if ( verticalInput != 0 || horizontalInput != 0 )
        {
            //Vector3 targetPosition = target.transform.position;
            //float closestDistanceSqr = Mathf.Infinity;

            foreach ( Collider collider in hitColliders )
            {
                if ( collider != target.GetComponent<Collider>() && collider.GetComponent<Rigidbody>() == null )
                {
                    canMove = true;
                }

                //Vector3 directionToTarget = collider.transform.position - targetPosition;
                //float probablyClosestDistanceSqr = directionToTarget.sqrMagnitude;

                //if ( probablyClosestDistanceSqr < closestDistanceSqr && collider != target.GetComponent<Collider>() )
                //{
                //    closestDistanceSqr = probablyClosestDistanceSqr;
                //    closestCollider = collider;
                //}
            }
        }
        if ( canMove )
        {
            //Debug.Log("WALLMOVE");
            Rigidbody rb = target.GetComponent<Rigidbody>();
            Vector3 directionFwd = Camera.main.GetComponent<CrosshairComponent>().crosshairRay.direction.normalized;
            Vector3 directionLeft = Vector3.Cross(directionFwd, target.transform.up).normalized; //Might be prone to bugs according to Slavi

            Vector3 forceFwd = Vector3.zero;
            Vector3 forceLeft = Vector3.zero;
            //float offset = 0f;/* target.GetComponent<CapsuleCollider>().bounds.extents.y;*/
            bool hasObjectBelow = Physics.Raycast(new Ray(rb.transform.position, Vector3.down), wallRadius);
            bool hasObjectAbove = Physics.Raycast(new Ray(rb.transform.position, Vector3.up), wallRadius);

            //if ( !hasObjectBelow
            //    && closestCollider.bounds.center.y < (target.GetComponent<CapsuleCollider>().bounds.center.y - offset)
            //    && closestCollider.bounds.center.y > target.GetComponent<CapsuleCollider>().bounds.center.y )
            Debug.DrawRay(rb.transform.position, rb.transform.TransformDirection(Vector3.down + Vector3.forward), Color.cyan, .1f);
            if ( !hasObjectBelow && Physics.Raycast(new Ray(rb.transform.position, rb.transform.TransformDirection(Vector3.down + Vector3.forward)), wallRadius/2) )
            {
                forceFwd += Vector3.up * 15f;   //multiplied by 15 because force is not set to impulse
            }
            //else if ( !hasObjectAbove
            //    && closestCollider.bounds.min.y > (target.GetComponent<CapsuleCollider>().bounds.center.y + offset)
            //    && closestCollider.bounds.min.y < target.GetComponent<CapsuleCollider>().bounds.max.y )
            if ( !hasObjectAbove && Physics.Raycast(new Ray(rb.transform.position, rb.transform.TransformDirection(Vector3.up + Vector3.forward)), wallRadius/2) )
            {
                forceFwd += Vector3.down * 15f;
            }


            if ( Camera.main.GetComponent<CameraControl>().rotationY >= SettingsComponent.gl_InvertAngle || Camera.main.GetComponent<CameraControl>().rotationY <= -SettingsComponent.gl_InvertAngle )
            {
                forceFwd += -directionFwd * speed;
                forceLeft = -directionLeft * speed;
            }
            else
            {
                forceFwd += directionFwd * speed;
                forceLeft = directionLeft * speed;
            }


            if ( verticalInput > 0 )
            {
                rb.AddForce(forceFwd);
            }
            else if ( verticalInput < 0 )
            {
                rb.AddForce(-forceFwd);
            }
            if ( horizontalInput > 0 )
            {
                rb.AddForce(-forceLeft);
            }
            else if ( horizontalInput < 0 )
            {
                rb.AddForce(forceLeft);
            }
        }
    }
}
