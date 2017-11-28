using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeebusTestProject
{
    class NUnitTest
    {
        private IWebDriver driver;
        private string mainUrl;

        [SetUp]
        public void Initialize()
        {
            driver = new ChromeDriver();
            mainUrl = "https://weebus.paulo.cloud";
        }

        [Test]
        public void OpenUrlAssertBrandName()
        {
            //Arrange
            driver.Navigate().GoToUrl(mainUrl);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            string expectedValue1 = "Wee";
            string expectedValue2 = "BUS";

            //Act
            Func<IWebDriver, bool> waitForElement = new Func<IWebDriver, bool>((IWebDriver Web) =>
            {
                var elemValue = Web.FindElement(By.ClassName("navbar-brand")).GetAttribute("innerHTML");
                return (elemValue.Contains(expectedValue1) && elemValue.Contains(expectedValue2));
            });
            bool found = wait.Until(waitForElement);

            //Assert
            Assert.IsTrue(found);
        }

        [Test]
        public void UserLocationImage()
        {
            //Arrange
            driver.Navigate().GoToUrl(mainUrl);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            string expectedValue = "https://weebus.paulo.cloud/assets/dist/img/user-minecraft-32x32.png";

            //Act
            Func<IWebDriver, bool> waitForElement = new Func<IWebDriver, bool>((IWebDriver Web) =>
            {
                var elemValue = Web.FindElement(By.ClassName("gmnoprint"))
                    .FindElement(By.TagName("img")).GetAttribute("src");
                return elemValue == expectedValue;
            });
            bool userImageFound = wait.Until(waitForElement);

            //Assert 
            Assert.IsTrue(userImageFound);
        }

        [Test]
        public void NearPointsLocated()
        {
            //Arrange
            driver.Navigate().GoToUrl(mainUrl);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            string expectedValue = "./assets/dist/img/bus-stop-32x";

            //Act
            Func<IWebDriver, bool> waitForElement = new Func<IWebDriver, bool>((IWebDriver Web) =>
            {
                var elements = Web.FindElements(By.TagName("agm-marker"));
                return elements.Any(e => e.GetAttribute("ng-reflect-icon-url") == expectedValue);
            });
            bool userImageFound = wait.Until(waitForElement);

            //Assert 
            Assert.IsTrue(userImageFound);
        }

        [Test]
        public void OpenWindowEstimateTimeArrival()
        {
            //Arrange
            var expectedPoint = "123";
            var pointEtaUrl = $"{mainUrl}/pointfull/{expectedPoint}";
            driver.Navigate().GoToUrl(pointEtaUrl);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            IWebElement markerFound = null;
            string expectedValue = $"Ponto: {expectedPoint}";

            //Act
            Func<IWebDriver, bool> waitForElement = new Func<IWebDriver, bool>((IWebDriver Web) =>
            {
                
                var elements = Web.FindElements(By.ClassName("box-title"));
                markerFound = elements.FirstOrDefault(e => e.GetAttribute("innerHTML") == expectedValue);
                return markerFound != null;
            });
            bool markerBusStopFound = wait.Until(waitForElement);

            //Assert
            Assert.IsTrue(markerBusStopFound);
        }

        [TearDown]
        public void EndTest()
        {
            driver.Close();
        }



    }
}
