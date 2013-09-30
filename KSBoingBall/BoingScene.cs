// Author:
//       Krumelur Soft
//       René Ruppert <rene.ruppert@gmail.com>
//
// Copyright (c) 2013 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using MonoTouch.UIKit;
using MonoTouch.SpriteKit;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreImage;
using MonoTouch.Foundation;

namespace KSBoingBall
{
	/// <summary>
	/// Boing scene. This scene displays the background grid and the bouncing ball.
	/// </summary>
	public class BoingScene : SKScene
	{
		/// <summary>
		/// Defines the rotation speed of the ball. The lower the value, the faster it spins.
		/// </summary>
		const double ROTATION_SPEED = 0.02f;

		/// <summary>
		/// Defines the shadow offset of the bouncing ball.
		/// </summary>
		const float SHADOW_OFFSET = 50f;

		/// <summary>
		/// Initializes a new instance of the <see cref="Boing.BoingScene"/> class.
		/// </summary>
		/// <param name="size">Size of the scene in screen units.</param>
		public BoingScene(SizeF size) : base(size)
		{
			this.ScaleMode = SKSceneScaleMode.AspectFill;

			// We're true retro fans, so let's use the correct background color of the Amiga original version.
			this.BackgroundColor = UIColor.FromRGB (0, 0, 0);

			// Create the background grid and add it to the center of the scene.
			var gridNode = CreateBackgroundNode (this.Frame.Size);
			gridNode.Position = new PointF (0, 0);
			this.AddChild (gridNode);

			// Create our bouncing ball sprite.
			this.ballNode = CreateBallNode (true);
			// Add the ball to the scene.
			this.AddChild (this.ballNode);

			// Place the ball in the center of the screen to start.
			this.ballNode.Position = new PointF (this.Frame.Width * .5f, this.Frame.Height - this.ballNode.Frame.Height * .5f);


			// Create a shadow version of the ball. This does not have physics and gets repositioned whenever the ball itself has moved.
			var ballShadowNode = CreateBallNode (false);
			ballShadowNode.Alpha = 0.8f;
			ballShadowNode.Color = UIColor.Black;
			ballShadowNode.ColorBlendFactor = 0.8f;
			ballShadowNode.Position = new PointF (SHADOW_OFFSET, -SHADOW_OFFSET);
			ballShadowNode.ZPosition = this.ballNode.ZPosition - 1;
			// Add the shadow as a child to the ball.
			ballNode.AddChild (ballShadowNode);

			// Let our scene have some gravity that is way lower then real earth's gravity, so the ball bounces nice and slowly.
			this.PhysicsWorld.Gravity = new CGVector (0, -1.5f);

			// Create some walls around the scene to make the ball bounce.
			this.PhysicsBody = SKPhysicsBody.BodyWithEdgeLoopFromRect (this.Frame);
			// Let the collission detection system know that this is a wall.
			this.PhysicsBody.SetCategoryBitMask (CONTACT_BITS.Wall);

			// Xamarin nicely wraps the SKPhysicsWorldDelegate protocol into C# events.
			this.PhysicsWorld.DidBeginContact += this.HandleDidBeginContact;

			// And for fancyness: let it snow! This demonstrated how to easily load a particle system that was created using Xcode's particle editor.
			var particleSystem = NSKeyedUnarchiver.UnarchiveFile ("./assets/SnowParticles.sks") as SKEmitterNode;
			particleSystem.Position = new PointF (this.Frame.Width * .5f, this.Frame.Height);
			particleSystem.Name = "SnowParticle";
			this.particleRate = particleSystem.ParticleBirthRate;
			// Turn snow off initially.
			particleSystem.ParticleBirthRate = 0;
			particleSystem.ParticleTexture = SKTexture.FromImageNamed ("./assets/spark.png");
			this.AddChild(particleSystem);

			// Init ramdomizer.
			this.rand = new Random ();
		}

		Random rand;
		float particleRate;

		/// <summary>
		/// Toggles the snow.
		/// </summary>
		public void ToggleSnow ()
		{
			// Nodes can be found using a name.
			var particleSystem = this.GetChildNode ("SnowParticle") as SKEmitterNode;
			if (particleSystem.ParticleBirthRate <= 0f)
			{
				particleSystem.ParticleBirthRate = this.particleRate;
			} else
			{
				particleSystem.ParticleBirthRate = 0f;
			}
		}

		/// <summary>
		/// Kicks the ball.
		/// </summary>
		public void KickBall()
		{
			int dx = this.rand.Next (200, 600);
			int dy = this.rand.Next (200, 600);

			int multX = this.rand.NextDouble () > 0.5f ? +1 : -1;
			int multY = this.rand.NextDouble () > 0.5f ? +1 : -1;

			this.ballNode.PhysicsBody.ApplyImpulse (new CGVector (dx * multX, dy * multY));
		}

