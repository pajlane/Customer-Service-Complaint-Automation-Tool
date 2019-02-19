using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace CustomerServiceComplaintAutomationTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WriteLine.txt");

        public MainWindow()
        {
            InitializeComponent();

            if (IsLoginInfoPresent() == true)
            {
                string[] login = System.IO.File.ReadAllLines(fileName);
                Usernamex.Text = login[0];
                Passwordx.Password = login[1];
                Emailx.Text = login[2];
            }
        }

        private bool IsLoginInfoPresent()
        {
            try
            {
                string[] login = System.IO.File.ReadAllLines(fileName);
                string testArray = login[0];
                return true;

            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            catch (Exception) //made for if the "WriteLine.txt" file doesn't exist, which it won't if the checkbox isn't checked.
            {
                return false;
            }
        }

        private bool IsElementPresent(By by, IWebDriver driver) //for selecting the new address radio button in magento by checking to see if an element exists
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
            catch (ElementNotVisibleException)
            {
                return false;
            }
            
        }

        public void Run_Click(object sender, RoutedEventArgs e) //Run button!
        {

            var driverService = ChromeDriverService.CreateDefaultService(@"\\brmpro\MACAPPS\ClickOnce\CustomerServiceAutomationTool");
            driverService.HideCommandPromptWindow = true;
            var driver = new ChromeDriver(driverService, new ChromeOptions());

            Close();

            if (Checkbox1.IsChecked.Value)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
                {
                    file.WriteLine(Usernamex.Text);
                    file.WriteLine(Passwordx.Password);
                    file.WriteLine(Emailx.Text);
                }
            }
            else if (System.IO.File.Exists(fileName))
            {
                try
                {
                    System.IO.File.Delete(fileName);
                }
                catch (System.IO.IOException)
                {
                    return;
                }
            }

            int complaintNumber = int.Parse(ComplaintNumberx.Text);
            string magentouserName = Usernamex.Text;
            string magentoPass = Passwordx.Password;
            string workEmail = Emailx.Text;

            driver.Url = "http://complaints/Home/ReadOnlyDetails/" + complaintNumber; //opens complaint webpage

            //gets the customers info for replacement orders

            var firstName = driver.FindElement(By.XPath("/html/body/div[2]/div/fieldset[1]/table/tbody/tr[2]/td[2]")).Text;

            var lastName = driver.FindElement(By.XPath("/html/body/div[2]/div/fieldset[1]/table/tbody/tr[2]/td[3]")).Text;

            var email = driver.FindElement(By.XPath("/html/body/div[2]/div/fieldset[1]/table/tbody/tr[4]/td[1]")).Text;

            var phoneNumber = driver.FindElement(By.XPath("/html/body/div[2]/div/fieldset[1]/table/tbody/tr[4]/td[2]")).Text;

            var country = driver.FindElement(By.XPath("/html/body/div[2]/div/fieldset[1]/table/tbody/tr[4]/td[4]")).Text;

            var streetAddress = driver.FindElement(By.XPath("/html/body/div[2]/div/fieldset[1]/table/tbody/tr[6]/td[1]")).Text;

            var city = driver.FindElement(By.XPath("/html/body/div[2]/div/fieldset[1]/table/tbody/tr[6]/td[2]")).Text;

            var state = driver.FindElement(By.XPath("/html/body/div[2]/div/fieldset[1]/table/tbody/tr[6]/td[3]")).Text;

            var zipcode = driver.FindElement(By.XPath("/html/body/div[2]/div/fieldset[1]/table/tbody/tr[6]/td[4]")).Text;

            var itemNumber = driver.FindElement(By.XPath("/html/body/div[2]/div/fieldset[2]/table/tbody/tr[2]/td[1]")).Text;

            //Beginning of Magento navigation

            driver.Url = "https://www.bobsredmill.com/index.php/admin/";

            var userName = driver.FindElement(By.XPath("//*[@id='username']")); //if using the app two times in a row this will throw an exception because you're already logged in!
            userName.SendKeys(magentouserName);                                 //that's only true if chromedrive is instantiated outside of run_click

            var password = driver.FindElement(By.XPath("//*[@id='login']"));
            password.SendKeys(magentoPass);

            var magLogin = driver.FindElement(By.XPath("//*[@id='loginForm']/div/div[5]/input"));
            magLogin.Click();


            //navigating to one's own account

            while (IsElementPresent(By.XPath("//*[@id='nav']/li[4]"), driver) == false)
            {
                if (IsElementPresent(By.XPath("//*[@id='nav']/li[4]"), driver) == true)
                    break;
            }
                var customersTab = driver.FindElement(By.XPath("//*[@id='nav']/li[4]"));
                customersTab.Click();

            while (IsElementPresent(By.XPath("//*[@id='nav']/li[4]/ul/li[1]/a"), driver) == false)
            {
                if (IsElementPresent(By.XPath("//*[@id='nav']/li[4]/ul/li[1]/a"), driver) == true)
                    break;
            }
            var manageCustomers = driver.FindElement(By.XPath("//*[@id='nav']/li[4]/ul/li[1]/a"));
            manageCustomers.Click();

            var csrepemailSearch = driver.FindElement(By.XPath("//*[@id='customerGrid_filter_email']"));
            csrepemailSearch.SendKeys(workEmail);
            csrepemailSearch.SendKeys(Keys.Return);

            var loadingMask = driver.FindElement(By.Id("loading-mask")); //element not visible after like ten seconds. I need something that asks if its visible, and to only continue after it is not visible, that's what loadingmask.click() is for
            while (IsElementPresent(By.Id("loading-mask"), driver) == true) 
            {
                try
                {
                    loadingMask.Click();
                }
                catch (ElementNotVisibleException)
                {
                    break;
                }
                catch (WebDriverException)
                {
                    break;
                }
                
            }
            var csrepemailEnter = driver.FindElement(By.ClassName("even"));
            csrepemailEnter.Click();

            while (IsElementPresent(By.CssSelector("#customer_info_tabs_addresses"), driver) == false)
            {
                if (IsElementPresent(By.CssSelector("#customer_info_tabs_addresses"), driver) == true)
                    break;
            }

            var movetoAddress = driver.FindElement(By.CssSelector("#customer_info_tabs_addresses"));
            movetoAddress.SendKeys(Keys.Return); //.SendKeys(Keys.Return) is necessary because the element is wrapped in a div or span, I guess.

            var addnewAddress = driver.FindElement(By.CssSelector("#add_address_button"));
            addnewAddress.Click();

            int count = 1;

            while (IsElementPresent(By.CssSelector("#address_item_shipping_item" + count), driver) == false) //loop throughs till it find the right CssSelector
            {
                count = count + 1;
            }

            var selectshipAdd = driver.FindElement(By.CssSelector("#address_item_shipping_item" + count));
            selectshipAdd.Click();

            System.Threading.Thread.Sleep(1000);

            //start inputting customer's address

            var fnField = driver.FindElement(By.CssSelector("#_item" + count + "firstname"));
            fnField.Clear();
            fnField.SendKeys(firstName);

            var lnField = driver.FindElement(By.CssSelector("#_item" + count + "lastname"));
            lnField.Clear();
            lnField.SendKeys(lastName);

            var addstreetAddress = driver.FindElement(By.CssSelector("#_item" + count + "street0"));
            addstreetAddress.SendKeys(streetAddress);

            var addCity = driver.FindElement(By.CssSelector("#_item" + count + "city"));
            addCity.SendKeys(city);

            var addState1 = driver.FindElement(By.CssSelector("#_item" + count + "region_id"));
            addState1.Click();
            addState1.SendKeys(state);
            addState1.SendKeys(Keys.Return);

            var addZip = driver.FindElement(By.CssSelector("#_item" + count + "postcode"));
            addZip.SendKeys(zipcode);

            if (phoneNumber == "")
            {
                phoneNumber = "555-555-5555";
            }
            var addPhone = driver.FindElement(By.CssSelector("#_item" + count + "telephone"));
            addPhone.SendKeys(phoneNumber);

            System.Threading.Thread.Sleep(1000);

            var saveandcontinueEdit = driver.FindElement(By.CssSelector("[title^= 'Save and Continue Edit']"));
            var js = (IJavaScriptExecutor)driver;
            IJavaScriptExecutor je = (IJavaScriptExecutor)driver; //scrolls up so the element is clickable
            je.ExecuteScript("arguments[0].scrollIntoView(false);", saveandcontinueEdit);
            saveandcontinueEdit.Click();

            //maybe add create order

            //that's it!

        }
    }
}
