using System;
using System.Collections.Generic;
using System.Drawing;
using Android.Views;

namespace StickyHeader.Animator
{
	public class AnimatorBuilder
	{
		public const float DefaultVelocityAnimator = 0.5f;

		private readonly IList<AnimatorBundle> listAnimatorBundles;

		public AnimatorBuilder()
		{
			listAnimatorBundles = new List<AnimatorBundle>(2);
		}

		public static AnimatorBuilder Create()
		{
			return new AnimatorBuilder();
		}

		public virtual AnimatorBuilder ApplyScale(View view, RectangleF finalRect)
		{
			if (view == null)
			{
				throw new ArgumentNullException("view");
			}

			var from = new RectangleF(view.Left, view.Top, view.Right, view.Bottom);
			var scaleX = 1f - finalRect.Width / from.Width;
			var scaleY = 1f - finalRect.Height/from.Height;

			return ApplyScale(view, scaleX, scaleY);
		}

		public virtual AnimatorBuilder ApplyScale(View view, float scaleX, float scaleY)
		{
			if (view == null)
			{
				throw new ArgumentNullException("view");
			}

			AnimatorBundle animatorScale = AnimatorBundle.Create(AnimatorBundle.TypeAnimation.Scale, view, scaleX, scaleY);

			AdjustTranslation(animatorScale);

			listAnimatorBundles.Add(animatorScale);

			return this;
		}

		/// <summary>
		///     Translate the top-left point of the view to finalPoint
		/// </summary>
		/// <param name="view"> </param>
		/// <param name="finalPoint">
		///     @return
		/// </param>
		public virtual AnimatorBuilder ApplyTranslation(View view, PointF finalPoint)
		{
			if (view == null)
			{
				throw new ArgumentNullException("view");
			}

			var from = new PointF(view.Left, view.Top);
			var translationX = finalPoint.X - from.X;
			var translationY = finalPoint.Y - from.Y;

			return ApplyTranslation(view, translationX, translationY);
		}

		public virtual AnimatorBuilder ApplyTranslation(View view, float translateX, float translateY)
		{
			if (view == null)
			{
				throw new ArgumentNullException("view");
			}

			AnimatorBundle animatorTranslation = AnimatorBundle.Create(AnimatorBundle.TypeAnimation.Translation, view,
				translateX, translateY);

			AdjustTranslation(animatorTranslation);

			listAnimatorBundles.Add(animatorTranslation);

			return this;
		}

		public virtual AnimatorBuilder ApplyFade(View viewToFade, float fade)
		{
			if (viewToFade == null)
			{
				throw new ArgumentNullException("viewToFade");
			}

			listAnimatorBundles.Add(AnimatorBundle.Create(AnimatorBundle.TypeAnimation.Fade, viewToFade, fade));

			return this;
		}

		/// <param name="viewToParallax"> </param>
		/// <param name="velocityParallax">
		///     the velocity to apply to the view in order to show the parallax effect. choose a velocity between 0 and 1 for
		///     better results
		///     @return
		/// </param>
		public virtual AnimatorBuilder ApplyVerticalParallax(View viewToParallax, float velocityParallax)
		{
			if (viewToParallax == null)
			{
				throw new ArgumentNullException("viewToParallax");
			}

			listAnimatorBundles.Add(AnimatorBundle.Create(AnimatorBundle.TypeAnimation.Parallax, viewToParallax,
				velocityParallax*-1));

			return this;
		}

		public virtual AnimatorBuilder ApplyVerticalParallax(View viewToParallax)
		{
			if (viewToParallax == null)
			{
				throw new ArgumentNullException("viewToParallax");
			}

			listAnimatorBundles.Add(AnimatorBundle.Create(AnimatorBundle.TypeAnimation.Parallax, viewToParallax,
				DefaultVelocityAnimator*-1));

			return this;
		}

