using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Etudiant
{
    public partial class FormListe : Form
    {
        SqlConnection cnx = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=E:\Etudiant\Etudiant\MaBase.mdf;Integrated Security=True");
        int idSelectionne = -1;

        public FormListe()
        {
            InitializeComponent();
            this.Load += FormListe_Load;
        }

        private void FormListe_Load(object sender, EventArgs e)
        {
            ChargerEtudiants();
            btnModifier.Enabled = false;
            btnSupprimer.Enabled = false;

            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        private void ChargerEtudiants()
        {
            try
            {
                cnx.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Etudiant", cnx);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
            finally
            {
                cnx.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    idSelectionne = Convert.ToInt32(row.Cells["ID"].Value);
                    txtNom.Text = row.Cells["Nom"].Value.ToString();
                    txtPrenom.Text = row.Cells["Prenom"].Value.ToString();
                    txtAdresse.Text = row.Cells["Adresse"].Value.ToString();
                    txtTelephone.Text = row.Cells["Telephone"].Value.ToString();

                    btnModifier.Enabled = true;
                    btnSupprimer.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("Erreur lors du chargement des données.");
                }
            }
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (idSelectionne != -1)
            {
                try
                {
                    cnx.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Etudiant SET Nom = @Nom, Prenom = @Prenom, Adresse = @Adresse, Telephone = @Tel WHERE ID = @ID", cnx);
                    cmd.Parameters.AddWithValue("@Nom", txtNom.Text);
                    cmd.Parameters.AddWithValue("@Prenom", txtPrenom.Text);
                    cmd.Parameters.AddWithValue("@Adresse", txtAdresse.Text);
                    cmd.Parameters.AddWithValue("@Tel", txtTelephone.Text);
                    cmd.Parameters.AddWithValue("@ID", idSelectionne);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Étudiant modifié avec succès !");
                    ChargerEtudiants();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur : " + ex.Message);
                }
                finally
                {
                    cnx.Close();
                }
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (idSelectionne != -1)
            {
                DialogResult res = MessageBox.Show("Supprimer cet étudiant ?", "Confirmation", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    try
                    {
                        cnx.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM Etudiant WHERE ID = @id", cnx);
                        cmd.Parameters.AddWithValue("@id", idSelectionne);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Étudiant supprimé.");
                        ChargerEtudiants();

                        btnModifier.Enabled = false;
                        btnSupprimer.Enabled = false;
                        idSelectionne = -1;

                        // Vider les champs
                        txtNom.Clear();
                        txtPrenom.Clear();
                        txtAdresse.Clear();
                        txtTelephone.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur : " + ex.Message);
                    }
                    finally
                    {
                        cnx.Close();
                    }
                }
            }
        }

        private void btnRetour_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form = new Form1();
            form.Show();
        }
    }
}
