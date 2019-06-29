using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ImpHunter {

    public enum CollisionResult { NONE, LEFT, RIGHT, TOP, BOTTOM };

    /// <summary>
    /// Adjusted SpriteGameObject which will handle our physics. Overrides Draw method WITHOUT call to base.Draw.
    /// </summary>
    class PhysicsObject : SpriteGameObject {
        protected float offsetDegrees, mass = 1f;
        protected Vector2 acceleration, force;

        private float radians;

		/// <summary>
		/// Returns / sets the mass.
		/// </summary>
		public virtual float Mass {
			get { return mass; }
			set { mass = value; }
		}

		/// <summary>
		/// Returns / sets the acceleration.
		/// </summary>
		public virtual Vector2 Acceleration {
            get { return force/mass; }
            set { force = value; }
        }

        /// <summary>
        /// Returns / sets angle in radians (0 - 2*PI)
        /// </summary>
        public float Angle {
            get { return radians; }
            set { radians = value; }
        }

        /// <summary>
        /// Returns / sets the offset degree to draw the sprite with.
        /// </summary>
        public float OffsetDegrees {
            get { return offsetDegrees; }
            set { offsetDegrees = value; }
        }

        /// <summary>
        /// PhysicsObject constructor.
        /// </summary>
        /// <param name="assetname">Assetname in Content project.</param>
        public PhysicsObject(string assetname) : base(assetname) {
            Acceleration = Vector2.Zero;
        }

        /// <summary>
        /// Draws a sprite on angle and custom shade.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if (!visible || sprite == null) {
                return;
            }
            spriteBatch.Draw(sprite.Sprite, GlobalPosition, null, shade, radians - MathHelper.ToRadians(offsetDegrees), Origin, scale, SpriteEffects.None, 0);
        }


        /// <summary>
        /// Updates the velocity based on the acceleration.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            Velocity += Acceleration;
			force = Vector2.Zero;
            base.Update(gameTime);
        }

        /// <summary>
        /// Returns on which side of this object a collision takes place.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public CollisionResult CollisionSide(SpriteGameObject other) {
            float distanceToLeftEdge = GlobalPosition.X - Origin.X + Width - other.GlobalPosition.X - other.Origin.X;
            float distanceToRightEdge = other.GlobalPosition.X - other.Origin.X + other.Width - GlobalPosition.X - Origin.X;
            float xOverlap = distanceToLeftEdge < 0 || distanceToRightEdge < 0 ? 0 : 
                distanceToLeftEdge <= distanceToRightEdge ? Math.Abs(distanceToLeftEdge) : Math.Abs(distanceToRightEdge);

            float distanceToTopEdge = GlobalPosition.Y - Origin.Y + Height - other.GlobalPosition.Y - other.Origin.Y;
            float distanceToBottomEdge = other.GlobalPosition.Y - other.Origin.Y + other.Height - GlobalPosition.Y - Origin.Y;
            float yOverlap = distanceToTopEdge < 0 || distanceToBottomEdge < 0 ? 0 : 
                distanceToTopEdge <= distanceToBottomEdge ? Math.Abs(distanceToTopEdge) : Math.Abs(distanceToBottomEdge);

            if (xOverlap < yOverlap) {
                if (GlobalPosition.X - Origin.X < other.GlobalPosition.X - other.Origin.X)
                    return CollisionResult.RIGHT;
                else
                    return CollisionResult.LEFT;
            } else {
                if (GlobalPosition.Y - Origin.Y < other.GlobalPosition.Y - other.Origin.Y)
                    return CollisionResult.BOTTOM;
                else
                    return CollisionResult.TOP;  
            }
        }
    }
}
