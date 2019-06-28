﻿using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using ImpHunter.GameObjects;

namespace ImpHunter {
    class PlayingState : GameObjectList{
        Cannon cannon;
        Crosshair crosshair;
        Fortress fortress;
		private GameObjectList balls;
		private GameObjectList imps;
		private ImpEnemy imp;
		private ImpBoss impBoss;
		private GameObjectList bossSwarm;

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
			Add(imps = new GameObjectList());
			imps.Add(imp = new ImpEnemy(fortress));

			impBoss = new ImpBoss();

			bossSwarm = new GameObjectList();
			bossSwarm.Add(new ImpEnemy(null, 0.5f));
			bossSwarm.Children[0].Position = new Vector2(impBoss.Position.X, impBoss.Position.Y + 80);

			bossSwarm.Add(new ImpEnemy(null, 0.5f));
			bossSwarm.Children[1].Position = new Vector2(impBoss.Position.X - 80, impBoss.Position.Y);

			bossSwarm.Add(new ImpEnemy(null, 0.5f));
			bossSwarm.Children[2].Position = new Vector2(impBoss.Position.X + 80, impBoss.Position.Y);

			Add(impBoss);
			Add(bossSwarm);



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

				if (bouncyBall.AverageSpeed() < 100 && Math.Abs(bouncyBall.Position.Y + bouncyBall.Height - fortress.Wall.Position.Y) < 3)
				{
					bouncyBall.Velocity = new Vector2(bouncyBall.Velocity.X, 0);
					bouncyBall.Acceleration = new Vector2(bouncyBall.Acceleration.X, 0);
					bouncyBall.Velocity *= 0.99f;
					if(bouncyBall.Velocity.X > -1 && bouncyBall.Velocity.X < 1)
					{
						bouncyBall.Velocity = new Vector2(0, 0);
						bouncyBall.Acceleration = new Vector2(0, 0);
					}
				}
				bouncyBall.Update(gameTime);
			}

			foreach(ImpEnemy impSelect in imps.Children)
			{
				impSelect.SteerTowards(cannon.Carriage);
			}

			foreach(ImpEnemy swarmImp in bossSwarm.Children)
			{
				swarmImp.SpringTowards(impBoss);
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

				Vector2 angDirection = new Vector2((float)Math.Cos(cannon.Barrel.Angle-1.57), (float)Math.Sin(cannon.Barrel.Angle-1.57));
				Vector2 cannonBallPos = cannon.Barrel.GlobalPosition + angDirection * (cannon.Barrel.Height-5);
				
				Vector2 direction = crosshair.GlobalPosition - cannon.GlobalPosition;
				direction.Normalize();

				this.balls.Add(new CannonBall(cannonBallPos, direction));
			}

			if(inputHelper.KeyPressed(Keys.Space))
			{
				int i = 0;
				foreach (CannonBall ballCount in balls.Children) { i++; }
				Debug.WriteLine(i.ToString());
			}
		}
    }
}
