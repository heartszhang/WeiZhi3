﻿using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Bot
{
    public abstract class AnimatingPanel : Panel
    {
        private const double c_diff = 0.1;
        private const double c_terminalVelocity = 10000;

        private static readonly DependencyProperty DataProperty =
            DependencyProperty.RegisterAttached("Data", typeof (AnimatingPanelItemData), typeof (AnimatingPanel));

        private readonly CompositionTargetRenderingListener m_listener = new CompositionTargetRenderingListener();

        protected AnimatingPanel()
        {
            m_listener.Rendering += compositionTarget_Rendering;
            m_listener.WireParentLoadedUnloaded(this);
        }

        #region DPs

        public static readonly DependencyProperty AttractionProperty =
            CreateDoubleDP("Attraction", 2, FrameworkPropertyMetadataOptions.None, 0, double.PositiveInfinity, false);

        public static readonly DependencyProperty DampeningProperty =
            CreateDoubleDP("Dampening", 0.2, FrameworkPropertyMetadataOptions.None, 0, 1, false);
        public static readonly DependencyProperty VariationProperty =
            CreateDoubleDP("Variation", 1, FrameworkPropertyMetadataOptions.None, 0, true, 1, true, false);

        public double Attraction
        {
            get { return (double)GetValue(AttractionProperty); }
            set { SetValue(AttractionProperty, value); }
        }

        public double Dampening
        {
            get { return (double) GetValue(DampeningProperty); }
            set { SetValue(DampeningProperty, value); }
        }
        public double Variation
        {
            get { return (double) GetValue(VariationProperty); }
            set { SetValue(VariationProperty, value); }
        }

        #endregion

        protected static DependencyProperty CreateDoubleDP(
            string name,
            double defaultValue,
            FrameworkPropertyMetadataOptions metadataOptions,
            double minValue,
            double maxValue,
            bool attached)
        {
            return CreateDoubleDP(name, defaultValue, metadataOptions, minValue, false, maxValue, false, attached);
        }

        protected static DependencyProperty CreateDoubleDP(
            string name,
            double defaultValue,
            FrameworkPropertyMetadataOptions metadataOptions,
            double minValue,
            bool includeMin,
            double maxValue,
            bool includeMax,
            bool attached)
        {
            Contract.Requires(!double.IsNaN(minValue));
            Contract.Requires(!double.IsNaN(maxValue));
            Contract.Requires(maxValue >= minValue);

            ValidateValueCallback validateValueCallback = delegate(object objValue)
                {
                    var value = (double)objValue;

                    if (includeMin)
                    {
                        if (value < minValue)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (value <= minValue)
                        {
                            return false;
                        }
                    }
                    if (includeMax)
                    {
                        if (value > maxValue)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (value >= maxValue)
                        {
                            return false;
                        }
                    }

                    return true;
                };

            if (attached)
            {
                return DependencyProperty.RegisterAttached(
                    name,
                    typeof(double),
                    typeof(AnimatingPanel),
                    new FrameworkPropertyMetadata(defaultValue, metadataOptions), validateValueCallback);
            }
            else
            {
                return DependencyProperty.Register(
                    name,
                    typeof(double),
                    typeof(AnimatingPanel),
                    new FrameworkPropertyMetadata(defaultValue, metadataOptions), validateValueCallback);
            }
        }

        protected void ArrangeChild(UIElement child, Rect bounds)
        {
            m_listener.StartListening();

            var data = (AnimatingPanelItemData)child.GetValue(DataProperty);
            if (data == null)
            {
                data = new AnimatingPanelItemData();
                child.SetValue(DataProperty, data);
                Debug.Assert(Equals(child.RenderTransform, Transform.Identity));
                child.RenderTransform = data.Transform;

                data.Current = ProcessNewChild(child, bounds);
            }
            Debug.Assert(Equals(child.RenderTransform, data.Transform));

            //set the location attached DP
            data.Target = bounds.Location;

            child.Arrange(bounds);
        }

        protected virtual Point ProcessNewChild(UIElement child, Rect providedBounds)
        {
            return providedBounds.Location;
        }
        private static bool UpdateChildData(AnimatingPanelItemData data, double dampening, double attractionFactor,
                                            double variation)
        {
            if (data == null)
            {
                return false;
            }
            else
            {
                Debug.Assert(dampening > 0 && dampening < 1);
                Debug.Assert(attractionFactor > 0 && !double.IsInfinity(attractionFactor));

                attractionFactor *= 1 + (variation * data.RandomSeed - .5);

                Point newLocation;
                Vector newVelocity;

                bool anythingChanged =
                    GeoHelper.Animate(data.Current, data.LocationVelocity, data.Target,
                                      attractionFactor, dampening, c_terminalVelocity, c_diff, c_diff,
                                      out newLocation, out newVelocity);

                data.Current = newLocation;
                data.LocationVelocity = newVelocity;

                Vector transformVector = data.Current - data.Target;
                data.Transform.SetToVector(transformVector);

                return anythingChanged;
            }
        }

        private void compositionTarget_Rendering(object sender, EventArgs e)
        {
            double dampening = Dampening;
            double attractionFactor = Attraction*.01;
            double variation = Variation;

            bool shouldChange = false;
            for (int i = 0; i < Children.Count; i++)
            {
                shouldChange = UpdateChildData(
                    (AnimatingPanelItemData) Children[i].GetValue(DataProperty),
                    dampening,
                    attractionFactor,
                    variation) || shouldChange;
            }

            if (!shouldChange)
            {
                m_listener.StopListening();
            }
        }
        #region Nested type: AnimatingPanelItemData

        private class AnimatingPanelItemData
        {
            public readonly double RandomSeed = Util.Rnd.NextDouble();
            public readonly TranslateTransform Transform = new TranslateTransform();
            public Point Current;
            public Vector LocationVelocity;
            public Point Target;
        }

        #endregion
    }
}