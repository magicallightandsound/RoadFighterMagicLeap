using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem;

public class PlayerInputSystem : JobComponentSystem, PlayerControls.ICarActions
{
    private PlayerControls m_PlayerControls;
    private Vector2 m_MovementInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        m_MovementInput = context.action.ReadValue<Vector2>();
    }

    protected override void OnCreate()
    {
        m_PlayerControls = new PlayerControls();

        #if UNITY_STANDALONE || UNITY_EDITOR
            m_PlayerControls.bindingMask = InputBinding.MaskByGroup(m_PlayerControls.StandaloneScheme.bindingGroup);
        #endif

        m_PlayerControls.Car.SetCallbacks(this);
        m_PlayerControls.Car.Enable();
    }

    protected override void OnDestroy()
    {
        m_PlayerControls.Car.Disable();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float x = m_MovementInput.x;

        // in: read only
        // ref: read and write
        var jobHandle = Entities.ForEach((ref Translation translation, ref InputData input) =>
            {
                input.Value = new float3(x, 0, 0);
            }).Schedule(inputDeps);

        return jobHandle;
    }
}
