using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using RectEx;
using System;

namespace OneLine {
	internal interface Slices : Slice, Drawable, IEnumerable<Slice> {
		float[] Weights { get; }
		float[] Widthes { get; }
		int CountPayload { get; }

		void Add(Slice slice);
		void AddBefore(Drawable drawable);
		void AddAfter(Drawable drawable);
	}

	internal class SlicesImpl: SliceImpl, Slices, IEnumerable<Slice> {

		private List<Slice> slices = new List<Slice>();
		private List<Drawable> before = new List<Drawable>();
		private List<Drawable> after = new List<Drawable>();

		public override float Weight { get { return Weights.Sum(); } }
		public override float Width { get { return Widthes.Sum(); } }
		
		public float[] Weights { get { return slices.Select(x => x.Weight).ToArray(); } }

		public float[] Widthes { get { return slices.Select(x => x.Width).ToArray(); } }

		public int CountPayload { 
			get { 
				return this.Sum(x => {
					if (x is Slices) {
						return (x as Slices).CountPayload;
					}
					else return 1;
				}); 
			} 
		}

		public SlicesImpl(){

		}

		#region Add

		public void Add(Slice slice){
			slices.Add(slice);
		}

		public void AddBefore(Drawable drawable) {
			before.Add(drawable);
		}

		public void AddAfter(Drawable drawable) {
			after.Add(drawable);
		}

		#endregion

		#region Enumerable

		public IEnumerator<Slice> GetEnumerator(){
			foreach (var slice in slices) {
				if (slice is Slices) {
					var children = slice as Slices;
					foreach (var child in children) {
						yield return child;
					}
				}
				else {
					yield return slice;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator(){
			return GetEnumerator();
		}

		#endregion

		#region Draw

		public override void Draw(Rect rect) {
			ForEach(rect, (r, x) => x.Draw(r));
		}

		public override void DrawHeader(Rect rect) {
			ForEach(rect, (r, x) => x.DrawHeader(r));
		}

		private void ForEach(Rect rect, Action<Rect, Drawable> action) {
            var rects = rect.Row(Weights, Widthes, 2);

			foreach (var drawable in before) {
				action(rect, drawable);
			}

            int rectIndex = 0;
            foreach (var slice in slices){
				action(rects[rectIndex], slice);
				rectIndex++;
            }

			foreach (var drawable in after) {
				action(rect, drawable);
			}
		}
		#endregion
	}
}