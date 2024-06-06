using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoModelo
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Sexo { get; set; }
        public string Celular {  get; set; }
        public int UsuarioId { get; set; }
        public Endereco Endereco { get; set; }

        public Cliente() 
        { 
            Id = 0;
            Nome = string.Empty;
            DataNascimento = DateTime.MinValue;
            CPF = string.Empty;
            Email = string.Empty;
            Sexo = string.Empty;
            Celular = string.Empty;
            UsuarioId = 0;
            Endereco = new Endereco();
        }
        AcessoBD acesso = new AcessoBD();
        DataTable dt = new DataTable();
        List<SqlParameter> parameters = new List<SqlParameter>();
        string sql = string.Empty;

        public DataTable Consultar()
        {
            try
            {
                parameters.Clear();
                sql = "select id, nome, data_nascimento, CPF, email, ";
                sql += "sexo, celular, usuarioId \n";
                sql += "from tblCliente \n";
                if (Id != 0)
                {
                    sql += "where id = @id \n";
                    parameters.Add(new SqlParameter("@id", Id));
                }
                else if (CPF != string.Empty)
                {
                    sql += "where cpf = @cpf \n";
                    parameters.Add(new SqlParameter("@cpf", CPF));
                }
                else if (Nome != string.Empty)
                {
                    sql += "where nome like @nome \n";
                    parameters.Add(new SqlParameter("@nome", '%' + Nome + '%'));
                }
                dt = acesso.Consultar(sql, parameters);
                if (Id != 0 || CPF != string.Empty && dt.Rows.Count == 1)
                {
                    Id = Convert.ToInt32(dt.Rows[0]["id"]);
                    Nome = dt.Rows[0]["nome"].ToString();
                    DataNascimento = Convert.ToDateTime(dt.Rows[0]["data_nascimento"]);
                    CPF = dt.Rows[0]["cpf"].ToString();
                    Email = dt.Rows[0]["email"].ToString();
                    Sexo = dt.Rows[0]["sexo"].ToString();
                    Celular = dt.Rows[0]["celular"].ToString();
                    UsuarioId = Convert.ToInt32(dt.Rows[0]["usuarioId"]);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
