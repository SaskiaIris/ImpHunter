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
		private float[] speedsPerFrames = new float[10];
		private int previousIndex = 0;
		private int frameCounter = 0;
		
		public CannonBall(Vector2 startPosition, Vector2 startingSpeed) : base("spr_cannon_ball")
		{
			this.position = startPosition;
			this.velocity = startingSpeed * speed;
			this.acceleration = new Vector2(0,gravity);
		}

		public override void HandleInput(InputHelper inputHelper)
		{
			base.HandleInput(inputHelper);
			
		}

		public override void Update(GameTime gameTime)
		{
			Console.WriteLine(this.velocity);
			base.Update(gameTime);

			if(frameCounter % 30 == 0)
			{
				speedsPerFrames[previousIndex] = this.velocity.Y;
				if (previousIndex == speedsPerFrames.Length-1)
				{
					previousIndex = 0;
				}
				else
				{
					previousIndex++;
				}

			}

			frameCounter++;
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

		public float AverageSpeed()
		{
			float averageNumber = 0;
			int usableValues = 0;

			foreach(float speedValue in speedsPerFrames)
			{
				if(speedValue != 0f)
				{
					averageNumber += Math.Abs(speedValue);
					usableValues++;
				}
			}

			return averageNumber / usableValues;
		}
	}
}
