using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KlinicosDirectivos
{
    public static class Repositorio
    {
        private static void ChangeDatabase(this Klinicos_BEntities source, int idEstablecimiento, string initialCatalog = "", string userId = "", string password = "", bool integratedSecuity = true, string configConnectionStringName = "")
        {
            try
            {

                // use the const name if it's not null, otherwise
                // using the convention of connection string = EF contextname
                // grab the type name and we're done
                var configNameEf = string.IsNullOrEmpty(configConnectionStringName)
                    ? source.GetType().Name
                    : configConnectionStringName;

                // add a reference to System.Configuration
                var entityCnxStringBuilder = new EntityConnectionStringBuilder
                    (System.Configuration.ConfigurationManager
                        .ConnectionStrings[configNameEf].ConnectionString);

                // init the sqlbuilder with the full EF connectionstring cargo
                var sqlCnxStringBuilder = new SqlConnectionStringBuilder
                    (entityCnxStringBuilder.ProviderConnectionString);

                // only populate parameters with values if added
                if (!string.IsNullOrEmpty(initialCatalog))
                    sqlCnxStringBuilder.InitialCatalog = initialCatalog;




                sqlCnxStringBuilder.DataSource = ObtenerIp(idEstablecimiento);



                if (!string.IsNullOrEmpty(userId))
                    sqlCnxStringBuilder.UserID = userId;
                if (!string.IsNullOrEmpty(password))
                    sqlCnxStringBuilder.Password = password;

                // set the integrated security status
                sqlCnxStringBuilder.IntegratedSecurity = integratedSecuity;

                // now flip the properties that were changed
                source.Database.Connection.ConnectionString
                    = sqlCnxStringBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //Crear metodo que reciba id establecimiento y devuelva IP

        //Lista de IP: 
        //Cemefir: 192.168.71.10
        //Eizaguirre: 172.48.1.10
        //Germani: 172.28.1.220
        //Giovinazzo: 172.26.1.10
        //Niños: 172.29.1.100
        //Policlinico: 192.168.34.10
        //Rebasa: 172.46.1.10
        //Sakamoto: 172.45.1.10
        //Salud Mental: 192.168.70.10


        private static string ObtenerIp(int id)
        {
            string ip;
            switch (id)
            {
                //Cemefir: 192.168.71.10
                case 15:
                    ip = "192.168.71.10";
                    break;

                //Rebasa: 172.46.1.10
                case 10:
                    ip = "172.46.1.10";
                    break;

                //Eizaguirre: 172.48.1.10
                case 11:
                    ip = "172.48.1.10";
                    break;

                //Germani: 172.28.1.220
                case 17:
                    ip = "172.28.1.220";
                    break;

                //Giovinazzo: 172.26.1.10
                case 9:
                    ip = "172.26.1.10";
                    break;

                //Niños: 172.29.1.100
                case 3:
                    ip = "172.29.1.100";
                    break;

                //Policlinico: 192.168.34.10
                case 4:
                    ip = "192.168.34.10";
                    break;

                //Sakamoto: 172.45.1.10
                case 8:
                    ip = "172.45.1.10";
                    break;

                //Salud     : 192.168.70.10
                case 14:
                    ip = "192.168.70.10";
                    break;

                case 13: // odonto
                    ip = "192.168.75.10";
                    break;
                case 2: // equiza
                    ip = "172.18.1.10";
                    break;
                case 16: // Tablada
                    ip = "172.47.1.10";
                    break;
                default:
                    ip = "172.16.127.150";
                    break;
            }

            return ip;
        }


        public static Klinicos_BEntities CrearEntityFramework()
        {
            try
            {
                Klinicos_BEntities entidad = new Klinicos_BEntities();
                HttpContext context = HttpContext.Current;
                int idEstableciemiento = (int)context.Session["Establecimiento"];

                if (idEstableciemiento != 0)
                {
                    entidad.ChangeDatabase(idEstableciemiento, userId: "sa", password: "sql2018*", integratedSecuity: false);
                    
                    try
                    {
                        entidad.Atenciones.First();
                    }
                    catch (Exception ex)
                    {
                        entidad.ChangeDatabase(idEstableciemiento);
                    }
                    finally
                    {
                        entidad.Atenciones.First();
                    }
                }
                else
                {
                    throw new Exception("Session Expirada");
                }

                return entidad;
            }
            catch (Exception ex)
            {
                Exception error = new Exception("Error al Conectarse a la Base de datos" + ex.Message);
                throw error;
            }
        }

    }
}