using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoModelo
{
    public partial class frmCliente : Form
    {
        public frmCliente()
        {
            InitializeComponent();
        }
        bool load = false;
        Cliente cliente = new Cliente();
        public void LimparCampos()
        {
            cliente = new Cliente();
            txtPesquisa.Clear();
            txtNome.Clear();
            dtpDataNascimento.Value = DateTime.Parse("01/01/1990");
            txtCPF.Clear();
            txtEmail.Clear();
            txtEndereco.Clear();
            txtNumero.Clear();
            txtComplemento.Clear();
            txtBairro.Clear();
            txtCEP.Clear();
            txtCelular.Clear();
            rdbMasculino.Checked = true;
            rdbFeminino.Checked = false;
            cboCidade.SelectedIndex = -1;
            cboEstado.SelectedIndex = -1;
            txtPesquisa.Focus();
        }
        private void CarregarEstados()
        {
            try
            {
                cboEstado.DataSource = Global.ConsultarEstados();
                cboEstado.DisplayMember = "Estado";
                cboEstado.ValueMember = "Id";
                cboEstado.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->"+ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CarregarCidades()
        {
            if (!load)
            {
                return;
            }
            try
            {
                int estado = Convert.ToInt32(cboEstado.SelectedValue);
                cboCidade.DataSource = Global.ConsultarCidades(estado);
                cboCidade.DisplayMember = "Cidade";
                cboCidade.ValueMember = "Id";
                cboCidade.SelectedIndex = -1;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        
        private void frmCliente_Load(object sender, EventArgs e)
        {
            CarregarGridCliente();
            CarregarEstados();
            load = true;
        }
        private void cboEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCidades();
        }
        private void txtCPF_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Global.SomenteNumeros(e.KeyChar, (sender as TextBox).Text); //sender as textbox == sender é um textbox
        }
        private void txtCEP_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Global.SomenteNumeros(e.KeyChar, (sender as TextBox).Text);
        }
        private void txtPesquisa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (rdbCPF.Checked)
            {
                e.Handled = Global.SomenteNumeros(e.KeyChar, (sender as TextBox).Text);
            }
        }
        private void rdbNome_CheckedChanged(object sender, EventArgs e)
        {
            LimparCampos();
        }
        private void CarregarGridCliente()
        {
            try
            {
                grdDados.DataSource = cliente.Consultar();
                //Ocultando Colunas
                grdDados.Columns[0].Visible = false;
                grdDados.Columns[2].Visible = false;
                grdDados.Columns[4].Visible = false;
                grdDados.Columns[5].Visible = false;
                grdDados.Columns[7].Visible = false;
                //Definindo Cabeçalhos
                grdDados.Columns[1].HeaderText = "Nome";
                grdDados.Columns[3].HeaderText = "CPF";
                grdDados.Columns[6].HeaderText = "Celular";
                //Definindo largura  das colunas
                grdDados.Columns[1].Width = 200;
                grdDados.Columns[3].Width = 100;
                grdDados.Columns[6].Width = 100;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            cliente = new Cliente();
            if (rdbNome.Checked )
            {
                cliente.Nome = txtPesquisa.Text;
                CarregarGridCliente();
            }
            else if(rdbCPF.Checked && txtPesquisa.Text.Length == 11)
            {
                cliente.CPF = txtCPF.Text;
                CarregarGridCliente();
            }            
        }
        private string ValidarPreenchimento()
        {
            string msgErro = string.Empty;
            try
            {
                if(txtNome.Text == string.Empty)
                {
                    msgErro = "Preencha o Campo NOME.\n";
                }
                if (dtpDataNascimento.Value == DateTime.Parse("01/01/1990"))
                {
                    msgErro = "Preencha o Campo DATA DE NASCIMENTO.\n";
                }
                if (txtCPF.Text == string.Empty)
                {
                    msgErro = "Preencha o Campo CPF.\n";
                }
                else
                {
                    Cliente c = new Cliente();
                    c.CPF = txtCPF.Text;
                    c.Consultar();
                    if (cliente.Id == 0 && c.Id != 0 ||
                        cliente.Id != 0 && cliente.Id != c.Id && c.Id != 0)
                    {
                        msgErro += "Cliente já existente.\n";
                    }
                }
                if (txtEndereco.Text == string.Empty)
                {
                    msgErro = "Preencha o Campo ENDEREÇO.\n";
                }
                if(txtCEP.Text == string.Empty)
                {
                    msgErro = "Preencha o Campo CEP.\n";
                }
                if (txtNumero.Text == string.Empty)
                {
                    msgErro = "Preencha o Campo NUMERO.\n";
                }
                if (txtBairro.Text == string.Empty)
                {
                    msgErro = "Preencha o Campo BAIRRO.\n";
                }
                if (cboCidade.SelectedIndex == -1)
                {
                    msgErro = "Selecione o Campo CIDADE.\n";
                }
                if (cboEstado.SelectedIndex == -1)
                {
                    msgErro = "Selecione o Campo ESTADO.\n";
                }
                if (txtCelular.Text == string.Empty)
                {
                    msgErro = "Preencha o Campo CELULAR.\n";
                }
                if(txtEmail.Text == string.Empty)
                {
                    msgErro = "Preencha o Campo E-mail.\n";
                }
                else
                {
                    try
                    {
                        MailAddress mail = new MailAddress(txtEmail.Text);
                    }
                    catch
                    {
                        msgErro = "Campo E-MAIL Inválido.\n";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return msgErro;
        }
        private void PreencherFormulario()
        {
            try
            {
                txtNome.Text = cliente.Nome;
                dtpDataNascimento.Value = cliente.DataNascimento;
                txtCPF.Text = cliente.CPF;
                txtEmail.Text = cliente.Email;
                txtEndereco.Text = cliente.Endereco.Logradouro;
                txtNumero.Text = cliente.Endereco.Numero;
                txtComplemento.Text = cliente.Endereco.Complemento;
                txtBairro.Text = cliente.Endereco.Bairro;
                txtCEP.Text = cliente.Endereco.CEP;
                txtCelular.Text = cliente.Celular;
                rdbMasculino.Checked = cliente.Sexo == "M";
                rdbFeminino.Checked = cliente.Sexo == "F";
                cboEstado.SelectedValue = Global.ConsultarEstado(cliente.Endereco.CidadeId);
                cboCidade.SelectedValue = cliente.Endereco.CidadeId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void PreencherClasse()
        {
            cliente.Nome = txtNome.Text;
            cliente.DataNascimento = dtpDataNascimento.Value;
            cliente.CPF = txtCPF.Text;
            cliente.Email = txtEmail.Text;
            cliente.Endereco.Logradouro = txtEndereco.Text;
            cliente.Endereco.Numero = txtNumero.Text;
            cliente.Endereco.Bairro = txtBairro.Text;
            cliente.Endereco.Complemento = txtComplemento.Text;
            cliente.Endereco.CEP = txtCEP.Text;
            cliente.Celular = txtCelular.Text;
            cliente.Endereco.CidadeId = Convert.ToInt32(cboCidade.SelectedValue);
            cliente.Sexo = rdbMasculino.Checked ? "M" : "F;";
        }
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void grdDados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                cliente = new Cliente();
                cliente.Id = Convert.ToInt32(grdDados.SelectedRows[0].Cells[0].Value);
                cliente.Consultar();
                PreencherFormulario();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                string mensagem = ValidarPreenchimento();
                if (mensagem != string.Empty)
                {
                    MessageBox.Show(mensagem, "Erro de Preenchimento",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                PreencherClasse();
                cliente.Gravar();
                MessageBox.Show("Cliente gravado com sucesso!",
                    "Cadastro de Cliente",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro-->" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
