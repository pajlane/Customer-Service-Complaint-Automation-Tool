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
            
        }

        private bool IsElementPresent(By by) //for selecting the new address radio button
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

        

        public void Run_Click(object sender, RoutedEventArgs e)
        {


            

            int complaintNumber = 31699;
                //int.Parse(ComplaintNumberx.Text);

            string magentouserName = "plane";
            string magentoPass = "e1696pl";
            string workEmail = "plane@bobsredmill.com";

   
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

            //new complaint policy is to have everyone send replacements from their Sale and Marketing accounts on Magento
            //adjustments will require magento to search for the Rep's account, then add the customer's desired shipping address.

            //navigating to one's own account

           

            var customersTab = driver.FindElement(By.XPath("//*[@id='nav']/li[4]"));
            customersTab.Click();

           

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

            bool elementPresent = (IsElementPresent(By.CssSelector("#address_item_shipping_item" + count)));


            while (elementPresent == false)
            {
                count = count + 1;
              
            }

            var selectshipAdd = driver.FindElement(By.CssSelector("#address_item_shipping_item" + count));
            selectshipAdd.Click();



           






            // the item number/path name changes after it is saved.

            //cssselector/XPath will change with every new shipping address...i.e. item, item2, item3, etc.

            //selecting by Name or ClassName selects first entry only
            //might have to find a way to cycle through all of the addresses to get the correct number for css or xpath...


            System.Threading.Thread.Sleep(1000); // for testing

            //start inputting customer's address

            var fnField = driver.FindElement(By.XPath(""));
            fnField.SendKeys(firstName);

            var lnField = driver.FindElement(By.XPath(""));
            lnField.SendKeys(lastName);

            var emailField = driver.FindElement(By.XPath(""));
            emailField.SendKeys(email);

             

            

            

            

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
