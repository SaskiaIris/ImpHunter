using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace ImpHunter {
    class PlayingState : GameObjectList{
        Cannon cannon;
        Crosshair crosshair;
        Fortress fortress;
		private GameObjectList balls;

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

			Add(balls = new GameObjectList());

            // Always draw the crosshair last.
            Add(crosshair = new Crosshair());
        }
        
        /// <summary>
        /// Updates the PlayingState.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

			if(fortress.CollidesWithTowers(cannon.Carriage))
			{
				cannon.Velocity *= -0.99f;
			}

			if (crosshair.Position.X >= cannon.Position.X && crosshair.Position.Y < cannon.Position.Y)
			{
				//EERSTE KWADRANT
				rotationVectorBarrel.X = crosshair.Position.X - cannon.Position.X;
				rotationVectorBarrel.Y = -1 * (cannon.Position.Y - crosshair.Position.Y);
			} else if (crosshair.Position.X >= cannon.Position.X && crosshair.Position.Y >= cannon.Position.Y)
			{
				//TWEEDE KWADRANT
				rotationVectorBarrel.X = crosshair.Position.X - cannon.Position.X;
				rotationVectorBarrel.Y = crosshair.Position.Y - cannon.Position.Y;
			}
			else if (crosshair.Position.X < cannon.Position.X && crosshair.Position.Y >= cannon.Position.Y)
			{
				//DERDE KWADRANT
				rotationVectorBarrel.X = -1 * (cannon.Position.X - crosshair.Position.X);
				rotationVectorBarrel.Y = crosshair.Position.Y - cannon.Position.Y;
			}
			else if (crosshair.Position.X < cannon.Position.X && crosshair.Position.Y < cannon.Position.Y)
			{
				//VIERDE KWADRANT
				rotationVectorBarrel.X = -1 * (cannon.Position.X - crosshair.Position.X);
				rotationVectorBarrel.Y = -1 * (cannon.Position.Y - crosshair.Position.Y);
			}
			else
			{
				rotationVectorBarrel = new Vector2(1, 1);
			}

			cannon.Barrel.Angle = (float)(Math.Atan2(rotationVectorBarrel.Y, rotationVectorBarrel.X) + (3.14 / 2));

			foreach(CannonBall bouncyBall in balls.Children)
			{
				foreach(SpriteGameObject toren in fortress.Towers.Children)
				{
					bouncyBall.CheckBounce(toren);
				}
				bouncyBall.CheckBounce(fortress.Wall);
				
				/*bool torenCollision = false;
				bool grondCollision = false;
				if(fortress.CollidesWithTowers(bouncyBall) || bouncyBall.Position.Y > 509)
				{
					//bouncyBall.Velocity *= -1;
					if(fortress.CollidesWithTowers(bouncyBall) == true)
					{
						torenCollision = true;
					}
					if(bouncyBall.Position.Y > 509)
					{
						grondCollision = true;
					}
					bouncyBall.BounceOnWall(torenCollision, grondCollision);
					//bouncyBall.Position = new Vector2(bouncyBall.Position.X, bouncyBall.Position.Y-1);
				}*/
				bouncyBall.Update(gameTime);
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
				//this.balls.Add(new CannonBall(new Vector2(cannon.Position.X, cannon.Position.Y - cannon.Barrel.Height/2 - 30), rotationVectorBarrel));
				//this.balls.Add(new CannonBall(new Vector2(cannon.Position.X + cannon.Barrel.Angle * cannon.Barrel.Width, cannon.Position.Y + cannon.Barrel.Angle * cannon.Barrel.Height), rotationVectorBarrel));
				//this.balls.Add(new CannonBall(new Vector2(cannon.Position.X - ((cannon.Barrel.Width/2) / cannon.Barrel.Angle), cannon.Position.Y -((cannon.Barrel.Height/2)/cannon.Barrel.Angle)), rotationVectorBarrel * 0.5f));
				//this.balls.Add(new CannonBall(cannon.Barrel.GlobalPosition+rotationVectorBarrel*cannon.Barrel.Height,rotationVectorBarrel));
				Vector2 angDirection = new Vector2((float)Math.Cos(cannon.Barrel.Angle-1.57), (float)Math.Sin(cannon.Barrel.Angle-1.57));
				Vector2 cannonBallPos = cannon.Barrel.GlobalPosition + angDirection * (cannon.Barrel.Height-5);
				
				Vector2 direction = crosshair.GlobalPosition - cannon.GlobalPosition;
				direction.Normalize();

				this.balls.Add(new CannonBall(cannonBallPos, direction));
			}

			if(inputHelper.KeyPressed(Keys.Space))
			{
				/*
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
				Debug.WriteLine(" ");*/
				int i = 0;
				foreach (CannonBall ballCount in balls.Children) { i++; }
				Debug.WriteLine(i.ToString());
			}
		}
    }
}
