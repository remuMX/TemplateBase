using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BOL
{
     class ModelBase
    {
        #region Singleton
        private DataAccess conexion = DataAccess.Instance();
        private static volatile ModelBase instance = null;
        private static readonly object padlock = new object();

        private ModelBase() { }
        public static ModelBase Instance()
        {
            if (instance == null)
                lock (padlock)
                    if (instance == null)
                        instance = new ModelBase();
            return instance;
        }
        #endregion

        // Metodo para añadir un contacto
        // IN: Objeto contacto
        // OUT: valor booleano
       public  bool Add(Contacto contacto)
        {
            try
            {
                // Se declara el arreglo de parametros
                SqlParameter[] parameters = new SqlParameter[7];

                // Se inicializa cada uno de los valores de cada parametro.
                parameters[0] = new SqlParameter("@nombre", contacto.nombre);
                parameters[1] = new SqlParameter("@estado_civil", contacto.estado_civil);
                parameters[2] = new SqlParameter("@fecha_nacimiento", contacto.fecha_nacimiento);
                parameters[3] = new SqlParameter("@sexo", contacto.sexo);
                parameters[4] = new SqlParameter("@telefono_fijo", contacto.telefono_fijo);
                parameters[5] = new SqlParameter("@telefono_movil", contacto.telefono_movil);
                parameters[6] = new SqlParameter("@tipo_contacto", contacto.tipo_contacto);

                // Se indica el sp a utilizar
                string query = "sp_contactos_add";

                // Si se retornan mas de una linea quiere decir que se inserto, es decir, exito!
                if (conexion.Execute(query, parameters) > 0)
                {
                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception ex)
            {
                // Excepcion
                throw new ApplicationException(ex.Message);
            }
        }

        public bool Update(Contacto contacto)
        {
            try
            {
                // Se declara el arreglo de parametros
                SqlParameter[] parameters = new SqlParameter[8];

                // Se inicializa cada uno de los valores de cada parametro.
                parameters[0] = new SqlParameter("@nombre", contacto.nombre);
                parameters[1] = new SqlParameter("@estado_civil", contacto.estado_civil);
                parameters[2] = new SqlParameter("@fecha_nacimiento", contacto.fecha_nacimiento);
                parameters[3] = new SqlParameter("@sexo", contacto.sexo);
                parameters[4] = new SqlParameter("@telefono_fijo", contacto.telefono_fijo);
                parameters[5] = new SqlParameter("@telefono_movil", contacto.telefono_movil);
                parameters[6] = new SqlParameter("@tipo_contacto", contacto.tipo_contacto);
                parameters[7] = new SqlParameter("@id_contacto", contacto.id_contacto);


                // Se indica el sp a utilizar
                string query = "sp_contactos_update";

                // Si se retornan mas de una linea quiere decir que se actualizo, es decir, exito!
                if (conexion.Execute(query, parameters) > 0)
                {
                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception ex)
            {
                // Excepcion
                throw new ApplicationException(ex.Message);
            }
        }

        public bool Delete(int id)
        {
            try
            {
                // Se declara el arreglo de parametros
                SqlParameter[] parameters = new SqlParameter[1];

                // Se inicializa cada uno de los valores de cada parametro.
                parameters[0] = new SqlParameter("@id_contacto", id);


                // Se indica el sp a utilizar
                string query = "sp_contactos_delete";

                // Si se retornan mas de una linea quiere decir que se elimino, es decir, exito!
                if (conexion.Execute(query, parameters) > 0)
                {
                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception ex)
            {
                // Excepcion
                throw new ApplicationException(ex.Message);
            }
        }


        public Contacto GetById(int id)
        {
            try
            {
                // Se declara el arreglo de parametros
                SqlParameter[] parameters = new SqlParameter[1];

                // Se inicializa cada uno de los valores de cada parametro.
                parameters[0] = new SqlParameter("@id_contacto", id);

                // Se indica el sp a utilizar
                string query = "sp_contactos_getbyid";

                // Si se retornan mas de una linea quiere decir que se inserto, es decir, exito!
                DataTable resultado = conexion.Query(query, parameters);

                if (resultado.Rows.Count > 0)
                {
                    var mensaje = resultado.Rows[0]["Tipo Contacto"];

                    // Se crea un objeto tipo contacto 
                    Contacto contacto = new Contacto()
                    {
                        id_contacto = (int)resultado.Rows[0]["id_contacto"],
                        nombre = (string)resultado.Rows[0]["nombre"],
                        telefono_fijo = (string)resultado.Rows[0]["Telefono Fijo"],
                        telefono_movil = (string)resultado.Rows[0]["Telefono Movil"],
                        tipo_contacto = Convert.ToChar(resultado.Rows[0]["Tipo Contacto"]),
                        sexo = Convert.ToChar(resultado.Rows[0]["Sexo"]),
                        fecha_nacimiento = (DateTime)resultado.Rows[0]["Fecha de Nacimiento"],
                        estado_civil = Convert.ToChar(resultado.Rows[0]["Estado Civil"])
                    };

                    // Se retorna el objeto
                    return contacto;
                }

                // Se retorna por protocolo, pues no deberia llegar hasta aca.
                return null;
            }
            catch (Exception ex)
            {
                // Excepcion
                throw new ApplicationException(ex.Message);
            }
        }


        //
        //
        public BindingList<object> GetAll()
        {
            try
            {
                String query = "sp_contactos_getall";

                DataTable resultado = conexion.Query(query);

                BindingList<object> contactos = new BindingList<object>();


                foreach (DataRow item in resultado.Rows)
                {
                   // Crear un objeto para cada contacto
                    object contacto = new
                    {
                        id_contacto = (int)item["id_contacto"],
                        nombre = (string)item["Nombre"],
                        telefono_fijo = (string)item["Telefono Fijo"],
                        telefono_movil = (string)item["Telefono Movil"],
                        tipo_contacto = tipoContacto[Convert.ToChar(item["Tipo Contacto"])]
                    };
                    // Añadir a lista de contactos
                    contactos.Add(contacto);

                }

                // Retorna lista
                return contactos;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error GetAll\n" + ex.Message);
            }
        }


        public BindingList<object> GetAll(string nombre)
        {
            try
            {
                String query = "sp_contactos_get_nombre";

                DataTable resultado = conexion.Query(query, new SqlParameter[] { new SqlParameter("@nombre", nombre) });

                BindingList<object> contactos = new BindingList<object>();


                foreach (DataRow item in resultado.Rows)
                {
                    object contacto = new
                    {
                        id_contacto = (int)item["id_contacto"],
                        nombre = (string)item["Nombre"],
                        telefono_fijo = (string)item["Telefono Fijo"],
                        telefono_movil = (string)item["Telefono Movil"],
                        tipo_contacto = tipoContacto[Convert.ToChar(item["Tipo Contacto"])]
                    };
                    contactos.Add(contacto);

                }


                return contactos;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error GetAll\n" + ex.Message);
            }
        }
    }
}
