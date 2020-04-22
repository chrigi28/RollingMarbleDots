using Assets.Common.Scripts;
using Assets.Common.Scripts.Components;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;



[AlwaysUpdateSystem]
class InputGatheringSystem : ComponentSystem, InputActions.ICharacterControllerActions
{
    InputActions m_InputActions;
    EntityQuery m_CharacterControllerInputQuery;
    Vector2 m_CharacterMovement;
    bool m_CharacterJumped;
    
    protected override void OnCreate()
    {
        m_InputActions = new InputActions();
        m_InputActions.CharacterController.SetCallbacks(this);
        m_CharacterControllerInputQuery = GetEntityQuery(typeof(CharacterControllerInput));
    }

    protected override void OnStartRunning() => m_InputActions.Enable();

    protected override void OnStopRunning() => m_InputActions.Disable();

    protected override void OnUpdate()
    {
        // character controller
        if (m_CharacterControllerInputQuery.CalculateEntityCount() == 0)
            EntityManager.CreateEntity(typeof(CharacterControllerInput));
        
        m_CharacterControllerInputQuery.SetSingleton(new CharacterControllerInput
        {
            Movement = m_CharacterMovement * 200,
            Jumped = m_CharacterJumped
        });

        m_CharacterJumped = false;
    }
    
    void InputActions.ICharacterControllerActions.OnMove(InputAction.CallbackContext context) => m_CharacterMovement = context.ReadValue<Vector2>();
    void InputActions.ICharacterControllerActions.OnJump(InputAction.CallbackContext context) { if (context.started) m_CharacterJumped = true; }
}
