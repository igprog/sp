using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Net;

namespace IGPROG
{
    /// <summary>
    /// Summary description for ApplicationSettings
    /// </summary>
    public class ApplicationSettings
    {


        string SiteUrl = ConfigurationManager.AppSettings["SiteUrl"].ToString();
        string ApplicationSettingsPath = ConfigurationManager.AppSettings["ApplicationSettingsPath"].ToString();
        string ApplicationSettingsPathForSave = ConfigurationManager.AppSettings["ApplicationSettingsPathForSave"];

        int _ActiveLanguage = 3;
        
        //info
        string _Name = null,
                _Address = null,
                _PostalCode = null,
                _City = null,
                _Country = null,
                _Tel = null,
                _GSM = null,
                _Site = null,
                _SiteURL = null,
                _Email = null,
                _EmailPassword = null,
                _ServerPort = null,
                _ServerHost = null,
                _AdditionalMail1 = null,
                _AdditionalMail2 = null,
                _AdditionalMail3 = null,
                _Iban = null,
                _SwiftCode = null;
        
        //content
        string _SiteTitle = "",
               _SiteShortDescription = "",
               _SiteLongDescription = "" ;
        
        //settings
        string _HrkEurRate = null;

        public ApplicationSettings()
        {
            //
            // TODO: Add constructor logic here
            //
          
            XmlDocument document = new XmlDocument();
            document.Load(SiteUrl + ApplicationSettingsPath);
            
            //Active language
            //XmlNodeList NodeList = document.DocumentElement.SelectNodes("/ApplicationSettings");
            //foreach (XmlNode node in NodeList)
            //{
            //    string language = node.SelectSingleNode("ActiveLanguage").InnerText;
            //    if (language == "Hrvatski") { _ActiveLanguage = 2; }
            //    if (language == "English") { _ActiveLanguage = 3; }
            //    if (language == "German") { _ActiveLanguage = 4; }
            //}

           

            //site info
            XmlNodeList infoNodeList = document.DocumentElement.SelectNodes("/ApplicationSettings/Info");
            foreach (XmlNode node in infoNodeList)
            {
                _Name = node.SelectSingleNode("Name").InnerText;
                _Address = node.SelectSingleNode("Address").InnerText;
                _PostalCode = node.SelectSingleNode("PostalCode").InnerText;
                _City = node.SelectSingleNode("City").InnerText;
                _Country = node.SelectSingleNode("Country").InnerText;
                _Tel = node.SelectSingleNode("Tel").InnerText;
                _GSM = node.SelectSingleNode("GSM").InnerText;
                _Site = node.SelectSingleNode("Site").InnerText;
                _SiteURL = node.SelectSingleNode("SiteURL").InnerText;
                _Email = node.SelectSingleNode("Email").InnerText;
                _EmailPassword = node.SelectSingleNode("EmailPassword").InnerText;
                _ServerPort = node.SelectSingleNode("ServerPort").InnerText;
                _ServerHost = node.SelectSingleNode("ServerHost").InnerText;
                _AdditionalMail1 = node.SelectSingleNode("AdditionalMail1").InnerText;
                _AdditionalMail2 = node.SelectSingleNode("AdditionalMail2").InnerText;
                _AdditionalMail3 = node.SelectSingleNode("AdditionalMail3").InnerText;
                _Iban = node.SelectSingleNode("Iban").InnerText;
                _SwiftCode = node.SelectSingleNode("SwiftCode").InnerText;
            }

            //site content
            XmlNodeList contentNodeList = document.DocumentElement.SelectNodes("/ApplicationSettings/SiteContent");
            foreach (XmlNode node in contentNodeList)
            {
                _SiteTitle = node.SelectSingleNode("SiteTitle").InnerText;
                _SiteShortDescription = node.SelectSingleNode("SiteShortDescription").InnerText;
                _SiteLongDescription = node.SelectSingleNode("SiteLongDescription").InnerText;
            }

            //settings
            XmlNodeList settingsNodeList = document.DocumentElement.SelectNodes("/ApplicationSettings/Settings");
            foreach (XmlNode node in settingsNodeList)
            {
                _HrkEurRate = node.SelectSingleNode("HrkEurRate").InnerText;
            }


        }


        public int ActiveLanguage
        {
            get { return _ActiveLanguage; }
            set { _ActiveLanguage = value; }
        }

        public string Name
        {
            get { return _Name; }
        }

        public string Address
        {
            get { return _Address; }
        }

        public string PostalCode
        {
            get { return _PostalCode; }
        }

        public string City
        {
            get { return _City; }
        }

        public string Country
        {
            get { return _Country; }
        }

        public string Tel
        {
            get { return _Tel; }
        }

        public string GSM
        {
            get { return _GSM; }
        }

        public string Site
        {
            get { return _Site; }
        }

        public string SiteURL
        {
            get { return _SiteURL; }
        }

        public string Email
        {
            get { return _Email; }
        }

        public string EmailPassword
        {
            get { return _EmailPassword; }
        }

        public string ServerPort
        {
            get { return _ServerPort; }
        }


        public string ServerHost
        {
            get { return _ServerHost; }
        }

        public string AdditionalMail1
        {
            get { return _AdditionalMail1; }
        }

        public string AdditionalMail2
        {
            get { return _AdditionalMail2; }
        }

        public string AdditionalMail3
        {
            get { return _AdditionalMail3; }
        }

        public string Iban
        {
            get { return _Iban; }
        }

        public string SwiftCode
        {
            get { return _SwiftCode; }
        }
             

        public string SiteTitle
        {
            get { return _SiteTitle; }
        }

        public string SiteShortDescription
        {
            get { return _SiteShortDescription; }
        }

        public string SiteLongDescription
        {
            get { return _SiteLongDescription; }
        }


        public string HrkEurRate
        {
            get { return _HrkEurRate; }
        }





    }

     

}