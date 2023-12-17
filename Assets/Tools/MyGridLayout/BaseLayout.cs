using UnityEngine;

namespace Tools.MyGridLayout
{
    public class BaseLayout: AbstractGridLayout
    {
        public void Align(int columnsCount) =>
            Align(1,columnsCount, Vector2.zero, Vector2.zero, Vector2.zero);
        
        public override void Align() =>
            Align(1,0, Vector2.zero, Vector2.zero, Vector2.zero);
    }
}