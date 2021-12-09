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

        //tipo anulado
        private int ?idClienteSeleciona = null;
        private int ?idVeiculoSeleciona = null;

        public Form1()
        {
            InitializeComponent();
            ConfiguracaoListas();
            carregarClientes();
            carregarVeiculo();

        }

        //************ Cliente **************
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

        private void ConfiguracaoListas()
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
            listCliente.Columns.Add("CPF ", 100, HorizontalAlignment.Left);
            listCliente.Columns.Add("Telefone ", 100, HorizontalAlignment.Left);
            listCliente.Columns.Add("ID veiculo ", 60, HorizontalAlignment.Left);



            listaVeiculo.View = View.Details;
            listaVeiculo.LabelEdit = true;
            listaVeiculo.AllowColumnReorder = true;
            listaVeiculo.FullRowSelect = true;
            listaVeiculo.GridLines = true;

            listaVeiculo.Columns.Add("ID ", 50, HorizontalAlignment.Left);
            listaVeiculo.Columns.Add("marca ", 110, HorizontalAlignment.Left);
            listaVeiculo.Columns.Add("modelo ", 110, HorizontalAlignment.Left);

            
            
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

                if (idClienteSeleciona == null)
                {
                    //INSERT

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
                else
                {
                    //UPDATE
                    
                    cmd.Prepare(); 

                    cmd.CommandText = "UPDATE cliente SET nome = @nome, cpf = @cpf, telefone = @telefone, fk_idveiculo = @fk_idveiculo " +
                                      " WHERE id = @id ";

                    cmd.Parameters.AddWithValue("@nome", txtNome.Text); 
                    cmd.Parameters.AddWithValue("@cpf", txtCPF.Text);
                    cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    cmd.Parameters.AddWithValue("@fk_idveiculo", txtidVeiculo.Text);
                    cmd.Parameters.AddWithValue("@id", idClienteSeleciona);

                    cmd.ExecuteNonQuery(); 

                    MessageBox.Show("Contato Atualizado!");
                    txtNome.Clear();
                    txtCPF.Clear();
                    txtTelefone.Clear();
                    txtidVeiculo.Clear();
                    carregarClientes();
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

        private void listCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            //quando selecionado um ListViewItem, o mesmo vai pra uma lista,
            //logo deve-se tratar essa lista, para ver qual ietm foi selecionado 
            ListView.SelectedListViewItemCollection itensSelecionados = listCliente.SelectedItems;
           

            foreach (ListViewItem item in itensSelecionados)
            {
               idClienteSeleciona = int.Parse(item.SubItems[0].Text);

                txtNome.Text = item.SubItems[1].Text; //subitems é cada coluna de uma lista
                txtCPF.Text = item.SubItems[2].Text;
                txtTelefone.Text = item.SubItems[3].Text;
                txtidVeiculo.Text = item.SubItems[4].Text;

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            idClienteSeleciona = null;
            txtCPF.Text = "";
            txtNome.Text = "";
            txtTelefone.Text = "";
            txtidVeiculo.Text = "";
            carregarClientes();
        }

        private void Clear()
        {
            idClienteSeleciona = null;
            txtCPF.Text = "";
            txtNome.Text = "";
            txtTelefone.Text = "";
            txtidVeiculo.Text = "";
            carregarClientes();
            idVeiculoSeleciona = null;
            txtMarca.Text = "";
            txtModelo.Text = "";
            carregarVeiculo();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //Menssagem de aviso
                DialogResult conf = MessageBox.Show("Tem certeza que deseja excluir o registro?",
                    "ops, tem certeza?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (conf == DialogResult.Yes)
                {
                    //Excluir contato
                    conexao = new MySqlConnection(data_source);

                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conexao;

                    cmd.Prepare();

                    cmd.CommandText = "DELETE FROM cliente WHERE id = @id ";

                    cmd.Parameters.AddWithValue("@id", idClienteSeleciona);

                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Contato exluido");
                    Clear();
                    carregarClientes();
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                //Menssagem de aviso
                DialogResult conf = MessageBox.Show("Tem certeza que deseja excluir o registro?",
                    "ops, tem certeza?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (conf == DialogResult.Yes)
                {
                    //Excluir contato
                    conexao = new MySqlConnection(data_source);

                    conexao.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conexao;

                    cmd.Prepare();

                    cmd.CommandText = "DELETE FROM cliente WHERE id = @id ";

                    cmd.Parameters.AddWithValue("@id", idClienteSeleciona);

                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Contato exluido");
                    Clear();
                    carregarClientes();
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

        //************ Veiculo **************
        private void carregarVeiculo()
        {
            try
            {
                conexao = new MySqlConnection(data_source);

                conexao.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.Prepare();
                cmd.CommandText = "SELECT * FROM veiculo ORDER BY marca";

                MySqlDataReader reader = cmd.ExecuteReader();

                listaVeiculo.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2)
                    };

                    listaVeiculo.Items.Add(new ListViewItem(row));
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
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {

                
                conexao = new MySqlConnection(data_source);

                conexao.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;

                if (idVeiculoSeleciona == null)
                {
                    //INSERT

                    cmd.Prepare(); 

                    cmd.CommandText = "INSERT INTO veiculo(marca, modelo) " + 
                                              " VALUES " +
                                              "(@marca, @modelo) ";

                    cmd.Parameters.AddWithValue("@marca", txtMarca.Text); 
                    cmd.Parameters.AddWithValue("@modelo", txtModelo.Text);
                    

                    cmd.ExecuteNonQuery(); 

                    MessageBox.Show("Veiculo Inserido");
                    txtModelo.Clear();
                    txtMarca.Clear();
                    carregarVeiculo();
                }
                else
                {
                    //UPDATE

                    cmd.Prepare();

                    cmd.CommandText = "UPDATE veiculo SET marca = @marca, modelo = @modelo " +
                                      " WHERE idVeiculo = @id ";

                    cmd.Parameters.AddWithValue("@marca", txtMarca.Text);
                    cmd.Parameters.AddWithValue("@modelo", txtModelo.Text);
                    cmd.Parameters.AddWithValue("@id", idVeiculoSeleciona);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("veiculo Atualizado!");
                    txtModelo.Clear();
                    txtMarca.Clear();
                    carregarVeiculo();
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
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                conexao = new MySqlConnection(data_source);

                conexao.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;

                cmd.Prepare();

                cmd.CommandText = "SELECT * FROM veiculo WHERE marca LIKE @b OR modelo LIKE @b or idVeiculo LIKE @b";

                cmd.Parameters.AddWithValue("@b", "%" + txtBuscarVeiculo.Text + "%");

                MySqlDataReader reader = cmd.ExecuteReader();

                listaVeiculo.Items.Clear();

                
                while (reader.Read())
                {
                    string[] row = 
                    {
                        reader.GetString(0), 
                        reader.GetString(1), 
                        reader.GetString(2), 
                        
                    };

                    listaVeiculo.Items.Add(new ListViewItem(row));
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

        private void button7_Click(object sender, EventArgs e)
        {
            idVeiculoSeleciona = null;
            txtMarca.Text = "";
            txtModelo.Text = "";
            carregarVeiculo();
        }

        private void listaVeiculo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection itensSelecionados = listaVeiculo.SelectedItems;


            foreach (ListViewItem item in itensSelecionados)
            {
                idVeiculoSeleciona = int.Parse(item.SubItems[0].Text);

                txtMarca.Text = item.SubItems[1].Text; 
                txtModelo.Text = item.SubItems[2].Text;

            }
        }
    }
}
