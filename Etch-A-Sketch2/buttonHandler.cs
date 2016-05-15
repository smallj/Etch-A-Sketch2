using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Etch_A_Sketch2
{
    class MouseState
    {
        public static Point mPrevPos;
        public static Size mPrevDelta;
        public static bool movingScribeVertical;
        public static bool movingScribeHorizontal;
    }

    public partial class Form1
    {
        /// <summary>
        /// Handle vertical dial press/drag.
        /// </summary>
        /// <param name="currMousePosition"></param>
        public void verticalButton_Pressed(Point currMousePosition)
        {
            int xDelta;
            int yDelta;
            MouseState.movingScribeVertical = true;
            xDelta = currMousePosition.X - MouseState.mPrevPos.X;
            yDelta = currMousePosition.Y - MouseState.mPrevPos.Y;

            if ((currMousePosition.X < (int)verticalButtonBounds.xMidPoint) && (yDelta < 0)
                || (currMousePosition.X > (int)verticalButtonBounds.xMidPoint) && (yDelta > 0)
                || (currMousePosition.Y < (int)verticalButtonBounds.yMidPoint) && (xDelta > 0) 
                || (currMousePosition.Y > (int)verticalButtonBounds.yMidPoint) && (xDelta < 0))
            {
                //turning clockwise
                verticalDialIndicator.angle -= verticalDialIndicator.step;
                MoveScribe(ScribePosition, Direction.Up);
            }
            else
            {
                //turning counter clockwise
                verticalDialIndicator.angle += verticalDialIndicator.step;
                MoveScribe(ScribePosition, Direction.Down);
            }

            //move the indicator on the dial
            Erase(new Rectangle((int)verticalButtonBounds.Left, (int)verticalButtonBounds.Top,
                (int)verticalButtonBounds.Right - (int)verticalButtonBounds.Left,
                (int)verticalButtonBounds.Bottom - (int)verticalButtonBounds.Top));
            verticalDialIndicator.location = RotateDial(verticalDialIndicator.center, verticalDialIndicator.radius, ref verticalDialIndicator.angle);
            drawDot(verticalDialIndicator.location, verticalDialIndicator.indicatorSize);

            //save the mouse position
            MouseState.mPrevPos = currMousePosition;
        }

        /// <summary>
        /// Handle horizontal dial press/drag.
        /// </summary>
        /// <param name="currMousePosition"></param>
        public void horizontalButton_Pressed(Point currMousePosition)
        {
            int xDelta;
            int yDelta;
            MouseState.movingScribeHorizontal = true;

            xDelta = currMousePosition.X - MouseState.mPrevPos.X;
            yDelta = currMousePosition.Y - MouseState.mPrevPos.Y;

            if ((currMousePosition.X < (int)horizontalButtonBounds.xMidPoint) && (yDelta < 0)
                || (currMousePosition.X > (int)horizontalButtonBounds.xMidPoint) && (yDelta > 0)
                || (currMousePosition.Y < (int)horizontalButtonBounds.yMidPoint) && (xDelta > 0)
                || (currMousePosition.Y > (int)horizontalButtonBounds.yMidPoint) && (xDelta < 0))
            {
                //turning clockwise
                horizontalDialIndicator.angle -= horizontalDialIndicator.step;
                MoveScribe(ScribePosition, Direction.Right);
            }
            else
            {
                //turning counter clockwise
                horizontalDialIndicator.angle += horizontalDialIndicator.step;
                MoveScribe(ScribePosition, Direction.Left);
            }

            //move the indicator
            Erase(new Rectangle((int)horizontalButtonBounds.Left, (int)horizontalButtonBounds.Top,
                (int)horizontalButtonBounds.Right - (int)horizontalButtonBounds.Left,
                (int)horizontalButtonBounds.Bottom - (int)horizontalButtonBounds.Top));
            horizontalDialIndicator.location = RotateDial(horizontalDialIndicator.center, horizontalDialIndicator.radius, ref horizontalDialIndicator.angle);
            drawDot(horizontalDialIndicator.location, horizontalDialIndicator.indicatorSize);

            //save the mouse position.
            MouseState.mPrevPos = currMousePosition;
        }


    }
}