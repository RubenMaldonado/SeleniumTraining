using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;

namespace SeleniumTraining.PageObject
{
    public class InboxPage : PageObject
    {
        
        [FindsBy(How = How.Id, Using = "rcmbtn104")]
        public IWebElement SettingsMenuButton { get; set; }

        [FindsBy(How = How.Id, Using = "rcmbtn107")]
        public IWebElement SettingsButtonFolderManagement { get; set; }

        [FindsBy(How = How.Id, Using = "rcmbtn110")]
        public IWebElement SettingsButtonAdd { get; set; }

        [FindsBy(How = How.Id, Using = "rcmbtn107")]
        public IWebElement WriteMailMenuButton { get; set; }

        [FindsBy(How = How.Id, Using = "rcmbtn107")]
        public IWebElement SendMailMenuButton { get; set; }

        [FindsBy(How = How.Id, Using = "preferences-frame")]
        public IWebElement PreferencesFrame { get; set; }

        [FindsBy(How = How.Id, Using = "_name")]
        public IWebElement FolderNameInput { get; set; }

        [FindsBy(How = How.Id, Using = "rcmbtn100")]
        public IWebElement ButtonSave { get; set; }


        [FindsBy(How = How.Id, Using = "toplogo")]
        public IWebElement HomeButton { get; set; }

        [FindsBy(How = How.Id, Using = "mailboxlist")]
        public IWebElement FolderList { get; set; }

        [FindsBy(How = How.Id, Using = "mailFrame")]
        public IWebElement MailFrame { get; set; }

        [FindsBy(How = How.Id, Using = "_to")]
        public IWebElement To { get; set; }

        [FindsBy(How = How.Id, Using = "compose-subject")]
        public IWebElement Subject { get; set; }

        [FindsBy(How = How.Id, Using = "composebody")]
        public IWebElement Composebody { get; set; }

        public InboxPage( IWebDriver drv ) : base( drv )
        {

            WaitUntilPageLoads();

            PageFactory.InitElements(Browser, this);

        }

        public bool IsPageLoaded()
        {
            var wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));

            try
            {
                //Use the user name label to check that the page was loaded correctly
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

        public string AddNewFolder(string folderName)
        {
            WaitUntilExist(MailFrame);

            Browser.SwitchTo().Frame(MailFrame);

            NavigateToConfiguration();

            WaitUntilExist(SettingsButtonFolderManagement);

            SettingsButtonFolderManagement.Click();

            WaitUntilExist(SettingsButtonAdd);
            
            SettingsButtonAdd.Click();

            WaitUntilExist(PreferencesFrame);

            Browser.SwitchTo().Frame(PreferencesFrame);
            
            FolderNameInput.SendKeys(folderName);

            ButtonSave.Click();

            Browser.SwitchTo().ParentFrame();

            HomeButton.Click();

            IsPageLoaded();

            return FolderList.Text;

        }

        private void NavigateToConfiguration()
        {
            WaitUntilExist(SettingsMenuButton);
            SettingsMenuButton.Click();
            WaitUntilPageLoads();
        }

        internal string SendEmail(string mailTo, string customSubject, string body)
        {
            WaitUntilExist(MailFrame);

            Browser.SwitchTo().Frame(MailFrame);

            WriteMailMenuButton.Click();

            To.SendKeys(mailTo);
            Subject.SendKeys(customSubject);
            Composebody.SendKeys(body);

            SendMailMenuButton.Click();

             var wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));
            var email = wait.Until<IWebElement>((d) =>
                {
                    IWebElement e = Browser.FindElements(By.ClassName("subject")).Where(x => x.Text.Contains(customSubject)).FirstOrDefault();
                    if (e != null)
                    {
                        return e;
                    }
                    return null;
                }
            );

            return email.Text;

        }

        [FindsBy(How = How.Id, Using = "lnkHeaderLogout")]
        public IWebElement LogoutButton { get; set; }

        [FindsBy(How = How.Id, Using = "login-status-message")]
        public IWebElement LoginStatusMessage { get; set; }
        internal string Logout()
        {
            Browser.SwitchTo().ParentFrame();

            WaitUntilExist(LogoutButton);

            LogoutButton.Click();

            WaitUntilExist(LoginStatusMessage);

            return LoginStatusMessage.Text;
        }
    }
}
