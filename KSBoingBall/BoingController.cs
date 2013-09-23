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

namespace KSBoingBall
{
	public class BoingController : UIViewController
	{
		public BoingController () : base()
		{
		}

		/// <summary>
		/// Gets direct access to the SpriteKit view this controller is using.
		/// </summary>
		public SKView SKView
		{
			get{
				// Our view is an SKView, so just cast it.
				return (SKView)this.View;
			}
		}

		public override void LoadView ()
		{
			// Let our view be an SKView.
			var skView = new SKView ();
			this.View = skView;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Add a button to switch snowing on or off.
			var btnSnow = new UIButton (UIButtonType.RoundedRect) {
				Frame = new RectangleF (10, this.View.Bounds.Height - 30, 100, 30),
				AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleRightMargin
			};
			btnSnow.SetTitle ("Toggle snow", UIControlState.Normal);
			btnSnow.TouchUpInside += (sender, e) => {
				if(this.SKView.Scene != null)
				{
					var boingScene = this.SKView.Scene as BoingScene;
					boingScene.ToggleSnow();
				}
			};


			this.View.Add (btnSnow);
		}

		public override bool CanBecomeFirstResponder
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Method invoked when a motion (shake) has finished.
		/// </summary>
		/// <param name="motion">Motion.</param>
		/// <param name="evt">Evt.</param>
		public override void MotionEnded (UIEventSubtype motion, UIEvent evt)
		{
			base.MotionEnded (motion, evt);

			// If the user shakes the device, kick the ball.
			if(motion == UIEventSubtype.MotionShake)
			{
				// Kick the ball.
				if(this.SKView.Scene != null)
				{
					var boingScene = this.SKView.Scene as BoingScene;
					boingScene.KickBall();
				}
			}
		}

		public override void ViewWillLayoutSubviews ()
		{
			base.ViewWillLayoutSubviews ();

			// Check if the scene is set up yet. It not, do it.
			// This is done on ViewWillLayoutSubviews to deal with screen size changes that happen when rotating the device.
			if(this.SKView.Scene == null)
			{
				// This is a demo app. Show some statistics.
				this.SKView.ShowsFPS = true;
				this.SKView.ShowsNodeCount = true;
				this.SKView.ShowsDrawCount = true;

				// Create our scene and bring it on the screen.
				var scene = new BoingScene (this.SKView.Bounds.Size);
				this.SKView.PresentScene (scene);
			}
		}

		public override bool ShouldAutorotate ()
		{
			return true;
		}
		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			if(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
			{
				return UIInterfaceOrientationMask.All;
			}
			else
			{
				return UIInterfaceOrientationMask.AllButUpsideDown;
			}
		}
	}
}

