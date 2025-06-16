using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Etudiant
{
    public partial class Form1 : Form
    {
        SqlConnection cnx;

        public Form1()
        {
            InitializeComponent();
            cnx = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=E:\Etudiant\Etudiant\MaBase.mdf;Integrated Security=True");
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            try
            {
                cnx.Open();

                string nom = txtNom.Text;
                string prenom = txtPren.Text;
                string adresse = txtAdrs.Text;
                string telephone = txtTel.Text;

                string requete = "INSERT INTO Etudiant (Nom, Prenom, Adresse, Telephone) VALUES (@Nom, @Prenom, @Adresse, @Telephone)";
                SqlCommand cmd = new SqlCommand(requete, cnx);

                cmd.Parameters.AddWithValue("@Nom", nom);
                cmd.Parameters.AddWithValue("@Prenom", prenom);
                cmd.Parameters.AddWithValue("@Adresse", adresse);
                cmd.Parameters.AddWithValue("@Telephone", telephone);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Étudiant ajouté avec succès !");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
            finally
            {
                cnx.Close();
            }

            // Rafraîchit les données affichées
            ChargerEtudiants();
        }

        private void btnCharger_Click(object sender, EventArgs e)
        {
            ChargerEtudiants();
        }

        private void ChargerEtudiants()
        {
            try
            {
                cnx.Open();
                string requete = "SELECT * FROM Etudiant";
                SqlDataAdapter da = new SqlDataAdapter(requete, cnx);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement : " + ex.Message);
            }
            finally
            {
                cnx.Close();
            }
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            this.Hide(); // Cache Form1

            FormListe formListe = new FormListe();
            formListe.FormClosed += (s, args) => this.Show(); // Quand on ferme FormListe, réaffiche Form1
            formListe.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Tu peux supprimer ça si inutilisé
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Tu peux supprimer ça si inutilisé
        }
    }
}
