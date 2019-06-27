using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ImpHunter.GameObjects
{
	class ImpEnemy : PhysicsObject
	{
		public ImpEnemy() : base("spr_imp_falling")
		{
			
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
	}
}
