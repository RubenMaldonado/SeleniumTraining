using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTraining.Models
{
    public class MailboxPage : BasePage
    {
        public bool IsPageLoaded { get; private set; }

        public MailboxPage( IWebDriver driver, string testId ) : base(driver)
        {
            this.TestCaseIdentifier = testId;
            WaitUntilPageLoads();
        }

        private void WaitUntilPageLoads()
        {
            IsPageLoaded = false;

            var wait = new WebDriverWait( Driver, TimeSpan.FromSeconds( 10 ) );

            try
            {
                wait.Until( ExpectedConditions.ElementExists( By.Id( "lblUserNameTxt" ) ) );
                IsPageLoaded = true;
                IsPageLoaded = true;
                TakeSnapshot("Inbox-Page-Loaded" );
            }
            catch ( NoSuchElementException ex )
            {
                IsPageLoaded = false;
                Debug.WriteLine( ex.ToString() );
            }
            catch ( Exception ex )
            {
                IsPageLoaded = false;
                Debug.WriteLine( ex.ToString() );
            }

        }
        
        private void NavigateToConfiguration()
        {
            if ( IsPageLoaded )
            {
                InitializeMailMenuElements();

                SettingsMenuButton.Click();
            }
        }

        private void InitializeMailMenuElements()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds( 10 ) );

            MailFrame = Driver.FindElement(By.Id("mailFrame"));

            Driver.SwitchTo().Frame(MailFrame);

            wait.Until( ExpectedConditions.ElementExists( By.Id( "rcmbtn104" ) ) );
            SettingsMenuButton = Driver.FindElement( By.Id( "rcmbtn104" ) );
            CreateMenuMenuButton = Driver.FindElement( By.Id( CreateMenuMenuButtonId ) );

        }

        public void InitializeSendMailMenuElements()
        {
            SendMailMenuSend = Driver.FindElement( By.Id("rcmbtn107") );
        }

        private IWebElement MailFrame {get; set;}
        private IWebElement PrefrencesFrame { get; set; }
        private IWebElement SettingsMenuButton { get; set; }
        private IWebElement SettingsButtonFolderManagement { get; set; }
        private IWebElement SettingsButtonAdd { get; set; }
        public IWebElement CreateMenuMenuButton { get; private set; }
        private IWebElement SendMailMenuSend { get; set; }

        private const string SettingsButtonAddId = "rcmbtn110";
        private const string CreateMenuMenuButtonId = "rcmbtn107";

        private void InitializeSettingsButtons()
        {
            WaitUntilElementExist("rcmbtn107");
            SettingsButtonFolderManagement = Driver.FindElement( By.Id( "rcmbtn107" ) );
            
        }

        
        //Actions
        public void AddNewFolder( string folderName )
        {
            NavigateToConfiguration();
            InitializeSettingsButtons();
            SettingsButtonFolderManagement.Click();

            WaitUntilElementExist(SettingsButtonAddId);
            SettingsButtonAdd = Driver.FindElement( By.Id(SettingsButtonAddId) );
            SettingsButtonAdd.Click();

            WaitUntilElementExist("preferences-frame");
            PrefrencesFrame = Driver.FindElement(By.Id("preferences-frame"));
            Driver.SwitchTo().Frame(PrefrencesFrame);

            IWebElement inputName = Driver.FindElement( By.Id( "_name" ) );
            inputName.SendKeys( folderName );

            IWebElement buttonSave = Driver.FindElement( By.Id( "rcmbtn100" ) );
            buttonSave.Click();

            Driver.SwitchTo().ParentFrame();

            IWebElement buttonHome = Driver.FindElement( By.Id( "toplogo" ) );
            buttonHome.Click();

            WaitUntilPageLoads();

            var list = Driver.FindElement(By.Id("mailboxlist"));
           
            Assert.IsTrue( list.Text.Contains( folderName ) );
        }

        public string CreateAndSendEmail( string mailto, string title, string body )
        {
            if ( IsPageLoaded )
            {
                InitializeMailMenuElements();
                CreateMenuMenuButton.Click();

                WaitUntilElementExist("_to");
                InitializeSendMailMenuElements();

                var to = Driver.FindElement( By.Id( "_to" ) );
                var subject = Driver.FindElement( By.Id( "compose-subject" ) );
                var composebody = Driver.FindElement( By.Id( "composebody" ) );

                to.SendKeys( mailto );
                subject.SendKeys( title );
                composebody.SendKeys( body );

                SendMailMenuSend.Click();

                WaitUntilElementExistByClassName("subject");
                
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var xx = wait.Until<IWebElement>( ( d ) =>
                                         {
                                             IWebElement e = Driver.FindElements(By.ClassName("subject")).Where(x => x.Text.Contains(title)).FirstOrDefault();
                                             if ( e != null )
                                             {
                                                 return e;
                                             }
                                             return null;
                                         }

                );

                return xx.Text;
            }

            return null;
        }
       
        //Tools
        private void WaitUntilElementExist( string id, int seconds = 10 )
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds));

            wait.Until(ExpectedConditions.ElementExists(By.Id(id)));
        }
        private void WaitUntilElementExistByClassName(string className, int seconds = 10)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds));

            wait.Until(ExpectedConditions.ElementExists(By.ClassName(className)));
            
        }
    }
    
}

