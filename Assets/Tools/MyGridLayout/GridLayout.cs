using UnityEngine;

namespace Tools.MyGridLayout
{
    public class GridLayout : AbstractGridLayout
    {
        [Min(1)] [SerializeField] private int columnCount;
        [Min(1)] [SerializeField] private int rowCount;
        [SerializeField] private Vector2 spacing;
        [SerializeField] private Vector2 horizontalPadding;
        [SerializeField] private Vector2 verticalPadding;
        
        private Vector2 Spacing => spacing / 1000;
        private Vector2 HorizontalPadding => horizontalPadding / 1000;
        private Vector2 VerticalPadding => verticalPadding / 1000;

        private void Update()
        {
            if(!Application.isPlaying )
                Align();
        }

        public override void Align() =>
            Align(rowCount,columnCount, Spacing, VerticalPadding, HorizontalPadding);
    }
}