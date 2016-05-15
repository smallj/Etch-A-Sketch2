using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Etch_A_Sketch2
{
    public partial class Form1
    {
        private Point ScribePosition;

        public class verticalDialIndicator
        {
            public static Point center;
            public static PointF location;
            public static double angle;
            public static int radius;
            public static int indicatorSize = 10;
            public static double step = 2 * Math.PI / 160;
        }

        public class horizontalDialIndicator
        {
            public static Point center;
            public static PointF location;
            public static double angle;
            public static int radius;
            public static int indicatorSize = 10;
            public static double step = 2 * Math.PI / 160;
        }

        public enum Direction : byte
        {
            Up = 0,
            Down,
            Left,
            Right
        }

        public enum drawingBounds : int
        {
            Left = 140,
            Top = 135,
            Right = 1445,
            Bottom = 750,
            xMidPoint = ((Right - Left) / 2) + Left,
            yMidPoint = ((Bottom - Top) / 2) + Top
        }

        public enum verticalButtonBounds : int
        {
            Left = 15,
            Top = 750,
            Right = 120,
            Bottom = 860,
            xMidPoint = ((Right - Left) / 2) + Left,
            yMidPoint = ((Bottom - Top) / 2) + Top
        }

        public enum horizontalButtonBounds : int
        {
            Left = 1440,
            Top = 750,
            Right = 1550,
            Bottom = 860,
            xMidPoint = ((Right - Left) / 2) + Left,
            yMidPoint = ((Bottom - Top) / 2) + Top
        }

        /// <summary>
        /// Draw a string to the screen starting at the given x and y position.
        /// </summary>
        /// <param name="drawText">String to "draw"</param>
        /// <param name="x">Starting horizontal position</param>
        /// <param name="y">Starting vertical position</param>
        public void DrawString(String drawText, float x, float y)
        {
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            string drawString = drawText;
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            formGraphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
            drawFont.Dispose();
            drawBrush.Dispose();
            formGraphics.Dispose();
        }

        /// <summary>
        /// Erase the given rectangle on the screen
        /// </summary>
        /// <param name="rect"></param>
        public void Erase(Rectangle rect)
        {
            this.Invalidate(rect);
        }

        /// <summary>
        /// Draw a rectangle on the screen of a given height and width with the upper left corner at the the x,y position 
        /// passed in.
        /// </summary>
        /// <param name="x">Upper left horizontal starting point</param>
        /// <param name="y">Upper left vertical starting point</param>
        /// <param name="width">Width to draw the rectangle</param>
        /// <param name="height">Height to draw the rectangle</param>
        public void DrawRect(int x, int y, int width, int height)
        {
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            Pen pen = new Pen(Color.Black);
            formGraphics.DrawRectangle(pen, x, y, width, height);
            formGraphics.Dispose();
        }

        /// <summary>
        /// Draw a line between two given points.
        /// </summary>
        /// <param name="start">Starting point</param>
        /// <param name="end">Ending point</param>
        public void DrawLine(Point start, Point end)
        {
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            Pen pen = new Pen(Color.Black);
            formGraphics.DrawLine(pen, start, end);
            formGraphics.Dispose();
        }

        /// <summary>
        /// Handle moving the "scribe" on the screen.  As the dials are turned a line will be draw one pixle at a time
        /// in either the horizontal or vertical direction.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="dir"></param>
        public void MoveScribe(Point start, Direction dir)
        {
            Point endPoint = new Point(start.X, start.Y);
            switch (dir)
            {
                case Direction.Up:
                    if (start.Y - 1 > (int)drawingBounds.Top)
                        endPoint.Y -= 1;
                    break;
                    
                case Direction.Down:
                    if (start.Y + 1 < (int)drawingBounds.Bottom)
                        endPoint.Y += 1;
                    break;

                case Direction.Left:
                    if (start.X - 1 > (int)drawingBounds.Left)
                        endPoint.X -= 1;
                    break;

                case Direction.Right:
                    if (start.X + 1 < (int)drawingBounds.Right)
                        endPoint.X += 1;
                    break;
            }
            DrawLine(start, endPoint);
            ScribePosition = endPoint;
        }

        /// <summary>
        /// Draw a "dot" on the dial to indicate the direction the dial is "turning".
        /// </summary>
        /// <param name="pCenter">The center point of the circle the "dot" will travle around</param>
        /// <param name="radius">The distance from the center to draw the "dot"</param>
        /// <param name="pAngle">The angle around the circle to draw the "dot"</param>
        /// <returns>A point used to draw a filled circle "dot"</returns>
        public PointF RotateDial(Point pCenter, int radius, ref double pAngle)
        {

            PointF returnPoint = new Point();

            if (pAngle > 2 * Math.PI)
                pAngle = 0;
            else if (pAngle < 0)
                pAngle = 2 * Math.PI;

            returnPoint.X = pCenter.X + (float)Math.Cos(pAngle) * radius;
            returnPoint.Y = pCenter.Y - (float)Math.Sin(pAngle) * radius;

            return returnPoint;
        }

        /// <summary>
        /// Draws a cirlce counter clockwise around a given point at a given radius.
        /// </summary>
        /// <param name="pCenter"></param>
        /// <param name="radius"></param>
        public void DrawCircleCCW(Point pCenter, int radius)
        {

            double step = 2 * Math.PI / 360;
            double theta = 0;
            PointF location = new Point();

            for (theta = 0; theta < 2 * Math.PI; theta += step)
            {
                location = RotateDial(pCenter, radius, ref theta);
                drawDot(location, 10);
                Thread.Sleep(5);
                
            }


        }

        /// <summary>
        /// Draws a circle clockwise around a given point at a given radius.
        /// </summary>
        /// <param name="pCenter"></param>
        /// <param name="radius"></param>
        public void DrawCircleCW(Point pCenter, int radius)
        {

            double step = 2 * Math.PI / 360;
            double theta = 0;
            PointF location = new Point();

            for (theta = 2 * Math.PI; theta > 0 ; theta -= step)
            {
                location = RotateDial(pCenter, radius, ref theta);
                drawDot(location, 10);
                Thread.Sleep(5);

            }


        }

        /// <summary>
        /// Draw a "dot" or filled circle at a given location with a given diameter.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="diameter"></param>
        public void drawDot(PointF location, int diameter)
        {
            System.Drawing.Graphics formGraphics = this.CreateGraphics();
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            formGraphics.FillEllipse(drawBrush, location.X - (diameter / 2), location.Y - (diameter / 2), diameter, diameter);
            formGraphics.Dispose();
        }

        /// <summary>
        /// Ramdomly erase a rectangle of 50 by 50 pixles in the drawing surface to simulate shaking of the etch-a-sketch.
        /// </summary>
        public void ShakeIt()
        {
            Rectangle rect = new Rectangle();

            Random randX = new Random((int)DateTime.Now.Ticks);
            Random randY = new Random((int)DateTime.Now.Ticks);

            rect.X = randX.Next((int)drawingBounds.Left, (int)drawingBounds.Right - 51);
            rect.Y = randY.Next((int)drawingBounds.Top, (int)drawingBounds.Bottom - 51);
            rect.Height = 50;
            rect.Width = 50;
            
            this.Invalidate(rect);
        }
    }
}
