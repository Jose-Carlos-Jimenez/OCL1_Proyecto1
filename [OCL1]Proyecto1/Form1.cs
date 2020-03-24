using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.Generic;

namespace _OCL1_Proyecto1
{
    public partial class Form1 : Form
    {
        String route;
        Scanner scanner;
        LinkedListNode<string> n;
        String AFD;
        public Form1()
        {
            InitializeComponent();
            this.pictureBox12.Visible = false;
            showLoading();
        }

        public async void showLoading()
        {
            ShowM();
            Task otask = new Task(sayHello);
            otask.Start();
            await otask;
            HideM();
        }

        public async void proccessingImg()
        {
            Task otask = new Task(sayHello);
            otask.Start();
            this.pictureBox12.Visible = true;
            this.pictureBox5.Visible = false;
            await otask;
            this.pictureBox5.Visible = true;
            this.pictureBox12.Visible = false;
        }

        public async void sayingBye()
        {
            Task otask = new Task(sayHello);
            otask.Start();
            this.Controls.Clear();
            PictureBox nueva = pictureBox14;
            this.Controls.Add(nueva);
            this.pictureBox14.Visible = true;
            this.pictureBox14.SetBounds(0, 0, 1295, 766);
            pictureBox14.Show();
            label3.Visible = label4.Visible = label5.Visible = label6.Visible = label7.Visible = label8.Visible = label9.Visible = false;
            await otask;
            Application.Exit();
        }

