using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using SeleniumTraining.PageObject;
using SeleniumTraining.Tools;

namespace SeleniumTraining
{
    [TestFixture]
    public class FinalTest
    {
        private readonly string START = "Start";
        private readonly string END = "End";
        private const string mailAddress = "selenium.training@testerfabrik.com";
        private readonly string mailPassword = "selenium.training.2017";
        private readonly string mailWrongPassword = "badpasswordhere";
        
        private IWebDriver _driver;
        private ScreenShot _screenShot;
        private PdfGenerator _pdfGenerator;
        private EmailSender _emailSender;
        private VideoRecorder _videoRecorder;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [SetUp]
        public void InitializeDrivers()
        {
            _driver = new ChromeDriver();
            _screenShot = new ScreenShot( _driver );
            _emailSender = new EmailSender();
            _pdfGenerator = new PdfGenerator();
            _videoRecorder = new VideoRecorder();
        }

        [TearDown]
        public void Disposse()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        //This is a test with a sample of all the crazy types of recordings that we can have, log, screenshot, pdf and video recording.
        [Test]
        public void Should_SuccessfulLogin_When_ValidCredentialsAreUsed()
        {
            _videoRecorder.StartRecording();

            var loginPage = new LoginPage(_driver);

            loginPage.Navigate();

            _screenShot.Take();
            
            InboxPage inboxPage = loginPage.Login(mailAddress, mailPassword);

            _screenShot.Take();

            Assert.IsTrue(inboxPage.IsPageLoaded());

            _screenShot.Take();
            
            log.Info( _screenShot.GeneratedFilesLog() );
            
            _pdfGenerator.ImageFileNames = _screenShot.GeneratedFiles;

            _pdfGenerator.GeneratePdf();
            
            _emailSender.SendEmail(
                mailto: "ruben.maldonado.tena@gmail.com", 
                subject: "Run - Test PDF File", 
                body: "Attached the screenshot of this great and fantastic text", 
                attachementFileName: _pdfGenerator.FileName );

            _videoRecorder.StopRecording();
        }

        [Test]
        public void Should_FailLogin_When_InvalidCredentialsAreUsed()
        {
            var loginPage = new LoginPage(_driver);

            loginPage.Navigate();

            InboxPage mailboxPage = loginPage.Login(mailAddress, mailWrongPassword);

            Assert.IsFalse(mailboxPage.IsPageLoaded());
            Assert.IsTrue(loginPage.IsErrorMessageDisplayed());
        }

        [Test]
        public void Should_DisplayErrorMessageOnLoginPage_When_InvalidCredentialsAreUsed()
        {
            var loginPage = new LoginPage(_driver);

            loginPage.Navigate();

            loginPage.Login(mailAddress, mailWrongPassword);

            Assert.IsTrue(loginPage.IsErrorMessageDisplayed());
        }

        [TestCase("Test")]
        public void Should_CreateNewFolder_When_OptionIsSelectedOnConfigurationScreen(string folderName)
        {
            var loginPage = new LoginPage(_driver);

            loginPage.Navigate();

            InboxPage inboxPage = loginPage.Login(mailAddress, mailPassword);

            string folderList = inboxPage.AddNewFolder(folderName);
                    
            Assert.IsTrue(folderList.Contains(folderName));
        }

        [TestCase(mailAddress, "Test Subject", "Test Body")]
        public void Should_ValidateEmailArrived_After_EmailWasSent(string mailTo, string subject, string body)
        {
            var loginPage = new LoginPage(_driver);

            loginPage.Navigate();

            InboxPage mailboxPage = loginPage.Login(mailAddress, mailPassword);

            string customSubject = $"{subject} {DateTime.Now.ToOADate()}";

            string validatedSubject = mailboxPage.SendEmail(mailTo, customSubject, body);

            Assert.AreEqual(customSubject, validatedSubject);

        }

        [Test]
        public void Should_SuccessfulLogout_When_LogoutButtonIsPressed()
        {
            var loginPage = new LoginPage(_driver);

            loginPage.Navigate();

            var inboxPage = loginPage.Login(mailAddress, mailPassword);

            inboxPage.WaitUntilPageLoads();
            
            string message = inboxPage.Logout();

            Assert.AreEqual(message, "You have logged out.");
        }
    }
}

