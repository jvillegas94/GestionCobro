using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace GestionCobro
{
    public static  class HANAConnection
    {
       // private static conn = new HanaConnection("Server=192.168.0.6:30015;UserID=ADMINEMPAGRO;Password=vAmUs5M5aYd35cAk");
        public static HanaConnection Conexion()
        {
           HanaConnection  conn = new HanaConnection(
        "Server=192.168.0.6:30015;UserID=ADMINEMPAGRO;Password=vAmUs5M5aYd35cAk");
            if (conn == null) {
                conn = new HanaConnection(
            "Server=192.168.0.6:30015;UserID=ADMINEMPAGRO;Password=vAmUs5M5aYd35cAk");
            }
            HanaConnection.ClearAllPools();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }
        public static void CerrarConexion()
        {
            HanaConnection.ClearAllPools();
            Conexion().Close();
            Conexion().Dispose();
        }

public static DataTable DQL(string OSQL) {
            DataTable dt;
            HanaDataAdapter dataAdapter = new HanaDataAdapter(OSQL, Conexion());
            DataSet dataset = new DataSet();
            dataset.Tables.Add("aaa");
            dataAdapter.Fill(dataset, "AAA");
            dt = dataset.Tables[0];
           CerrarConexion();
            return dt;
        }
        public static int DML(string OSQL)
        {
            HanaCommand Cmd = new HanaCommand(OSQL, Conexion());
            int i = Cmd.ExecuteNonQuery();
            CerrarConexion();
            return i;
        }
        public static object Excalar(string OSQL)
        {
            HanaCommand Cmd = new HanaCommand(OSQL, Conexion());
            object i = Cmd.ExecuteScalar();
            CerrarConexion();
            return i;
        }
    }
}