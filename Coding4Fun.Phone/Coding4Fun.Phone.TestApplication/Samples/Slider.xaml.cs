﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class Slider : PhoneApplicationPage
    {
        public Slider()
        {
            InitializeComponent();
        }

        private void ResultSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SliderResult.Text = e.NewValue.ToString();
        }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			sliderVisTest.Visibility = (sliderVisTest.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
		}
    }
}