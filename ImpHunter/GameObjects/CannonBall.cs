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
		public CannonBall(Vector2 startPosition, Vector2 startingSpeed) : base("spr_cannon_ball")
		{
			origin = Center;
			this.position = startPosition;
			this.velocity = startingSpeed * speed;
		}

		public override void HandleInput(InputHelper inputHelper)
		{
			base.HandleInput(inputHelper);
			
		}
	}
}
