using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace comp476_a1
{
    public class PCS : Sprite
    {
        public PCS(Model newModel, Vector3 newSize, Vector3 newPosition, float newVelocity, Vector3 newDirection, float newRotation, int newAlpha, int newColour)
        {
            model = newModel;
            size = newSize;
            position = newPosition;
            velocity = newVelocity;
            direction = newDirection;
            rotation = newRotation;
            alpha = newAlpha;
            colour = newColour;
        }


        public override void Update(GameTime gameTime)
        {
            KeyboardActions(gameTime);
            Direction();
            this.position += direction * velocity;
        }

        public override void Draw(Matrix view, Matrix projection) 
        {
            base.Draw(view, projection);

        }

        private void KeyboardActions(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] keys = keyboardState.GetPressedKeys();
            
            foreach (Keys key in keys)
            {
                switch (key)
                {
                    case Keys.Right:
                        rotation -= 0.1f;
                        if (Math.Abs(rotation) > 2 * Math.PI)
                        {
                            rotation = 0.0f;
                        }
                        break;
                    case Keys.Left:
                        rotation += 0.1f;
                        if (Math.Abs(rotation) > 2 * Math.PI)
                        {
                            rotation = 0.0f;
                        }
                        break;
                    case Keys.Up:
                        velocity += 0.01f;                        
                        break;
                    case Keys.Down:
                        velocity -= 0.01f;
                        break;                    
                }                
            }
        }
        

        private void Direction()
        {
            float horizontalDirection = (float)(Math.Cos(rotation));
            float verticalDirection = (float)(Math.Sin(rotation - Math.PI));
            direction = new Vector3(0.0f, verticalDirection, horizontalDirection);
        }

        private float FindRotation(Vector3 target)
        {
            direction = target - position;

            direction.Normalize();

            //  Based on the calculation from Direction, Z normalized represents the amount it should be rotated by, just need to do inverse cosine
            return direction.Z;
        }
        
    }
}
