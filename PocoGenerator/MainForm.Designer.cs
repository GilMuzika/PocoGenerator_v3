namespace PocoGenerator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ltbTables = new System.Windows.Forms.ListBox();
            this.btnGeneratePocos = new System.Windows.Forms.Button();
            this.btnChooseDataBase = new System.Windows.Forms.Button();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.chkNewSavingPath = new System.Windows.Forms.CheckBox();
            this.chkInheritFromInterface = new System.Windows.Forms.CheckBox();
            this.txtInterfaceName = new System.Windows.Forms.TextBox();
            this.chkOverrideOverload = new System.Windows.Forms.CheckBox();
            this.chkInsertConstructor = new System.Windows.Forms.CheckBox();
            this.chkSingularizeName = new System.Windows.Forms.CheckBox();
            this.chkAddEnumeration = new System.Windows.Forms.CheckBox();
            this.chkAddToString = new System.Windows.Forms.CheckBox();
            this.lblMessageWhileCreating = new System.Windows.Forms.Label();
            this.btnSeletAllInListBox = new System.Windows.Forms.Button();
            this.lblTimeElapsedForPocosGeneration = new System.Windows.Forms.Label();
            this.cmbSelectDBEngine = new System.Windows.Forms.ComboBox();
            this.lblSelectDBEngine = new System.Windows.Forms.Label();
            this.btnSqlitePickDatabase = new System.Windows.Forms.Button();
            this.btnSqliteGo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ltbTables
            // 
            this.ltbTables.FormattingEnabled = true;
            this.ltbTables.Location = new System.Drawing.Point(2, 96);
            this.ltbTables.Name = "ltbTables";
            this.ltbTables.Size = new System.Drawing.Size(296, 251);
            this.ltbTables.TabIndex = 1;
            // 
            // btnGeneratePocos
            // 
            this.btnGeneratePocos.Location = new System.Drawing.Point(123, 350);
            this.btnGeneratePocos.Name = "btnGeneratePocos";
            this.btnGeneratePocos.Size = new System.Drawing.Size(75, 23);
            this.btnGeneratePocos.TabIndex = 2;
            this.btnGeneratePocos.Text = "Generate";
            this.btnGeneratePocos.UseVisualStyleBackColor = true;
            this.btnGeneratePocos.Click += new System.EventHandler(this.btnGeneratePocos_Click);
            // 
            // btnChooseDataBase
            // 
            this.btnChooseDataBase.Location = new System.Drawing.Point(2, 41);
            this.btnChooseDataBase.Name = "btnChooseDataBase";
            this.btnChooseDataBase.Size = new System.Drawing.Size(109, 23);
            this.btnChooseDataBase.TabIndex = 4;
            this.btnChooseDataBase.Text = "Choose database";
            this.btnChooseDataBase.UseVisualStyleBackColor = true;
            this.btnChooseDataBase.Click += new System.EventHandler(this.btnMSSQLChooseDataBase_Click);
            // 
            // txtNameSpace
            // 
            this.txtNameSpace.Location = new System.Drawing.Point(2, 70);
            this.txtNameSpace.Name = "txtNameSpace";
            this.txtNameSpace.Size = new System.Drawing.Size(296, 20);
            this.txtNameSpace.TabIndex = 5;
            this.txtNameSpace.Text = "namespace of your project";
            // 
            // chkNewSavingPath
            // 
            this.chkNewSavingPath.AutoSize = true;
            this.chkNewSavingPath.Location = new System.Drawing.Point(2, 350);
            this.chkNewSavingPath.Name = "chkNewSavingPath";
            this.chkNewSavingPath.Size = new System.Drawing.Size(106, 17);
            this.chkNewSavingPath.TabIndex = 6;
            this.chkNewSavingPath.Text = "New saving path";
            this.chkNewSavingPath.UseVisualStyleBackColor = true;
            // 
            // chkInheritFromInterface
            // 
            this.chkInheritFromInterface.AutoSize = true;
            this.chkInheritFromInterface.Location = new System.Drawing.Point(2, 380);
            this.chkInheritFromInterface.Name = "chkInheritFromInterface";
            this.chkInheritFromInterface.Size = new System.Drawing.Size(125, 17);
            this.chkInheritFromInterface.TabIndex = 7;
            this.chkInheritFromInterface.Text = "Inherit from interface:";
            this.chkInheritFromInterface.UseVisualStyleBackColor = true;
            // 
            // txtInterfaceName
            // 
            this.txtInterfaceName.Location = new System.Drawing.Point(123, 377);
            this.txtInterfaceName.Name = "txtInterfaceName";
            this.txtInterfaceName.Size = new System.Drawing.Size(100, 20);
            this.txtInterfaceName.TabIndex = 8;
            this.txtInterfaceName.Text = "IPoco";
            // 
            // chkOverrideOverload
            // 
            this.chkOverrideOverload.AutoSize = true;
            this.chkOverrideOverload.Location = new System.Drawing.Point(2, 404);
            this.chkOverrideOverload.Name = "chkOverrideOverload";
            this.chkOverrideOverload.Size = new System.Drawing.Size(167, 17);
            this.chkOverrideOverload.TabIndex = 9;
            this.chkOverrideOverload.Text = "==, !=, Equals, GetHashCode ";
            this.chkOverrideOverload.UseVisualStyleBackColor = true;
            // 
            // chkInsertConstructor
            // 
            this.chkInsertConstructor.AutoSize = true;
            this.chkInsertConstructor.Location = new System.Drawing.Point(2, 428);
            this.chkInsertConstructor.Name = "chkInsertConstructor";
            this.chkInsertConstructor.Size = new System.Drawing.Size(108, 17);
            this.chkInsertConstructor.TabIndex = 10;
            this.chkInsertConstructor.Text = "Insert constructor";
            this.chkInsertConstructor.UseVisualStyleBackColor = true;
            // 
            // chkSingularizeName
            // 
            this.chkSingularizeName.AutoSize = true;
            this.chkSingularizeName.Checked = true;
            this.chkSingularizeName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSingularizeName.Location = new System.Drawing.Point(137, 428);
            this.chkSingularizeName.Name = "chkSingularizeName";
            this.chkSingularizeName.Size = new System.Drawing.Size(161, 17);
            this.chkSingularizeName.TabIndex = 11;
            this.chkSingularizeName.Text = "Singularize Poco class name";
            this.chkSingularizeName.UseVisualStyleBackColor = true;
            // 
            // chkAddEnumeration
            // 
            this.chkAddEnumeration.AutoSize = true;
            this.chkAddEnumeration.Location = new System.Drawing.Point(2, 451);
            this.chkAddEnumeration.Name = "chkAddEnumeration";
            this.chkAddEnumeration.Size = new System.Drawing.Size(106, 17);
            this.chkAddEnumeration.TabIndex = 12;
            this.chkAddEnumeration.Text = "Add enumeration";
            this.chkAddEnumeration.UseVisualStyleBackColor = true;
            // 
            // chkAddToString
            // 
            this.chkAddToString.AutoSize = true;
            this.chkAddToString.Location = new System.Drawing.Point(175, 405);
            this.chkAddToString.Name = "chkAddToString";
            this.chkAddToString.Size = new System.Drawing.Size(94, 17);
            this.chkAddToString.TabIndex = 13;
            this.chkAddToString.Text = "Add ToString()";
            this.chkAddToString.UseVisualStyleBackColor = true;
            // 
            // lblMessageWhileCreating
            // 
            this.lblMessageWhileCreating.Location = new System.Drawing.Point(41, 124);
            this.lblMessageWhileCreating.Name = "lblMessageWhileCreating";
            this.lblMessageWhileCreating.Padding = new System.Windows.Forms.Padding(5);
            this.lblMessageWhileCreating.Size = new System.Drawing.Size(208, 188);
            this.lblMessageWhileCreating.TabIndex = 14;
            this.lblMessageWhileCreating.Text = "label1";
            // 
            // btnSeletAllInListBox
            // 
            this.btnSeletAllInListBox.Location = new System.Drawing.Point(240, 350);
            this.btnSeletAllInListBox.Name = "btnSeletAllInListBox";
            this.btnSeletAllInListBox.Size = new System.Drawing.Size(59, 23);
            this.btnSeletAllInListBox.TabIndex = 15;
            this.btnSeletAllInListBox.Text = "Select All";
            this.btnSeletAllInListBox.UseVisualStyleBackColor = true;
            // 
            // lblTimeElapsedForPocosGeneration
            // 
            this.lblTimeElapsedForPocosGeneration.AutoSize = true;
            this.lblTimeElapsedForPocosGeneration.Location = new System.Drawing.Point(188, 46);
            this.lblTimeElapsedForPocosGeneration.Name = "lblTimeElapsedForPocosGeneration";
            this.lblTimeElapsedForPocosGeneration.Size = new System.Drawing.Size(35, 13);
            this.lblTimeElapsedForPocosGeneration.TabIndex = 16;
            this.lblTimeElapsedForPocosGeneration.Text = "label1";
            // 
            // cmbSelectDBEngine
            // 
            this.cmbSelectDBEngine.FormattingEnabled = true;
            this.cmbSelectDBEngine.Location = new System.Drawing.Point(66, 165);
            this.cmbSelectDBEngine.Name = "cmbSelectDBEngine";
            this.cmbSelectDBEngine.Size = new System.Drawing.Size(146, 21);
            this.cmbSelectDBEngine.TabIndex = 17;
            // 
            // lblSelectDBEngine
            // 
            this.lblSelectDBEngine.AutoSize = true;
            this.lblSelectDBEngine.Location = new System.Drawing.Point(66, 149);
            this.lblSelectDBEngine.Name = "lblSelectDBEngine";
            this.lblSelectDBEngine.Size = new System.Drawing.Size(35, 13);
            this.lblSelectDBEngine.TabIndex = 18;
            this.lblSelectDBEngine.Text = "label1";
            // 
            // btnSqlitePickDatabase
            // 
            this.btnSqlitePickDatabase.Location = new System.Drawing.Point(2, 41);
            this.btnSqlitePickDatabase.Name = "btnSqlitePickDatabase";
            this.btnSqlitePickDatabase.Size = new System.Drawing.Size(62, 23);
            this.btnSqlitePickDatabase.TabIndex = 19;
            this.btnSqlitePickDatabase.Text = "Pick DB";
            this.btnSqlitePickDatabase.UseVisualStyleBackColor = true;
            this.btnSqlitePickDatabase.Click += new System.EventHandler(this.btnSqlitePickDatabase_Click);
            // 
            // btnSqliteGo
            // 
            this.btnSqliteGo.Location = new System.Drawing.Point(66, 41);
            this.btnSqliteGo.Name = "btnSqliteGo";
            this.btnSqliteGo.Size = new System.Drawing.Size(42, 23);
            this.btnSqliteGo.TabIndex = 20;
            this.btnSqliteGo.Text = "Go";
            this.btnSqliteGo.UseVisualStyleBackColor = true;
            this.btnSqliteGo.Click += new System.EventHandler(this.btnSqliteGo_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 469);
            this.Controls.Add(this.btnSqliteGo);
            this.Controls.Add(this.btnSqlitePickDatabase);
            this.Controls.Add(this.lblSelectDBEngine);
            this.Controls.Add(this.cmbSelectDBEngine);
            this.Controls.Add(this.lblTimeElapsedForPocosGeneration);
            this.Controls.Add(this.btnSeletAllInListBox);
            this.Controls.Add(this.lblMessageWhileCreating);
            this.Controls.Add(this.chkAddToString);
            this.Controls.Add(this.chkAddEnumeration);
            this.Controls.Add(this.chkSingularizeName);
            this.Controls.Add(this.chkInsertConstructor);
            this.Controls.Add(this.chkOverrideOverload);
            this.Controls.Add(this.txtInterfaceName);
            this.Controls.Add(this.chkInheritFromInterface);
            this.Controls.Add(this.chkNewSavingPath);
            this.Controls.Add(this.txtNameSpace);
            this.Controls.Add(this.btnChooseDataBase);
            this.Controls.Add(this.btnGeneratePocos);
            this.Controls.Add(this.ltbTables);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Poco Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox ltbTables;
        private System.Windows.Forms.Button btnGeneratePocos;
        private System.Windows.Forms.Button btnChooseDataBase;
        private System.Windows.Forms.TextBox txtNameSpace;
        private System.Windows.Forms.CheckBox chkNewSavingPath;
        //private System.Windows.Forms.CheckBox chkInheritFromInterface = new System.Windows.Forms.CheckBox();
        //private System.Windows.Forms.CheckBox chkIsInterfaceEmpty = new System.Windows.Forms.CheckBox();
        //private System.Windows.Forms.TextBox txtInterfaceName = new System.Windows.Forms.TextBox();
        private System.Windows.Forms.CheckBox chkInheritFromInterface;
        private System.Windows.Forms.TextBox txtInterfaceName;
        private System.Windows.Forms.CheckBox chkOverrideOverload;
        private System.Windows.Forms.CheckBox chkInsertConstructor;
        private System.Windows.Forms.CheckBox chkSingularizeName;
        private System.Windows.Forms.CheckBox chkAddEnumeration;
        private System.Windows.Forms.CheckBox chkAddToString;
        private System.Windows.Forms.Label lblMessageWhileCreating;
        private System.Windows.Forms.Button btnSeletAllInListBox;
        private System.Windows.Forms.Label lblTimeElapsedForPocosGeneration;
        private System.Windows.Forms.ComboBox cmbSelectDBEngine;
        private System.Windows.Forms.Label lblSelectDBEngine;
        private System.Windows.Forms.Button btnSqlitePickDatabase;
        private System.Windows.Forms.Button btnSqliteGo;
    }
}

