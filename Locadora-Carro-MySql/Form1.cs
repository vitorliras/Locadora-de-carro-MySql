using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Locadora_Carro_MySql
{
    public partial class Form1 : Form
    {
        MySqlConnection conexao;
        string data_source = "Server=localhost;user id=root;password=root;DATABASE=Locadora";
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //conectando com Preparing Statements
                conexao = new MySqlConnection(data_source);

                //abertura da conexao
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand();

                //definindo qual vai ser a conexao
                cmd.Connection = conexao;

                cmd.Prepare(); //preparo do comando de execução

                cmd.CommandText = "INSERT INTO cliente(nome,cpf,telefone,fk_idveiculo) " + //comando do SQL que vai ser executados
                                          " VALUES " +
                                          "(@nome, @cpf, @telefone, @fk_idveiculo) ";

                cmd.Parameters.AddWithValue("@nome", txtNome.Text); // criando paramentro e adicionando valores neles
                cmd.Parameters.AddWithValue("@cpf", txtCPF.Text);
                cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                cmd.Parameters.AddWithValue("@fk_idveiculo", txtidVeiculo.Text);

                cmd.ExecuteNonQuery(); //execução do comando

                MessageBox.Show("Contato Inserido");
                txtNome.Clear();
                txtCPF.Clear();
                txtTelefone.Clear();
                txtidVeiculo.Clear();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu: " + ex.Message,
                                "Erro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                conexao.Close();
            }

        }
    }
}
