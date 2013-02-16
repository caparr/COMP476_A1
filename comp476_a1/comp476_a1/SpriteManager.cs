using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace comp476_a1
{
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {

        //  Textures
        SpriteBatch spriteBatch;
        Model sprite;        
        Model cube;

        //  Arena        
        Matrix arena;

        public static Sprite target;        
        public static ASprite seeker;
        List<CSprite> evaders;        

        public SpriteManager(Game1 game)
            : base(game)
        {
            base.Initialize();
        }

        public override void Initialize()
        {
            arena = Matrix.CreateScale(20.0f) *  Matrix.CreateTranslation(Vector3.Zero);

            InitializeTarget();
            seeker = InitializeSeeker();

            evaders = new List<CSprite>();
            evaders.Add(InitializeEvader());
            evaders.Add(InitializeEvader());            
            base.Initialize();
        }

        private void InitializeTarget()
        {
            float velocity = 0.0f;
            float rotation = 0.0f;
            int alpha = 1;
            int colour = 1;

            Vector3 size = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 position = Vector3.Zero;
            Vector3 direction = Vector3.Zero;

            if (Game1.currentBehaviour.Equals(Game1.behaviour.TAG))
            {
                target = new CSprite(sprite, size, position, velocity, direction, rotation, alpha, colour);
            }
            else
            {
                target = new PCS(sprite, size, position, velocity, direction, rotation, alpha, colour);
            }

        }

        private ASprite InitializeSeeker()
        {
            float velocity = 0.0f;
            float rotation = 0.0f;
            int alpha = 1;
            int colour = 0;

            Vector3 size = new Vector3(1.0f, 1.0f, 1.0f);
            float yPos = Game1.seed.Next(0, 60);
            yPos = (yPos>30) ? 30 - yPos : yPos;

            float zPos = Game1.seed.Next(0, 70);
            zPos = (zPos>35) ? 35 - zPos : zPos;

            Vector3 position = new Vector3(0.0f, yPos, zPos);
            Vector3 direction = Vector3.Zero;

            return new ASprite(sprite, size, position, velocity, direction, rotation, alpha, colour);
        }

        private CSprite InitializeEvader()
        {
            float velocity = 0.0f;
            float rotation = 0.0f;
            int alpha = 1;
            int colour = 2;

            Vector3 size = new Vector3(1.0f, 1.0f, 1.0f);
            float yPos = Game1.seed.Next(0, 60);
            yPos = (yPos > 30) ? 30 - yPos : yPos;

            float zPos = Game1.seed.Next(0, 70);
            zPos = (zPos > 35) ? 35 - zPos : zPos;

            Vector3 position = new Vector3(0.0f, yPos, zPos);
            Vector3 direction = Vector3.Zero;
            
            return new CSprite(sprite, size, position, velocity, direction, rotation, alpha, colour);
        }

        protected override void LoadContent()
        {
            // Load ACS and PCS
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            sprite = Game.Content.Load<Model>("comp476_model");
            cube = Game.Content.Load<Model>("cube");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Collision(seeker, target) && Game1.currentBehaviour.Equals(Game1.behaviour.TAG))
            {
                YourIt();
            }
            Warp(target);
            Warp(seeker);
            foreach (var evader in evaders)
            {
                Warp(evader);
            }
            target.Update(gameTime);
            seeker.Update(gameTime);
            foreach (var evader in evaders)
            {
                evader.Update(gameTime);
            }            
        }

        private void YourIt()
        {
            Vector3 seekerPosition = seeker.position;
            Vector3 targetPosition = target.position;

            CSprite evader = InitializeEvader();
            evader.position = seeker.position;
            evaders.Add(evader);

            seeker = InitializeSeeker();
            seeker.position = targetPosition;

            target = evaders[0];
            target.colour = 1;
            evaders.RemoveAt(0);
            

        }
        public static void Teleport()
        {
            float yPos = Game1.seed.Next(0, 66);
            yPos = (yPos>33) ? 33 - yPos : yPos;

            float zPos = Game1.seed.Next(0, 76);
            zPos = (zPos>33) ? 33 - zPos : zPos;

            target.position = new Vector3(0.0f, yPos, zPos);
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.End();
            
            //  Draw arena
            DrawModel(cube, arena, Game1.view, Game1.projection);



            target.Draw(Game1.view, Game1.projection);
            seeker.Draw(Game1.view, Game1.projection);
            foreach (var evader in evaders)
            {
                evader.Draw(Game1.view, Game1.projection);
            }
            base.Draw(gameTime);
        }


        private void Warp(Sprite sprite)
        {
            if (sprite.position.Z > 33.0f)
            {
                sprite.position = new Vector3(sprite.position.X, sprite.position.Y, -33.0f);
            }
            else if (sprite.position.Z < -33.0f)
            {
                sprite.position = new Vector3(sprite.position.X, sprite.position.Y, 33.0f);
            }

            if (sprite.position.Y > 33.0f)
            {
                sprite.position = new Vector3(sprite.position.X, -33.0f, sprite.position.Z);
            }
            else if (sprite.position.Y < -33.0f)
            {
                sprite.position = new Vector3(sprite.position.X, 33.0f, sprite.position.Z);
            }
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = world * mesh.ParentBone.Transform;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.Alpha = 0.5f;
                }
                mesh.Draw();
            }
        }

        public bool Collision(Sprite it, Sprite notIt)
        {
            Vector3 difference = notIt.position - it.position;
            if (difference.Length() <= 0.25f)
            {
                return true;
            }
            return false;
            //for (int meshIndex1 = 0; meshIndex1 < it.model.Meshes.Count; meshIndex1++)
            //{
            //    BoundingSphere sphere1 = it.model.Meshes[meshIndex1].BoundingSphere;
            //    sphere1 = sphere1.Transform(it.world);

            //    for (int meshIndex2 = 0; meshIndex2 < notIt.model.Meshes.Count; meshIndex2++)
            //    {
            //        BoundingSphere sphere2 = notIt.model.Meshes[meshIndex2].BoundingSphere;
            //        sphere2 = sphere2.Transform(notIt.world);

            //        if (sphere1.Intersects(sphere2))
            //            return true;
            //    }
            //}
            //return false;
        }

    }
}
