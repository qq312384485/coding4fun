﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

using Coding4Fun.Toolkit.Controls.Common;

// This is a heavily modified version based on ColorSlider in their ColorPicker sample by 
// Author: Page Brooks
// Website: http://www.pagebrooks.com
namespace Coding4Fun.Toolkit.Controls
{
    public class ColorSlider : ColorBaseControl
    {
        const double HueSelectorSize = 24;
        bool _fromSliderChange;

        #region controls on template
        protected Grid Body;
        private const string BodyName = "Body";

        protected Rectangle SelectedColor;
        private const string SelectedColorName = "SelectedColor";

        protected SuperSlider Slider;
        private const string SliderName = "Slider";
        #endregion

        public ColorSlider()
        {
            DefaultStyleKey = typeof(ColorSlider);

            IsEnabledChanged += SuperSlider_IsEnabledChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Body = GetTemplateChild(BodyName) as Grid;
            Slider = GetTemplateChild(SliderName) as SuperSlider;

            if (Thumb == null)
                Thumb = new ColorSliderThumb();

            SelectedColor = GetTemplateChild(SelectedColorName) as Rectangle;
            
            SizeChanged += UserControl_SizeChanged;

            if (Slider != null)
            {
                Slider.ValueChanged += Slider_ValueChanged;

                if (Color.A == 0 && Color.R == 0 && Color.G == 0 && Color.B == 0)
                    Color = System.Windows.Media.Color.FromArgb(255, 6, 255, 0);
                else
                    UpdateLayoutBasedOnColor();
            }

			IsEnabledVisualStateUpdate();
        }

        void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SetColorFromSlider(e.NewValue);
        }

        private void SetColorFromSlider(double value)
        {
            _fromSliderChange = true;
            ColorChanging(ColorSpace.GetColorFromHueValue((int)value % 360));
            _fromSliderChange = false;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustLayoutBasedOnOrientation();
        }

        void SuperSlider_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IsEnabledVisualStateUpdate();
        }

        #region dependency properties


        public object Thumb
        {
            get { return (object)GetValue(ThumbProperty); }
            set { SetValue(ThumbProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Thumb.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbProperty =
            DependencyProperty.Register("Thumb", typeof(object), typeof(ColorSlider), new PropertyMetadata(null));

        

        public bool IsColorVisible
        {
            get { return (bool)GetValue(IsColorVisibleProperty); }
            set { SetValue(IsColorVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsColorVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsColorVisibleProperty =
            DependencyProperty.Register("IsColorVisible", typeof(bool), typeof(ColorSlider), new PropertyMetadata(true, OnIsColorVisibleChanged));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ColorSlider), new PropertyMetadata(Orientation.Vertical, OnOrientationPropertyChanged));
        #endregion

        private static void OnIsColorVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = d as ColorSlider;

            if (slider != null)
                slider.AdjustLayoutBasedOnOrientation();
        }

        private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slider = d as ColorSlider;
            
            if (slider != null)
                slider.AdjustLayoutBasedOnOrientation();
        }

        private void AdjustLayoutBasedOnOrientation()
        {
            if (Body == null ||
                Slider == null ||
                SelectedColor == null)
                return;

            var isVert = Orientation == Orientation.Vertical;

			IsEnabledVisualStateUpdate();
            
            Body.RowDefinitions.Clear();
            Body.ColumnDefinitions.Clear();

            if (isVert)
            {
                Body.RowDefinitions.Add(new RowDefinition());
                Body.RowDefinitions.Add(new RowDefinition());
            }
            else
            {
                Body.ColumnDefinitions.Add(new ColumnDefinition());
                Body.ColumnDefinitions.Add(new ColumnDefinition());
            }

            var thumb = ((FrameworkElement)Slider.Thumb);
            if (thumb != null)
            {
                thumb.Height = isVert ? HueSelectorSize : double.NaN;
                thumb.Width = isVert ? double.NaN : HueSelectorSize;
            }

            SelectedColor.SetValue(Grid.RowProperty, isVert ? 1 : 0);
            SelectedColor.SetValue(Grid.ColumnProperty, isVert ? 0 : 1);

            var sliderWidth = Slider.ActualWidth;
            var sliderHeight = Slider.ActualHeight;

            var squareSize = isVert ? sliderWidth : sliderHeight;

            SelectedColor.Height = SelectedColor.Width = squareSize;

            if (isVert)
            {
                Body.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                Body.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);
            }
            else
            {
                Body.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                Body.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Auto);
            }

            SelectedColor.Visibility = (IsColorVisible) ? Visibility.Visible : Visibility.Collapsed;
        }

        protected internal override void UpdateLayoutBasedOnColor()
        {
            if (_fromSliderChange)
                return;

            base.UpdateLayoutBasedOnColor();

            if(Slider != null)
                Slider.Value = Color.GetHue();
        }

        private void IsEnabledVisualStateUpdate()
        {
            VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);
			Slider.Background = IsEnabled ? ColorSpace.GetColorGradientBrush(Orientation) : ColorSpace.GetBlackAndWhiteGradientBrush(Orientation);
        }
    }
}
