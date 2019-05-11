using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

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

			/*if(crosshair.Position.X > cannon.Barrel.Origin.X)
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
			}*/

			if (crosshair.Position.X >= cannon.Position.X && crosshair.Position.Y >= cannon.Position.Y)
			{
				//TWEEDE KWADRANT
				rotationVectorBarrel.X = crosshair.Position.X - cannon.Position.X;
				rotationVectorBarrel.Y = crosshair.Position.Y - cannon.Position.Y;
				//rotationVectorBarrel.Y *= -1;
				//cannon.Barrel.Angle = (float)(Math.Atan2(rotationVectorBarrel.Y, rotationVectorBarrel.X) + (180 * 180 / 3.14));
				cannon.Barrel.Angle = (float)(Math.Atan2(rotationVectorBarrel.Y, rotationVectorBarrel.X) + (3.14/2));
			}
			else if (crosshair.Position.X >= cannon.Position.X && crosshair.Position.Y < cannon.Position.Y)
			{
				//EERSTE KWADRANT
				rotationVectorBarrel.X = crosshair.Position.X - cannon.Position.X;
				rotationVectorBarrel.Y = cannon.Position.Y - crosshair.Position.Y;
				//rotationVectorBarrel.X *= -1;
				//rotationVectorBarrel.Y *= -1;
				cannon.Barrel.Angle = (float)(Math.Atan2(-1 * rotationVectorBarrel.Y, rotationVectorBarrel.X) + (3.14/2));
			}
			else if (crosshair.Position.X < cannon.Position.X && crosshair.Position.Y >= cannon.Position.Y)
			{
				//DERDE KWADRANT
				rotationVectorBarrel.X = cannon.Position.X - crosshair.Position.X;
				rotationVectorBarrel.Y = crosshair.Position.Y - cannon.Position.Y;
				//rotationVectorBarrel.Y = cannon.Position.Y - crosshair.Position.Y;
				cannon.Barrel.Angle = (float)(Math.Atan2(rotationVectorBarrel.Y, -1 * rotationVectorBarrel.X) + (3.14 / 2));
			}
			else if (crosshair.Position.X < cannon.Position.X && crosshair.Position.Y < cannon.Position.Y)
			{
				//VIERDE KWADRANT
				rotationVectorBarrel.X = cannon.Position.X - crosshair.Position.X;
				rotationVectorBarrel.Y = cannon.Position.Y - crosshair.Position.Y;
				//rotationVectorBarrel.X *= -1;
				//rotationVectorBarrel.Y *= -1;
				//cannon.Barrel.Angle = (float)(Math.Atan2(rotationVectorBarrel.Y, rotationVectorBarrel.X) * -1);
				cannon.Barrel.Angle = (float)(Math.Atan2(-1 * rotationVectorBarrel.Y, -1 * rotationVectorBarrel.X) + (3.14 / 2));
			}
			else
			{
				//rotationVectorBarrel = new Vector2(1, 1);
			}

			/*Debug.WriteLine("cannon.Barrel.Origin.X = ", cannon.Barrel.Origin.X.ToString());
			Debug.WriteLine("cannon.Barrel.Origin.Y = ", cannon.Barrel.Origin.Y.ToString());
			Debug.WriteLine("crosshair.Position.X = ", crosshair.Position.X.ToString());
			Debug.WriteLine("crosshair.Position.Y = ", crosshair.Position.Y.ToString());
			Debug.WriteLine("rotationVectorBarrel.X = ", rotationVectorBarrel.X.ToString());
			Debug.WriteLine("rotationVectorBarrel.Y = ", rotationVectorBarrel.Y.ToString());
			Debug.WriteLine("cannonBarrelAngle = ", cannon.Barrel.Angle.ToString());*/

			/*if(crosshair.Position.X < cannon.Position.X)
			{
				cannon.Barrel.Angle = (float)Math.Atan2(-1 * rotationVectorBarrel.Y, rotationVectorBarrel.X);
			} else
			{*/

			//}

			/*if(crosshair.Position.X == cannon.Position.X)
			{
				cannon.Barrel.Angle = 0f;
			}*/


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

			if(inputHelper.KeyPressed(Keys.Space))
			{
				/*Debug.WriteLine(" = cannon.Barrel.Origin.X", cannon.Barrel.Origin.X.ToString());
				Debug.WriteLine(" = cannon.Barrel.Origin.Y", cannon.Barrel.Origin.Y.ToString());
				Debug.WriteLine(" ");
				Debug.WriteLine(" = cannon.Barrel.Position.X", cannon.Barrel.Position.X.ToString());
				Debug.WriteLine(" = cannon.Barrel.Position.Y", cannon.Barrel.Position.Y.ToString());
				Debug.WriteLine(" ");*/
				Debug.WriteLine(" = cannon.Position.X", cannon.Position.X.ToString());
				Debug.WriteLine(" = cannon.Position.Y", cannon.Position.Y.ToString());
				Debug.WriteLine(" ");
				Debug.WriteLine(" = inputHelper.MousePosition.X", inputHelper.MousePosition.X.ToString());
				Debug.WriteLine(" = inputHelper.MousePosition.Y", inputHelper.MousePosition.Y.ToString());
				Debug.WriteLine(" ");
				Debug.WriteLine(" = crosshair.Position.X", crosshair.Position.X.ToString());
				Debug.WriteLine(" = crosshair.Position.Y", crosshair.Position.Y.ToString());
				Debug.WriteLine(" ");
				Debug.WriteLine(" = rotationVectorBarrel.X", rotationVectorBarrel.X.ToString());
				Debug.WriteLine(" = rotationVectorBarrel.Y", rotationVectorBarrel.Y.ToString());
				Debug.WriteLine(" ");
				Debug.WriteLine(" = cannon.Barrel.Angle", cannon.Barrel.Angle.ToString());
				Debug.WriteLine(" ");
			}
		}
    }
}
