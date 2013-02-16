using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace comp476_a1
{
    public class ASprite : Sprite
    {
        float gapDistance = 10.0f;
        float detectDistance = 20.0f;

        public ASprite(Model newModel, Vector3 newSize, Vector3 newPosition, float newVelocity, Vector3 newDirection, float newRotation, int newAlpha, int newColour)
        {
            maxVelocity = 0.75f;
            minVelocity = 0.2f;
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

            //  Resets rotation if it exceeds 2pi
            if (Math.Abs(rotation) > 2 * Math.PI)
            {
                rotation = 0.0f;
            }
            else if (Math.Abs(rotation) <= 0.0f)
            {
                rotation += (float)(2 * Math.PI);
            }


            if (Game1.heuristic)
            {
                if (SpriteManager.target.velocity < 0.15f)
                {
                    SeekOneA(SpriteManager.target);
                }
                else
                {
                    SeekOneB(SpriteManager.target);
                }
            }
            else
            {
                if (SpriteManager.target.velocity < 0.15f)
                {
                    SeekTwoA(SpriteManager.target);
                }
                else
                {
                    SeekTwoB(SpriteManager.target);
                }
            }
            //  Movement
            position += velocity * direction;
        }


        public override void Draw(Matrix view, Matrix projection)
        {
            base.Draw(view, projection);
        }

        public override void Direction(float aRotation)
        {
            if (Game1.currentBehaviour.Equals(Game1.behaviour.TAG))
            {
                velocity = 0.80f;
            }
            float horizontalDirection = (float)(Math.Cos(aRotation));
            float verticalDirection = (float)(Math.Sin(aRotation - Math.PI));
            direction = new Vector3(0.0f, verticalDirection, horizontalDirection);
        }

        private void PickVelocity(Sprite target)
        {
            Vector3 difference = target.position - position;

            if (difference.Length() > 15.0f)
            {
                velocity = maxVelocity;
            }
            else if (velocity <= minVelocity)
            {
                velocity = minVelocity;
            }
            else
            {
                velocity -= 0.01f;
            }
            
        }

        private void SeekOneA(Sprite target)
        {
            Vector3 difference = target.position - this.position;
            float desiredRotation = (float)Math.Atan2(-difference.Y, difference.Z);

            if (difference.Length() <= gapDistance)
            {
                Direction(desiredRotation);
                velocity = 0.4f;
            }
            else
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
                        velocity = 0.0f;
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

        private void SeekOneB(Sprite target)
        {
            Vector3 difference = target.position - this.position;
            float desiredRotation = (float)Math.Atan2(-difference.Y, difference.Z);

            float rotationDifference = Math.Abs(rotation - desiredRotation);

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
                    velocity = 0.0f;
                }
            }
            else
            {
                rotation = desiredRotation;
                velocity = 0.30f;
                Direction(rotation);
            }
            

        }


        private void SeekTwoA(Sprite target)
        {
            Vector3 difference = target.position - position;
            float desiredRotation = (float)Math.Atan2(-difference.Y, difference.Z);
            PickVelocity(target);

            if (!difference.Equals(Vector3.Zero))
            {                
                //if (gapDistance <= difference.Length())
                //{
                //    rotation = desiredRotation;
                //    Direction(desiredRotation);
                //}
                //else
                //{
                    float diff = Math.Abs((float)(desiredRotation - rotation));
                    if (diff < 0.06f)
                    {
                        rotation = desiredRotation;
                    }
                    else
                    {
                        if (rotation > desiredRotation)
                        {
                            diff = Math.Abs((float)(Math.PI + desiredRotation - rotation));
                            if (diff <= Math.PI)
                            {
                                rotation += 0.05f;
                            }
                            else
                            {
                                rotation -= 0.05f;
                            }
                        }
                        else if (rotation <= desiredRotation)
                        {
                            diff = Math.Abs((float)(Math.PI + rotation - desiredRotation));
                            if (diff < Math.PI)
                            {
                                rotation -= 0.05f;
                            }
                            else
                            {
                                rotation += 0.05f;
                            }
                        }
                    }
                    Direction(rotation);
                //} 
            }            
        }

        private void RotateTwo(Vector3 target)
        {
            Vector3 directionVector = target - position;
            float desiredRotation = 0.0f;

            if (Math.Abs(rotation) > 2 * Math.PI)
            {
                rotation = 0.0f;
            }
            else if (Math.Abs(rotation) <= 0.0f)
            {
                rotation += (float)(2 * Math.PI);
            }

            if (!directionVector.Equals(Vector3.Zero))
            {
                directionVector = Vector3.Normalize(directionVector);
                desiredRotation = (float)Math.Atan2(-directionVector.Y, directionVector.Z);

                if (rotation > desiredRotation)
                {
                    float diff = Math.Abs((float)(Math.PI + desiredRotation - rotation));
                    if (diff <= Math.PI)
                    {
                        rotation += 0.05f;
                    }
                    else
                    {
                        rotation -= 0.05f;
                    }
                }
                else if (rotation <= desiredRotation)
                {
                    float diff = Math.Abs((float)(Math.PI + rotation - desiredRotation));
                    if (diff < Math.PI)
                    {
                        rotation -= 0.05f;
                    }
                    else
                    {
                        rotation += 0.05f;
                    }
                }
            }

            if (desiredRotation == rotation)
            {
                Direction(desiredRotation);
            }
        }

        private void SeekTwoB(Sprite target)
        {
            Vector3 difference = target.position - position;
            float desiredRotation = (float)Math.Atan2(-difference.Y, difference.Z);
            PickVelocity(target);

            if (!difference.Equals(Vector3.Zero))
            {
                //if (gapDistance <= difference.Length())
                //{
                //    rotation = desiredRotation;
                //    Direction(desiredRotation);
                //}
                //else
                //{
                    float diff = Math.Abs((float)(desiredRotation - rotation));
                    if (diff < 0.06f)
                    {
                        rotation = desiredRotation;
                    }
                    else
                    {
                        if (rotation > desiredRotation)
                        {
                            diff = Math.Abs((float)(Math.PI + desiredRotation - rotation));
                            if (diff <= Math.PI)
                            {
                                rotation += 0.05f;
                            }
                            else
                            {
                                rotation -= 0.05f;
                            }
                        }
                        else if (rotation <= desiredRotation)
                        {
                            diff = Math.Abs((float)(Math.PI + rotation - desiredRotation));
                            if (diff < Math.PI)
                            {
                                rotation -= 0.05f;
                            }
                            else
                            {
                                rotation += 0.05f;
                            }
                        }
                    }
                    Direction(rotation);
                //}
            } 
        }
    }
}
