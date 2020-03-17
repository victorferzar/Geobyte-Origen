using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace geologycmcc.Models.RolModels
{
    public class Rol
    {
        public string username { get; set; }

        public string userrol { get; set; }


        public Rol(string username, string userrol) {

            this.username = username;
            this.userrol = userrol;
        }
    }
}