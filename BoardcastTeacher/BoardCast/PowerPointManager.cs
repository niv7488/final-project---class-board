using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Win32;
using Application = Microsoft.Office.Interop.PowerPoint.Application;

namespace BoardCast
{
    class PowerPointManager
    {
        Application oPPT;
        Presentations objPresSet;
        Presentation objPres;
        SlideShowView oSlideShowView;

        public bool OpenPowerPoint()
        {
            try
            {
                //Create an instance of PowerPoint.
                oPPT = new Application();
                // Show PowerPoint to the user.
                oPPT.Visible = MsoTriState.msoTrue;
                objPresSet = oPPT.Presentations;
                //oPPT.SlideShowNextClick += this.SlideShowNextClick;

                OpenFileDialog Opendlg = new OpenFileDialog();

                Opendlg.Filter = "Powerpoint|*.ppt;*.pptx|All files|*.*";

                // Open file when user  click "Open" button  
                if (Opendlg.ShowDialog() == true)
                {
                    string pptFilePath = Opendlg.FileName;
                    //open the presentation
                    objPres = objPresSet.Open(pptFilePath, MsoTriState.msoFalse,
                    MsoTriState.msoTrue, MsoTriState.msoTrue);

                    objPres.SlideShowSettings.ShowPresenterView = MsoTriState.msoFalse;
                    objPres.SlideShowSettings.Run();
                    
                    oSlideShowView = objPres.SlideShowWindow.View;
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Unable to open Power Point, please make sure you have the program installed correctly");
                return false;
            }
        }

        public void ShowNextSlide()
        {
            oSlideShowView.Application.SlideShowWindows[1].Activate();
            oSlideShowView.Next();
            
        }

        public void ShowPreviousSlide()
        {
            oSlideShowView.Previous();
        }

        public void ClosePowerPoint()
        {
            oSlideShowView.Exit();
            objPres.Close();
        }
    }
}
