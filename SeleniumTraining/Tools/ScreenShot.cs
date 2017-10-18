using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumTraining.Tools
{
    public class ScreenShot
    {
        public List<string> GeneratedFiles;

        private readonly IWebDriver _browser;
        public ScreenShot( IWebDriver brw )
        {
            this._browser = brw;
            GeneratedFiles = new List<string>();
        }

        public void Take()
        {
            string fileName = GetFileName();

            try
            {
                Screenshot scrShot = ( ( ITakesScreenshot ) _browser ).GetScreenshot();
                scrShot.SaveAsFile(fileName, ScreenshotImageFormat.Jpeg );
            }
            catch
            {
                // ignored
            }
            finally
            {
                GeneratedFiles.Add( fileName );
            }

        }

        public string GetFileName()
        {
            const string extension = ".jpg";
            string screenshotDirectory = $@"{AppDomain.CurrentDomain.BaseDirectory}/Screenshots";
            string uniquePart = $@"{Guid.NewGuid()}";
            string dateTime = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}-" +
                              $"{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}{DateTime.Now.Millisecond}";
            
            if ( !Directory.Exists( screenshotDirectory ) )
            {
                Directory.CreateDirectory( screenshotDirectory );
            }

            return $@"{screenshotDirectory}/{dateTime}-{uniquePart}{extension}";
        }

        public string GeneratedFilesLog()
        {
            return "Generated Files: " + string.Join( ",", GeneratedFiles.ToArray() );
        }
    }
}

    
