using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public class PetMap : MonoBehaviour
    {
        [SerializeField]
        private GameObject _minimap;

        public void Display(bool display)
        {
            _minimap.SetActive(display);
        }

        private void DisplayBoneSpots()
        {
            //TODO - Show arrows pointing all 
        }
    }
}