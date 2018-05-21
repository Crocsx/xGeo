using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine.UI;

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager instance;

    public delegate void onNewControllerAssigned(Player nPlayer, int i);
    public event onNewControllerAssigned OnNewControllerAssigned;


    public delegate void onControllerUnAssigned(Player nPlayer, int i);
    public event onControllerUnAssigned OnControllerUnAssigned;

    #region Variables
    public float MAX_CONTROLLER = 2;
    private int rewiredPlayerIdCounter = 0;
    public List<int> unassignedController = new List<int>();
    public List<int> assignedController = new List<int>();

    public GameObject AddControllerPannel;
    #endregion

    #region Singleton Initialization 
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;

        DontDestroyOnLoad(transform.gameObject);
    }
    #endregion

    private void Start()
    {
        AssignAllJoysticksToSystemPlayer(true);

        if (assignedController.Count == 0)
        {
            ShowAddController();
        }
    }

    void Update()
    {
        CheckNewController();
    }

    void CheckNewController()
    {
        if (assignedController.Count > 0)
        {
            HideAddController();
        }

        if (ReInput.players.GetSystemPlayer().GetButtonDown("JoinGame"))
        {
            AssignNextPlayer();
        }
    }

    void ShowAddController()
    {
        CanvasGroup assignController = GameObject.FindGameObjectWithTag("AssignController").GetComponent<CanvasGroup>();
        assignController.alpha = 1;
        assignController.blocksRaycasts = true;
    }

    void HideAddController()
    {
        CanvasGroup assignController = GameObject.FindGameObjectWithTag("AssignController").GetComponent<CanvasGroup>();
        assignController.alpha = 0f;
        assignController.blocksRaycasts = false;
    }

    void AssignAllJoysticksToSystemPlayer(bool removeFromOtherPlayers)
    {
        foreach (var j in ReInput.controllers.Joysticks)
        {
            ReInput.players.GetSystemPlayer().controllers.AddController(j, removeFromOtherPlayers);
        }
    }

    void AssignNextPlayer()
    {
        if (rewiredPlayerIdCounter >= MAX_CONTROLLER)
        {
            Debug.Log("Max player limit already reached!");
            return;
        }

        // Get the next Rewired Player Id
        int rewiredPlayerId = GetNextGamePlayerId();

        // Get the Rewired Player
        Player rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerId);

        // Determine which Controller was used to generate the JoinGame Action
        Player systemPlayer = ReInput.players.GetSystemPlayer();
        var inputSources = systemPlayer.GetCurrentInputSources("JoinGame");

        foreach (var source in inputSources)
        {

            if (source.controllerType == ControllerType.Keyboard || source.controllerType == ControllerType.Mouse)
            { // Assigning keyboard/mouse

                // Assign KB/Mouse to the Player
                AssignKeyboardAndMouseToPlayer(rewiredPlayer);

                // Disable KB/Mouse Assignment category in System Player so it doesn't assign through the keyboard/mouse anymore
                ReInput.players.GetSystemPlayer().controllers.maps.SetMapsEnabled(false, ControllerType.Keyboard, "Assignment");
                ReInput.players.GetSystemPlayer().controllers.maps.SetMapsEnabled(false, ControllerType.Mouse, "Assignment");
                break;

            }
            else if (source.controllerType == ControllerType.Joystick)
            { // assigning a joystick

                // Assign the joystick to the Player. This will also un-assign it from System Player
                AssignJoystickToPlayer(rewiredPlayer, source.controller as Joystick);
                break;

            }
            else
            { // Custom Controller
                throw new System.NotImplementedException();
            }
        }

        // Enable UI map so Player can start controlling the UI
        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "UI");
    }

    private void AssignKeyboardAndMouseToPlayer(Player player)
    {
        // Assign mouse to Player
        player.controllers.hasMouse = true;

        // Load the keyboard and mouse maps into the Player
        player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "UI", "Default", true);
        player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "Default", "Default", true);
        player.controllers.maps.LoadMap(ControllerType.Mouse, 0, "Default", "Default", true);

        // Exclude this Player from Joystick auto-assignment because it is the KB/Mouse Player now
        player.controllers.excludeFromControllerAutoAssignment = true;

        Debug.Log("Assigned Keyboard/Mouse to Player " + player.name);
    }

    private void AssignJoystickToPlayer(Player player, Joystick joystick)
    {
        // Assign the joystick to the Player, removing it from System Player
        player.controllers.AddController(joystick, true);

        // Mark this joystick as assigned so we don't give it to the System Player again
        assignedController.Add(joystick.id);

        OnNewControllerAssigned(player, joystick.id);

        Debug.Log("Assigned " + joystick.name + " to Player " + player.name);
    }

    #region OnControllerEvent
    // This function will be called when a controller is connected
    // You can get information about the controller that was connected via the args parameter
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);

        if (args.controllerType != ControllerType.Joystick) return;

        // Check if this Joystick has already been assigned. If so, just let Auto-Assign do its job.
        if (assignedController.Contains(args.controllerId)) return;

        // Joystick hasn't ever been assigned before. Make sure it's assigned to the System Player until it's been explicitly assigned
        ReInput.players.GetSystemPlayer().controllers.AddController<Joystick>(
            ReInput.controllers.GetJoystick(args.controllerId).id,
            true // remove any auto-assignments that might have happened
        );
    }

    // This function will be called when a controller is fully disconnected
    // You can get information about the controller that was disconnected via the args parameter
    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }

    // This function will be called when a controller is about to be disconnected
    // You can get information about the controller that is being disconnected via the args parameter
    // You can use this event to save the controller's maps before it's disconnected
    void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller is being disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }
    #endregion

    private int GetNextGamePlayerId()
    {
        return rewiredPlayerIdCounter++;
    }
}