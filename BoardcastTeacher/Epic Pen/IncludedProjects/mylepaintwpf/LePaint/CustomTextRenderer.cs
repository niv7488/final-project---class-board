using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

using System.Reflection;

using LePaint.Shapes;
using LePaint.Controller;

namespace LePaint
{
    public class CustomTextRenderer : FrameworkElement
    {
        FormattedText text = new FormattedText("Hello, world",
            CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
            new Typeface("Verdana"), 24, Brushes.Black);

        ToolShapes tools;

        public CustomTextRenderer()
        {
            tools = ToolShapes.Self;
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            foreach (LeShape shape in tools)
            {
                if (shape.Name == Tool)
                {
                    shape.DrawText(drawingContext);
                    shape.Draw(drawingContext);
                }
            }
            //drawingContext.DrawText(text, new Point(0, 0));
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            text.MaxTextWidth = availableSize.Width;
            text.MaxTextHeight = availableSize.Height;
            return new Size(text.Width, text.Height);
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            text.MaxTextWidth = finalSize.Width;
            text.MaxTextHeight = finalSize.Height;
            return finalSize;
        }

        public static readonly DependencyProperty ToolProperty;
        static CustomTextRenderer()
        {
            PropertyMetadata metaData;

            metaData = new PropertyMetadata("Square");

            ToolProperty = DependencyProperty.Register(
                "Tool", typeof(string), typeof(CustomTextRenderer),
                metaData);
        }

        public string Tool
        {
            get
            {
                return (string)GetValue(ToolProperty);
            }
            set
            {
                SetValue(ToolProperty, value);
            }
        }

    }
}
