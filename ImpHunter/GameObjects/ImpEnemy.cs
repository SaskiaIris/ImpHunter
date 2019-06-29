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
		private float maximumSpeed = 100;
		private float maximumSteering = 15;
		private float arivalRadius = 200;

		public ImpEnemy(Fortress fort = null, float scale = 1) : base("spr_imp_flying")
		{
			//Zorgen dat de imp binnen de torens van het fort begint
			if(fort != null)
			{
				SpriteGameObject towerOne = (SpriteGameObject) fort.Towers.Children[0];
				SpriteGameObject towerTwo = (SpriteGameObject)fort.Towers.Children[1];
				this.position = new Vector2(GameEnvironment.Random.Next((int)(towerOne.Position.X + towerOne.Width), (int)towerTwo.Position.X), -100);

			}

			Velocity = new Vector2(0, maximumSpeed);
			mass = 2;
			Scale = scale;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		//Hiermee zoekt en gaat de imp naar het kanon
		public void SteerTowards(SpriteGameObject other)
		{
			Vector2 targetPosition = new Vector2((other.GlobalPosition.X - other.Width / 2), other.GlobalPosition.Y - other.Height / 2);
			Vector2 newVelocity = targetPosition - GlobalPosition;

			if (Vector2.Distance(other.GlobalPosition, GlobalPosition) >= arivalRadius)
			{
				//Seeking behaviour
				newVelocity.Normalize();
				newVelocity *= maximumSpeed;

				Vector2 steeringForce = newVelocity - Velocity;
				steeringForce = Truncate(steeringForce, maximumSteering) / mass;

				Velocity += steeringForce;
				Velocity = Truncate(Velocity, maximumSpeed);
			}
			else
			{
				//Arrival behaviour
				if (Vector2.Distance(targetPosition, GlobalPosition) < 10) {
					Velocity = Vector2.Zero;
				}
				else
				{
					Velocity = newVelocity / 2;
				}
            }
		}

		//Hiermee springen/cirkelen de kleine imps om de grote imp heen
		public void SpringTowards(SpriteGameObject other)
		{
			float springConstant = 0.7f;
			Vector2 springForce = (other.GlobalPosition - GlobalPosition) * springConstant;
			force += springForce;
		}

		//Truncate functie waardoor de vector niet langer dan een bepaalde lengte kan worden
		private Vector2 Truncate(Vector2 vector, float length)
		{
			if(vector.LengthSquared() > length * length)
			{
				vector.Normalize();
				vector *= length;
			}
			return vector;
		}
	}
}
