using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms.VisualStyles;

namespace BoardCast
{
    public class UploadManager
    {
        private static UploadManager instance;
        private static object syncRoot = new Object();
        private static object dataToken = new Object();
        private static bool isMainThreadRunning = false;
        public bool Stop { get; set; }
        //queue of document to add and remove
        public Stack<string> uploadFilesStack = new Stack<string>();
        private string uploadedFileName;
        private string base64String;
        private int timeCounter = 0;
        private bool isBase64Converted = false;

        public static UploadManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new UploadManager();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Main Thread Loop Function
        /// </summary>
        public void Main()
        {
            lock (syncRoot)
            {
                if (isMainThreadRunning) return;
                isMainThreadRunning = true;
            }

            while (!Stop)
            {
                lock (syncRoot)
                {
                    //if add queue not empty extract and add
                    if (uploadFilesStack.Count != 0)
                    {
                        uploadedFileName = (string)uploadFilesStack.Peek();
                        Console.WriteLine(uploadedFileName + " Just poped from stack");
                    }
                    else
                    {
                        uploadedFileName = null;
                        Thread.Sleep(5000);
                    }
                }
                if (uploadedFileName != null)
                {
                    //The actual adding
                    lock (dataToken)
                    {
                        Console.WriteLine("Sending image " + uploadedFileName + " to server");
                        if(!isBase64Converted)
                            Base64Convert();
                        else
                        {
                            UploadFileToServer();
                        }
                        //uploadFilesStack.Pop();
                    }
                }
            }

            //instance finished running
            lock (syncRoot)
            {
                isMainThreadRunning = false;
            }
        }
        #region base64Converter
        private void Base64Convert()
        {
            //the path is the folder that saves the Export image screen shot
            byte[] bytes = File.ReadAllBytes(uploadedFileName);
            Console.WriteLine("Bytes Length " + bytes.Length);
            base64String = Convert.ToBase64String(bytes);
            isBase64Converted = true;
        }
        #endregion

        private void UploadFileToServer()
        {
            //serialize the json so that the server will know what values we sent
            string json = new JavaScriptSerializer().Serialize(new
            {
                // base64 = base64String,                  //the picture after transfoming into base64 string
                filename = Path.GetFileNameWithoutExtension(uploadedFileName),                 //the name of the pic-->need to be changed according to each pic
                course_id = ToolsWindow.courseID,
                date = ToolsWindow.date
            });
            //opening a connection with the server
            var baseAddress = "https://boardcast-ws.herokuapp.com/testchannel/";
            //deffine the request methood
            //var http = HttpWebRequest.Create(new Uri(baseAddress));
            var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";

            string parsedContent = json;
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes1 = encoding.GetBytes(parsedContent);

            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes1, 0, bytes1.Length);
            newStream.Close();

            var response2 = http.GetResponse();

            var stream = response2.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
            Console.WriteLine(content);
            uploadFilesStack.Pop();
            uploadedFileName = null;
            isBase64Converted = false;
            base64String = null;
        }
    }
}
