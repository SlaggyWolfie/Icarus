using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump
{
    private float _jumpBoost;
    private Collider[] _hitColliders;

    public void HoldBoost ( float jumpInput, GameObject target, float jumpRadius, float jumpBoostMax )
    {
        //_target = target;
        bool canBoost = false;

        if ( jumpInput != 0 )
        {
            Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, jumpRadius);
            foreach ( Collider collider in hitColliders )
            {
                //NOTE: IMMOVABLE TAG DOES NOT YET EXIST
                //MEANS: DO NOT USE
                if ( collider.GetType() == typeof(BoxCollider) || collider.gameObject.CompareTag("Immovable") )
                {
                    canBoost = true;
                }
            }
        }
        if ( canBoost )
        {
            _jumpBoost += jumpInput;
            _jumpBoost = Mathf.Min(_jumpBoost, jumpBoostMax);
        }
    }

    public float HoldBoostF ( float jumpInput, GameObject target, float jumpRadius, float jumpBoostMax )
    {
        //_target = target;
        bool canBoost = false;

        if ( jumpInput != 0 )
        {
            _hitColliders = Physics.OverlapSphere(target.transform.position, jumpRadius);
            foreach ( Collider collider in _hitColliders )
            {
                //NOTE: IMMOVABLE TAG DOES NOT YET EXIST
                if ( collider != target.GetComponent<Collider>() )// || collider.gameObject.CompareTag("Immovable"))
                {
                    canBoost = true;
                }
            }
        }
        if ( canBoost )
        {
            _jumpBoost += jumpInput;
            _jumpBoost = Mathf.Min(_jumpBoost, jumpBoostMax);
        }
        return _jumpBoost;
    }

    public void BoostJump ( GameObject target, float pJumpInputHorizontal, float pJumpInputVertical )
    {
        Rigidbody targetRigidbody = target.GetComponent<Rigidbody>();
        if ( targetRigidbody == null )
            Debug.LogError("Jump target missing Rigidbody");

        Vector3 directionFwd = Camera.main.GetComponent<CrosshairComponent>().crosshairRay.direction;
        Vector3 directionRight = -Vector3.Cross(directionFwd, Vector3.up);

        Vector3 forceFwd = directionFwd.normalized * _jumpBoost;
        Vector3 forceRight = directionRight.normalized * _jumpBoost;
        Vector3 force = Vector3.zero;

        if ( pJumpInputVertical > 0 )
        {
            //rb.AddForce(forceFwd);
            force += forceFwd;
        }
        else if ( pJumpInputVertical < 0 )
        {
            //rb.AddForce(-forceFwd);
            force -= forceFwd;
        }
        if ( pJumpInputHorizontal > 0 )
        {
            //rb.AddForce(forceRight);
            force += forceRight;
        }
        else if ( pJumpInputHorizontal < 0 )
        {
            //rb.AddForce(-forceRight);
            force -= forceRight;
        }
        
        //shortest distance collider
        //Sort requires default equality comparer and that won't work here
        //Linq - OrderBy - lambda expression was fucking up
        //Manual Way implemented here
        Collider colliderToJumpFrom = null;
        float leastDistance = float.PositiveInfinity;
        foreach (Collider col in _hitColliders)
        {
            float distance = (col.gameObject.transform.position - target.transform.position).magnitude;
            if ( distance < leastDistance && col != target.GetComponent<Collider>())
            {
                colliderToJumpFrom = col;
                leastDistance = distance;
            }
        }
        //Debug.Log(colliderToJumpFrom.name);
        //Push shortest distance collider
        if (colliderToJumpFrom != null)
        {
            //Debug.Log("Distance: " + leastDistance);
            Rigidbody colliderRigidbody = colliderToJumpFrom.GetComponent<Rigidbody>();
            if (colliderRigidbody != targetRigidbody)
            {
                //Only push away the object if it's smaller than you
                if (colliderRigidbody == null || targetRigidbody.mass < colliderRigidbody.mass)
                {
                    //force *= 10;
                    targetRigidbody.AddForce(force, ForceMode.Impulse);
                }

                if ( colliderRigidbody )
                {
                    colliderRigidbody.AddForce(-force, ForceMode.Impulse);
                }
            }
        }

        //List<Collider> colliders = new List<Collider>(_hitColliders);
        //colliders.OrderBy(c => (c.gameObject.transform.position - target.transform.position).magnitude);
        //foreach (Collider collider in colliders)
        //{
        //    Debug.Log("Distance to player: " + (collider.gameObject.transform.position - target.transform.position).magnitude);
        //}

        //force *= -1; //Just for not using it later on //Test
        //foreach (Collider collider in _hitColliders)
        //{
        //    Rigidbody collRB = collider.GetComponent<Rigidbody>();
        //    if (collRB != null && collRB != rb)
        //        collRB.AddForce(force, ForceMode.Impulse);
        //}

        _hitColliders = null;
        _jumpBoost = 0;
    }
}
