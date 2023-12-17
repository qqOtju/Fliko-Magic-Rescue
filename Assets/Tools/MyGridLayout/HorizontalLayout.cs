using UnityEngine;

namespace Tools.MyGridLayout
{
    [ExecuteInEditMode]
    public class HorizontalLayout : AbstractGridLayout
    {
        [Min(1)] [SerializeField] private int columnCount;
        [SerializeField] private Vector2 spacing;
        [SerializeField] private Vector2 horizontalPadding;
        [SerializeField] private Vector2 verticalPadding;
        
        private Vector2 Spacing => spacing / 1000;
        private Vector2 HorizontalPadding => horizontalPadding / 1000;
        private Vector2 VerticalPadding => verticalPadding / 1000;

        private void Awake() =>
            Align();

        public override void Align()
        {
            Align(1,columnCount, Spacing, VerticalPadding, HorizontalPadding);
        }

        private void Update()
        {
            if(!Application.isPlaying)
            {
                Awake();
            }
        }
    }
}