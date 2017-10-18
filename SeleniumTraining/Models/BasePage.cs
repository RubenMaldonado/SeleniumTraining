using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumTraining.Models
{
    public class BasePage
    {
        protected IWebDriver Driver { get; }

        public string TestCaseIdentifier { get; set; }

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
        }
        
        public void TakeSnapshot(string filename)
        {
            Screenshot scrShot = ((ITakesScreenshot)Driver).GetScreenshot();
            scrShot.SaveAsFile($"{AppDomain.CurrentDomain.BaseDirectory}/{TestCaseIdentifier}-{filename}.jpg", ScreenshotImageFormat.Jpeg);
        }

        public IWebElement FindElementById(string id)
        {
            return Driver.FindElement( By.Id( id ) );
        }

        public void NavigateToUrl( string url )
        {
            Driver.Navigate().GoToUrl( url );
        }

    }
}
