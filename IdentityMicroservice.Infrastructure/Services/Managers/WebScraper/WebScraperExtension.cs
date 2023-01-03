using IdentityMicroservice.Application.ViewModels.AppInternal;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicroservice.Infrastructure.Services.Managers.WebScraper
{
    public class WebScraperExtension
    {
        public delegate dynamic FindElement(IWebElement element, string xPath);

        public static dynamic ErrorHandlingWrapper(FindElement findElementDelegate, IWebElement element, string xPath, string alternateValue = null)
        {
            try
            {
                return findElementDelegate(element, xPath);
            }
            catch (NoSuchElementException)
            {
                return alternateValue;
            }
        }

        public static void FindAboutInfo(IWebElement element, ref LinkedInFullProfileDTO profile)
        {
            string FindAboutProfilePicture(IWebElement element)
            {
                FindElement del = delegate (IWebElement element, string className) { return element.FindElement(By.ClassName(className)).GetAttribute("currentSrc"); };
                dynamic profilePicture = ErrorHandlingWrapper(del, element, "pv-top-card-profile-picture__image");
                return profilePicture;
            }

            Tuple<string, string> FindAboutName(IWebElement element)
            {
                FindElement del = delegate (IWebElement element, string className) { return element.FindElement(By.ClassName(className)).GetAttribute("title"); };
                dynamic name = ErrorHandlingWrapper(del, element, "pv-top-card-profile-picture__image");
                dynamic lastSpaceIndex = name.LastIndexOf(' ');
                dynamic firstName = name.Substring(0, lastSpaceIndex);
                dynamic lastName = name.Substring(lastSpaceIndex + 1);
                return new Tuple<string, string>(firstName, lastName);
            }

            string FindAboutSummary(IWebElement element)
            {
                FindElement del = delegate (IWebElement element, string xPath) { return element.FindElement(By.XPath(xPath)).GetAttribute("innerText"); };
                dynamic summary = ErrorHandlingWrapper(del, element, "//div[contains(@id, 'about')]/following-sibling::div[2]/div/div/div/span[1]");
                return summary;
            }

            string FindAboutCurrentEmployer(IWebElement element)
            {
                FindElement del = delegate (IWebElement element, string xPath) { return element.FindElement(By.XPath(xPath)).GetAttribute("innerText"); };
                dynamic currentEmployer = ErrorHandlingWrapper(del, element, "//div[contains(@aria-label, 'Current company')]", "Unemployed");
                return currentEmployer;
            }

            Tuple<string, string>? name = FindAboutName(element);
            profile.FirstName = name.Item1;
            profile.LastName = name.Item2;
            profile.ProfilePicture = FindAboutProfilePicture(element);
            profile.Summary = FindAboutSummary(element);
            profile.CurrentEmployer = FindAboutCurrentEmployer(element);
        }


        public static void FindBasicExperienceInfo(IWebElement element, ref LinkedInFullProfileDTO profile)
        {
            string FindExperienceEmployer(IWebElement element)
            {
                FindElement del = delegate (IWebElement element, string xPath) { return element.FindElement(By.XPath(xPath)).GetAttribute("innerText"); };
                dynamic employer = ErrorHandlingWrapper(del, element, "./span[1]/span[1]");
                return employer;
            }

            string FindExperienceRole(IWebElement element)
            {
                FindElement del = delegate (IWebElement element, string xPath) { return element.FindElement(By.XPath(xPath)).GetAttribute("innerText"); };
                dynamic role = ErrorHandlingWrapper(del, element, "./div/span/span[1]");
                return role;
            }

            string FindExperienceInterval(IWebElement element)
            {
                FindElement del = delegate (IWebElement element, string xPath) { return element.FindElement(By.XPath(xPath)).GetAttribute("innerText"); };
                dynamic interval = ErrorHandlingWrapper(del, element, "./span[2]/span[1]");
                return interval;
            }

            string FindExperienceLocation(IWebElement element)
            {
                FindElement del = delegate (IWebElement element, string xPath) { return element.FindElement(By.XPath(xPath)).GetAttribute("innerText"); };
                dynamic location = ErrorHandlingWrapper(del, element, "./span[3]/span[1]", "Work from home");
                return location;
            }

            string FindExperienceDescription(IWebElement element)
            {
                FindElement del = delegate (IWebElement element, string xPath) { return element.FindElement(By.XPath(xPath)).GetAttribute("innerText"); };
                dynamic description = ErrorHandlingWrapper(del, element, "./ul/li/div/ul/li/div/div/div/span[1]");
                var ceva = description.ToString();
                return description;
            }

            IWebElement header = element.FindElement(By.XPath("./div[1]/*[1]"));
            LinkedInExperienceDTO experience = new LinkedInExperienceDTO
            {
                Employer = FindExperienceEmployer(header),
                Role = FindExperienceRole(header),
                Interval = FindExperienceInterval(header),
                Location = FindExperienceLocation(header),
                Description = FindExperienceDescription(element)
            };

            profile.Experiences.Add(experience);
        }


        public static void FindAdvancedExperienceInfo(IWebElement element, ref LinkedInFullProfileDTO profile)
        {
            string FindExperienceDescription(IWebElement element)
            {
                FindElement del = delegate (IWebElement element, string xPath) { return element.FindElement(By.XPath(xPath)).GetAttribute("innerText"); };
                dynamic description = ErrorHandlingWrapper(del, element, "./div[2]/ul/li/div/ul/li/div/div/div/span[1]");
                return description;
            }

            string FindExperienceRole(IWebElement element)
            {
                FindElement del = delegate (IWebElement element, string xPath) { return element.FindElement(By.XPath(xPath)).GetAttribute("innerText"); };
                dynamic role = ErrorHandlingWrapper(del, element, "./div/span/span[1]");
                return role;
            }

            string FindExperienceInterval(IWebElement element)
            {
                FindElement del = delegate (IWebElement element, string xPath) { return element.FindElement(By.XPath(xPath)).GetAttribute("innerText"); };
                dynamic interval = ErrorHandlingWrapper(del, element, "./span/span[1]");
                return interval;
            }

            IWebElement header = element.FindElement(By.XPath("./div[1]/*[1]"));
            string employer = header.FindElement(By.XPath("./div/span[1]/span[1]")).GetAttribute("innerText");
            string location = header.FindElement(By.XPath("./span[2]/span[1]")).GetAttribute("innerText");

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> descriptions = element.FindElements(By.XPath("./div[2]/ul/li/div/div/div[1]/ul/li/div/div[2]"));
            foreach (IWebElement desc in descriptions)
            {
                IWebElement descHeader = desc.FindElement(By.XPath("./div[1]/a"));
                LinkedInExperienceDTO experience = new LinkedInExperienceDTO
                {
                    Employer = employer,
                    Location = location,
                    Description = FindExperienceDescription(desc),
                    Role = FindExperienceRole(descHeader),
                    Interval = FindExperienceInterval(descHeader)
                };

                profile.Experiences.Add(experience);
            }
        }
    }
}
