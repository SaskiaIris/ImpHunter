using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ImpHunter {
    class Cannon : GameObjectList {
        PhysicsObject carriage, barrel;

        // The whole cannon wants to have an acceleration instead of just its children.
        Vector2 acceleration;
		float friction = 0.99f;
        
        /// <summary>
        /// Returns / sets the acceleration of the cannon.
        /// </summary>
        public Vector2 Acceleration {
            get { return acceleration; }
            set { acceleration = value; }
        }

        /// <summary>
        /// Returns the barrel of the cannon.
        /// </summary>
        public PhysicsObject Barrel {
            get { return barrel; }
        }

        /// <summary>
        /// Returns the carriage of the cannon.
        /// </summary>
        public PhysicsObject Carriage {
            get { return carriage; }
        }

        /// <summary>
        /// Cannon constructor which builds the cannon out of the carriage and barrel.
        /// </summary>
        public Cannon() {
            carriage = new PhysicsObject("spr_cannon_carriage");
            carriage.Origin = carriage.Center;

            Add(barrel = new PhysicsObject("spr_cannon_barrel"));
            barrel.Origin = new Vector2(barrel.Center.X, barrel.Center.Y + 20);
            barrel.Position = new Vector2(carriage.Position.X, carriage.Position.Y);

            Add(carriage);
        }

        /// <summary>
        /// Adds an acceleration to the cannon when a key is pressed and aims the barrel at the position of the mouse.
        /// </summary>
        /// <param name="inputHelper"></param>
        public override void HandleInput(InputHelper inputHelper) {
            base.HandleInput(inputHelper);

			if(inputHelper.IsKeyDown(Keys.A) || inputHelper.IsKeyDown(Keys.Left))
			{
				acceleration = new Vector2(-2f, 0);
			}

			if (inputHelper.IsKeyDown(Keys.D) || inputHelper.IsKeyDown(Keys.Right))
			{
				acceleration = new Vector2(2f, 0);
			}

			if(!inputHelper.IsKeyDown(Keys.D) && !inputHelper.IsKeyDown(Keys.A)
				&& !inputHelper.IsKeyDown(Keys.Left) && !inputHelper.IsKeyDown(Keys.Right))
			{
				acceleration = new Vector2(0, 0);
			}			
		}

		/// <summary>
		/// Moves the cannon based on an acceleration, slowing it down with a friction until it comes to a full stop.
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime) {
            base.Update(gameTime);

			velocity += acceleration;
			velocity *= friction;

			if (velocity.X < 10 && acceleration.X == 0 && velocity.X > -10)
			{
				velocity.X = 0;
			}
		}
        
        /// <summary>
        /// Checks wheter the cannon collides with an object horizontally and bounces it when it does.
        /// </summary>
        /// <param name="other"></param>
        public void CheckBounce(SpriteGameObject other) {
            if (!carriage.CollidesWith(other)) return;

            CollisionResult side = carriage.CollisionSide(other);

            switch (side) {
                case CollisionResult.LEFT:
                    position.X = other.Position.X + other.Width + carriage.Center.X;
                    break;
                case CollisionResult.RIGHT:
                    position.X = other.Position.X - carriage.Center.X;
                    break;
            }
        }
    }
}