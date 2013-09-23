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
using MonoTouch.SpriteKit;

namespace KSBoingBall
{
	/// <summary>
	// Extensions methods.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Sets the test bit mask of a body. Allows using an enum instead of uint bit values.
		/// </summary>
		/// <param name="body">Body.</param>
		/// <param name="bits">Bits.</param>
		/// <param name="overwrite">If set to <c>true</c> overwrite.</param>
		public static void SetTestBitMask(this SKPhysicsBody body, CONTACT_BITS bits, bool overwrite = true)
		{
			if(body == null)
			{
				return;
			}

			if (overwrite)
			{
				// Overwrite all  bits.
				body.ContactTestBitMask = (uint)bits;
			}
			else{
				// Add missing bits.
				body.ContactTestBitMask |= (uint)bits;
			}
		}

		/// <summary>
		/// Sets the category bit mask of a body. Allows using an enum instead of uint bit values.
		/// </summary>
		/// <param name="body">Body.</param>
		/// <param name="bits">Bits.</param>
		/// <param name="overwrite">If set to <c>true</c> overwrite.</param>
		public static void SetCategoryBitMask(this SKPhysicsBody body, CONTACT_BITS bits, bool overwrite = true)
		{
			if(body == null)
			{
				return;
			}

			if (overwrite)
			{
				// Overwrite all  bits.
				body.CategoryBitMask = (uint)bits;
			}
			else{
				// Add missing bits.
				body.CategoryBitMask |= (uint)bits;
			}
		}

		/// <summary>
		/// Determines if a body belongs to a specific category.
		/// </summary>
		/// <returns><c>true</c> body is of the category; otherwise, <c>false</c>.</returns>
		/// <param name="body">Body.</param>
		/// <param name="bits">Bits.</param>
		/// <param name="matchAll">If set to <c>true</c> match all bits.</param>
		public static bool IsOfCategory(this SKPhysicsBody body, CONTACT_BITS bits, bool matchAll = false)
		{
			if(body == null)
			{
				return false;
			}

			if(matchAll)
			{
				// All bits must match
				return (body.CategoryBitMask & (uint)bits) == (uint)bits;
			}
			else
			{
				// Any bit must match
				return (body.CategoryBitMask & (uint)bits) > 0;
			}
		}
	}
}

