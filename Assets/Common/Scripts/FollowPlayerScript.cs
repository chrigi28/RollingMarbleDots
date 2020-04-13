using System.Collections;
using System.Collections.Generic;
using Assets.Common.Scripts;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class FollowPlayerScript : MonoBehaviour, IReceiveEntityPosition
{
    private Translation pos = new Translation { Value = float3.zero };
    [SerializeField] float3 offset = new Vector3(0, 5, -8);

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = this.pos.Value + this.offset;
    }

    public void SendPosition(Translation newPos)
    {
        this.pos = newPos;
    }
}
