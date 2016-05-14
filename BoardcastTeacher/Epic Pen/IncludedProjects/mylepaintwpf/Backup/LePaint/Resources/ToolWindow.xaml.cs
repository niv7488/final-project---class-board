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
using System.Windows.Shapes;

using LePaint.Controller;

namespace LePaint
{
    /// <summary>
    /// Interaction logic for ToolWindow.xaml
    /// </summary>
    public partial class ToolWindow : Window
    {
        ToolShapes toolShape;
        public ToolWindow()
        {
            InitializeComponent();

            toolShape = new ToolShapes();

            DataTemplate cardLayout = new DataTemplate { DataType = typeof(LeShape) };
            Grid grid = new Grid();
            cardLayout.Resources.Add(grid, null);

            //foreach(LeShape shape in toolShape.shapes){
            //    listView1.Items.Add(shape); 
            //}

            //bool check=listView1.ApplyTemplate(cardLayout);

        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LeShape item = listView1.SelectedItem as LeShape ;
            Window1.Self.SetDrawingTool(item.GetType()); 
            //myTool
        }
    }
}
