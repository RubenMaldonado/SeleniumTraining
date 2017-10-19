using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Expression.Encoder.ScreenCapture;

namespace SeleniumTraining.Tools
{
    public class VideoRecorder
    {
        private readonly ScreenCaptureJob _screenCaptureJob;
        public string FileName { get; }

        public VideoRecorder()
        {
            FileName = GetFileName();

            _screenCaptureJob = new ScreenCaptureJob { OutputScreenCaptureFileName = FileName };

        }

        public void StartRecording()
        {
            _screenCaptureJob.Start();
        }

        public void StopRecording()
        {
            _screenCaptureJob.Stop();
            _screenCaptureJob.Dispose();
        }

        private static string GetFileName()
        {
            const string extension = ".wmv";
            string pdfDirectory = $@"{AppDomain.CurrentDomain.BaseDirectory}/ScreenRecording";
            string uniquePart = $@"{Guid.NewGuid()}";
            string dateTime = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}-" +
                              $"{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}";

            if (!Directory.Exists(pdfDirectory))
            {
                Directory.CreateDirectory(pdfDirectory);
            }

            return $@"{pdfDirectory}/{dateTime}-{uniquePart}{extension}";
        }
    }
}
