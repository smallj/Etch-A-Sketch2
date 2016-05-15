using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Etch_A_Sketch2
{
    public partial class Form1 : Form
    {

    
        public Form1()
        {

            InitializeComponent();

            initializeDrawing();
        }

        /// <summary>
        /// Initialize the starting point for the etch-a-sketch to the center of the drawing surface.
        /// </summary>
        private void initializeDrawing()
        {
            //Set the "scribe" position to the center of the canvas.
            ScribePosition.X = (int)drawingBounds.xMidPoint;
            ScribePosition.Y = (int)drawingBounds.yMidPoint;

            //Set the dial indicator to the top center of the knob for the vertical dial control.
            verticalDialIndicator.center.X = (int)verticalButtonBounds.xMidPoint;
            verticalDialIndicator.center.Y = (int)verticalButtonBounds.yMidPoint;
            verticalDialIndicator.radius = 35;
            verticalDialIndicator.angle =  Math.PI / 2;
            verticalDialIndicator.location = RotateDial(verticalDialIndicator.center, verticalDialIndicator.radius, ref verticalDialIndicator.angle);

            //Set the dial indicator to the top center of the knob for the horizontal dial control.
            horizontalDialIndicator.center.X = (int)horizontalButtonBounds.xMidPoint;
            horizontalDialIndicator.center.Y = (int)horizontalButtonBounds.yMidPoint;
            horizontalDialIndicator.radius = 35;
            horizontalDialIndicator.angle = Math.PI / 2;
            horizontalDialIndicator.location = RotateDial(horizontalDialIndicator.center, horizontalDialIndicator.radius, ref horizontalDialIndicator.angle);

        }


        /// <summary>
        /// Right click context menu close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Right click context menu erase.  Erases the drawing screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eraseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clear the screen.
            Erase(new Rectangle((int)drawingBounds.Left, (int)drawingBounds.Top, 
                (int)drawingBounds.Right - (int)drawingBounds.Left, 
                (int)drawingBounds.Bottom - (int)drawingBounds.Top));
        }

        /// <summary>
        /// Capture the left mouse click when not on a dial to move the etch-a-sketch around.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //Get the distance the mouse pointer moved.
            Size delta = new Size(e.X - MouseState.mPrevPos.X, e.Y - MouseState.mPrevPos.Y);

            //Check if this is the left mouse button that was pressed.
            if (e.Button == System.Windows.Forms.MouseButtons.Left) 
            {
                //If the pointer is over the vertical dial "button" handle the mouse movment acordingly.
                if (((e.X > (int)verticalButtonBounds.Left) && (e.X < (int)verticalButtonBounds.Right)) && 
                    ((e.Y >= (int)verticalButtonBounds.Top) && (e.Y < (int)verticalButtonBounds.Bottom)) ||
                    (MouseState.movingScribeVertical))
                {
                    verticalButton_Pressed(e.Location);
                }
                //If the pointer is over the horizontal dial "button" handle the mouse movment acordingly.
                else if (((e.X > (int)horizontalButtonBounds.Left) && (e.X < (int)horizontalButtonBounds.Right)) && 
                        ((e.Y >= (int)horizontalButtonBounds.Top) && (e.Y < (int)horizontalButtonBounds.Bottom)) ||
                        (MouseState.movingScribeHorizontal ))
                {
                    horizontalButton_Pressed(e.Location);
                }
                //Anywhere else on the form move the form.
                else if ((!MouseState.movingScribeVertical) && (!MouseState.movingScribeHorizontal))
                {
                    this.Location += delta;

                    //If the form has moved a significant amount assume the user is "shaking" the etch-a-sketch.
                    if ((delta.Height > 500) ||
                        (delta.Width > 500))
                        ShakeIt();

                    MouseState.mPrevDelta = delta;
                }
            }

        }

        /// <summary>
        /// Handle maouse down events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //Save the mouse pointer position.
            MouseState.mPrevPos = new Point(e.X, e.Y);

#if DEBUG
            //Print the mouse pointer position for debugging.
            DrawString(String.Format("Mouse X: {0}, Y: {1}", e.X, e.Y), 150.0f, 50.0f);
#endif
        }

        /// <summary>
        /// Handle mouse up events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            //Save the mouse pointer.
            MouseState.mPrevPos = new Point(e.X, e.Y);

            //Reset the drawing flags.
            MouseState.movingScribeVertical = false;
            MouseState.movingScribeHorizontal = false;

#if DEBUG
            //Erase the mouse position string.
            Erase(new Rectangle(150, 50, 1000, 100));
#endif
        }

        /// <summary>
        /// Right click context menu item draw circle counter clockwise.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawDotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawCircleCCW(ScribePosition, 50);
        }

        /// <summary>
        /// Right click context menu item draw circle clockwise.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawDotCwToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawCircleCW(ScribePosition, 50);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            drawDot(verticalDialIndicator.location, 10);
            drawDot(horizontalDialIndicator.location, 10);
        }

    }
}
