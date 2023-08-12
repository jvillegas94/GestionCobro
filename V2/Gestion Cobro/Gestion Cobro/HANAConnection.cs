using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Gestion_Cobro
{
    public class HANAConnection
    {
        private HanaConnection conn;

        public HanaConnection Conn { get => conn; set => conn = value; }

        public HANAConnection()
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] Array_a_Descifrar = Convert.FromBase64String("7+IykzdS+M5Uokq3+QAmiQ==");
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
            SecureString clave = new NetworkCredential("", UTF8Encoding.UTF8.GetString(resultArray)).SecurePassword;
            clave.MakeReadOnly();
            HanaCredential hanaCredential = new HanaCredential("SYSTEM", clave);
            HanaConnection.ClearAllPools();
            Conn = new HanaConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Hana"].ConnectionString, hanaCredential);
        }

        public HanaConnection Conexion()
        {
            HanaConnection.ClearAllPools();
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            return Conn;
        }
        public void CerrarConexion()
        {
            Conn.Close();
        }

        public DataTable DQL(string OSQL)
        {
            DataTable dt;
            HanaDataAdapter dataAdapter = new HanaDataAdapter(OSQL, Conn);
            DataSet dataset = new DataSet();
            dataset.Tables.Add("aaa");
            dataAdapter.Fill(dataset, "AAA");
            dt = dataset.Tables[0];
            CerrarConexion();
            return dt;
        }
        public int DML(string OSQL)
        {
            HanaCommand Cmd = new HanaCommand(OSQL, Conn);
            int i = Cmd.ExecuteNonQuery();
            return i;
        }
    }
}