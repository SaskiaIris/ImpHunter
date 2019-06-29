using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ImpHunter.GameObjects
{
	class ImpBoss : PhysicsObject
	{
		private Vector2[] waypoints = new Vector2[4];
		private float maximumSpeed = 200;
		private float maximumSteering = 15;
		private float arrivalRadius = 100;
		private int currentWayPoint = 0;

		public ImpBoss() : base("spr_imp_flying")
		{
			Position = new Vector2(GameEnvironment.Screen.X / 2, 50);
			mass = 1;

			//Waypoints waar de boss heen gaat
			waypoints[0] = new Vector2(158, 30);
			waypoints[1] = new Vector2(650, 100);
			waypoints[2] = new Vector2(400, 50);
			waypoints[3] = new Vector2(260, 180);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			//Het sturen tussen de verschillende waypoints
			if (SteerToPoint(waypoints[currentWayPoint])) {
				currentWayPoint++;
			}

			if (currentWayPoint > waypoints.Length - 1)
			{
				currentWayPoint = 0;
			}

		}

		//De functie die de impBoss naar de waypoints laat gaan en laat wisselen als hij dichtbij een waypoint komt
		public bool SteerToPoint(Vector2 other)
		{
			Vector2 targetPosition = new Vector2(other.X, other.Y);
			Vector2 newVelocity = targetPosition - GlobalPosition;

			//Deciding whether to seek or arrive
			if (Vector2.Distance(other, GlobalPosition) >= arrivalRadius)
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
				if (Vector2.Distance(targetPosition, GlobalPosition) < 10)
				{
					maximumSpeed = 200;
					return true;
				}
				else
				{
					newVelocity.Normalize();
					maximumSpeed = Vector2.Distance(targetPosition, GlobalPosition) / mass;
					Velocity = newVelocity * maximumSpeed;
				}
			}

			return false;
		}

		//Truncate functie waardoor de vector niet langer dan een bepaalde lengte kan worden
		private Vector2 Truncate(Vector2 vector, float length)
		{
			if (vector.LengthSquared() > length * length)
			{
				vector.Normalize();
				vector *= length;
			}
			return vector;
		}
	}
}

