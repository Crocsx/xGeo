using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{

    public static InputManager instance;

    #region Variables
    public const float AXIS_DEAD_ZONE = 0.25f;
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