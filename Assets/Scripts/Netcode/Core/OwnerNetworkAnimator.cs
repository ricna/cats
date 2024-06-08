using Unity.Netcode.Components;
using UnityEngine;

namespace Unrez.Netcode
{
    [RequireComponent(typeof(Animator))]
    public class OwnerNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}