		private void AdjustTranslation(AnimatorBundle newAnimator)
		{
			AnimatorBundle animatorScale = null, animatorTranslation = null;

			foreach (AnimatorBundle animator in listAnimatorBundles)
			{
				if (newAnimator.mView == animator.mView)
				{
					if (newAnimator.mTypeAnimation == AnimatorBundle.TypeAnimation.Scale &&
					    animator.mTypeAnimation == AnimatorBundle.TypeAnimation.Translation)
					{
						animatorScale = newAnimator;
						animatorTranslation = animator;
					}
					else if (newAnimator.mTypeAnimation == AnimatorBundle.TypeAnimation.Translation &&
					         animator.mTypeAnimation == AnimatorBundle.TypeAnimation.Scale)
					{
						animatorScale = animator;
						animatorTranslation = newAnimator;
					}

					if (animatorScale != null)
					{
						float? translationX = (float?) animatorTranslation.mValues[0] -
						                      (animatorTranslation.mView.Width*(float?) animatorScale.mValues[0]/2f);
						float? translationY = (float?) animatorTranslation.mValues[1] -
						                      (animatorTranslation.mView.Height*(float?) animatorScale.mValues[1]/2f);

						animatorTranslation.mValues[0] = translationX;
						animatorTranslation.mValues[1] = translationY;

						break;
					}
				}
			}
		}

		protected internal virtual void AnimateOnScroll(float boundedRatioTranslationY, float translationY)
		{
			foreach (AnimatorBundle animatorBundle in listAnimatorBundles)
			{
				switch (animatorBundle.mTypeAnimation)
				{
					case AnimatorBundle.TypeAnimation.Fade:
						animatorBundle.mView.Alpha = boundedRatioTranslationY; //TODO performance issues?
						break;

					case AnimatorBundle.TypeAnimation.Translation:
						animatorBundle.mView.TranslationX = (float) animatorBundle.mValues[0]*boundedRatioTranslationY;
						animatorBundle.mView.TranslationY = ((float) animatorBundle.mValues[1]*boundedRatioTranslationY) - translationY;
						break;

					case AnimatorBundle.TypeAnimation.Scale:
						animatorBundle.mView.ScaleX = 1f - (float) animatorBundle.mValues[0]*boundedRatioTranslationY;
						animatorBundle.mView.ScaleY = 1f - (float) animatorBundle.mValues[1]*boundedRatioTranslationY;
						break;

					case AnimatorBundle.TypeAnimation.Parallax:
						animatorBundle.mView.TranslationY = (float) animatorBundle.mValues[0]*translationY;
						break;

					default:
						break;
				}
			}
		}

		public virtual bool HasAnimatorBundles()
		{
			return listAnimatorBundles.Count > 0;
		}

		//public static RectangleF BuildViewRect(View view)
		//{
		//	//TODO get coordinates related to the header
		//	return new RectangleF(view.Left, view.Top, view.Right, view.Bottom);
		//}

		//public static PointF BuildPointView(View view)
		//{
		//	return new PointF(view.Left, view.Top);
		//}

		//public static float CalculateScaleX(RectangleF from, RectangleF to)
		//{
		//	return 1f - to.Width/@from.Width;
		//}

		//public static float CalculateScaleY(RectangleF from, RectangleF to)
		//{
		//	return 1f - to.Height/@from.Height;
		//}

		//public static float CalculateTranslationX(PointF from, PointF to)
		//{
		//	return to.X - from.X;
		//}

		//public static float CalculateTranslationY(PointF from, PointF to)
		//{
		//	return to.Y - from.Y;
		//}

		public class AnimatorBundle
		{
			public enum TypeAnimation
			{
				Scale,
				Fade,
				Translation,
				Parallax
			}

			internal readonly TypeAnimation mTypeAnimation;
			internal object[] mValues;
			internal View mView;

			internal AnimatorBundle(TypeAnimation typeAnimation)
			{
				mTypeAnimation = typeAnimation;
			}

			public static AnimatorBundle Create(TypeAnimation typeAnimation, View view, params object[] values)
			{
				var animatorBundle = new AnimatorBundle(typeAnimation);

				animatorBundle.mView = view;
				animatorBundle.mValues = values;

				return animatorBundle;
			}
		}
	}
}