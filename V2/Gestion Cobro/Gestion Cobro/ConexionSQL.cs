using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Gestion_Cobro
{
    public static class ConexionSQL
    {
        public static SqlConnection ConexionBD()
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                SqlConnectionStringBuilder osql = new SqlConnectionStringBuilder();
                osql.DataSource = "192.168.0.3";
                osql.InitialCatalog = "BRC_BD";
                osql.UserID = "master";
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                byte[] Array_a_Descifrar = Convert.FromBase64String("ixoL5NZphq+/3XO4Acy46Q==");
                byte[] keyArray;

                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes("Empaques Agroindustriales S.A."));

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
                {
                    Key = keyArray,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);
                osql.Password = UTF8Encoding.UTF8.GetString(resultArray);
                string conector = osql.ConnectionString;
                conn = new SqlConnection(conector);
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                //
            }

            return conn;
        }
        public static void CerrarConexionBD()
        {
            ConexionBD().Close();
        }
        public static void CerrarConexionBD1()
        {
            ConexionBD().Close();
            ConexionBD().Dispose();
        }
        //Metodo para consultar usuario
        public static SqlDataReader consultarUsuario(SqlCommand comando) //Salen las credenciasles en blanco
        {
            SqlConnection con = ConexionBD();
            comando.Connection = con;
            SqlDataReader reader = comando.ExecuteReader();
            con.Close();
            return reader;

        }
        //DEvuelve un DataSet
        public static DataSet consulta(string instruccion, string nombre_tabla)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = ConexionBD();
                SqlCommand cmd = new SqlCommand(instruccion, con);
                SqlDataAdapter adaptador = new SqlDataAdapter(cmd);
                adaptador.Fill(ds, nombre_tabla);
                con.Close();
            }
            catch (Exception ex)
            {
                //
            }
            return ds;
        }



        //Devuelve un DataTable
        public static DataTable consultaDataTable(string instruccion, string nombre_tabla)
        {
            SqlConnection con = ConexionBD();
            System.Data.SqlClient.SqlDataAdapter adaptador = new SqlDataAdapter(instruccion, con);
            System.Data.DataSet dsDataSet = new System.Data.DataSet();
            DataTable dtDataTable = null;
            adaptador.Fill(dsDataSet, nombre_tabla);
            dtDataTable = dsDataSet.Tables[0];
            con.Close();
            return dtDataTable;
        }

        // Data Manipulation Language => DML
        // Metodo DML q se dedica a realizar cambios en las tablas (insert, delete, update)
        public static int DML(string instruccion)
        {
            int result = 0;
            SqlConnection con = ConexionBD();
            SqlCommand cmd = new SqlCommand(instruccion, con);
            result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }
        // es para ver si existen datos con

        public static int existencia(string strSQL)
        {

            SqlConnection con = ConexionBD();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(strSQL, con);
            da.Fill(dt);
            con.Close();
            return dt.Rows.Count;
        }

        public static string ConsultaUnica(SqlCommand comando)
        {
            SqlConnection con = ConexionBD();
            comando.Connection = con;
            String resultado = comando.ExecuteScalar().ToString();
            con.Close();
            return resultado;

        }
        //-----------------------------------------------------------------------------------------------------------------------









    }
}