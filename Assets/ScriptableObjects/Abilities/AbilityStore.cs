using System;
using UnityEngine;

namespace Unrez.BackyardShowdown
{
    [CreateAssetMenu(fileName = "New Pet Ability", menuName = "Unrez/Cats/Pet Ability", order = 0)]
    public class AbilityStore : ScriptableObject
    {
        public PawnType Type;
        public Type abt;
       // public TypedReference abtReference;
        public Ability[] Abilities;
    }
}
