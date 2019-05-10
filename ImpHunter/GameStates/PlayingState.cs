using Microsoft.Xna.Framework;
using System;

namespace ImpHunter {
    class PlayingState : GameObjectList{
        Cannon cannon;
        Crosshair crosshair;
        Fortress fortress;

        private const int SHOOT_COOLDOWN = 20;
        private int shootTimer = SHOOT_COOLDOWN;
		Vector2 rotationVectorBarrel;

        /// <summary>
        /// PlayingState constructor which adds the different gameobjects and lists in the correct order of drawing.
        /// </summary>
        public PlayingState() {
			rotationVectorBarrel = new Vector2();

            Add(new SpriteGameObject("spr_background"));

            Add(cannon = new Cannon());
            cannon.Position = new Vector2(GameEnvironment.Screen.X / 2, 490);

            Add(fortress = new Fortress());

            // Always draw the crosshair last.
            Add(crosshair = new Crosshair());
        }
        
        /// <summary>
        /// Updates the PlayingState.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
			//cannon.Barrel.LookAt(crosshair, -90);

			if(fortress.CollidesWithTowers(cannon.Carriage))
			{
				cannon.Velocity *= -0.99f;
			}

			//cannon.Barrel.OffsetDegrees = 90;
			//cannon.Barrel.Angle = (float)Math.Atan2(cannon.Barrel.Position.Y - crosshair.Position.Y, -1 * (cannon.Barrel.Position.X - crosshair.Position.X));
			//cannon.Barrel.Angle = (float)Math.Atan2(crosshair.Position.Y - cannon.Barrel.Origin.Y, crosshair.Position.X - cannon.Barrel.Origin.Y);

			if(crosshair.Position.X > cannon.Barrel.Origin.X)
			{
				rotationVectorBarrel.X = crosshair.Position.X - cannon.Barrel.Origin.X;
			} else
			{
				rotationVectorBarrel.X = cannon.Barrel.Origin.X - crosshair.Position.X;
				rotationVectorBarrel.Y *= -1;
				
			}

			if(crosshair.Position.Y > cannon.Barrel.Origin.Y)
			{
				rotationVectorBarrel.Y = crosshair.Position.Y - cannon.Barrel.Origin.Y;
			} else
			{
				rotationVectorBarrel.Y = cannon.Barrel.Origin.Y - crosshair.Position.Y;
			}

			if(crosshair.Position.X < cannon.Position.X)
			{
				cannon.Barrel.Angle = (float)Math.Atan2(-1 * rotationVectorBarrel.Y, rotationVectorBarrel.X);
			} else
			{
				cannon.Barrel.Angle = (float)Math.Atan2(rotationVectorBarrel.Y, rotationVectorBarrel.X);
			}

			if(crosshair.Position.X == cannon.Position.X)
			{
				cannon.Barrel.Angle = 0f;
			}

			
        }

        /// <summary>
        /// Allows the player to shoot after a cooldown.
        /// </summary>
        /// <param name="inputHelper"></param>
        public override void HandleInput(InputHelper inputHelper) {
            base.HandleInput(inputHelper);

            shootTimer++;

            if (inputHelper.MouseLeftButtonPressed() && shootTimer > SHOOT_COOLDOWN) {
                crosshair.Expand(SHOOT_COOLDOWN);
                shootTimer = 0;
            }
		}
    }
}
