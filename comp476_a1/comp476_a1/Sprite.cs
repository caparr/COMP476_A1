using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace comp476_a1
{
    public abstract class Sprite
    {
        public static Vector3[] skinColour = new Vector3[] 
        { 
            Color.Red.ToVector3(),          //  Red
            Color.Green.ToVector3(),        //  Green
            Color.Blue.ToVector3(),         //  Blue
            Color.Yellow.ToVector3(),       //  Yellow
            Color.Purple.ToVector3(),       //  Purple
            Color.BurlyWood.ToVector3(),    //  Orange
            Color.Cyan.ToVector3()          //  Cyan
        };


        public int alpha { get; set; }
        public float maxVelocity { get; set; }
        public float minVelocity { get; set; }
        public float velocity { get; set; }
        public float rotation { get; set; }

        public Model model { get; set; }
        public Matrix world { get; private set; }
        
        public Vector3 size { get; set; }
        public Vector3 position { get; set; }        
        public int colour { get; set; }
        public Vector3 direction { get; set; }
        public abstract void Update(GameTime gameTime);

        public virtual void Draw(Matrix view, Matrix projection)
        {
            Matrix world = Matrix.CreateScale(size) * Matrix.CreateRotationX(rotation) * Matrix.CreateTranslation(position);//* Matrix.CreateFromAxisAngle(new Vector3(1.0f, 0.0f, 0.0f), rotation);
            
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    //effect.AmbientLightColor = bubbleColour[colour];
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.LightingEnabled = true;
                    effect.Alpha = alpha;
                    effect.AmbientLightColor = skinColour[colour];
                }
                mesh.Draw();
            }
        }

        public virtual void Direction(float aRotation) { }
    }
}
