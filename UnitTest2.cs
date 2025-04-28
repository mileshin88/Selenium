using System;
using System.Globalization;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace First_Independent_autotest;

public class UnitTest2
{
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver(); // запуск браузера    
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); // установка неявного ожидания  
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5)); // установка явного ожидания
            driver.Manage().Window.Maximize(); // развернуть на весь экран
        } 
        

        [TearDown]
        public void TearDown()
        {
            // Закрываем браузер после теста
            driver.Quit();
            driver.Dispose(); 
        }

        public void Authorize()
        {
        
         driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/"); // переход по урлу

         var login = driver.FindElement(By.Id("Username")); // ищем кнопку логин
         login.SendKeys(""); // вводим логин

         var password = driver.FindElement(By.Id("Password")); // ищем кнопку пароль
         password.SendKeys("()"); // вводим пароль

         var enter = driver.FindElement(By.Name("button")); // ищем кнопку войти
         enter.Click(); // жмём кнопку войти

         wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='Title']"))); // явное ожидание появления заголовка
       
        }




        [Test]
        public void Authorization_test()
        {

         Authorize(); // авторизуемся
        
         Assert.That(driver.Title, Does.Contain("Новости"), "После авторизации ожидаем заголовок 'Новости' "); // проверяем, что заголовок = Новости
       
        }

        [Test]
        public void Community_Section_Exists()
        {
       
        Authorize();
  
        var community = driver.FindElement(By.CssSelector("[data-tid='Community'")); // находим кнопку Сообщества
        community.Click(); // жмём на Сообщества

        var titlePageElement = driver.FindElement(By.CssSelector("[data-tid='Title']")); // находим заголовок

        Assert.That(titlePageElement.Text, Does.Contain("Сообщества"), 
        "После перехода в раздел ожидаем заголовок 'Сообщества'"); // проверяем что заголовок = Сообщества

        }

        [Test]
        public void Create_community()
        {

        Authorize();
       
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities"); // переход по урлу в раздел Сообщества
        
        // найти и нажать кнопку "+ Создать"
        
        var create = driver.FindElement(By.CssSelector("[data-tid='PageHeader']")).FindElement(By.XPath("//button[contains(text(), 'СОЗДАТЬ')]"));
        create.Click(); 

        // найти поле ввода Названия сообщества и ввести "тест_автотест"

        var community_name = driver.FindElement(By.CssSelector("[data-tid='Name']"));
        community_name.SendKeys("тест_автотест");

        // найти и нажать кнопку "Создать"

        var create2 = driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
        create2.Click();

        // проверяем, что попали в раздел "основные настройки"

        var basicsettings = driver.FindElement(By.CssSelector("[data-tid='SettingsTabWrapper']"));
        Assert.That(basicsettings.Text, Does.Contain("Основные настройки"), "Страница должна содержать заголовок 'Основные настройки'");
      
        // после ассёрта удаляем созданное сообщество
        // находим кнопку "удалить сообщество"

        var delete = driver.FindElement(By.CssSelector("[data-tid='DeleteButton']"));
        delete.Click();

        // находим и жмём кнопку удаления для подтверждения удаления

        var delete_confirm = driver.FindElement(By.CssSelector("[data-tid='ModalPageFooter'] [data-tid='DeleteButton']"));
        delete_confirm.Click();

        }

        [Test]
        public void Community_Name_Field_is_required()
        {

        Authorize();
       
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities"); // переход по урлу в раздел Сообщества

        // нажать кнопку "+ Создать"

        var create = driver.FindElement(By.CssSelector("[data-tid='PageHeader']")).FindElement(By.XPath("//button[contains(text(), 'СОЗДАТЬ')]"));
        create.Click(); 

        // нажать кнопку "Создать" без ввода Названия сообщества

        var create2 = driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
        create2.Click();


        var validationmessage = driver.FindElement(By.CssSelector("[data-tid='validationMessage']"));
        Assert.That(validationmessage.Text, Does.Contain("Поле обязательно для заполнения."), 
        "Сообщение об ошибке должно содеражать текст 'Поле обязательно для заполнения.");// проверяем, что появилось сообщение "Поле обязательно для заполнения."

        }

        [Test]
        public void NewYear_Theme_is_activated()
        {

        Authorize();
        
        // нажать кнопку Меню профиля

        var menu_profile = driver.FindElement(By.CssSelector("[data-tid='PopupMenu__caption']"));
        menu_profile.Click();

        // нажать кнопку Настройки

        var options = driver.FindElement(By.CssSelector("[data-tid='Settings']"));
        options.Click();

        // нажать кноку Новогодняя тема

        var NYTheme = driver.FindElement(By.CssSelector("[class='react-ui-1suascv']")); // пробовал искать через XPath("//button[contains(text(), 'Новогодняя тема')]. Не получилось(((((
        NYTheme.Click();
        
        // нажать кнопку "Сохранить"

        var save = driver.FindElement(By.CssSelector("[class='react-ui-1m5qr6w']")); // та же история, через XPath никак не находит(((
        save.Click();
        
        // проверяем, что появился снеговик на главном экране
        // Поиск элемента по CSS-селектору
        var snow = driver.FindElement(By.CssSelector("[class='sc-kizEQm sc-kmIPcE irzWzB htFMXJ']"));

        // Выполнение ассерта
        Assert.IsTrue(snow != null, "Элемент с указанным CSS-селектором не найден на странице");
                
        }  

        [Test]
        public void Search_Files_Create_Folder()
        {

        Authorize();

        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/files"); // переходим по урлу
       
        var dropbar = driver.FindElement(By.CssSelector("[class='react-ui-1jhn7yl']")); // по тексту на кнопке не смог найти(((
        dropbar.Click(); // жмём на дроп бар

        var creatdbutton = driver.FindElement(By.CssSelector("[data-tid='ScrollContainer__inner'] [data-tid='CreateFolder']"));
        creatdbutton.Click(); // жмём кнопку Папку
        
        var namefolder = driver.FindElement(By.CssSelector("[data-tid='ModalPageBody'] [data-tid='Input']"));
        namefolder.SendKeys("AutoTest"); // вводим имя папки

        var save = driver.FindElement(By.CssSelector("[data-tid='ModalPageFooter'] [data-tid='SaveButton']"));
        save.Click(); // жмём Сохранить

        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/files"); // обновляем страницу

        var foldername = driver.FindElement(By.CssSelector("[data-tid='Folders'] [data-tid='Title']"));
        Assert.That(foldername.Text, Does.Contain("AutoTest"), "Последняя созданная папка должна называться AutoTest");  // проверяем, что только что созданная папка называется AutoTest

        // удаляем папку после теста

        var threedot = driver.FindElement(By.CssSelector("[data-tid='ListItemWrapper'] [data-tid='PopupMenu__caption']"));
        threedot.Click(); // находим и жмём три точки

        var delete = driver.FindElement(By.CssSelector("[data-tid='PopupContentInner'] [data-tid='DeleteFile']"));
        delete.Click(); // находим и жмём "Удалить"

        var delete2 = driver.FindElement(By.CssSelector("[data-tid='ModalPageFooter'] [data-tid='DeleteButton']"));
        delete2.Click(); // находим и жмём "Удалить"
                
        }
}