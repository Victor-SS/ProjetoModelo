using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            CarregarEstados();
            load = true;
        }

        private void cboEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarCidades();
        }
    }
}
