using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Unrez.Pets.Abilities;

namespace Unrez.Pets
{
    [CreateAssetMenu(fileName = "New Pet Ability", menuName = "Unrez/Cats/Pet Ability", order = 0)]
    public class AbilityStore : ScriptableObject
    {
        public PetType Type;
        public Type abt;
       // public TypedReference abtReference;
        public Ability[] Abilities;
    }
}
