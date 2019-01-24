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

        public MainWindow()
        {
            InitializeComponent();
    
        }


        public void Run_Click(object sender, RoutedEventArgs e)
        {
            IWebDriver driver = new ChromeDriver();

            int complaintNumber = int.Parse(ComplaintNumberx.Text);

            string magentouserName = Usernamex.Text;
            string magentoPass = Passwordx.Text;

   
            //opens complaint webpage


            driver.Url = "http://complaints/Home/ReadOnlyDetails/" + complaintNumber;

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

            var userName = driver.FindElement(By.XPath("//*[@id='username']"));
            userName.SendKeys(magentouserName);

            var password = driver.FindElement(By.XPath("//*[@id='login']"));
            password.SendKeys(magentoPass);

            var magLogin = driver.FindElement(By.XPath("//*[@id='loginForm']/div/div[5]/input"));
            magLogin.Click();

            var customersTab = driver.FindElement(By.XPath("//*[@id='nav']/li[4]"));
            customersTab.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            var manageCustomers = driver.FindElement(By.XPath("//*[@id='nav']/li[4]/ul/li[1]/a"));
            manageCustomers.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

            var addNewCustomer = driver.FindElement(By.CssSelector("div.content-header:nth-child(2) > table:nth-child(1) > tbody:nth-child(1) > tr:nth-child(1) > td:nth-child(2) > button:nth-child(1)")); //SUCCESS!!!!
            addNewCustomer.Click();

            //new complaint policy is to have everyone send replacements from their Sale and Marketing accounts on Magento
            //adjustments will require magento to search from the individual's account, then add the customer's desired shipping address.

            //start inputting customer's information

            var assostoWebdd = driver.FindElement(By.XPath("//*[@id='_accountwebsite_id']"));
            SelectElement select = new SelectElement(assostoWebdd);
            select.SelectByText("Main Website");

            var fnField = driver.FindElement(By.XPath("//*[@id='_accountfirstname']"));
            fnField.SendKeys(firstName);

            var lnField = driver.FindElement(By.XPath("//*[@id='_accountlastname']"));
            lnField.SendKeys(lastName);

            var emailField = driver.FindElement(By.XPath("//*[@id='_accountemail']"));
            emailField.SendKeys(email);

            string passW = "password"; //intentionally left blank so accounts are not actually created during testing. Normally we just set it as "password"
            var passField = driver.FindElement(By.XPath("//*[@id='_accountpassword']"));
            passField.SendKeys(passW);

            //start inputting customer's address

            var movetoAddress = driver.FindElement(By.CssSelector("#customer_info_tabs_addresses"));
            movetoAddress.SendKeys(Keys.Return); //.SendKeys(Keys.Return) is necessary because the element is wrapped in a div or span, I guess.

            var addnewAddress = driver.FindElement(By.CssSelector("#add_address_button"));
            addnewAddress.Click();

            var selectbilAdd = driver.FindElement(By.CssSelector("#address_item_billing_item1"));
            selectbilAdd.Click();

            var selectshipAdd = driver.FindElement(By.CssSelector("#address_item_shipping_item1"));
            selectshipAdd.Click();

            var addstreetAddress = driver.FindElement(By.XPath("//*[@id='_item1street0']"));
            addstreetAddress.SendKeys(streetAddress);

            var addCity = driver.FindElement(By.XPath("//*[@id='_item1city']"));
            addCity.SendKeys(city);

            var addState1 = driver.FindElement(By.XPath("//*[@id='_item1region_id']"));
            addState1.Click();
            addState1.SendKeys(state);
            addState1.SendKeys(Keys.Return);

            var addZip = driver.FindElement(By.XPath("//*[@id='_item1postcode']"));
            addZip.SendKeys(zipcode);

            var addPhone = driver.FindElement(By.XPath("//*[@id='_item1telephone']"));
            addPhone.SendKeys(phoneNumber);

            var saveandcontinueEdit = driver.FindElement(By.CssSelector("div.content-header:nth-child(2) > p:nth-child(2) > button:nth-child(4)"));
            saveandcontinueEdit.Click();

            //that's it!

        }


    }
}
