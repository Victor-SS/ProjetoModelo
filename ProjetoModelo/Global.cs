using System;
using System.Collections.Generic;
using System.Configuration;
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
    }
}
