using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Drawing.Printing;
using System.IO;
using Usuarios_planta.Capa_presentacion;





namespace Usuarios_planta.Formularios
{
    public partial class FormOrden : Form
    {
        MySqlConnection con = new MySqlConnection("server=;Uid=;password=;database=dblibranza;port=3306;persistsecurityinfo=True;");

        Comandos cmds = new Comandos();
        MySqlDataReader dr;

        public FormOrden()
        {
            InitializeComponent();
            cargar_coordinador();            
        }

        DateTime fecha = DateTime.Now;

        private void FormOrden_Load(object sender, EventArgs e)        
        {
            TxtRauto.Enabled = false;
            TxtValor_Rtq.Enabled = false;
            Txtcod_giro.Enabled = false;
            lbexonerar.Visible = false;
            lblfecha_actual.Text = fecha.ToString();
            BtnSimulador.Visible = false;
            TxtNom_entidad1.Visible = false;
            Txtobligacion1.Visible = false;
            TxtNit1.Visible = false;
            TxtValor1.Visible = false;
            TxtNom_entidad2.Visible = false;
            Txtobligacion2.Visible = false;
            TxtNit2.Visible = false;
            TxtValor2.Visible = false;
            TxtNom_entidad3.Visible = false;
            Txtobligacion3.Visible = false;
            TxtNit3.Visible = false;
            TxtValor3.Visible = false;
            TxtNom_entidad4.Visible = false;
            Txtobligacion4.Visible = false;
            TxtNit4.Visible = false;
            TxtValor4.Visible = false;
            TxtNom_entidad5.Visible = false;
            Txtobligacion5.Visible = false;
            TxtNit5.Visible = false;
            TxtValor5.Visible = false;
            TxtNom_entidad6.Visible = false;
            Txtobligacion6.Visible = false;
            TxtNit6.Visible = false;
            TxtValor6.Visible = false;
            TxtNom_entidad7.Visible = false;
            Txtobligacion7.Visible = false;
            TxtNit7.Visible = false;
            TxtValor7.Visible = false;
            TxtNom_entidad8.Visible = false;
            Txtobligacion8.Visible = false;
            TxtNit8.Visible = false;
            TxtValor8.Visible = false;
            MySqlCommand cmd = new MySqlCommand("SELECT nombre_entidad FROM tf_entidades", con);
            con.Open();
            dr = cmd.ExecuteReader();
            AutoCompleteStringCollection Collection = new AutoCompleteStringCollection();
            while (dr.Read())
            {
                Collection.Add(dr.GetString(0));
            }
            TxtNom_entidad1.AutoCompleteCustomSource = Collection;
            TxtNom_entidad2.AutoCompleteCustomSource = Collection;
            TxtNom_entidad3.AutoCompleteCustomSource = Collection;
            TxtNom_entidad4.AutoCompleteCustomSource = Collection;
            TxtNom_entidad5.AutoCompleteCustomSource = Collection;
            TxtNom_entidad6.AutoCompleteCustomSource = Collection;
            TxtNom_entidad7.AutoCompleteCustomSource = Collection;
            TxtNom_entidad8.AutoCompleteCustomSource = Collection;
            dr.Close();
            con.Close();
        }

        public void cargar_coordinador()
        {
            con.Open();
            string query = "SELECT nombre_coordinador from tf_coordinador order by nombre_coordinador asc";
            MySqlCommand comando = new MySqlCommand(query, con);
            MySqlDataAdapter da1 = new MySqlDataAdapter(comando);
            DataTable dt = new DataTable();
            da1.Fill(dt);
            con.Close();
            DataRow fila = dt.NewRow();
            fila["nombre_coordinador"] = "Seleccione el coordinador";
            dt.Rows.InsertAt(fila, 0);
            cmbCoordinador.ValueMember = "nombre_coordinador";
            cmbCoordinador.DisplayMember = "nombre_coordinador";
            cmbCoordinador.DataSource = dt;
        }

        private double cpk1, cpk2, cpk3, cpk4, cpk5, cpk6, cpk7, cpk8, cpk9, cpk10, cpk11, cpktotal=0, cpksaldo=0;

