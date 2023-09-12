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
    public class HANAConnection
    {

        private static HANAConnection instance;
        private string connectionString;
        public static HanaConnection con;
        private HANAConnection()
        {
            // Configurar la cadena de conexión aquí
            connectionString = "Server=192.168.0.6:30015;UserID=ADMINEMPAGRO;Password=vAmUs5M5aYd35cAk";
        }
        public static HANAConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HANAConnection();
                }
                return instance;
            }
        }

        public HanaConnection GetConnection()
        {
            if (con == null)
            {
                con = new HanaConnection(connectionString); // Ejemplo para SQL Server
                con.Open();
            }
            else
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
            }
            return con;
        }
        public static DataTable DQL(string OSQL)
        {
            DataTable dt;
            HanaDataAdapter dataAdapter = new HanaDataAdapter(OSQL, Instance.GetConnection());
            DataSet dataset = new DataSet();
            dataset.Tables.Add("aaa");
            dataAdapter.Fill(dataset, "AAA");
            dt = dataset.Tables[0];
            Instance.GetConnection().Close();
            return dt;
        }
        public static int DML(string OSQL)
        {
            HanaCommand Cmd = new HanaCommand(OSQL, Instance.GetConnection());
            int i = Cmd.ExecuteNonQuery();
            Instance.GetConnection().Close();
            return i;
        }
        public static object Excalar(string OSQL)
        {
            HanaCommand Cmd = new HanaCommand(OSQL, Instance.GetConnection());
            object i = Cmd.ExecuteScalar();
            Instance.GetConnection().Close();
            return i;
        }
    }
}