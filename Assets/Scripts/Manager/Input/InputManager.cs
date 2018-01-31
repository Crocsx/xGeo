using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{

    public static InputManager instance;

    public delegate void onNewController(int i);
    public event onNewController OnNewController;

    #region Variables
    public float AXIS_DEAD_ZONE = 0.25f;
    public float MAX_CONTROLLER = 2;
    public List<int> assignedController = new List<int>();
    #endregion

    #region Singleton Initialization 
    void Awake()
    {

        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(transform.gameObject);
    }
    #endregion

    void Update()
    {
        Debug.Log(InputManager.instance.GetAxis("Joy1TriggerRight"));
        CheckNewController();
    }

    void CheckNewController()
    {
        for (var i = 1; i <= MAX_CONTROLLER; i++)
        {
            if (assignedController.Contains(i))
                continue;

            if (GetButton("Joy" + i + "Start"))
            {
                assignedController.Add(i);
                if(OnNewController != null)
                    OnNewController(i);
            }
        }
    }

    #region Methods
    public Vector2 GetThumstickAxis(string nameAxis1, string nameAxis2)
    {
        Vector2 thumstickInput = new Vector2(Input.GetAxis(nameAxis1), Input.GetAxis(nameAxis2));
        if (thumstickInput.magnitude < AXIS_DEAD_ZONE)
            thumstickInput = Vector2.zero;
        else
            thumstickInput = thumstickInput.normalized * ((thumstickInput.magnitude - AXIS_DEAD_ZONE) / (1 - AXIS_DEAD_ZONE));

        return thumstickInput;
    }

    public float GetAxis(string nameAxis)
    {
        float axisInput = Input.GetAxis(nameAxis);
        if (axisInput < AXIS_DEAD_ZONE)
            axisInput = 0;
        else
            axisInput = axisInput * ((axisInput - AXIS_DEAD_ZONE) / (1 - AXIS_DEAD_ZONE));

        return axisInput;
    }

    public bool GetButton(string buttonName)
    {
        return Input.GetButton(buttonName);
    }
    #endregion
}