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
		private float speed = 450;
		//private float gravity = 7.1f;
		private float gravity = 4f;
		public CannonBall(Vector2 startPosition, Vector2 startingSpeed) : base("spr_cannon_ball")
		{
			origin = Center;
			this.position = startPosition;
			this.velocity = startingSpeed * speed;
			this.acceleration = new Vector2(-0.99f, gravity);
			//this.acceleration = new Vector2(gravity, gravity);
		}

		public override void HandleInput(InputHelper inputHelper)
		{
			base.HandleInput(inputHelper);
			
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			/*
			if(this.velocity.X < 1)
			{
				this.acceleration.X = 0;
			}

			if(this.velocity.Y < 1)
			{
				this.acceleration.Y = 0;
				this.velocity.Y = 0;
			}
			
			if (this.position.Y >= 509)
			{
				this.position.Y = 510;
			}*/
		}

		public void CheckBounce(SpriteGameObject other)
		{
			if (!this.CollidesWith(other)) return;

			CollisionResult side = this.CollisionSide(other);

			switch (side)
			{
				case CollisionResult.LEFT:
					position.X = other.Position.X + other.Width + Width;
					velocity.X *= -1 * 0.7f;
					break;
				case CollisionResult.RIGHT:
					position.X = other.Position.X - Width;
					velocity.X *= -1 * 0.7f;
					break;
				case CollisionResult.TOP:
					position.Y = other.Position.Y - Height;
					velocity.Y *= -1 * 0.7f;
					break;
				case CollisionResult.BOTTOM:
					position.Y = other.Position.Y - Height;
					velocity.Y *= -1 * 0.7f;
					break;
			}
		}

		/*public void BounceOnWall(bool towerCollision, bool groundCollision)
		{
			this.velocity.X *= 0.99f;
			this.velocity.Y *= 0.99f;

			if(groundCollision && !towerCollision)
			{
				this.velocity.Y *= -1;
				
			} else if(towerCollision && !groundCollision)
			{
				this.velocity.X *= -1;
			} else if(towerCollision && groundCollision)
			{
				this.velocity *= -1;
			}
		}*/
	}
}
