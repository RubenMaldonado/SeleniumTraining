using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTraining.PageObject
{
    public class LoginPage : PageObject
    {
        private readonly string URL = "https://securem50.sgcpanel.com:2096/cpsess7434911414/webmail/Crystal/index.html?mailclient=roundcube";

        public LoginPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements( Browser, this );
        }

        [FindsBy(How = How.Id, Using = "user")]
        public IWebElement UserInput { get; set; }

        [FindsBy(How = How.Id, Using = "pass")]
        public IWebElement PasswordInput { get; set; }

        [FindsBy(How = How.Id, Using = "login_submit")]
        public IWebElement LoginButton { get; set; }

        [FindsBy(How = How.Id, Using = "login-status-message")]
        public IWebElement StatusMessageDiv { get; set; }

        [FindsBy(How = How.ClassName, Using = "error-notice")]
        public IWebElement ErrorStatusMessageDiv { get; set; }


        public void Navigate()
        {
            this.Browser.Navigate().GoToUrl( URL );
        }

        internal InboxPage Login(string mailAddress, string mailPassword)
        {
            this.UserInput.Clear();
            this.UserInput.SendKeys( mailAddress );
            this.PasswordInput.Clear();
            this.PasswordInput.SendKeys( mailPassword );
            this.LoginButton.Click();

            return new InboxPage( Browser );
        }


        internal bool IsErrorMessageDisplayed()
        {
            var wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(3));

            try
            {
      
                wait.Until(ExpectedConditions.ElementExists(By.ClassName("error-notice")));
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