		/// <summary>
		/// This gets called by the physics simulation if two objects collide.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		void HandleDidBeginContact(object sender, EventArgs args)
		{
			var contact = sender as SKPhysicsContact;

			// If the ball collides with the wall, play a sound. This check is pointless because we only have the ball and the walls that collide.
			// But it's still there to show the concepts in case you have more bodies.
			if(contact.BodyA.IsOfCategory(CONTACT_BITS.Wall) && contact.BodyB.IsOfCategory(CONTACT_BITS.Ball))
			{
				// Say "boing" if we haven't just said boing to avoid boinginess.
				var now = DateTime.Now;
				if((now - this.lastContact).TotalMilliseconds > 700)
				{
					this.lastContact = now;
					var soundAction = SKAction.PlaySoundFileNamed ("./assets/slam.mp3", true);
					this.RunAction (soundAction);
				}
			}
		}

		DateTime lastContact;

		// Our bouncing ball sprite.
		SKSpriteNode ballNode;
	
		/// <summary>
		/// Helper to create the ball node.
		/// </summary>
		/// <param name="isMainBall">if TRUE the main ball image is created, otherwise the shadow</param>
		/// <returns>The ball node.</returns>
		static SKSpriteNode CreateBallNode(bool isMainBall)
		{
			// Init with the first PNG to get dimensions set up correctly.
			var ballNode = SKSpriteNode.FromImageNamed ("./assets/BallFrames/0001.png");

			// Weird issue with scaling: when running on the simulator, the ball is way too big. On the iPhone it is too small. I presume a bug here but maybe
			// I'm doing something wrong.
			float scaleFix = isMainBall && UIDevice.CurrentDevice.Model == "iPhone Simulator" ? 0.5f : 1f;
			ballNode.Scale = scaleFix;

			// Load the frames of the bouncing ball and save them as an SKAction that makes the ball sprite rotate.
			var ballFrameTextures = new List<SKTexture>();
			for(int frameIndex = 1; frameIndex <= 59; frameIndex++)
			{
				var filename = string.Format ("./assets/BallFrames/{0:0000}.png", frameIndex);
				var ballFrameTexture = SKTexture.FromImageNamed (filename);
				ballFrameTextures.Add (ballFrameTexture);
			}

			// Create an action that loops all the frames.
			var rotationAction = SKAction.AnimateWithTextures (ballFrameTextures.ToArray (), ROTATION_SPEED);

			// In order to have the loop being restarted, we wrap the action into another action.
			var endlessRotationAction = SKAction.RepeatActionForever (rotationAction);

			// Let the ball sprite run the endless rotation.
			ballNode.RunAction (endlessRotationAction);

			if (isMainBall)
			{
				// Apply physical behavior to the ball.
				ballNode.PhysicsBody = SKPhysicsBody.BodyWithCircleOfRadius (ballNode.Frame.Width * 0.5f);
				ballNode.PhysicsBody.LinearDamping = 0.001f;
				ballNode.PhysicsBody.Friction = 0.01f;
				// In our simulation we don't want rotation. The ball is supposed to animate "manually".
				ballNode.PhysicsBody.AllowsRotation = false;
				ballNode.PhysicsBody.Restitution = 0.93f;
				ballNode.PhysicsBody.Mass = 0.5f;
				ballNode.PhysicsBody.Velocity = new CGVector (200, -1f);

				// We need to inform the collission detection system about the category of our sprite.
				ballNode.PhysicsBody.SetCategoryBitMask (CONTACT_BITS.Ball);

				// We also let the system know that a ball should trigger a contact delegate in case it hits a wall.
				ballNode.PhysicsBody.SetTestBitMask (CONTACT_BITS.Wall);
			}

			return ballNode;
		}

		/// <summary>
		/// Helper method that creates the grid shown in the background of the scene.
		/// </summary>
		/// <returns>The background node.</returns>
		/// <param name="size">Size of the scene.</param>
		/// <param name="height">Height of the scene.</param>
		static SKNode CreateBackgroundNode(SizeF size)
		{
			// Use a SKShapeNode. This can display any arbitrary CG content.
			var shapeNode = new SKShapeNode ();
			var path = new CGPath();
			shapeNode.StrokeColor = UIColor.FromRGB (112, 80, 160);
			float cellSize = 32f;

			// Draw vertical lines.
			float x = 0f;
			do
			{
				path.MoveToPoint (x, 0);
				path.AddLineToPoint (x, size.Height);
				x += cellSize;
			} while(x < size.Width);

			// Draw horizontal lines.
			float y = 0f;
			do
			{
				path.MoveToPoint (0, y);
				path.AddLineToPoint (size.Width, y);
				y += cellSize;
			} while(y < size.Height);

			shapeNode.Path = path;

			return shapeNode;
		}
	}
}

