//----------------------------------------------
//            Simple Car Controller
//
// Copyright ?2014 - 2023 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Input receiver through the Unity's new Input System.
/// </summary>
public class SCC_InputManager : SCC_Singleton<SCC_InputManager> {

    public SCC_Inputs inputs;       //  Actual inputs.
    public static SCC_InputActions inputActions;

    private void Awake() {

        inputs = new SCC_Inputs();

    }

    private void Update() {

        //  Creating inputs.
        if (inputs == null)
            inputs = new SCC_Inputs();

        //  Receive inputs from the controller.
        GetInputs();

    }

    /// <summary>
    /// Gets all inputs and registers button events.
    /// </summary>
    /// <returns></returns>
    public void GetInputs() {

        if (inputActions == null)
        {
            if (InputManager.Instance != null)
            {
                inputActions = 
                    InputManager.Instance.carInputActions;
                
            }
            else
            {
                Debug.Log("No inputmanager! Excute SCC Input");
                SCC_GetInputs();
                return;
            }

        }
        //inputActions.Enable();

        Vector2 v = inputActions.Vehicle.MouseDelta.ReadValue<Vector2>();
        //Debug.Log(v);

        inputs.cameraVector = v;
        
        inputs.throttleInput = inputActions.Vehicle.Throttle.ReadValue<float>();
        inputs.brakeInput = inputActions.Vehicle.Brake.ReadValue<float>();
        inputs.steerInput = inputActions.Vehicle.Steering.ReadValue<float>();
        inputs.handbrakeInput = inputActions.Vehicle.Handbrake.ReadValue<float>();


    }

    public void SCC_GetInputs()
    {

        if (inputActions == null)
        {

            inputActions = new SCC_InputActions();
            inputActions.Enable();

        }

        inputs.throttleInput = inputActions.Vehicle.Throttle.ReadValue<float>();
        inputs.brakeInput = inputActions.Vehicle.Brake.ReadValue<float>();
        inputs.steerInput = inputActions.Vehicle.Steering.ReadValue<float>();
        inputs.handbrakeInput = inputActions.Vehicle.Handbrake.ReadValue<float>();

    }
}
