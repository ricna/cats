using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace Unrez
{
    public class Map: NetworkBehaviour
    {
        private IEnumerable<BoneSpot> _boneSpots;
        public List<BoneSpot> BoneSpots { get; private set; }
        private List<ScareTree> _scareTree;
    }
}