using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PocoGenerator
{
    class pnlSqliteUIModulePanel : Panel
    {
        public TextBox txtPathToDb = new TextBox();
        public OpenFileDialog dlgPathToDb = new OpenFileDialog();
        public pnlSqliteUIModulePanel()
        {
            Initialize();
        }
        private void Initialize()
        {
            this.BorderStyle = BorderStyle.None;
            this.Location = new System.Drawing.Point(2, 8);
            this.Size = new System.Drawing.Size(296, 21);

            dlgPathToDb.Filter = "SQlite DB files(*.db)|*.db";

            txtPathToDb.Location = new System.Drawing.Point(0, 0);
            txtPathToDb.Size = this.Size;

            string pathToSqliteDbFile = $"{Directory.GetCurrentDirectory()}\\_pathToSqlite_DB_file.txt";
            if (File.Exists(pathToSqliteDbFile))
                txtPathToDb.Text = File.ReadAllText(pathToSqliteDbFile);

            txtPathToDb.LostFocus += (object sender, EventArgs e) => 
                {
                    File.WriteAllText($"{Directory.GetCurrentDirectory()}\\_pathToSqlite_DB_file.txt", txtPathToDb.Text);
                };

            this.Controls.Add(txtPathToDb);
        }
        public void SetSqiteDBfilepath()
        {
            dlgPathToDb.ShowDialog();
            if (!String.IsNullOrEmpty(dlgPathToDb.FileName) && !String.IsNullOrWhiteSpace(dlgPathToDb.FileName))
            {
                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\_pathToSqlite_DB_file.txt", dlgPathToDb.FileName);
                txtPathToDb.Text = dlgPathToDb.FileName;
            }

            
                
            

        }
    }
}
