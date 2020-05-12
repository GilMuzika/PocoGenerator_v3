using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PocoGenerator
{
    class pnlMSSQLUIModulePanel : Panel
    {
        public ComboBox cmbDataBases = new ComboBox();
        public Label lblWaitMessage = new Label(); //Label that hiding the ComboBox "cmbDataBases" until the names of the databases are retrived           

        public pnlMSSQLUIModulePanel()
        {
            Initialize();

        }
        private void Initialize()
        {
            this.BorderStyle = BorderStyle.None;
            this.Location = new System.Drawing.Point(2, 8);
            this.Size = new System.Drawing.Size(296, 21);

            cmbDataBases.Visible = false;
            cmbDataBases.FormattingEnabled = true;
            cmbDataBases.Location = new Point(0, 0);
            cmbDataBases.Name = "cmbDataBases";
            cmbDataBases.Size = this.Size;
            cmbDataBases.TabIndex = 3;
            
            lblWaitMessage.AutoSize = false;
            lblWaitMessage.Location = cmbDataBases.Location;
            lblWaitMessage.Size = cmbDataBases.Size;
            lblWaitMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblWaitMessage.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
            lblWaitMessage.drawBorder(1, Color.Black);
            lblWaitMessage.Text = "Please wait while fetching databases";

            this.Controls.Add(cmbDataBases);
            this.Controls.Add(lblWaitMessage);
        }
    }
}
