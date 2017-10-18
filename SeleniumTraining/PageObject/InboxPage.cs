using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTraining.PageObject
{
    public class InboxPage : PageObject
    {
        
        public InboxPage( IWebDriver drv ) : base( drv )
        {

        }

        public bool IsPageLoaded()
        {
            var wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));

            try
            {
                wait.Until(ExpectedConditions.ElementExists(By.Id("lblUserNameTxt")));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
