using System;
using System.Drawing;

namespace Nth.Eindhoven.Fontys
{
    [Serializable]
    public class ControlNode
    {
        public ControlNode( Point position,
                            Image backGroundImage,
                            bool isMarked = true )
        {
            Position = position;
            BackGroundImage = backGroundImage;
            IsMarked = isMarked;
        }

        public Point Position
        {
            get;
            set;
        }

        public Image BackGroundImage
        {
            get;
            set;
        }

        public bool IsMarked
        {
            get;
            set;
        }
    }


}
