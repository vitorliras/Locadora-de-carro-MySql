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
            ConfiguracaoListaClientes();
            carregarClientes();



        }

        private void carregarClientes()
        {
            try
            {
                conexao = new MySqlConnection(data_source);

                conexao.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.Prepare();
                cmd.CommandText = "SELECT * FROM cliente ORDER BY id DESC";

                MySqlDataReader reader = cmd.ExecuteReader();

                listCliente.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4)
                    };

                    listCliente.Items.Add(new ListViewItem(row));
                }
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

        private void ConfiguracaoListaClientes()
        {
            //configurando as colunas
            listCliente.View = View.Details; // forma de mostra os elementos na tela
            listCliente.LabelEdit = true;  
            listCliente.AllowColumnReorder = true;
            listCliente.FullRowSelect = true;
            listCliente.GridLines = true;

            //adicionando as colunas da lista cliente
            listCliente.Columns.Add("ID ", 50, HorizontalAlignment.Left);
            listCliente.Columns.Add("Nome ", 80, HorizontalAlignment.Left);
            listCliente.Columns.Add("CPF ", 60, HorizontalAlignment.Left);
            listCliente.Columns.Add("Telefone ", 60, HorizontalAlignment.Left);
            listCliente.Columns.Add("ID veiculo ", 60, HorizontalAlignment.Left);
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
                carregarClientes();
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection(data_source);

                conexao.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;

                cmd.Prepare();

                cmd.CommandText = "SELECT * FROM cliente WHERE nome LIKE @b OR cpf LIKE @b";

                cmd.Parameters.AddWithValue("@b", "%" + txtBuscarCliente.Text + "%"); //usando o % para buscar qualuqer caracter

                MySqlDataReader reader = cmd.ExecuteReader();  //fazendo um leitor, prara recuperar os dados existentes no mysql

                listCliente.Items.Clear(); //limpando a list, antes de executar

                //Forma de pecorrer todos os resultados do banco
                while (reader.Read())//reader é o leitor de dados e read é o metodo que ler cada um das linhas, caso n tiver, o while retorna false
                {
                    string[] row = // vetor prar organizar as linhas na lista
                    {
                        reader.GetString(0), //retorna o campo id
                        reader.GetString(1), //nome
                        reader.GetString(2), //cpf
                        reader.GetString(3), //telefone
                        reader.GetString(4)  //fkidveiculo
                    };

                    listCliente.Items.Add(new ListViewItem(row)); //abastecendo cada linha
                }

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
