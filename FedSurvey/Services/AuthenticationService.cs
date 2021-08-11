using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FedSurvey.Services
{
    public class AuthenticationService
    {
        private readonly IConfiguration _configuration;

        public AuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetSamAccountName(string currentUserId, string otherUserId, string domain)
        {
            string samAccountName = (string.IsNullOrEmpty(otherUserId) ? currentUserId : otherUserId);

            // Strip out the domain name if it was supplied
            if (domain != null && (samAccountName.StartsWith(domain) || samAccountName.StartsWith(domain.ToLower())))
                samAccountName = samAccountName.Substring(domain.Length);

            return samAccountName;
        }

        private static string FormatDomainUsername(string userName, string domain)
        {
            string domainUsername;

            if (!string.IsNullOrEmpty(domain) && !userName.StartsWith(domain.ToLower()) && !userName.StartsWith(domain.ToUpper()))
                domainUsername = string.Format("{0}{1}", domain, userName);
            else
                domainUsername = userName;

            return domainUsername;
        }

        public bool Authenticate(string userName, string pwd)
        {
            bool success = false;

            try
            {
                SearchResult entry = GetADEntry(userName, pwd, null, null);

                // in future: use this variable to determine access to groups
                success = (entry != null) && entry.Properties["physicaldeliveryofficename"].Contains("OD/DPCPSI/OSC");
            }
            catch
            {
                // nothing to do; login failed, so success is false
            }
            return success;
        }

        private SearchResult GetADEntry(string userId, string userPwd, string samAccountName, string searchFilter)
        {
            string domainName = _configuration["DefaultDomain"];
            samAccountName = GetSamAccountName(userId, samAccountName, domainName);
            userId = FormatDomainUsername(userId, domainName);

            // Search the AD to find the user      
            using (DirectoryEntry dirEntry = new DirectoryEntry(_configuration["LDAPConnection"], userId, userPwd))
            {
                using (DirectorySearcher dirSearcher = new DirectorySearcher(dirEntry))
                {
                    if (string.IsNullOrEmpty(searchFilter))
                        dirSearcher.Filter = string.Format("(SAMAccountName={0})", samAccountName);
                    else
                        dirSearcher.Filter = searchFilter;

                    dirSearcher.PropertiesToLoad.Add("SAMAccountName");
                    dirSearcher.PropertiesToLoad.Add("givenName");
                    dirSearcher.PropertiesToLoad.Add("sn");
                    dirSearcher.PropertiesToLoad.Add("mail");
                    dirSearcher.PropertiesToLoad.Add("displayName");
                    dirSearcher.PropertiesToLoad.Add("distinguishedName");
                    dirSearcher.PropertiesToLoad.Add("userAccountControl");
                    dirSearcher.PropertiesToLoad.Add("physicaldeliveryofficename");

                    return dirSearcher.FindOne();
                }
            }
        }
    }
}
