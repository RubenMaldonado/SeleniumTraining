using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTraining.PageObject
{
    public class PageObject
    {
        protected IWebDriver Browser;

        public PageObject(IWebDriver drv)
        {
            Browser = drv;
        }

        //waits for existence of element up to timeout amount
        public bool WaitUntilExist(IWebElement element, int seconds = 5) 
        {
            try
            {
                var wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(seconds));
                wait.Until(ExpectedConditions.ElementToBeClickable(element));
                return true;
            }
            catch (Exception e) //didnt appear so exception thrown return false

            {
                return false;
            }
        }

        public void WaitUntilPageLoads(int seconds = 3)
        {
            new WebDriverWait(Browser, TimeSpan.FromSeconds(seconds)).Until(
                d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

        }
    }
}
