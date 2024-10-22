using UnityEngine;

namespace Unrez.BackyardShowdown
{

    [CreateAssetMenu(fileName = "New Structure Data", menuName = "Unrez/Structure")]
    public class StructureData : ScriptableObject
    {
        public Sprite CornerBottom;
        public Sprite CornerTop;
        public Sprite EndBottom;
        public Sprite EndCenter;
        public Sprite EndTop;
        public Sprite RepeatHorizontal;
        public Sprite TBottom;
        public Sprite TSide;
        public Sprite TTop;
    }
}