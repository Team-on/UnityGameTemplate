using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OneLine {
	internal interface Drawable {
		void Draw(Rect rect);
		void DrawHeader(Rect rect);
	}

	internal class DrawableImpl : Drawable {

		private Action<Rect> draw;
		private Action<Rect> drawHeader;

		protected DrawableImpl() {}

		public DrawableImpl(Action<Rect> draw) 
		: this(draw, null) { 

		}

		public DrawableImpl(Action<Rect> draw, Action<Rect> drawHeader) {
			this.draw = draw;
			this.drawHeader = drawHeader;
		}

		public virtual void Draw(Rect rect){
			if (draw != null){
				draw(rect);
			}
		}

		public virtual void DrawHeader(Rect rect){
			if (drawHeader != null) {
				drawHeader(rect);
			}
		}
	}
}