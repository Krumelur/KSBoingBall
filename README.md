KSBoingBall
===========

Challenged by Xamarin's Recipe Cook-off (http://blog.xamarin.com/xamarin-recipe-cook-off/)
I wrote this little demo app for iOS 7 which covers the following topics:

 * SpriteKit in general.
 * Creating a UIViewController and adding an SKScene to it.
 * Implement interaction with the scene by standard UIControls and device shaking.
 * Demonstrate how to use the physics engine of SpriteKit to make a ball bounce realistically.
 * Show how to create SKSpriteNodes from a CGContext.
 * Show how to load an animated SKSpriteNode from individual textures.
 * Demonstrate loading and using particle systems created with Xcode's particle editor.
 * Show how to implement collision detection using the physics engine.
 * Show how to play sound effects.

You can find a more detailed post about the project on my blog at http://krumelur.me/?p=164.

The recipe's idea is based on the good old Amiga Boing Ball demo from the 1980s.
I found a very good animation of the ball at http://www.amigalog.com/amiga-boing-ball-logo-and-animation/ for downloading to get me started. 

The result looks like this, but you have to see it in action. It is running at constant 60fps on my iPhone 5.

Without particle effects:
![KSBoingBall](https://raw.github.com/Krumelur/KSBoingBall/master/screenshot/screenshot1.png)

With particle effects enabled:
![KSBoingBall](https://raw.github.com/Krumelur/KSBoingBall/master/screenshot/screenshot2.png)

The project was built using Xamaring.iOS Indie edition 7.0.0.11.

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