        private void TxtValor8_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(TxtValor8.Text, out cpk8))
                Sumar1();
            else
                TxtValor8.Text = cpk8.ToString();
        }        

        private void TxtValor_aprobado_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(TxtValor_aprobado.Text, out cpk10))
                Restar1();
            else
                TxtValor_aprobado.Text = cpk10.ToString();
        }

        private void TxtTotal_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(TxtTotal.Text, out cpk9))
                Restar1();
            else
                TxtSaldo.Text = cpk9.ToString();

            if (Convert.ToDouble(TxtSaldo.Text) <= 2000000)
            {
                MessageBox.Show("Saldo del cliente menor a 2 millones por favor proceder a realizar simulador", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                BtnSimulador.Visible = true;
            }
            else
            {
                BtnSimulador.Visible = false;
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        Bitmap bmp;

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            if (cmbcambio_condiciones.Text== "Cliente Acepta" || cmbcambio_condiciones.Text == "No Aplica")
            {
                if (cbimpagos.Checked && cbcuenta.Checked && cbrestriccion.Checked && cbpagador.Checked)
                {
                    if (TxtRauto.Text!="" && TxtValor_Rtq.Text=="")
                    {
                        MessageBox.Show("Importante diligenciar el valor del retanqueo Automatico para validar el simulador","Información",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    }
                    else
                    {
                        BtnGuardar.Visible = false;
                        BtnImprimir.Visible = false;
                        BtnLimpiar.Visible = false;

                        Graphics g = this.CreateGraphics();
                        bmp = new Bitmap(this.Size.Width, this.Size.Height, g);
                        Graphics mg = Graphics.FromImage(bmp);
                        mg.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, this.Size);
                        printPreviewDialog1.ShowDialog();
                        BtnGuardar.Visible = true;
                        BtnImprimir.Visible = true;
                        BtnLimpiar.Visible = true;
                    }                                   
                }
                else
                {
                    MessageBox.Show("Algunas de las actividades importantes se encuentra sin marcar", "Favor validar !!!", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }             
            } 
            else
            {
                MessageBox.Show("Por favor revisar cambio de condiciones", "Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }                                
        }

        private void TxtNom_entidad1_TextChanged_1(object sender, EventArgs e)
        {
            MySqlCommand comando = new MySqlCommand("SELECT * FROM tf_entidades WHERE nombre_entidad = @nombre_entidad ", con);
            comando.Parameters.AddWithValue("@nombre_entidad", TxtNom_entidad1.Text);
            con.Open();
            MySqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                TxtNit1.Text = registro["nit_entidad"].ToString();
            }
            con.Close();
        }       

        private void BtnBuscar_Click(object sender, EventArgs e)
        {

            cmds.Buscar_desembolso(TxtRadicado, TxtCedula, TxtNombre, TxtEstatura, TxtPeso, TxtCuenta, TxtScoring, TxtValor_aprobado,
                              TxtPlazo_solicitado, Txtplazo_aprobado, cmbDestino, cmbcambio_condiciones, TxtRauto, TxtValor_Rtq,TxtConvenio, TxtCod_oficina,  TxtNom_oficina, TxtCiudad,
                              Txtcod_giro, Txtoficina_girar, TxtId_gestor, TxtNom_gestor, cmbCoordinador, cmbDactiloscopia, cmbG_telefonica,cmbcampaña,
                              Txtobligacion1, TxtNom_entidad1, TxtNit1, TxtValor1, Txtobligacion2, TxtNom_entidad2, TxtNit2, TxtValor2,
                              Txtobligacion3, TxtNom_entidad3, TxtNit3, TxtValor3, Txtobligacion4, TxtNom_entidad4, TxtNit4, TxtValor4,
                              Txtobligacion5, TxtNom_entidad5, TxtNit5, TxtValor5, Txtobligacion6, TxtNom_entidad6, TxtNit6, TxtValor6,
                              Txtobligacion7, TxtNom_entidad7, TxtNit7, TxtValor7, Txtobligacion8, TxtNom_entidad8, TxtNit8, TxtValor8,
                              TxtTotal, TxtSaldo, cmbestado,TxtPendientes);

            if (TxtNom_entidad1.Text!= "")
            {
                Txtobligacion1.Visible = true;
                TxtNom_entidad1.Visible = true;
                TxtNit1.Visible = true;
                TxtValor1.Visible = true;                
            }
            if (TxtNom_entidad2.Text != "")
            {
                Txtobligacion2.Visible = true;
                TxtNom_entidad2.Visible = true;
                TxtNit2.Visible = true;
                TxtValor2.Visible = true;
            }
            if (TxtNom_entidad3.Text != "")
            {
                Txtobligacion3.Visible = true;
                TxtNom_entidad3.Visible = true;
                TxtNit3.Visible = true;
                TxtValor3.Visible = true;
            }
            if (TxtNom_entidad4.Text != "")
            {
                Txtobligacion4.Visible = true;
                TxtNom_entidad4.Visible = true;
                TxtNit4.Visible = true;
                TxtValor4.Visible = true;
            }
            if (TxtNom_entidad5.Text != "")
            {
                Txtobligacion5.Visible = true;
                TxtNom_entidad5.Visible = true;
                TxtNit5.Visible = true;
                TxtValor5.Visible = true;
            }
            if (TxtNom_entidad6.Text != "")
            {
                Txtobligacion6.Visible = true;
                TxtNom_entidad6.Visible = true;
                TxtNit6.Visible = true;
                TxtValor6.Visible = true;
            }
            if (TxtNom_entidad7.Text != "")
            {
                Txtobligacion7.Visible = true;
                TxtNom_entidad7.Visible = true;
                TxtNit7.Visible = true;
                TxtValor7.Visible = true;
            }
            if (TxtNom_entidad8.Text != "")
            {
                Txtobligacion8.Visible = true;
                TxtNom_entidad8.Visible = true;
                TxtNit8.Visible = true;
                TxtValor8.Visible = true;
            }
        }

        private bool validar()
        {
            bool ok = true;

            if (TxtCedula.Text == "")
            {
                ok = false;
                epError.SetError(TxtCedula, "Digitar cedula del cliente");
            }
            if (TxtNombre.Text == "")
            {
                ok = false;
                epError.SetError(TxtNombre, "Digitar nombre del cliente");
            }
            if (TxtScoring.Text == "")
            {
                ok = false;
                epError.SetError(TxtScoring, "Digitar n° Scoring");
            }
            if (cmbestado.Text == "")
            {
                ok = false;
                epError.SetError(cmbestado, "Digitar Valor");
            }
            if (TxtValor_aprobado.Text == "")
            {
                ok = false;
                epError.SetError(TxtValor_aprobado, "Digitar Valor");
            }
            if (TxtPlazo_solicitado.Text == "")
            {
                ok = false;
                epError.SetError(TxtPlazo_solicitado, "Digitar plazo solicitado");
            }
            if (Txtplazo_aprobado.Text == "")
            {
                ok = false;
                epError.SetError(Txtplazo_aprobado, "Digitar plazo solicitado");
            }
            if (cmbcampaña.Text=="")
            {
                ok = false;
                epError.SetError(cmbcampaña,"Debe seleccionar el tipo de campaña para el caso en gestion");
            }
            if (cmbDestino.Text == "")
            {
                ok = false;
                epError.SetError(cmbcampaña, "Debe seleccionar el destino de la operacion");
            }
            return ok;
        }

        private void BorrarMensajeError()
        {
            epError.SetError(TxtCedula, "");
            epError.SetError(TxtNombre, "");
            epError.SetError(TxtScoring, "");
            epError.SetError(cmbestado, "");
            epError.SetError(TxtPlazo_solicitado, "");
            epError.SetError(Txtplazo_aprobado, "");
            epError.SetError(Txtplazo_aprobado, "");
            epError.SetError(TxtValor_aprobado, "");
            epError.SetError(cmbcampaña, "");
            epError.SetError(cmbDestino,"");
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            BorrarMensajeError();
            if (validar())
            {
            cmds.Guardar_datos_desembolso(TxtRadicado, TxtCedula, TxtNombre, TxtEstatura, TxtPeso, TxtCuenta, TxtScoring, TxtValor_aprobado,
                              TxtPlazo_solicitado, Txtplazo_aprobado, cmbDestino, cmbcambio_condiciones, TxtRauto, TxtValor_Rtq, TxtConvenio, TxtCod_oficina, TxtNom_oficina, TxtCiudad,
                              Txtcod_giro, Txtoficina_girar, TxtId_gestor, TxtNom_gestor, cmbCoordinador, cmbDactiloscopia, cmbG_telefonica, cmbcampaña,
                              Txtobligacion1, TxtNom_entidad1, TxtNit1, TxtValor1, Txtobligacion2, TxtNom_entidad2, TxtNit2, TxtValor2,
                              Txtobligacion3, TxtNom_entidad3, TxtNit3, TxtValor3, Txtobligacion4, TxtNom_entidad4, TxtNit4, TxtValor4,
                              Txtobligacion5, TxtNom_entidad5, TxtNit5, TxtValor5, Txtobligacion6, TxtNom_entidad6, TxtNit6, TxtValor6,
                              Txtobligacion7, TxtNom_entidad7, TxtNit7, TxtValor7, Txtobligacion8, TxtNom_entidad8, TxtNit8, TxtValor8,
                              TxtTotal, TxtSaldo, cmbestado,TxtPendientes);
            }  

        }
        string estado_oficina;
        string estado_cartera;

        public string Identificacion { get; private set; }

        private void TxtNom_entidad1_TextChanged(object sender, EventArgs e)
        {
            MySqlCommand comando = new MySqlCommand("SELECT * FROM tf_entidades WHERE nombre_entidad = @nombre_entidad ", con);
            comando.Parameters.AddWithValue("@nombre_entidad", TxtNom_entidad1.Text);
            con.Open();
            MySqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                estado_cartera = registro["estado_entidad"].ToString();

                if (estado_cartera == "Cerrada")
                {
                    MessageBox.Show("Entidad se encuentra suspendida para Comprar!!! por favor revisar segun corresponda", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtNit1.Text = null;
                }
                else 
                {
                    TxtNit1.Text = registro["nit_entidad"].ToString();
                }                
            }
            con.Close();
        }

        private void TxtNom_entidad2_TextChanged(object sender, EventArgs e)
        {
            MySqlCommand comando = new MySqlCommand("SELECT * FROM tf_entidades WHERE nombre_entidad = @nombre_entidad ", con);
            comando.Parameters.AddWithValue("@nombre_entidad", TxtNom_entidad2.Text);
            con.Open();
            MySqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                estado_cartera = registro["estado_entidad"].ToString();

                if (estado_cartera == "Cerrada")
                {
                    MessageBox.Show("Entidad se encuentra suspendida para Comprar!!! por favor revisar segun corresponda", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtNit2.Text = null;
                }
                else
                {
                    TxtNit2.Text = registro["nit_entidad"].ToString();
                }
            }
            con.Close();
        }

        private void TxtNom_entidad3_TextChanged(object sender, EventArgs e)
        {
            MySqlCommand comando = new MySqlCommand("SELECT * FROM tf_entidades WHERE nombre_entidad = @nombre_entidad ", con);
            comando.Parameters.AddWithValue("@nombre_entidad", TxtNom_entidad3.Text);
            con.Open();
            MySqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                estado_cartera = registro["estado_entidad"].ToString();

                if (estado_cartera == "Cerrada")
                {
                    MessageBox.Show("Entidad se encuentra suspendida para Comprar!!! por favor revisar segun corresponda", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtNit3.Text = null;
                }
                else
                {
                    TxtNit3.Text = registro["nit_entidad"].ToString();
                }
            }
            con.Close();
        }

        private void TxtNom_entidad4_TextChanged(object sender, EventArgs e)
        {
            MySqlCommand comando = new MySqlCommand("SELECT * FROM tf_entidades WHERE nombre_entidad = @nombre_entidad ", con);
            comando.Parameters.AddWithValue("@nombre_entidad", TxtNom_entidad4.Text);
            con.Open();
            MySqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                estado_cartera = registro["estado_entidad"].ToString();

                if (estado_cartera == "Cerrada")
                {
                    MessageBox.Show("Entidad se encuentra suspendida para Comprar!!! por favor revisar segun corresponda", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtNit4.Text = null;
                }
                else
                {
                    TxtNit4.Text = registro["nit_entidad"].ToString();
                }
            }
            con.Close();
        }

        private void TxtNom_entidad5_TextChanged(object sender, EventArgs e)
        {
            MySqlCommand comando = new MySqlCommand("SELECT * FROM tf_entidades WHERE nombre_entidad = @nombre_entidad ", con);
            comando.Parameters.AddWithValue("@nombre_entidad", TxtNom_entidad5.Text);
            con.Open();
            MySqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                estado_cartera = registro["estado_entidad"].ToString();

                if (estado_cartera == "Cerrada")
                {
                    MessageBox.Show("Entidad se encuentra suspendida para Comprar!!! por favor revisar segun corresponda", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtNit5.Text = null;
                }
                else
                {
                    TxtNit5.Text = registro["nit_entidad"].ToString();
                }
            }
            con.Close();
        }

        private void TxtNom_entidad6_TextChanged(object sender, EventArgs e)
        {
            MySqlCommand comando = new MySqlCommand("SELECT * FROM tf_entidades WHERE nombre_entidad = @nombre_entidad ", con);
            comando.Parameters.AddWithValue("@nombre_entidad", TxtNom_entidad6.Text);
            con.Open();
            MySqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                estado_cartera = registro["estado_entidad"].ToString();

                if (estado_cartera == "Cerrada")
                {
                    MessageBox.Show("Entidad se encuentra suspendida para Comprar!!! por favor revisar segun corresponda", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtNit6.Text = null;
                }
                else
                {
                    TxtNit6.Text = registro["nit_entidad"].ToString();
                }
            }
            con.Close();
        }

        private void TxtNom_entidad7_TextChanged(object sender, EventArgs e)
        {
            MySqlCommand comando = new MySqlCommand("SELECT * FROM tf_entidades WHERE nombre_entidad = @nombre_entidad ", con);
            comando.Parameters.AddWithValue("@nombre_entidad", TxtNom_entidad7.Text);
            con.Open();
            MySqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                estado_cartera = registro["estado_entidad"].ToString();

                if (estado_cartera == "Cerrada")
                {
                    MessageBox.Show("Entidad se encuentra suspendida para Comprar!!! por favor revisar segun corresponda", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtNit7.Text = null;
                }
                else
                {
                    TxtNit7.Text = registro["nit_entidad"].ToString();                    
                }
            }
            con.Close();
        }

        private void TxtNom_entidad8_TextChanged(object sender, EventArgs e)
        {
            MySqlCommand comando = new MySqlCommand("SELECT * FROM tf_entidades WHERE nombre_entidad = @nombre_entidad ", con);
            comando.Parameters.AddWithValue("@nombre_entidad", TxtNom_entidad8.Text);
            con.Open();
            MySqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                estado_cartera = registro["estado_entidad"].ToString();

                if (estado_cartera == "Cerrada")
                {
                    MessageBox.Show("Entidad se encuentra suspendida para Comprar!!! por favor revisar segun corresponda", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtNit8.Text = null;
                }
                else
                {
                    TxtNit8.Text = registro["nit_entidad"].ToString();
                }
            }
            con.Close();
        }
        private void cmbCoordinador_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            
        }

        private void TxtValor1_Validated(object sender, EventArgs e)
        {
            if (Convert.ToDouble(TxtValor1.Text) > 0)
            {
               
                TxtValor1.Text = string.Format("{0:#,##0.00}", double.Parse(TxtValor1.Text));
                TxtTotal.Text = string.Format("{0:#,##0.00}", double.Parse(TxtTotal.Text));
                TxtSaldo.Text = string.Format("{0:#,##0.00}", double.Parse(TxtSaldo.Text));
            }
            else if (TxtValor1.Text == "")
            {
                TxtValor1.Text = Convert.ToString(0);
            }          
        }

        private void TxtValor2_Validated(object sender, EventArgs e)
        {
            if (Convert.ToDouble(TxtValor2.Text) > 0)
            {
                TxtValor2.Text = string.Format("{0:#,##0.00}", double.Parse(TxtValor2.Text));
                TxtTotal.Text = string.Format("{0:#,##0.00}", double.Parse(TxtTotal.Text));
                TxtSaldo.Text = string.Format("{0:#,##0.00}", double.Parse(TxtSaldo.Text));
            }
            else if (TxtValor2.Text == "")
            {
                TxtValor2.Text = Convert.ToString(0);
            }
        }

        private void TxtValor3_Validated(object sender, EventArgs e)
        {
            if (Convert.ToDouble(TxtValor3.Text) > 0)
            {
                TxtValor3.Text = string.Format("{0:#,##0.00}", double.Parse(TxtValor3.Text));
                TxtTotal.Text = string.Format("{0:#,##0.00}", double.Parse(TxtTotal.Text));
                TxtSaldo.Text = string.Format("{0:#,##0.00}", double.Parse(TxtSaldo.Text));
            }
            else if (TxtValor3.Text == "")
            {
                TxtValor3.Text = Convert.ToString(0);
            }
        }

        private void TxtValor4_Validated(object sender, EventArgs e)
        {
            if (Convert.ToDouble(TxtValor4.Text) > 0)
            {
                TxtValor4.Text = string.Format("{0:#,##0.00}", double.Parse(TxtValor4.Text));
                TxtTotal.Text = string.Format("{0:#,##0.00}", double.Parse(TxtTotal.Text));
                TxtSaldo.Text = string.Format("{0:#,##0.00}", double.Parse(TxtSaldo.Text));
            }
            else if (TxtValor4.Text == "")
            {
                TxtValor4.Text = Convert.ToString(0);
            }
        }

        private void TxtValor5_Validated(object sender, EventArgs e)
        {
            if (Convert.ToDouble(TxtValor5.Text) > 0)
            {
                TxtValor5.Text = string.Format("{0:#,##0.00}", double.Parse(TxtValor5.Text));
                TxtTotal.Text = string.Format("{0:#,##0.00}", double.Parse(TxtTotal.Text));
                TxtSaldo.Text = string.Format("{0:#,##0.00}", double.Parse(TxtSaldo.Text));
            }
            else if (TxtValor5.Text == "")
            {
                TxtValor5.Text = Convert.ToString(0);
            }
        }

        private void TxtValor6_Validated(object sender, EventArgs e)
        {
            if (Convert.ToDouble(TxtValor6.Text) > 0)
            {
                TxtValor6.Text = string.Format("{0:#,##0.00}", double.Parse(TxtValor6.Text));
                TxtTotal.Text = string.Format("{0:#,##0.00}", double.Parse(TxtTotal.Text));
                TxtSaldo.Text = string.Format("{0:#,##0.00}", double.Parse(TxtSaldo.Text));
            }
            else if (TxtValor6.Text == "")
            {
                TxtValor6.Text = Convert.ToString(0);
            }
        }

        private void TxtValor7_Validated(object sender, EventArgs e)
        {
            if (Convert.ToDouble(TxtValor7.Text) > 0)
            {
                TxtValor7.Text = string.Format("{0:#,##0.00}", double.Parse(TxtValor7.Text));
                TxtTotal.Text = string.Format("{0:#,##0.00}", double.Parse(TxtTotal.Text));
                TxtSaldo.Text = string.Format("{0:#,##0.00}", double.Parse(TxtSaldo.Text));
            }
            else if (TxtValor7.Text == "")
            {
                TxtValor7.Text = Convert.ToString(0);
            }
        }

        private void Btn_calculo_Click(object sender, EventArgs e)
        {
            TxtSaldo.Text = string.Format("{0:#,##0.00}", double.Parse(TxtSaldo.Text));
        }


        private void TxtValor_aprobado_Validated(object sender, EventArgs e)
        {
            TxtValor_aprobado.Text = string.Format("{0:#,##0.00}", double.Parse(TxtValor_aprobado.Text));
            TxtSaldo.Text = string.Format("{0:#,##0.00}", double.Parse(TxtSaldo.Text));
        }

        private void TxtValor8_Validated(object sender, EventArgs e)
        {
            if (Convert.ToDouble(TxtValor8.Text) > 0)
            {
                TxtValor8.Text = string.Format("{0:#,##0.00}", double.Parse(TxtValor8.Text));
                TxtTotal.Text = string.Format("{0:#,##0.00}", double.Parse(TxtTotal.Text));
                TxtSaldo.Text = string.Format("{0:#,##0.00}", double.Parse(TxtSaldo.Text));
            }
            else if (TxtValor8.Text == "")
            {
                TxtValor8.Text = Convert.ToString(0);
            }
        }

        private void TxtNit1_KeyDown(object sender, KeyEventArgs e)
        {
            TxtNit1.ReadOnly = true;
        }

        private void TxtNit2_KeyDown(object sender, KeyEventArgs e)
        {
            TxtNit2.ReadOnly = true;
        }

        private void TxtNit3_KeyDown(object sender, KeyEventArgs e)
        {
            TxtNit3.ReadOnly = true;
        }

        private void TxtNit4_KeyDown(object sender, KeyEventArgs e)
        {
            TxtNit4.ReadOnly = true;
        }

        private void TxtNit5_KeyDown(object sender, KeyEventArgs e)
        {
            TxtNit5.ReadOnly = true;
        }

        private void TxtNit6_KeyDown(object sender, KeyEventArgs e)
        {
            TxtNit6.ReadOnly = true;
        }

        private void TxtNit7_KeyDown(object sender, KeyEventArgs e)
        {
            TxtNit7.ReadOnly = true;
        }

        private void TxtNit8_KeyDown(object sender, KeyEventArgs e)
        {
            TxtNit8.ReadOnly = true;
        }

        private void TxtRadicado_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtCedula_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtEstatura_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtPeso_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtCuenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtScoring_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtValor_aprobado_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtPlazo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtRauto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtConvenio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtCod_oficina_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtId_gestor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtNom_gestor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void Txtobligacion1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void Txtobligacion3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void Txtobligacion4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void Txtobligacion5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void Txtobligacion6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void Txtobligacion7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void Txtobligacion8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtNom_entidad1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtNom_entidad2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtNom_entidad3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtNom_entidad4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtNom_entidad5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtNom_entidad6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtNom_entidad7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtNom_entidad8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void TxtCod_oficina_Validated(object sender, EventArgs e)
        {
            MySqlCommand comando = new MySqlCommand("SELECT * FROM tf_oficinas WHERE codigo_oficina = @codigo ", con);
            comando.Parameters.AddWithValue("@codigo", TxtCod_oficina.Text);
            con.Open();
            MySqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                estado_oficina = registro["estado_oficina"].ToString();

                if (estado_oficina == "Cerrada")
                {
                    MessageBox.Show("Oficina a segmentar se encuentra cerrada, por favor revisar !!!", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    TxtNom_oficina.Text = registro["sucursal"].ToString();
                    TxtCiudad.Text = registro["ciudad"].ToString();
                    Txtcod_giro.Text = registro["cod_principal"].ToString();
                    Txtoficina_girar.Text = registro["sucursal_principal"].ToString();
                }
            }
            con.Close();

            if (TxtNom_oficina.Text.Contains("EMPRESAS"))
            {
                MessageBox.Show("Por favor confirmar con el asesor oficina a girar el cheque", "Importante !!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Txtcod_giro.Enabled = true;
            }
            else if (TxtNom_oficina.Text.Contains("BANCA PERSONAL"))
            {
                Txtcod_giro.Enabled = false;
            }           
        }

        private void Txtcod_giro_Validated(object sender, EventArgs e)
        {
            MySqlCommand comando = new MySqlCommand("SELECT * FROM tf_oficinas WHERE codigo_oficina = @codigo ", con);
            comando.Parameters.AddWithValue("@codigo", Txtcod_giro.Text);
            con.Open();
            MySqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                Txtoficina_girar.Text = registro["sucursal"].ToString();
            }
            con.Close();
        }

        private void Btn_comentarios_Click(object sender, EventArgs e)
        {
            Form formulario = new Frmcomentarios();
            formulario.Show();
        }

        private void Txtplazo_aprobado_Validated(object sender, EventArgs e)
        {
            if (TxtPlazo_solicitado.Text!= Txtplazo_aprobado.Text)
            {
                MessageBox.Show("Proceder a realizar cambio de condiciones con el cliente","Importante !!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                cmbcambio_condiciones.Text = "Pendiente";
            }
            else
            {
                cmbcambio_condiciones.Text = "No Aplica";
            }
        }

        private void Txtplazo_aprobado_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void cmbcambio_condiciones_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }
        }

        private void btn_segmentacion_Click(object sender, EventArgs e)
        {
            Form formulario = new Esquema_segmentacion();
            formulario.Show();
        }

        private void cmbDestino_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cmbDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDestino.Text== "Compra de Cartera" && TxtNom_oficina.Text=="LETICIA")
            {
                lbexonerar.Visible = true;
            }
            else if (cmbDestino.Text == "CPK + RTQ" && TxtNom_oficina.Text == "LETICIA")
            {
                lbexonerar.Visible = true;
            }
            else if (cmbDestino.Text == "Compra de Cartera" && TxtNom_oficina.Text == "SAN ANDRES")
            {
                lbexonerar.Visible = true;
            }
            else if (cmbDestino.Text == "CPK + RTQ" && TxtNom_oficina.Text == "SAN ANDRES")
            {
                lbexonerar.Visible = true;
            }
            else
            {
                lbexonerar.Visible = false;
            }
            if (cmbDestino.Text == "Retanqueo")
            {
                TxtValor_Rtq.Enabled = true;
                TxtRauto.Enabled = true;
            }
            else if (cmbDestino.Text == "CPK + RTQ")
            {
                TxtValor_Rtq.Enabled = true;
                TxtRauto.Enabled = true;
                TxtNom_entidad1.Visible = true;
                Txtobligacion1.Visible = true;
                TxtNit1.Visible = true;
                TxtValor1.Visible = true;
            }
            else if (cmbDestino.Text == "Compra de Cartera")
            {                
                TxtNom_entidad1.Visible = true;
                Txtobligacion1.Visible = true;
                TxtNit1.Visible = true;
                TxtValor1.Visible = true;
            }
            else if (cmbDestino.Text == "Libre Inversion")
            {
                TxtNom_entidad1.Visible = false;
                Txtobligacion1.Visible = false;
                TxtNit1.Visible = false;
                TxtValor1.Visible = false;
            }
            else
            {
                TxtValor_Rtq.Enabled = false;
                TxtRauto.Enabled = false;
            }

        }

        private void TxtNom_oficina_TextChanged(object sender, EventArgs e)
        {
            if (cmbDestino.Text == "Compra de Cartera" && TxtNom_oficina.Text == "LETICIA")
            {
                lbexonerar.Visible = true;
            }
            else if (cmbDestino.Text == "CPK + RTQ" && TxtNom_oficina.Text == "LETICIA")
            {
                lbexonerar.Visible = true;
            }
            else if (cmbDestino.Text == "Compra de Cartera" && TxtNom_oficina.Text == "SAN ANDRES")
            {
                lbexonerar.Visible = true;
            }
            else if (cmbDestino.Text == "CPK + RTQ" && TxtNom_oficina.Text == "SAN ANDRES")
            {
                lbexonerar.Visible = true;
            }
            else
            {
                lbexonerar.Visible = false;
            }
        }

        private void TxtNom_entidad1_Validated(object sender, EventArgs e)
        {
            if (TxtNom_entidad1.Text=="")
            {
                TxtNit1.Text = null;                
            }
        }

        private void TxtNom_entidad2_Validated(object sender, EventArgs e)
        {
            if (TxtNom_entidad2.Text == "")
            {
                TxtNit2.Text = null;
            }
        }

        private void TxtNom_entidad3_Validated(object sender, EventArgs e)
        {
            if (TxtNom_entidad3.Text == "")
            {
                TxtNit3.Text = null;
            }
        }

        private void TxtNom_entidad4_Validated(object sender, EventArgs e)
        {
            if (TxtNom_entidad4.Text == "")
            {
                TxtNit4.Text = null;
            }
        }

        private void TxtNom_entidad5_Validated(object sender, EventArgs e)
        {
            if (TxtNom_entidad5.Text == "")
            {
                TxtNit5.Text = null;
            }
        }

        private void TxtNom_entidad6_Validated(object sender, EventArgs e)
        {
            if (TxtNom_entidad6.Text == "")
            {
                TxtNit6.Text = null;
            }
        }

        private void TxtNom_entidad7_Validated(object sender, EventArgs e)
        {
            if (TxtNom_entidad7.Text == "")
            {
                TxtNit7.Text = null;
            }
        }

        private void TxtValor_Rtq_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(TxtValor_Rtq.Text, out cpk11))
                Sumar1();
            else
                TxtValor_Rtq.Text = cpk11.ToString();
        }

        private void TxtValor_Rtq_Validated(object sender, EventArgs e)
        {
            if (Convert.ToDouble(TxtValor_Rtq.Text) > 0)
            {

                TxtValor_Rtq.Text = string.Format("{0:#,##0.00}", double.Parse(TxtValor_Rtq.Text));
                TxtTotal.Text = string.Format("{0:#,##0.00}", double.Parse(TxtTotal.Text));
                TxtSaldo.Text = string.Format("{0:#,##0.00}", double.Parse(TxtSaldo.Text));
            }
            else if (TxtValor_Rtq.Text == "")
            {
                TxtValor_Rtq.Text = Convert.ToString(0);
            }
        }

        private void cmbCampaña_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbcampaña.Text== "Campaña Digital Covid 19")
            {
                MessageBox.Show("Realizar validaciones correspondientes al evidente master, Base sigdoc y estado de pagare.", "Información",MessageBoxButtons.OK,MessageBoxIcon.Information);  
            }
            else if (cmbcampaña.Text == "Campaña CAP")
            {
                MessageBox.Show("Recordar verificar Scoring aprobado en cascada antigua EVR o cascada nueva PRL.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (cmbcampaña.Text == "Campaña Leads Digital Covid 19")
            {
                MessageBox.Show("Realizar validaciones correspondientes al evidente master, Base sigdoc y estado de pagare.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (cmbcampaña.Text == "Campaña Leads")
            {
                MessageBox.Show("No tiene ninguna Validacion adicional, sin novedad.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }     
        }

        private void TxtCuenta_Validated(object sender, EventArgs e)
        {
            string largo = TxtCuenta.Text;
            string length = Convert.ToString(largo.Length);

            if (Convert.ToInt32(length) < 20)
            {
                MessageBox.Show("Numero de la cuenta del cliente no contiene los 20 digitos correspondientes !! por favor revisar");
            }
        }
        
        private void pbAñadir_cpk_Click(object sender, EventArgs e)
        {
            if (cmbDestino.Text== "CPK + RTQ" || cmbDestino.Text == "Compra de Cartera" || cmbDestino.Text == "Retanqueo")
            {
                if (Txtobligacion1.Visible == false)
                {
                    Txtobligacion1.Visible = true;
                    Txtobligacion1.Visible = true;
                    TxtNit1.Visible = true;
                    TxtValor1.Visible = true;
                }
                else if (Txtobligacion1.Visible == true && Txtobligacion2.Visible == false)
                {
                    TxtNom_entidad2.Visible = true;
                    Txtobligacion2.Visible = true;
                    TxtNit2.Visible = true;
                    TxtValor2.Visible = true;
                }
                else if (Txtobligacion1.Visible == true && Txtobligacion2.Visible == true && Txtobligacion3.Visible == false)
                {
                    TxtNom_entidad3.Visible = true;
                    Txtobligacion3.Visible = true;
                    TxtNit3.Visible = true;
                    TxtValor3.Visible = true;
                }
                else if (Txtobligacion1.Visible == true && Txtobligacion2.Visible == true && Txtobligacion3.Visible == true
                      && Txtobligacion4.Visible == false)
                {
                    TxtNom_entidad4.Visible = true;
                    Txtobligacion4.Visible = true;
                    TxtNit4.Visible = true;
                    TxtValor4.Visible = true;
                }
                else if (Txtobligacion1.Visible == true && Txtobligacion2.Visible == true && Txtobligacion3.Visible == true
                      && Txtobligacion4.Visible == true && Txtobligacion5.Visible == false)
                {
                    TxtNom_entidad5.Visible = true;
                    Txtobligacion5.Visible = true;
                    TxtNit5.Visible = true;
                    TxtValor5.Visible = true;
                }
                else if (Txtobligacion1.Visible == true && Txtobligacion2.Visible == true && Txtobligacion3.Visible == true
                      && Txtobligacion4.Visible == true && Txtobligacion5.Visible == true && Txtobligacion6.Visible == false)
                {
                    TxtNom_entidad6.Visible = true;
                    Txtobligacion6.Visible = true;
                    TxtNit6.Visible = true;
                    TxtValor6.Visible = true;
                }
                else if (Txtobligacion1.Visible == true && Txtobligacion2.Visible == true && Txtobligacion3.Visible == true
                      && Txtobligacion4.Visible == true && Txtobligacion5.Visible == true && Txtobligacion6.Visible == true
                      && Txtobligacion7.Visible == false)
                {
                    TxtNom_entidad7.Visible = true;
                    Txtobligacion7.Visible = true;
                    TxtNit7.Visible = true;
                    TxtValor7.Visible = true;
                }
                else if (Txtobligacion1.Visible == true && Txtobligacion2.Visible == true && Txtobligacion3.Visible == true
                      && Txtobligacion4.Visible == true && Txtobligacion5.Visible == true && Txtobligacion6.Visible == true
                      && Txtobligacion7.Visible == true && Txtobligacion8.Visible == false)
                {
                    TxtNom_entidad8.Visible = true;
                    Txtobligacion8.Visible = true;
                    TxtNit8.Visible = true;
                    TxtValor8.Visible = true;
                }
            }
            else if (cmbDestino.Text == "Libre Inversion")
            {
                MessageBox.Show("Destino del credito no admite cpk, por favor revisar!!","Información",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Previo a agregar carteras por favor seleccionar destino de la operación!!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            this.Close();
            Form formulario = new FormOrden();
            formulario.Show();
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TxtNom_entidad8_Validated(object sender, EventArgs e)
        {
            if (TxtNom_entidad8.Text == "")
            {
                TxtNit8.Text = null;
            }
        }
        private void TxtValor7_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(TxtValor7.Text, out cpk7))
                Sumar1();
            else
                TxtValor7.Text = cpk7.ToString();
        }

        private void TxtValor6_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(TxtValor6.Text, out cpk6))
                Sumar1();
            else
                TxtValor6.Text = cpk6.ToString();
        }

        private void TxtValor5_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(TxtValor5.Text, out cpk5))
                Sumar1();
            else
                TxtValor5.Text = cpk5.ToString();
        }

        private void TxtValor4_TextChanged(object sender, EventArgs e)
        {

            if (double.TryParse(TxtValor4.Text, out cpk4))
                Sumar1();
            else
                TxtValor4.Text = cpk4.ToString();
        }

        private void TxtValor3_TextChanged(object sender, EventArgs e)
        {

            if (double.TryParse(TxtValor3.Text, out cpk3))
                Sumar1();
            else
                TxtValor3.Text = cpk3.ToString();
        }

        private void TxtValor2_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(TxtValor2.Text, out cpk2))
                Sumar1();
            else
                TxtValor2.Text = cpk2.ToString();
        }

        private void TxtValor1_TextChanged(object sender, EventArgs e)
        {

            if (double.TryParse(TxtValor1.Text, out cpk1))
                Sumar1();
            else
                TxtValor1.Text = cpk1.ToString();
        }

        private void Sumar1()
        {
            cpktotal = cpk1+cpk2+cpk3+cpk4+cpk5+cpk6+cpk7+cpk8+cpk11;
            TxtTotal.Text = cpktotal.ToString();
        }
       
        private void Restar1()
        {
            cpksaldo = cpk10 - cpk9;
            TxtSaldo.Text = cpksaldo.ToString();
        }
    }
}
                        