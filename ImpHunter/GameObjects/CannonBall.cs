using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ImpHunter
{
	class CannonBall : PhysicsObject
	{
		private float speed = 300;
		private float gravity = 7.1f;
		public CannonBall(Vector2 startPosition, Vector2 startingSpeed) : base("spr_cannon_ball")
		{
			origin = Center;
			this.position = startPosition;
			this.velocity = startingSpeed * speed;
			this.acceleration = new Vector2(-0.99f, gravity);
		}

		public override void HandleInput(InputHelper inputHelper)
		{
			base.HandleInput(inputHelper);
			
		}

		public void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if(this.velocity.X < 0)
			{
				this.velocity.X = 0;
				this.acceleration.X = 0;
				this.Visible = false;
			}
		}

		public void CheckBounce(SpriteGameObject other)
		{
			if (!this.CollidesWith(other)) return;

			CollisionResult side = this.CollisionSide(other);

			switch (side)
			{
				case CollisionResult.LEFT:
					position.X = other.Position.X + other.Width + this.Center.X;
					break;
				case CollisionResult.RIGHT:
					position.X = other.Position.X - this.Center.X;
					break;
			}
		}

		public void BounceOnWall()
		{
			this.velocity.X *= -0.99f;
		}
	}
}
