using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using geologycmcc.Models.RolModels;

namespace geologycmcc.Controllers.SeguridadRoles
{
    public class RolData
    {

        public RolData() {


        }

        public string permisoRoles(string rol) {
            String data = "";
            var resultado = getRoles().Where(x => x.userrol.Equals(rol)).ToList();
            int i = 0;

            foreach (Rol r in resultado) {
                i++;
                if (i == 1)
                {
                    data = r.username;
                }
                else {

                    data = data + "," + r.username;
                }

            }

            return data;
        }

        public List<Rol> getRoles() {
            List<Rol> lista = new List<Rol>();

            lista.Add(new Rol("Americas\verarc9", "Home"));


            return lista;
        } 
    }
}