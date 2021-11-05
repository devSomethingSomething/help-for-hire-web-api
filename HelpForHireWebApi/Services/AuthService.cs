using HelpForHireWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Services
{
    [Obsolete("Will be replaced soon")]
    public static class AuthService
    {
        private static List<Auth> auths;

        static AuthService()
        {
            auths = new List<Auth>()
            {
                new Auth()
                {
                    Id = "1",
                    Password = "123"
                },
                new Auth()
                {
                    Id = "2",
                    Password = "123"
                }
            };
        }
        
        // public static List<Auth> GetAuths() => auths;

        public static Auth GetAuth(string id) => auths.FirstOrDefault(a => a.Id == id);
    }
}
