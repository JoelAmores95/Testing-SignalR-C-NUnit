using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestFixture]
    public class TestAlertas
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;


        /* ############ INICIO TEST ############# */

        [Test]
        // [Ignore("Funciona OK")]
        public void EnviarMensajeUserVacio()
        {
            // Navega a la página
            driver.Navigate().GoToUrl("http://localhost:5054/");

            // Ingresa un nombre de usuario vacio
            driver.FindElement(By.Id("userInput")).Click();
            driver.FindElement(By.Id("userInput")).Clear();

            // Ingresa un mensaje
            driver.FindElement(By.Id("messageInput")).Click();
            driver.FindElement(By.Id("messageInput")).Clear();
            driver.FindElement(By.Id("messageInput")).SendKeys("mensaje");

            // Envía el mensaje
            driver.FindElement(By.XPath("//input[@value = \"Enviar Mensaje\"]"))
                  .Click();

            // Verifica que no se ha agregado ningún mensaje a la lista
            IAlert alertButton = driver.SwitchTo().Alert();

            Assert.AreEqual("Por favor, complete los campos de usuario y mensaje antes de enviar", alertButton.Text);
            alertButton.Accept();

            IList<IWebElement> mensajes = driver.FindElements(By.XPath("//*[@id='messagesList']/li"));
            Assert.AreEqual(0, mensajes.Count);
        }

        [Test]
        // [Ignore("Funciona OK")]
        public void EnviarMensajeCampoVacio()
        {
            // Navega a la página
            driver.Navigate().GoToUrl("http://localhost:5054/");

            // Ingresa un nombre de usuario
            driver.FindElement(By.Id("userInput")).Click();
            driver.FindElement(By.Id("userInput")).Clear();
            driver.FindElement(By.Id("userInput")).SendKeys("user");

            // Ingresa un mensaje vacío
            driver.FindElement(By.Id("messageInput")).Click();
            driver.FindElement(By.Id("messageInput")).Clear();

            // Envía el mensaje
            driver.FindElement(By.XPath("//input[@value = \"Enviar Mensaje\"]"))
                  .Click();

            // Verifica que no se ha agregado ningún mensaje a la lista
            IAlert alertButton = driver.SwitchTo().Alert();

            Assert.AreEqual("Por favor, complete los campos de usuario y mensaje antes de enviar", alertButton.Text);
            alertButton.Accept();

            IList<IWebElement> mensajes = driver.FindElements(By.XPath("//*[@id='messagesList']/li"));
            Assert.AreEqual(0, mensajes.Count);
        }

        [Test]
        // [Ignore("Funciona OK")]
        public void EnviarMensajeLargo()
        {
            // Navegar a la página y crear un usuario
            driver.Navigate().GoToUrl("http://localhost:5054/");
            driver.FindElement(By.Id("userInput")).SendKeys("user");
            driver.FindElement(By.Id("messageInput")).Click();

            // Enviar un mensaje largo y verificar que aparece un mensaje de error
            string mensajeLargo = new string('a', 150);
            driver.FindElement(By.Id("messageInput")).SendKeys(mensajeLargo);
            driver.FindElement(By.Id("sendButton")).Click();
            IAlert alertButton = driver.SwitchTo().Alert();

            Assert.AreEqual("El mensaje no puede tener más de 150 caracteres.", alertButton.Text);

        }

        [Test]
        // [Ignore("Funciona OK")]
        public void AlertaPredefinida()
        {
            // Paso 1
            driver.Navigate().GoToUrl("http://localhost:5054/");

            // Paso 2
            driver.FindElement(By.XPath("//input[@value = \"Aviso Mantenimiento\"]")).Click();
            IAlert alertButton = driver.SwitchTo().Alert();
            string textoEsperado = "Se recomienda salir de la sala, entraremos en mantenimiento en breves";
            Assert.AreEqual(textoEsperado, alertButton.Text);

        }

        [Test]
        // [Ignore("Funciona OK")]
        public void AlertaEscrita()
        {
            driver.Navigate().GoToUrl("http://localhost:5054/");

            driver.FindElement(By.Id("alertaInput")).Click();
            driver.FindElement(By.Id("alertaInput")).Clear();
            driver.FindElement(By.Id("alertaInput")).SendKeys("alerta");
            Thread.Sleep(500);

            driver.FindElement(By.XPath("//input[@value = \"Enviar Alerta\"]")).Click();
            IAlert alertButton = driver.SwitchTo().Alert();

            Assert.AreEqual("alerta", alertButton.Text);
            alertButton.Accept();
            Thread.Sleep(500);

        }

        /* ############ FIN TEST ############# */
        
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver();
            baseURL = "https://www.blazedemo.com/";
            verificationErrors = new StringBuilder();

            // Maximiza la pantalla
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

    }

}
