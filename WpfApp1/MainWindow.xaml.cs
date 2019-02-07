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
        IWebDriver driver = new ChromeDriver();

        public MainWindow()
        {
            InitializeComponent();

            if (IsLoginInfoPresent() == true)
            {
                string[] login = System.IO.File.ReadAllLines(@"C:\Users\plane\Desktop\WriteLine.txt");
                Usernamex.Text = login[0];
                Passwordx.Password = login[1];
                Emailx.Text = login[2];
            }
        }

        private bool IsLoginInfoPresent()
        {
            try
            {
                string[] login = System.IO.File.ReadAllLines(@"C:\Users\plane\Desktop\WriteLine.txt");
                string testArray = login[0];
                return true;

            }

            catch (IndexOutOfRangeException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool IsElementPresent(By by) //for selecting the new address radio button in magento by checking to see if an element exists
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
             //find a way for it to be auto-checked on startup
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\plane\Desktop\WriteLine.txt"))
                {
                    file.WriteLine(Usernamex.Text);
                    file.WriteLine(Passwordx.Password);
                    file.WriteLine(Emailx.Text);
                }
        }


        public void CheckBox_Unchecked(object sender, RoutedEventArgs e) //if left unchecked it will delete txtfile containing login info
            {                                                                      //currently doesn't work
                if (System.IO.File.Exists(@"C:\Users\plane\Desktop\WriteLine.txt"))
                {
                    try
                    {
                        System.IO.File.Delete(@"C:\Users\plane\Desktop\WriteLine.txt");
                    }
                    catch (System.IO.IOException)
                    {
                        return;
                    }
            }
    }
    
    public void Run_Click(object sender, RoutedEventArgs e)
        {
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
            userName.SendKeys(magentouserName);

            var password = driver.FindElement(By.XPath("//*[@id='login']"));
            password.SendKeys(magentoPass);

            var magLogin = driver.FindElement(By.XPath("//*[@id='loginForm']/div/div[5]/input"));
            magLogin.Click();

            //navigating to one's own account

            System.Threading.Thread.Sleep(1000);

            var customersTab = driver.FindElement(By.XPath("//*[@id='nav']/li[4]"));
            customersTab.Click();

            System.Threading.Thread.Sleep(3000);

            var manageCustomers = driver.FindElement(By.XPath("//*[@id='nav']/li[4]/ul/li[1]/a"));
            manageCustomers.Click();

            var csrepemailSearch = driver.FindElement(By.XPath("//*[@id='customerGrid_filter_email']"));
            csrepemailSearch.SendKeys(workEmail);
            csrepemailSearch.SendKeys(Keys.Return);

            System.Threading.Thread.Sleep(4000); //NEEDED, otherwise the next line won't click the right element

            var csrepemailEnter = driver.FindElement(By.ClassName("even"));
            csrepemailEnter.Click();

            var movetoAddress = driver.FindElement(By.CssSelector("#customer_info_tabs_addresses"));
            movetoAddress.SendKeys(Keys.Return); //.SendKeys(Keys.Return) is necessary because the element is wrapped in a div or span, I guess.

            var addnewAddress = driver.FindElement(By.CssSelector("#add_address_button"));
            addnewAddress.Click();

            int count = 1;

            while (IsElementPresent(By.CssSelector("#address_item_shipping_item" + count)) == false) //loop throughs till it find the right CssSelector
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

            //Could add country here...but we only send replacements domestically.

            var addZip = driver.FindElement(By.CssSelector("#_item" + count + "postcode"));
            addZip.SendKeys(zipcode);

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
