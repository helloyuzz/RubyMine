using Newtonsoft.Json;
using RubyMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace RubyMine {
    public class CookieUtils {
        public static User Get(List<Claim> claims) {
            User user = null;
            string xmlContent = claims.FirstOrDefault(t => t.Type.Contains("emailaddress")).Value;
            if (string.IsNullOrEmpty(xmlContent) == false) {
                user = JsonConvert.DeserializeObject<User>(xmlContent);
                user.HashedPassword = null;
            } else {
                user = new User();
            }
            return user;
        }
    }
}