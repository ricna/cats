using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Unrez
{
    public class Map: NetworkBehaviour
    {
        [field: SerializeField]
        public List<BoneSpot> BoneSpots { get; private set; }
        private List<ScareTree> _scareTree;
    }
}