        public void sayHello()
        {
            Thread.Sleep(4000);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            TabPage nuevaPestaña = new CustomTab("Nueva pestaña " + (tabControl1.TabCount + 1));
            tabControl1.TabPages.Add(nuevaPestaña);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if(this.scanner != null && this.scanner.getTokens().Count > 0)
            {
                this.n = this.scanner.names.First;
                this.AFD = "";
                String route = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + n.Value + ".png";
                String tableRoute = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + n.Value + "_TABLE" + ".png";
                Image actual = Image.FromFile(route);
                Image actualTable = Image.FromFile(tableRoute);
                this.pictureBox1.Image = actual;
                this.pictureBox2.Image = actualTable;

            }
            else
            {
                MessageBox.Show("¡Vaya! Lo siento, aún no tienes autómatas listos para mostrar, ingresalo en el editor de texto", "¡CIELOS GUMP ERES UN GENIO!", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            if (tabControl1.TabCount != 0)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Extensión .er |*.er";
                ofd.Title = "Elija el archivo de expresiones regulares";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName.Length > 0)
                {
                    route = ofd.FileName;
                    tabControl1.SelectedTab.Text = ofd.SafeFileName;
                    String read = System.IO.File.ReadAllText(route);
                    tabControl1.SelectedTab.Controls[0].Text = read;
                }
            }
            else
            {
                MessageBox.Show("Debe abrir una pestaña", "¡ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            if(tabControl1.SelectedTab != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Guarde el archivo";
                sfd.Filter = "Extensión .er |*.er";
                sfd.DefaultExt = "er";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                DialogResult result = sfd.ShowDialog();

                if (result == DialogResult.OK)
                {
                    String text = tabControl1.SelectedTab.Controls[0].Text;
                    StreamWriter sw = new StreamWriter(sfd.FileName);
                    sw.WriteLine(text);
                    sw.Close();
                }
            }
            else
            {
                MessageBox.Show("Ufff, parece que aún no tienes nada que guardar.", "¡Atención!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            sayingBye();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.pictureBox1.Image = this.pictureBox2.Image = null;
            if(tabControl1.SelectedTab != null)
            {
                string entry = this.tabControl1.SelectedTab.Controls[0].Text;
                scanner = new Scanner();
                scanner.analyze(entry);
                if (!scanner.error)
                {
                    scanner.getConj();
                    scanner.getExpr();
                    this.console.Text = scanner.getValuations();
                    this.console.BackColor = Color.FromArgb(29, 29, 29);
                    MessageBox.Show("¡Se ha finalizado el analisis!", "¡Felicidades!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Aún no has abierto una pestaña", "¡ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void ShowM()
        {
            this.pictureBox11.Visible = true;
            label3.Visible = label4.Visible = label5.Visible = label6.Visible = label7.Visible = label8.Visible = label9.Visible = false;
        }

        public void HideM()
        {
            this.pictureBox11.Visible = false;
            label3.Visible = label4.Visible = label5.Visible = label6.Visible = label7.Visible = label8.Visible = label9.Visible = true;
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            if(scanner != null && scanner.getTokens().Count > 0)
            {
                string route = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Tokens_error.xml";
                using (StreamWriter writer = new StreamWriter(route))
                {
                    //INICIO 
                    String doc = "<ListaErrores>\n";
                    //AGREGA LOS TOKENS ENCONTRADOS
                    foreach (Token tok in scanner.getTokens())
                    {
                        if (tok.getType() == Token.Type.ERROR)
                        {
                            doc += "<Error>\n";
                            doc += "\t<Valor>" + tok.getLex() + "</Valor>\n";
                            doc += "\t<Fila>" + tok.getRow() + "</Fila>\n";
                            doc += "\t<Columna>" + tok.getCol() + "</Columna>\n</Error>";
                        }
                    }
                    //CERRAR HTML
                    doc += "</ListaErrores>";
                    writer.WriteLine(doc);

                }

                String table = "digraph A{\nSD [shape=none, margin= 0, label=\n";
                table += " <<TABLE BORDER=\"0\" CELLBORDER=\"1\" CELLSPACING=\"0\" CELLPADDING=\"4\">\n";
                table += " <TR><TD bgcolor=\"black\"><font color=\"magenta\">VALOR</font></TD>";
                table += "<TD bgcolor=\"black\"><font color=\"magenta\">FILA</font></TD>";
                table += "<TD bgcolor=\"black\"><font color=\"magenta\">COLUMNA</font></TD>";
                table += "</TR>\n";

                foreach(Token tok in scanner.getTokens())
                {
                    if(tok.type == Token.Type.ERROR)
                    {
                        table += " <TR><TD bgcolor=\"black\"><font color=\"yellow\">" + tok.getLex() + "</font></TD>";
                        table += "<TD bgcolor=\"black\"><font color=\"red\">" + tok.getRow() + "</font></TD>";
                        table += "<TD bgcolor=\"black\"><font color=\"red\">" + tok.getCol() + "</font></TD>";
                        table += "</TR>\n";
                    }
                }

                table += "</TABLE>\n >];\n}";

                Console.WriteLine(table);

                String rutapdf = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Errores.pdf";
                String rutadot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Errores.dot";
                System.IO.File.WriteAllText(rutadot, table);
                String commandoDot = "dot.exe -Tpdf " + rutadot + " -o " + rutapdf;
                var comando = string.Format(commandoDot);
                var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/c" + comando);
                var proc = new System.Diagnostics.Process();
                procStart.UseShellExecute = false;
                procStart.CreateNoWindow = true;
                procStart.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                proc.StartInfo = procStart;
                proc.Start();
                proc.WaitForExit();

                MessageBox.Show("El reporte ha sido creado", "¡Reporte ensamblado!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                System.Diagnostics.Process.Start(route);
                System.Diagnostics.Process.Start(rutapdf);
            }
            else
            {
                MessageBox.Show("Aún no existe un documento analizado, por favor ingresa uno.", "¡RAYOS!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if(this.scanner != null)
            {
                proccessingImg();
                if (AFD == "")
                {
                    AFD = "_AFD";
                }
                else
                {
                    AFD = "";
                }
                getImage();
            }
            else
            {
                MessageBox.Show("¡Algo anda mal!Asegurate de cargar unos autómatas y leer los tokens.", "¡Aún no hay nada en edición!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            
        }

        private string  getImages()
        {
            if(n == null || n == this.scanner.names.Last)
            {
                this.n = this.scanner.names.First;
            }
            else
            {
                this.n = n.Next;
            }
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + n.Value + this.AFD + ".png";
        }

        private string getTable()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + n.Value + "_TABLE" + ".png";
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            try
            {
                Image actual = Image.FromFile(getImages());
                Image actualTable = Image.FromFile(getTable());
                this.pictureBox1.Image = actual;
                this.pictureBox2.Image = actualTable;

            }
            catch
            {
                MessageBox.Show("¡Algo anda mal!Asegurate de cargar unos autómatas y leer los tokens.", "¡Aún no hay nada en edición!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
        }

        private void getImage()
        {

            if(this.n != null)
            {
                this.n = this.scanner.names.First;
                String route = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + n.Value + this.AFD + ".png";
                Image actual = Image.FromFile(route);
                this.pictureBox1.Image = actual;

            }
            else
            {
                MessageBox.Show("No sea feca, ponga un archivo válido.", "¿ME QUIERES VER LA CARA?", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            if(scanner!= null)
            {
                string route = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Tokens.xml";
                using (StreamWriter writer = new StreamWriter(route))
                {
                    String doc = "<ListaTokens>\n";
                    foreach (Token tok in scanner.getTokens())
                    {
                        if(tok.getType() != Token.Type.ERROR)
                        {
                            doc += "<Token>\n" + "\t<Nombre>" + tok.getTypeDesc() + "</Nombre>\n";
                            doc += "\t<Valor>" + tok.getLex() + "</Valor>\n";
                            doc += "\t<Fila>" + tok.getRow() + "</Fila>\n";
                            doc += "\t<Columna>" + tok.getCol() + "</Columna>\n</Token>";
                        }
                    }
                    doc += "</ListaTokens>";
                    writer.WriteLine(doc);

                }
                MessageBox.Show("El reporte ha sido creado", "¡Reporte ensamblado!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                System.Diagnostics.Process.Start(route);
            }
            else
            {
                MessageBox.Show("Aún no hay nada para leer", "¡Oh lo lamento!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
    }
}
