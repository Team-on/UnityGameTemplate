using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OneLine {
	internal interface Slice : Drawable {
		float Weight {get;}
		float Width {get;}
	}
	internal class SliceImpl : DrawableImpl, Slice {

		public virtual float Weight { get; private set; }
		public virtual float Width { get; private set; }

		private string header;

		protected SliceImpl() : base() {

		}

		public SliceImpl(float weight, float width, Action<Rect> draw)
		: this(weight, width, draw, null){
		}

		public SliceImpl(float weight, float width, Action<Rect> draw, Action<Rect> drawHeader) 
		: base(draw, drawHeader) {
			Weight = weight;
			Width = width;
		}

	}
}