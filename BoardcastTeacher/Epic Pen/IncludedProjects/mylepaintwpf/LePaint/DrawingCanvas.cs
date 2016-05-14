using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Windows.Shapes;



namespace LePaint
{
    /// <summary>
    /// Canvas used as host for DrawingVisual objects.
    /// Allows to draw graphics objects using mouse.
    /// </summary>
    public class ToolCanvas : Canvas
    {
        private GraphicsRect element = new GraphicsRect();

        public static readonly DependencyProperty ToolProperty;
        static ToolCanvas()
        {
            PropertyMetadata metaData;

            metaData = new PropertyMetadata(new Rectangle());

            ToolProperty = DependencyProperty.Register(
                "Tool", typeof(DrawingVisual), typeof(ToolCanvas),
                metaData);
        }

        public ToolCanvas() { }

        public DrawingVisual Tool
        {
            get
            {
                return (DrawingVisual)GetValue(ToolProperty);
            }
            set
            {
                SetValue(ToolProperty, value);
            }
        }

        #region Visual Children Overrides

        /// <summary>
        /// Get number of children: VisualCollection count.
        /// If in-place editing textbox is active, add 1.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Get visual child - one of GraphicsBase objects
        /// or in-place editing textbox, if it is active.
        /// </summary>
        protected override Visual GetVisualChild(int index)
        {
            return element;
        }

        #endregion Visual Children Overrides

    }
}
