using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Blaster.Player.Network 
{
    [System.Serializable]
    public class PlayerInput : INetworkSerializable
    {
        public int requestId = 0;

        public virtual void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref requestId);
        }
    }

    [System.Serializable]
    public class PlayerMoveInput : PlayerInput {
        public Vector2 inputDirection = Vector2.zero;
        public bool jump = false;

        public override void NetworkSerialize<T>(BufferSerializer<T> serializer) {
            base.NetworkSerialize(serializer);
            serializer.SerializeValue(ref inputDirection);
        }
    }
}

