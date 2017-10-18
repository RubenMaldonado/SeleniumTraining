using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;

namespace SeleniumTraining.PageObject
{
    public class PageObject
    {
        protected IWebDriver Browser;
        
        public PageObject( IWebDriver drv )
        {
            Browser = drv;
        }
    }
}
