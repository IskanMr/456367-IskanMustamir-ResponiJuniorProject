using Npgsql;
using System.Data;
using System.Windows.Forms;

namespace _456367_IskanMustamir_ResponsiJuniorProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private NpgsqlConnection _conn;
        String connstring = "Host=localhost;Port=2022;Username=postgres;Password=informatika;Database=ReponsiIskan";
        public static NpgsqlCommand cmd;
        private string sql = null;

        private DataTable _dt;
        private DataGridViewRow _r;

        public NpgsqlConnection conn { get { return _conn; } set { _conn = value; } }
        public DataTable dt { get { return _dt; } set { _dt = value; } }
        public DataGridViewRow r { get { return _r; } set { _r = value; } }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from k_insert(:_nama,:_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_nama", tbNamaKaryawan.Text);
                cmd.Parameters.AddWithValue("_id_dep", tbDepId.Text);
                if((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Berhasil menambahkan " + tbNamaKaryawan.Text, "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btnLoad.PerformClick();
                    tbNamaKaryawan = tbDepId = null;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                dgvData.DataSource = null;
                sql = @"select * from k_select()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                NpgsqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dgvData.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                r = dgvData.Rows[e.RowIndex];
                tbNamaKaryawan.Text = r.Cells["_nama"].Value.ToString();
                tbDepId.Text = r.Cells["_id_dep"].Value.ToString();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from k_update(:_id_karyawan,:_nama,:_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                cmd.Parameters.AddWithValue("_nama", tbNamaKaryawan.Text);
                cmd.Parameters.AddWithValue("_id_dep", tbDepId.Text);
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Berhasil mengedit " + tbNamaKaryawan.Text, "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btnLoad.PerformClick();
                    tbNamaKaryawan = tbDepId = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from k_delete(:_id_karyawan)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());                
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Berhasil menghapus " + tbNamaKaryawan.Text, "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btnLoad.PerformClick();
                    tbNamaKaryawan = tbDepId = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}