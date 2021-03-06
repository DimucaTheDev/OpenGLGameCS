using OpenGLGameCS.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace OpenGLGameCS.Game.Rendering
{
    public class Player : PhysicalObject
    {
        public float CameraRotationX { get; set; }
        public float CameraRotationY { get; set; }

        public Player()
        {

        }

        public override void Reset()
        {
            CameraRotationX = 0;
            CameraRotationY = 0;
            base.Reset();
        }

        public override void Update(MouseState mouse, KeyboardState keyboard)
        {
            base.Update(mouse, keyboard);
            Look(mouse.Delta.X, mouse.Delta.Y);
            float moveF = 0.0f, moveL = 0.0f, moveV = 0.0f;

            if (keyboard.IsKeyDown(Keys.W))
                moveF += 1.0f;

            if (keyboard.IsKeyDown(Keys.S))
                moveF -= 1.0f;

            if (keyboard.IsKeyDown(Keys.A))
                moveL += 1.0f;

            if (keyboard.IsKeyDown(Keys.D))
                moveL -= 1.0f;

            if (keyboard.IsKeyDown(Keys.LeftShift))
                moveV += 1.0f;

            if (keyboard.IsKeyDown(Keys.Space))
                moveV -= 1.0f;

            if (keyboard.IsKeyDown(Keys.R))
            {
                moveF = 0;
                moveL = 0;
                moveV = 0;
            }

            Move(moveF, moveL, moveV);
        }

        public void Move(float moveF, float moveL, float moveV)
        {
            float speed = 5.0f;
            // in order to move forward in the direction of the camera, some matrix magic is needed
            // im not entirely sure how this works but it does lol
            Matrix4 camToWorld = LocalToWorld() * Matrix4.RotateY(CameraRotationY);
            Velocity += camToWorld.MultiplyDirection(new Vector3(-moveL, -moveV, -moveF)) * (speed * GameTime.Delta);

/*            if (moveL > 0)
                moveL -= 0.1f;
            if (moveF > 0)
                moveF -= 0.1f;
            if (moveV > 0)
                moveV -= 0.1f;*/
        }

        public void Look(float x, float y)
        {
            float sensitivity = 0.001f;
            // the reason you take Y from X is due to the matrix stuff,
            // the world is a bit inverted basically, so Y affects X
            CameraRotationX -= y * sensitivity;
            CameraRotationY -= x * sensitivity;

            // sort of clamp the X and Y looking
            if (CameraRotationX > (MathF.PI / 2))
            {
                CameraRotationX = MathF.PI / 2;
            }
            else if (CameraRotationX < ((-MathF.PI) / 2))
            {
                CameraRotationX = ((-MathF.PI) / 2);
            }

            if (CameraRotationY > MathF.PI)
            {
                CameraRotationY -= MathF.PI * 2;
            }
            else if (CameraRotationY < (-MathF.PI))
            {
                CameraRotationY += MathF.PI * 2;
            }
        }

        public Matrix4 WorldToCamera()
        {
            return 
                Matrix4.RotateX(-CameraRotationX) *
                Matrix4.RotateY(-CameraRotationY) *
                WorldToLocal();
        }
    }
}
