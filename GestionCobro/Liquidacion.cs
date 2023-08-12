using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GestionCobro
{
    public class Liquidacion
    {
        private string Ruta = "", DocNum = "", CardName = "", Total = "", FechaInicio = "", FechaFin = "", Empresa = "", Codigo = "", fechaRegistro = "", horaRegistro = "", asignador = "", fechaLiquidacion = "", horaLiquidacion = "",fechaRecibo="",NumRecibo="",montoRecibo="",tipoPago="",tipoDocumento="",liquidador="",monto="",estadoAsignacion="",tipo="";

        public string Ruta1 { get => Ruta; set => Ruta = value; }
        public string DocNum1 { get => DocNum; set => DocNum = value; }
        public string CardName1 { get => CardName; set => CardName = value; }
        public string Total1 { get => Total; set => Total = value; }
        public string FechaInicio1 { get => FechaInicio; set => FechaInicio = value; }
        public string FechaFin1 { get => FechaFin; set => FechaFin = value; }
        public string Empresa1 { get => Empresa; set => Empresa = value; }
        public string Codigo1 { get => Codigo; set => Codigo = value; }
        public string FechaRegistro { get => fechaRegistro; set => fechaRegistro = value; }
        public string HoraRegistro { get => horaRegistro; set => horaRegistro = value; }
        public string Asignador { get => asignador; set => asignador = value; }
        public string FechaLiquidacion { get => fechaLiquidacion; set => fechaLiquidacion = value; }
        public string HoraLiquidacion { get => horaLiquidacion; set => horaLiquidacion = value; }
        public string FechaRecibo { get => fechaRecibo; set => fechaRecibo = value; }
        public string MontoRecibo { get => montoRecibo; set => montoRecibo = value; }
        public string NumRecibo1 { get => NumRecibo; set => NumRecibo = value; }
        public string TipoPago { get => tipoPago; set => tipoPago = value; }
        public string TipoDocumento { get => tipoDocumento; set => tipoDocumento = value; }
        public string Liquidador { get => liquidador; set => liquidador = value; }
        public string Monto { get => monto; set => monto = value; }
        public string EstadoAsignacion { get => estadoAsignacion; set => estadoAsignacion = value; }
        public string Tipo { get => tipo; set => tipo = value; }

        public Liquidacion() {
            Ruta1 = "";
            DocNum1 = "";
            CardName1 = "";
            Total1 = "";
            FechaInicio1 = "";
            FechaFin1 = "";
            Empresa1 = "";
            Codigo1 = "";
            FechaRegistro = "";
            HoraRegistro = "";
            Asignador = "";
            FechaLiquidacion = "";
            HoraLiquidacion = "";
            FechaRecibo = "";
            NumRecibo1 = "";
            MontoRecibo = "";
            TipoPago = "";
            TipoDocumento = "";
            Liquidador = "";
            Monto = "";
            EstadoAsignacion = "";
            Tipo = "";
        }
        public Liquidacion(string pDocNum,string pEmpresa,string pTipo)
        {
            DataTable dt;
            dt = HANAConnection.DQL(string.Format("Select {0}U_Ruta{0},TO_CHAR({0}DocNum{0}){0}DocNum{0}, {0}CardName{0}, {0}Total{0}, TO_CHAR({0}U_FechaInicio{0}, 'yyyy-MM-dd'){0}Desde{0}, TO_CHAR({0}U_FechaFin{0}, 'yyyy-MM-dd'){0}Hasta{0}, {0}Empresa{0},{0}Code{0},TO_CHAR({0}U_FechaRegistro{0}, 'yyyy-MM-dd')U_FechaRegistro,{0}U_HoraRegistro{0},{0}U_Asignador{0},TO_CHAR({0}U_FechaLiquidacion{0}, 'yyyy-MM-dd')U_FechaLiquidacion,{0}U_HoraLiquidacion{0},TO_CHAR({0}U_FechaRecibo{0}, 'yyyy-MM-dd')U_FechaRecibo,{0}U_ReciboNum{0},{0}U_MontoRecibo{0},{0}U_TipoPago{0},{0}U_TipoDocumento{0},{0}U_Liquidador{0},{0}U_Estado{0},{0}U_MontoDoc{0},{0}U_Tipo{0} from {0}SB1LD_EPG_PRO{0}.{0}@LOG_COBRO{0} a inner join {0}SB1LD_EPG_PRO{0}.{0}VwQQINV{0} b on a.{0}U_DocNum{0} = b.{0}DocNum{0} and a.{0}U_Empresa{0} = b.{0}Empresa{0}  and {0}U_Tipo{0}={0}Tipo{0} where {0}U_DocNum{0}='{1}' and {0}U_Empresa{0}='{2}' and {0}U_Tipo{0}='{3}'", (char)34, pDocNum, pEmpresa, pTipo));
            if (dt.Rows.Count > 0)
            {
                Ruta1 = dt.Rows[0][0].ToString();
                Total1 = Convert.ToDouble(dt.Rows[0][3].ToString()).ToString("C", CultureInfo.CurrentCulture);
                CardName1 = dt.Rows[0][2].ToString();
                FechaInicio1 = dt.Rows[0][4].ToString();
                FechaFin1 = dt.Rows[0][5].ToString();
                Empresa1 = dt.Rows[0][6].ToString();
                Codigo1 = dt.Rows[0][7].ToString();
                FechaRegistro = dt.Rows[0][8].ToString();
                HoraRegistro = dt.Rows[0][9].ToString();
                Asignador = dt.Rows[0][10].ToString();
                FechaLiquidacion = dt.Rows[0][11].ToString();
                HoraLiquidacion = dt.Rows[0][12].ToString();
                DocNum1 = pDocNum;
                FechaRecibo = dt.Rows[0][13].ToString();
                NumRecibo1 = dt.Rows[0][14].ToString();
                MontoRecibo = Convert.ToDouble(dt.Rows[0][15].ToString()).ToString("C", CultureInfo.CurrentCulture);
                TipoPago = dt.Rows[0][16].ToString();
                TipoDocumento = dt.Rows[0][17].ToString();
                Liquidador = dt.Rows[0][18].ToString();
                EstadoAsignacion = dt.Rows[0][19].ToString();
                MontoRecibo = dt.Rows[0][20].ToString();
                Tipo = dt.Rows[0][21].ToString();
            }
        }

    }
}