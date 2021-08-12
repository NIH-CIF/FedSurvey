using FedSurvey.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FedSurvey.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        internal static readonly char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        public static string GetUniqueKey(int size)
        {
            byte[] data = new byte[4 * size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }

        public static string GetUniqueKeyOriginal_BIASED(int size)
        {
            char[] chars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public bool IsValid(string body, CoreDbContext context)
        {
            var children = _configuration.GetChildren();

            if (children.Any(item => item.Key == "DefaultDomain") && children.Any(item => item.Key == "LDAPConnection"))
            {
                Token token = context.Tokens.Where(t => t.Body == body).FirstOrDefault();

                if (token == null || DateTime.Compare(DateTime.Now, token.ExpiresAt) >= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public bool IsValidHeaders(IHeaderDictionary headers, CoreDbContext context)
        {
            var children = _configuration.GetChildren();

            if (headers.ContainsKey("token"))
                return IsValid(headers["token"], context);
            else if (!(children.Any(item => item.Key == "DefaultDomain") && children.Any(item => item.Key == "LDAPConnection")))
                return true;
            else
                return false;
        }
    }
}