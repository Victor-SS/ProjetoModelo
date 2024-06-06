using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjetoModelo
{
    public static class Global
    {
        public static string conexao = string.Empty;
        public static string servidor = string.Empty;
        public static string banco = string.Empty;
        public static int IdUsuarioLogado = 0;
        
        public static string Criptografar(string senha)
        {
            Byte[] original;
            Byte[] criptografado;
            MD5 md5 = new MD5CryptoServiceProvider();
            original = ASCIIEncoding.Default.GetBytes(senha);
            criptografado = md5.ComputeHash(original);

            return Regex.Replace(BitConverter.ToString(criptografado), "-", "").ToLower();
        }
        public static void LerAppConfig()
        {
            servidor = ConfigurationManager.AppSettings.Get("servidor");
            banco = ConfigurationManager.AppSettings.Get("banco");

            conexao = $"Data Source={servidor};Initial catalog={banco};" +
                $"Integrated Security=true;";
        }
        public static DataTable ConsultarEstados()
        {
            try
            {
                string sql = "select id, estado from tblEstado \n";
                return new AcessoBD().Consultar(sql, new List<SqlParameter>());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataTable ConsultarCidades(int estadoId)
        {
            try
            {
                string sql = "select id, cidade from tblCidade \n";
                sql += "where estadoId = @estadoId";
                DataTable dt = new DataTable();
                AcessoBD acesso = new AcessoBD();
                List<SqlParameter> lista = new List<SqlParameter>();
                lista.Add(new SqlParameter("@estadoId", estadoId));
                dt = acesso.Consultar(sql, lista);
                return dt;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static int ConsultarEstado(int cidadeId)
        {
            try
            {
                int estado = 0;
                string sql = "select EstadoId from tblCidade \n";
                sql += "where Id = @Id";
                DataTable dt = new DataTable();
                AcessoBD acesso = new AcessoBD();
                List<SqlParameter> lista = new List<SqlParameter>();
                lista.Add(new SqlParameter("@Id", cidadeId));
                dt = acesso.Consultar(sql, lista);
                if (dt.Rows.Count == 1)
                {
                    estado = Convert.ToInt32(dt.Rows[0]["EstadoId"]);
                }
                return estado;
                //return new AcessoBD().Consultar(sql, new List<SqlParameter>().Add(new SqlParameter("@Id", cidadeId)));
            }
            catch (Exception ex) { throw new Exception(); }
        }
    }
}
