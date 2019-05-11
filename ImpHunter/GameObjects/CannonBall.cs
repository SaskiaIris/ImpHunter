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
		public CannonBall(Vector2 startPosition, Vector2 startingSpeed) : base("spr_cannon_ball")
		{
			origin = Center;
			this.position = startPosition;
			this.acceleration = startingSpeed;
		}

		/// <summary>
		/// Adds an acceleration to the cannon when a key is pressed and aims the barrel at the position of the mouse.
		/// </summary>
		/// <param name="inputHelper"></param>
		public override void HandleInput(InputHelper inputHelper)
		{
			base.HandleInput(inputHelper);
			
		}
		/// <summary>
		/// Checks wheter the cannon collides with an object horizontally and bounces it when it does.
		/// </summary>
		/// <param name="other"></param>
		public void CheckBounce(SpriteGameObject other)
		{
			/*if (!carriage.CollidesWith(other)) return;

			CollisionResult side = carriage.CollisionSide(other);

			switch (side)
			{
				case CollisionResult.LEFT:
					position.X = other.Position.X + other.Width + carriage.Center.X;
					break;
				case CollisionResult.RIGHT:
					position.X = other.Position.X - carriage.Center.X;
					break;
			}*/
		}
	}
}
