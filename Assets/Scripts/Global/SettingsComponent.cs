using UnityEngine;
using System.Collections;
using System;
public class SettingsComponent : MonoBehaviour
{
    public float variableUpdateTime = 2;
    [Header("Controller")]
    public bool useController;
    public ControllerTypes controllerType;
    public enum ControllerTypes
    {
        PS4,
        XBOX
    }
    public static ControllerTypes gl_ControllerType
    {
        get;
        private set;
    }

    public static string ControllerSuffix
    {
        get;
        private set;
    }

    public static bool gl_isUsingController
    {
        get;
        private set;
    }
    [Header("Grappling Hook")]
    public float hookSpeed = 10f;
    public float maxHookDistance = 20f;
    public float timeBeforeAllowingHookReturn = 2f;
    public float stickRange = 2f;
    public static float gl_HookLaunchSpeed
    {
        get;
        private set;
    }
    public static float gl_MaxHookDistance
    {
        get;
        private set;
    }
    public static float gl_TimeBeforeAllowingHookReturn
    {
        get;
        private set;
    }
    public static float gl_StickRange
    {
        get;
        private set;
    }


    [Header("Grappling Hook - Player")]
    public float pullSpeed = 20f;
    public float distanceFromDetach = 1.5f;
    public float legalWallShootingRange = 3f;

    public static float gl_PullSpeed
    {
        get;
        private set;
    }
    public static float gl_distanceFromDetach
    {
        get;
        private set;
    }
    public static float gl_minimumHookRange
    {
        get;
        private set;
    }
    [Header("Camera")]
    public bool useInvertedControlsUpsideDown;
    public static bool gl_UseInvertedControlsUpsideDown
    {
        get;
        private set;
    }

    [Tooltip("The angle at which the controls get inverted when upside-down.")]
    [Range(90, 150)]
    public float invertAngle = 110;
    public static float gl_InvertAngle
    {
        get;
        private set;
    }

    public bool invertVerticalOrientation;
    public static bool gl_InvertVerticalOrientation
    {
        get;
        private set;
    }


    [Header("Camera - Controller")]

    [Header("X rotation")]
    public float xSensitivityController = 150f;
    [Range(0, 180)]
    [Tooltip("Clamps the x rotation of the camera to a certain angle (no restriction if 0)")]
    public float maxAngleXController = 0f;

    [Header("Y rotation")]
    public float ySensitivityController = 150f;
    [Range(0, 180)]
    [Tooltip("Clamps the y rotation of the camera to a certain angle (no restriction if 0)")]
    public float maxAngleYController;

    [Header("Z rotation")]
    public float zSensitivityController = 150f;
    [Range(0, 180)]
    [Tooltip("Clamps the z rotation of the camera to a certain angle (no restriction if 0)")]
    public float maxAngleZController;

    [Header("Camera - Mouse")]

    [Header("X rotation")]
    public float xSensitivity = 50f;
    [Range(0, 180)]
    [Tooltip("Clamps the x rotation of the camera to a certain angle (no restriction if 0)")]
    public float maxAngleX = 0f;

    [Header("Y rotation")]
    public float ySensitivity = 50f;
    [Range(0, 180)]
    [Tooltip("Clamps the y rotation of the camera to a certain angle (no restriction if 0)")]
    public float maxAngleY;

    [Header("Z rotation")]
    public float zSensitivity = 50f;
    [Range(0, 180)]
    [Tooltip("Clamps the z rotation of the camera to a certain angle (no restriction if 0)")]
    public float maxAngleZ;

    public static float gl_CameraSensitivityX
    {
        get;
        private set;
    }

    public static float gl_CameraMaxAngleX
    {
        get;
        private set;
    }
    public static float gl_CameraSensitivityY
    {
        get;
        private set;
    }

    public static float gl_CameraMaxAngleY
    {
        get;
        private set;
    }
    public static float gl_CameraSensitivityZ
    {
        get;
        private set;
    }

    public static float gl_CameraMaxAngleZ
    {
        get;
        private set;
    }

    [Header("Player Controls")]
    [Header("Jump")]
    public float jumpRadius = 1f;
    public float jumpBoostMax = 20f;

    public static float gl_JumpRadius
    {
        get;
        private set;
    }
    public static float gl_JumpBoostMax
    {
        get;
        private set;
    }

    [Header("Wall Movement")]
    public float wallGlideSpeed = 7.5f;

    public static float gl_WallGlideSpeed
    {
        get;
        private set;
    }

    [Header("Interact")]
    public float interactDistance = 1f;

    public static float gl_InteractDistance
    {
        get;
        private set;
    }

    public float holdDistance = 1f;

    public static float gl_HoldDistance
    {
        get;
        private set;
    }

    [Tooltip("The radius to check when holding something to see if it is still there. Will progressively lag the bigger it is.")]
    public float holdRadiusCheck = 0.5f;

    public static float gl_HoldRadiusCheck
    {
        get;
        private set;
    }

    public bool physicsBasedHolding = true;

    public static bool gl_PhysicsBasedHolding
    {
        get;
        private set;
    }

    public bool holdControlToHold = false;

    public static bool gl_HoldControlToHold
    {
        get;
        private set;
    }

    [Header("Health")]

    public bool canDie = true;

    public static bool gl_CanDie
    {
        get;
        private set;
    }

    public float maxHealth = 100;

    public static float gl_MaxHealth
    {
        get;
        private set;
    }

    public bool canTakeDamage = true;

    public static bool gl_CanTakeDamage
    {
        get;
        private set;
    }

