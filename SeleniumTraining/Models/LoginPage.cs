using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTraining.Models
{
    public class LoginPage: BasePage
    {
        public IWebElement User { get; private set; }
        public IWebElement Password { get; private set; }
        public IWebElement ButtonLogin { get; private set; }
   
        public LoginPage(IWebDriver driver, string testUniqueIdentifier = "") : base(driver)
        {
            this.TestCaseIdentifier = testUniqueIdentifier;
        }

        private void InitializePageElements()
        {
            User = FindElementById("user");
            Password = FindElementById("pass");
            ButtonLogin = FindElementById("login_submit");
        }
        
        public MailboxPage Login(string url, string user, string password)
        {
            NavigateToUrl(url);
            WaitUntilPageLoads();
            InitializePageElements();
            User.SendKeys(user);
            Password.SendKeys(password);
            TakeSnapshot("Fill-Login-Fields");
            ButtonLogin.Click();
            
            return new MailboxPage(Driver, this.TestCaseIdentifier);
        }

        private void WaitUntilPageLoads(int seconds = 3)
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds( seconds )).Until(
                d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

        }

        public string GetStatusMessage()
        {
            IWebElement demoDiv = FindElementById("login-status-message");

            return demoDiv.GetAttribute( "textContent" );
        }

        internal string Logout()
        {
            Driver.FindElement( By.Id("lnkHeaderLogout") ).Click();

            WaitUntilElementExist( "login-status-message" );

            return Driver.FindElement( By.Id( "login-status-message" ) ).Text;
        }

        private void WaitUntilElementExist(string id, int seconds = 10)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds));

            wait.Until(ExpectedConditions.ElementExists(By.Id(id)));
        }
    }
}
