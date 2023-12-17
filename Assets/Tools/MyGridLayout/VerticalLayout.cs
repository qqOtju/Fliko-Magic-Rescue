using UnityEngine;

namespace Tools.MyGridLayout
{
    [ExecuteInEditMode]
    public class VerticalLayout : AbstractGridLayout
    {
        [Min(1)] [SerializeField] private int rowCount;
        [SerializeField] private Vector2 spacing;
        [SerializeField] private Vector2 horizontalPadding;
        [SerializeField] private Vector2 verticalPadding;
        private Vector2 Spacing => spacing / 1000;
        private Vector2 HorizontalPadding => horizontalPadding / 1000;
        private Vector2 VerticalPadding => verticalPadding / 1000;

        public override void Align() =>
            Align(rowCount,1, Spacing, VerticalPadding, HorizontalPadding);

        private void Update()
        {
            if(!Application.isPlaying)
                Align();
        }
    }
}