    public bool healthRegenerates = true;

    public static bool gl_HealthRegenerates
    {
        get;
        private set;
    }

    public float healthPointsRegeneration = 0.5f;

    public static float gl_HealthPointsRegeneration
    {
        get;
        private set;
    }

    [Tooltip("Seconds")]
    public float healthRegenerationDelay = 5;

    public static float gl_SecondsBeforeHealthRegenerationStarts
    {
        get;
        private set;
    }

    [Header("Damage")]
    [Tooltip("Seconds")]
    public float gettingDamagedCooldown = 1;

    public static float gl_CooldownOnGettingDamagedInSeconds
    {
        get;
        private set;
    }

    public float steamDamage = 5;

    public static float gl_SteamDamage
    {
        get;
        private set;
    }

    public float heatDamage = 7;

    public static float gl_HeatDamage
    {
        get;
        private set;
    }

    //------------------------ AWAKE ---------------------
    private void Awake ()
    {
        variableUpdatesEvent += UpdateVariables;
        StartCoroutine(constantVariableUpdate());
    }
    //----------------------------------------------------


    public delegate void VariableUpdateDelegate ();
    public static event VariableUpdateDelegate variableUpdatesEvent;


    [UpdateVariables]
    private void InvokeVariablesUpdate ()
    {
        variableUpdatesEvent();
    }


    IEnumerator constantVariableUpdate ()
    {
        while ( true )
        {
            InvokeVariablesUpdate();
            yield return new WaitForSeconds(variableUpdateTime);
        }
    }
    public void ControllerCheck ()
    {
        string[] currentControllers = new string[Input.GetJoystickNames().Length];
        int numberOfControllers = 0;
        for ( int i = 0; i < Input.GetJoystickNames().Length; i++ )
        {
            currentControllers[i] = Input.GetJoystickNames()[i].ToLower();
            if ( (currentControllers[i] == "controller (xbox 360 for windows)"
                || currentControllers[i] == "controller (xbox 360 wireless receiver for windows)"
                || currentControllers[i] == "controller (xbox one for windows)") )
            {
                useController = true;
                controllerType = ControllerTypes.XBOX;
                ControllerSuffix = "XBOX";
            }
            else if ( currentControllers[i] == "wireless controller" )
            {
                useController = true;
                controllerType = ControllerTypes.PS4;
                ControllerSuffix = "PS";
            }
            else if ( currentControllers[i] == "" )
            {
                numberOfControllers++;
            }
            if ( currentControllers[i] != "" )
            {
                Debug.Log(currentControllers[i] + " is detected.");
            }
        }
        if ( numberOfControllers == Input.GetJoystickNames().Length )
        {
            useController = false;
            ControllerSuffix = "";
            Debug.Log("No controller found, using mouse and keyboard settings!");
        }
    }

    private void UpdateVariables ()
    {
        //controller
        ControllerCheck();
        gl_isUsingController = useController;
        gl_ControllerType = controllerType;
        //hook
        gl_HookLaunchSpeed = hookSpeed;
        gl_MaxHookDistance = maxHookDistance;
        gl_TimeBeforeAllowingHookReturn = timeBeforeAllowingHookReturn;
        gl_StickRange = stickRange;
        //hook - player
        gl_PullSpeed = pullSpeed;
        gl_distanceFromDetach = distanceFromDetach;
        gl_minimumHookRange = legalWallShootingRange;
        //camera
        gl_UseInvertedControlsUpsideDown = useInvertedControlsUpsideDown;
        gl_InvertAngle = invertAngle;
        gl_InvertVerticalOrientation = invertVerticalOrientation;

        if ( !gl_isUsingController )
        {
            //X rotation
            gl_CameraSensitivityX = xSensitivity;
            gl_CameraMaxAngleX = maxAngleX;
            //Y rotation
            gl_CameraSensitivityY = ySensitivity;
            gl_CameraMaxAngleY = maxAngleY;
            //Z rotation
            gl_CameraSensitivityZ = zSensitivity;
            gl_CameraMaxAngleZ = maxAngleZ;
        }
        else
        {
            //X rotation
            gl_CameraSensitivityX = xSensitivityController;
            gl_CameraMaxAngleX = maxAngleXController;
            //Y rotation
            gl_CameraSensitivityY = ySensitivityController;
            gl_CameraMaxAngleY = maxAngleYController;
            //Z rotation
            gl_CameraSensitivityZ = zSensitivityController;
            gl_CameraMaxAngleZ = maxAngleZController;
        }
        //Player controls
        //Jump
        gl_JumpRadius = jumpRadius;
        gl_JumpBoostMax = jumpBoostMax;
        //Wall Movement
        gl_WallGlideSpeed = wallGlideSpeed;
        //Interaction
        gl_InteractDistance = interactDistance;
        gl_HoldDistance = holdDistance;
        gl_HoldRadiusCheck = holdRadiusCheck;
        gl_PhysicsBasedHolding = physicsBasedHolding;
        gl_HoldControlToHold = holdControlToHold;
        //Health
        gl_MaxHealth = maxHealth;
        gl_SecondsBeforeHealthRegenerationStarts = healthRegenerationDelay;
        gl_HealthPointsRegeneration = healthPointsRegeneration;
        gl_CanDie = canDie;
        gl_CanTakeDamage = canTakeDamage;
        gl_HealthRegenerates = healthRegenerates;
        //Damage
        gl_CooldownOnGettingDamagedInSeconds = gettingDamagedCooldown;
        gl_SteamDamage = steamDamage;
        gl_HeatDamage = heatDamage;

    }
}