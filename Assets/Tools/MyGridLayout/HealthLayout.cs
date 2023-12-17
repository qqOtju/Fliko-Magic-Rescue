using UnityEngine;

namespace Tools.MyGridLayout
{
    public class HealthLayout: AbstractGridLayout
    {
        [SerializeField] private Vector2 _spacing;
        [SerializeField] private Vector2 _horizontalPadding;
        [SerializeField] private Vector2 _verticalPadding;
        
        private Vector2 Spacing => _spacing / 1000;
        private Vector2 HorizontalPadding => _horizontalPadding / 1000;
        private Vector2 VerticalPadding => _verticalPadding / 1000;
        public void Align(int columnsCount) =>
            Align(1,columnsCount, Spacing, VerticalPadding, HorizontalPadding);
        
        public override void Align() =>
            Align(1,0, Spacing, VerticalPadding, HorizontalPadding);
    }
}