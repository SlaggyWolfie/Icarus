using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CrosshairComponent : MonoBehaviour
{
    //public Image crosshair;
    //[Range(0, 5)]
    //public float rotationSpeed = 5;

    //private void Update()
    //{
    //    RaycastHit hitInfo;
    //    if (Physics.Raycast(crosshairRay, out hitInfo, SettingsComponent.InteractDistance) &&
    //        (hitInfo.collider.gameObject.GetComponent<ActivateComponent>() != null  //rotates the crosshair if there is an object that has an "ActiveComponent" component
    //        || hitInfo.collider.gameObject.GetComponent<HoldComponent>() != null) )  //rotates the crosshair if there is an object that has a "HoldComponent" component
    //    {
    //        crosshair.rectTransform.localRotation *= Quaternion.AngleAxis(rotationSpeed, Vector3.forward);
    //    }
    //}

    public Ray crosshairRay
    {
        get
        {
            return new Ray(gameObject.transform.position, transform.TransformDirection(Vector3.forward));   //projects a ray in the direction where the camera is facing
        }
    }

    public RaycastHit GetRaycastHitInfo(float pDistance = 1f)
    {
        RaycastHit hit;
        Physics.Raycast(crosshairRay, out hit, pDistance);
        return hit;
    }

    public RaycastHit[] GetRaycastAllInfo(float pDistance = 1f)
    {
        return Physics.RaycastAll(crosshairRay, pDistance);
    }
}
