using IdentityMicroservice.Application.Common.Configurations;
using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.ViewModels.AppInternal;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Infrastructure.Services.Managers.WebScraper
{
    public class WebScraperManager : IWebScraperManager
    {
        private readonly WebScraperOptions _options;
        public WebScraperManager(IOptions<WebScraperOptions> webScraperOptions)
        {
            _options = webScraperOptions.Value;
        }
        public async Task<LinkedInFullProfileDTO> ScrapeLinkedInUser(string url)
        {
            // CONFIG
            var _chromeDriverPath = _options.ChromeDriverOptions.LocalPath;
            var _email = _options.LinkedInCredentialsOptions.Username;
            var _password = _options.LinkedInCredentialsOptions.Password;


            // LOGIN
            ChromeDriver? driver = new ChromeDriver(_chromeDriverPath);
            driver.Navigate().GoToUrl("https://www.linkedin.com/login");

            IWebElement? condWait = new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(drv => drv.FindElement(By.Id("username")));
            condWait.Clear();

            IWebElement? email = driver.FindElement(By.Id("username"));
            IWebElement? password = driver.FindElement(By.Id("password"));

            email.SendKeys(_email);
            password.SendKeys(_password);

            IWebElement? loginButton = driver.FindElement(By.ClassName("btn__primary--large"));
            loginButton.Click();


            LinkedInFullProfileDTO? profile = new LinkedInFullProfileDTO
            {
                Experiences = new List<LinkedInExperienceDTO>()
            };


            // PROFILE
            driver.Navigate().GoToUrl(url);
            Thread.Sleep(2000);

            IWebElement? aboutCard = driver.FindElement(By.XPath("//div[contains(@id, 'profile-content')]"));
            WebScraperExtension.FindAboutInfo(aboutCard, ref profile);


            // EXPERIENCE
            driver.Navigate().GoToUrl(url + "/details/experience/");
            Thread.Sleep(2000);

            //System.Collections.ObjectModel.ReadOnlyCollection<IWebElement>? experienceCards = driver.FindElements(By.XPath("(//ul[contains(@class, 'pvs-list')])[1]/li/div/div[1]"));
            //foreach (IWebElement? experienceCard in experienceCards)
            //{
            //    IWebElement? titleSection = experienceCard.FindElement(By.XPath("./div[2]/div[2]"));
            //    switch (titleSection.GetAttribute("localName"))
            //    {
            //        case "div":
            //            WebScraperExtension.FindBasicExperienceInfo(experienceCard, ref profile);
            //            break;

            //        case "a":
            //            WebScraperExtension.FindAdvancedExperienceInfo(experienceCard, ref profile);
            //            break;
            //    }
            //}

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement>? experienceCards = driver.FindElements(By.XPath("(//ul[contains(@class, 'pvs-list')])[1]/li/div/div[1]"));
            foreach ( IWebElement? experienceCard in experienceCards)
            {
                IWebElement? titileSection = experienceCard.FindElement(By.XPath("./div[2]"));
                switch (titileSection.GetAttribute("localName"))
                {
                    
                    case "div":
                        LinkedInExperienceDTO experience;
                        try
                        {
                            string? role = titileSection.FindElement(By.XPath("./div[1]/div/div/span/span[1]")).GetAttribute("innerText");
                            string? experienceInterval = titileSection.FindElement(By.XPath("./div[1]/div/span[2]/span[1]")).GetAttribute("innerText");
                            string? workLocation = titileSection.FindElement(By.XPath("./div[1]/div/span[3]/span[1]")).GetAttribute("innerText");
                            string? employerName = titileSection.FindElement(By.XPath("./div[1]/div/span[1]/span[1]")).GetAttribute("innerText");
                            string? experienceDescription = titileSection.FindElement(By.XPath("./div[2]/ul/li/div/ul/li/div/div/div/span[1]")).GetAttribute("innerText");

                            experience = new LinkedInExperienceDTO
                            {
                                Employer = employerName,
                                Role = role,
                                Interval = experienceInterval,
                                Location = workLocation,
                                Description = experienceDescription
                            };
                        }
                        catch (NoSuchElementException)
                        {

                            experience = new LinkedInExperienceDTO
                            {
                                Employer = null,
                                Role = null,
                                Interval = null,
                                Location = null,
                                Description = null
                            };
                            
                        }
                     
                        profile.Experiences.Add(experience);
                        
                        break;
                    case "a":
                        break;
                }
            }

            // END
            return profile;
        }
    }
}
