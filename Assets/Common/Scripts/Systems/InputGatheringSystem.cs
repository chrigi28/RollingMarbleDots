using System.Collections.Concurrent;
using Assets.Common.Scripts;
using Assets.Common.Scripts.Components;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using RaycastHit = UnityEngine.RaycastHit;


[AlwaysUpdateSystem]
class InputGatheringSystem : ComponentSystem, InputActions.ICharacterControllerActions
{
    private InputActions m_InputActions;
    private EntityQuery m_CharacterControllerInputQuery;
    private Vector2 m_CharacterMovement;
    private bool m_CharacterJumped;
    private bool canJump = false;
    private ConcurrentQueue<IEnableJumpMessage> jumpQueue = new ConcurrentQueue<IEnableJumpMessage>();
    private Entity playerQuery;

    protected override void OnCreate()
    {
        m_InputActions = new InputActions();
        m_InputActions.CharacterController.SetCallbacks(this);
        m_CharacterControllerInputQuery = GetEntityQuery(typeof(CharacterControllerInput));
        
        EventCenter.EnableJumpEvent.AddListener(m => { this.jumpQueue.Enqueue(m); });
    }

    protected override void OnStartRunning() => m_InputActions.Enable();
    protected override void OnStopRunning() => m_InputActions.Disable();

    protected override void OnUpdate()
    {
        // character controller
        if (m_CharacterControllerInputQuery.CalculateEntityCount() == 0)
        {
            EntityManager.CreateEntity(typeof(CharacterControllerInput));
        }

        if (this.jumpQueue.Count > 0 && this.jumpQueue.TryDequeue(out var m))
        {
            this.EnableJump();
        }

        m_CharacterControllerInputQuery.SetSingleton(new CharacterControllerInput
        {
            Movement = m_CharacterMovement * 200,
            Jumped = m_CharacterJumped
        });

        m_CharacterJumped = false;
    }

    void InputActions.ICharacterControllerActions.OnMove(InputAction.CallbackContext context) => m_CharacterMovement = context.ReadValue<Vector2>();
    void InputActions.ICharacterControllerActions.OnJump(InputAction.CallbackContext context)
    {
        if (context.started && this.canJump)
        {
            this.m_CharacterJumped = true;
            this.canJump = false;
            EntityManager.World.GetExistingSystem<PlayerTriggerSystem>().Enabled = true;
        }
    }

    private void EnableJump()
    {
        this.canJump = true;
        EntityManager.World.GetExistingSystem<PlayerTriggerSystem>().Enabled = false;
    }
}
