using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace comp476_a1
{
    public class CSprite : Sprite
    {
        float evadeDistance = 10.0f;
        float detectDistance = 40.0f;
        bool hasStopped = false;

        public CSprite(Model newModel, Vector3 newSize, Vector3 newPosition, float newVelocity, Vector3 newDirection, float newRotation, int newAlpha, int newColour)
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
            if (Game1.heuristic)
            {
                EvadeOne(SpriteManager.seeker);
            }
            else
            {
                EvadeTwo(SpriteManager.seeker);
            }
            position += velocity * direction;
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            base.Draw(view, projection);
        }

        public override void Direction(float aRotation)
        {
            float horizontalDirection = (float)(Math.Cos(aRotation));
            float verticalDirection = (float)(Math.Sin(aRotation - Math.PI));
            direction = new Vector3(0.0f, verticalDirection, horizontalDirection);
        }

        private void EvadeOne(Sprite target)
        {
            Vector3 difference = target.position - this.position;
            float desiredRotation = (float)Math.Atan2(difference.Y, -difference.Z);

            if (difference.Length() <= evadeDistance)
            {
                Direction(desiredRotation);
                velocity = 0.4f;
            }
            else
            {
                if (difference.Length() < detectDistance)
                {
                    if (Math.Abs(rotation - desiredRotation) > 0.05f)
                    {
                        //  Stopped condition
                        if (velocity <= 0.0f)
                        {
                            //  Rotate (turn on the spot away from the target)
                            if (rotation > desiredRotation)
                            {
                                rotation -= 0.05f;
                            }
                            else
                            {
                                rotation += 0.05f;
                            }
                        }
                        //  Slowing down condition
                        else
                        {
                            velocity -= 0.01f;
                        }
                    }
                    else
                    {
                        rotation = desiredRotation;
                        velocity = 0.30f;
                        Direction(rotation);
                    }
                }                
            }            
        }

        private void EvadeTwo(Sprite target)
        {
            Vector3 difference = target.position - this.position;
            float desiredRotation = (float)Math.Atan2(difference.Y, -difference.Z);

            if (difference.Length() <= evadeDistance)
            {
                rotation = desiredRotation;
                Direction(desiredRotation);
                velocity = 0.4f;
            }
            else
            {
                if (difference.Length() < detectDistance)
                {
                    if (Math.Abs(rotation - desiredRotation) > 0.05f)
                    {
                        //  Rotate (turn on the spot away from the target)
                        if (rotation > desiredRotation)
                        {
                            rotation -= 0.05f;
                        }
                        else
                        {
                            rotation += 0.05f;
                        }
                        Direction(rotation);
                    }
                    else
                    {
                        rotation = desiredRotation;
                        velocity = 0.30f;
                        Direction(rotation);
                    }
                }
            } 
        }
    }
}
