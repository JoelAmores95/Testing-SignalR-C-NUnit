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
    public class TestBotones
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;


        /* ############ INICIO TEST ############# */

        [Test]
        // [Ignore("Funciona OK")]
        public void ValidarBotonSignalRChat()
        {
            driver.Navigate().GoToUrl("http://localhost:5054/");
            driver.FindElement(By.XPath("//*[text() = \"SignalRChat\"]")).Click();

            // Valido que estoy en la ruta raiz
            string expectedUrl = "http://localhost:5054/";
            string actualUrl = driver.Url;
            Assert.AreEqual(expectedUrl, actualUrl, "El navegador no está en la ruta raíz.");
        }

        [Test]
        // [Ignore("Funciona OK")]
        public void ValidarBotonHome()
        {
            driver.Navigate().GoToUrl("http://localhost:5054/");
            driver.FindElement(By.XPath("//*[text() = \"Home\"]")).Click();

            // Valido que estoy en la ruta raiz
            string expectedUrl = "http://localhost:5054/";
            string actualUrl = driver.Url;
            Assert.AreEqual(expectedUrl, actualUrl, "El navegador no está en la ruta raíz.");
        }

        [Test]
        // [Ignore("Funciona OK")]
        public void ValidarBotonPrivacy()
        {
            driver.Navigate().GoToUrl("http://localhost:5054/");
            driver.FindElement(By.XPath("//*[text() = \"Privacy\"]")).Click();

            // Valido que estoy en la ruta /Privacy
            string expectedUrl = "http://localhost:5054/Privacy";
            string actualUrl = driver.Url;
            Assert.AreEqual(expectedUrl, actualUrl, "El navegador no está en la ruta Privacy.");
        }

        [Test]
        // [Ignore("Funciona OK")]
        public void CrearMensaje()
        {
            // Crea un mensaje con el usuario "user" y el mensaje "message"
            driver.Navigate().GoToUrl("http://localhost:5054/");
            driver.FindElement(By.Id("userInput")).Click();
            driver.FindElement(By.Id("userInput")).Clear();
            driver.FindElement(By.Id("userInput")).SendKeys("user");
            driver.FindElement(By.Id("messageInput")).Click();
            driver.FindElement(By.Id("messageInput")).Clear();
            driver.FindElement(By.Id("messageInput")).SendKeys("message");
            driver.FindElement(By.XPath("//input[@value = \"Enviar Mensaje\"]")).Click();

            // Comprueba que ha aparecido el mensaje con el texto esperado
            IWebElement mensaje = driver.FindElement(By.XPath("//*[@id='messagesList']/li[1]"));
            string textoEsperado = "user says message";
            Assert.AreEqual(textoEsperado, mensaje.Text);

        }

        [Test]
        // [Ignore("Funciona OK")]
        public void MensajeEnviadoApareceEnOtraVentana()
        {
            // Crear primera instancia del driver
            IWebDriver driver1 = new ChromeDriver();

            // Navegar a la página en la primera instancia del driver
            driver1.Navigate().GoToUrl("http://localhost:5054/");

            // Crear segundo instancia del driver
            IWebDriver driver2 = new ChromeDriver();

            // Navegar a la página en la segunda instancia del driver
            driver2.Navigate().GoToUrl("http://localhost:5054/");

            // Crear un mensaje en la primera instancia del driver y enviarlo
            IWebElement userInput = driver1.FindElement(By.Id("userInput"));
            userInput.Clear();
            userInput.SendKeys("user1");
            IWebElement messageInput = driver1.FindElement(By.Id("messageInput"));
            messageInput.Clear();
            messageInput.SendKeys("Hola desde la primera instancia");
            IWebElement enviarBtn = driver1.FindElement(By.CssSelector("input[value='Enviar Mensaje']"));
            enviarBtn.Click();

            // Esperar un tiempo para que se actualice la página
            Thread.Sleep(1000);

            // Verificar que el mensaje enviado desde la primera instancia aparece en la lista de mensajes en la segunda instancia del driver
            IWebElement mensaje = driver2.FindElement(By.XPath("//ul[@id='messagesList']/li[1]"));
            Assert.AreEqual("user1 says Hola desde la primera instancia", mensaje.Text);

            // Cerrar ambas instancias del driver
            driver1.Quit();
            driver2.Quit();
        }

        [Test]
        // [Ignore("Funciona OK")]
        public void BorrarPrimerMensaje()
        {
            // Paso 1 - Crear Mensajes
            CrearMensaje();

            for (int i = 0; i < 2; i++)
            {
                driver.FindElement(By.Id("userInput")).Click();
                driver.FindElement(By.Id("userInput")).Clear();
                driver.FindElement(By.Id("userInput")).SendKeys("user " + i);
                driver.FindElement(By.Id("messageInput")).Click();
                driver.FindElement(By.Id("messageInput")).Clear();
                driver.FindElement(By.Id("messageInput")).SendKeys("message" + i);
                driver.FindElement(By.XPath("//input[@value = \"Enviar Mensaje\"]")).Click();
            }

            // Obtener la lista de elementos antes de hacer clic en el botón
            IList<IWebElement> elementosAntes = driver.FindElements(By.XPath("//ul[@id=\"messagesList\"]/li"));
            string primerElemento = elementosAntes[0].Text;

            // Eliminar primer elemento
            driver.FindElement(By.XPath("//input[@id = \"deleteFirstButton\"]")).Click();

            // Obtener la lista de elementos después de hacer clic en el botón
            IList<IWebElement> elementosDespues = driver.FindElements(By.XPath("//ul[@id=\"messagesList\"]/li"));

            // Verificar que la cantidad de elementos de la lista después de hacer clic es igual a la cantidad de elementos de la lista antes de hacer clic menos uno
            Assert.AreEqual(elementosAntes.Count - 1, elementosDespues.Count);

            // Verificar que el primer elemento de la lista después de hacer clic es distinto al primer elemento de la lista antes de hacer clic
            Assert.AreNotEqual(primerElemento, elementosDespues[0].Text);

        }

        [Test]
        // [Ignore("Funciona OK")]
        public void BorrarUltimoMensaje()
        {

            // Paso 1 - Crear Mensajes
            CrearMensaje();

            for (int i = 0; i < 2; i++)
            {
                driver.FindElement(By.Id("userInput")).Click();
                driver.FindElement(By.Id("userInput")).Clear();
                driver.FindElement(By.Id("userInput")).SendKeys("user " + i);
                driver.FindElement(By.Id("messageInput")).Click();
                driver.FindElement(By.Id("messageInput")).Clear();
                driver.FindElement(By.Id("messageInput")).SendKeys("message" + i);
                driver.FindElement(By.XPath("//input[@value = \"Enviar Mensaje\"]")).Click();
            }

            // Obtener el número de elementos de la lista antes de eliminar el último mensaje
            IList<IWebElement> elementosAntes = driver.FindElements(By.XPath("//ul[@id=\"messagesList\"]/li"));
            int numMensajesAntes = elementosAntes.Count;

            // Hacer clic en el botón que elimina el último mensaje
            driver.FindElement(By.XPath("//input[@id = \"deleteLastButton\"]")).Click();

            // Obtener el número de elementos de la lista después de eliminar el último mensaje
            IList<IWebElement> elementosDespues = driver.FindElements(By.XPath("//ul[@id=\"messagesList\"]/li"));
            int numMensajesDespues = elementosDespues.Count;

            // Verificar que el número de elementos de la lista después de eliminar el último mensaje es uno menos que el número de elementos de la lista antes de eliminar el último mensaje
            Assert.AreEqual(numMensajesAntes - 1, numMensajesDespues);

        }

        [Test]
        // [Ignore("Funciona OK")]
        public void BorrarTodosMensajes()
        {
            // Paso 1
            CrearMensaje();

            // Paso 2
            driver.FindElement(By.XPath("//input[@value = \"Vaciar Mensajes\"]")).Click();

            ReadOnlyCollection<IWebElement> listaMensajes = driver.FindElements(By.CssSelector("#messagesList > li"));
            Assert.AreEqual(0, listaMensajes.Count, "La lista de mensajes no está vacía después de eliminar todos los mensajes.");
        }

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
