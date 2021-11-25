using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace BombSurvivor.Models
{
    public class Player : Sprite
    {
        public bool HasDied = false;
        public bool isFlip = false;
        

        public Player(Texture2D texture) : base(texture)
        {
            
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Move();

            foreach (var sprite in sprites)
            {
                if (sprite is Player) 
                    continue;
                if (sprite.Rectangle.Intersects(this.Rectangle))
                {
                    HasDied = true;
                }
            }

            Position += Velocity;
            //Mantiene el Sprite dentro de la pantalla
            Position.X = MathHelper.Clamp(Position.X, -30, Game1.ScreenWidth - (Rectangle.Width/3 +30));
            //Resetea la velocidad cuando el usuario no este presionando una teccla
            Velocity = Vector2.Zero;
        }

        private void Move()
        {
            if (Input == null)
                throw new Exception("Please assing a value to Input");

            if (Keyboard.GetState().IsKeyDown(Input.Left))
            {
                Velocity.X = -Speed;
                isFlip = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
            {
                Velocity.X = Speed;
                isFlip = true;

            }
                
                    
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(isFlip)
            {
                spriteBatch.Draw(_texture, Position,null,Color.White,0f,new Vector2(0,0),0.3f,SpriteEffects.FlipHorizontally,0);
                
            }
            else
            {
                spriteBatch.Draw(_texture, Position,null,Color.White,0f,new Vector2(0,0),0.3f,SpriteEffects.None,0);
            }
            
        }
    }
}