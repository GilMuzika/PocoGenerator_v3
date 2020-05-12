using Microsoft.SqlServer.Management.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace PocoGenerator
{
    public partial class MainForm : Form
    {
        private const int TIMEOUT = 1000;
        private readonly string[] SQL_SERVERS_INSTANCE_NAME_TO_TRY = new string[] { @"", @"\SQLEXPRESS" };   // @"\SQLEXPRESS";
        //private readonly string[] SQL_SERVERS_INSTANCE_NAME_TO_TRY = new string[] { @"\SQLEXPRESS", @"" };   // @"\SQLEXPRESS";
        private string _globalInstanceName;
        private const int MESSAGE_WHILE_GENERATING_ROW_NUM = 5;

        private int _threadsMissionsCount = 0;
        private int _WhileGeneratingMesageCanBeClosed = 0;
        private string _savingPath;
        private string lblWaitMessageText; //string that appears on the Label "lblWaitMessage" 

        private Queue<string> _fileNamesForMessageWhileCreatingQueue = new Queue<string>();
        private List<string> _helpingList = new List<string>();
        private Stopwatch _stopwatch = new Stopwatch();

        //Database acsess class
        private IDAO _currentDAO;

        private ToolTip _toolTip = new ToolTip();
        private ToolTip forltbTables = new ToolTip();

        private pnlMSSQLUIModulePanel _mssqlUserlInterfacePanel = new pnlMSSQLUIModulePanel();
        private pnlSqliteUIModulePanel _sqliteUserInterfacePanel = new pnlSqliteUIModulePanel();






        public MainForm()
        {
            FlexibleMessageBox.MAX_WIDTH_FACTOR = Screen.PrimaryScreen.WorkingArea.Width;
            FlexibleMessageBox.MAX_HEIGHT_FACTOR = Screen.PrimaryScreen.WorkingArea.Height;

            InitializeComponent();
            Initialize();
            
        }
        private void Initialize()
        {
            btnChooseDataBase.Visible = false;
            btnSqlitePickDatabase.Visible = false;
            btnSqliteGo.Visible = false;

            cmbSelectDBEngine.Width = lblMessageWhileCreating.Width - lblMessageWhileCreating.Width / 5;
            cmbSelectDBEngine.Location = new Point(lblMessageWhileCreating.Location.X + lblMessageWhileCreating.Width / 2 - cmbSelectDBEngine.Width /2, lblMessageWhileCreating.Location.Y + lblMessageWhileCreating.Height / 5);
            cmbSelectDBEngine.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSelectDBEngine.Items.Add(new ComboItem<string>("MSSQL"));
            cmbSelectDBEngine.Items.Add(new ComboItem<string>("SQLite"));
            lblSelectDBEngine.Text = "Please select a Database engine";
            lblSelectDBEngine.Location = new Point(cmbSelectDBEngine.Location.X, cmbSelectDBEngine.Location.Y - lblSelectDBEngine.Height - 2);

            lblTimeElapsedForPocosGeneration.Text = string.Empty;
            lblTimeElapsedForPocosGeneration.AutoSize = true;

            btnSeletAllInListBox.Click += (object sender, EventArgs e) =>
            {
                for (int i = 0; i < ltbTables.Items.Count; i++) ltbTables.SetSelected(i, true);
            };

            lblMessageWhileCreating.Text = string.Empty;
            lblMessageWhileCreating.Width = ltbTables.Width / 4 * 3;
            lblMessageWhileCreating.Height = ltbTables.Height / 4 * 3;
            lblMessageWhileCreating.Location = new Point(ltbTables.Location.X + (ltbTables.Width / 2 - lblMessageWhileCreating.Width / 2), ltbTables.Location.Y + (ltbTables.Height / 2 - lblMessageWhileCreating.Height / 2));
            lblMessageWhileCreating.drawBorder(3, Color.DarkBlue);
            lblMessageWhileCreating.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
            lblMessageWhileCreating.Padding = new Padding(5);

            btnGeneratePocos.Enabled = false;
            btnChooseDataBase.Enabled = false;
            
            
            
            
            
            txtInterfaceName.Enabled = false; 
            chkInheritFromInterface.CheckedChanged += (object sender, EventArgs e) =>
            {
                if (!chkInheritFromInterface.Checked)
                        txtInterfaceName.Enabled = false;                

                else txtInterfaceName.Enabled = true;                
            };


            ////////////////////////////////////////////
            //Here was the code that now in InitializeMSSQl()
            ////////////////////////////////////////////
            SelectDBEngine();


            ltbTables.SelectionMode = SelectionMode.MultiExtended;

            _toolTip.AutoPopDelay = 5000;
            _toolTip.InitialDelay = 1;
            _toolTip.ReshowDelay = 500;
            _toolTip.ShowAlways = true;
            _toolTip.IsBalloon = true;
            _toolTip.SetToolTip(txtNameSpace, "Namespace of the project where the pocos will be used. \nBy default is your chosen database name");
            _toolTip.SetToolTip(chkNewSavingPath, "If not checked, browsing dialog won't appear and saving path from previous time will be used");
            _toolTip.SetToolTip(chkOverrideOverload, "Overload the operators \"==\" and \"!=\" and override the methods \"Equals()\" and \"GetHashCode()\" to comparsion by the first table value (the poco first property), usually ID");
            _toolTip.SetToolTip(chkAddEnumeration, "Add enumeration \"['pocoTypeName']PropertyNumber\" that enumerates all the property names of the current poco clas, in accending order from zero.");
            _toolTip.SetToolTip(chkInheritFromInterface, "If you want to inherit from more than one empty interface, just write the names of all the empty interfaces you want in this textbox, separated with comma");

            if (File.Exists($"{Directory.GetCurrentDirectory()}\\_savingPath.txt"))
                _savingPath = File.ReadAllText($"{Directory.GetCurrentDirectory()}\\_savingPath.txt");

            if (String.IsNullOrEmpty(_savingPath)) chkNewSavingPath.Checked = true;

            ltbTables.Click += (object sender, EventArgs e) => { forltbTables.RemoveAll(); };
        }
        private void SelectDBEngine()
        {
            cmbSelectDBEngine.SelectedIndexChanged += (object sender, EventArgs e) => 
                {
                    bool dbEngineChoosen = false;
                    lblMessageWhileCreating.Click += (object sender2, EventArgs e2) =>
                    {
                        if (dbEngineChoosen)
                        {
                            if (_WhileGeneratingMesageCanBeClosed >= ltbTables.SelectedItems.Count)
                                (sender2 as Label).Visible = false;
                            _WhileGeneratingMesageCanBeClosed = 0;
                        }
                    };

                    switch (((sender as ComboBox).SelectedItem as ComboItem<string>).Item)
                    {                        
                        case "MSSQL":
                            dbEngineChoosen = true;

                            _currentDAO = new DAOMSSQL();
                            this.Controls.Add(_mssqlUserlInterfacePanel);                           
                            lblWaitMessageText = "Please wait while fetching databases";
                            _mssqlUserlInterfacePanel.cmbDataBases.SelectedIndexChanged += (object sender2, EventArgs e2) => { btnChooseDataBase.Enabled = true; };
                            btnChooseDataBase.Visible = true;

                            InitializeMSSQL();
                            break;
                        case "SQLite":
                            dbEngineChoosen = true;

                            _currentDAO = new DAOSqlite();
                            this.Controls.Add(_sqliteUserInterfacePanel);
                            btnSqlitePickDatabase.Visible = true;
                            btnSqliteGo.Visible = true;
                            break;
                    }

                    if(dbEngineChoosen)
                    {                        
                        lblMessageWhileCreating.Visible = false;
                        lblSelectDBEngine.Visible = false;
                        cmbSelectDBEngine.Visible = false;
                    }
                };


            
        }
        private void InitializeMSSQL()
        {
            List<string> alldataBases = new List<string>();
            //if MSSQL is installed, all the purpose of the code in the fillowing "if" section is to fill the "alldataBases" list with the names of all the MSSQL databases
            if (IsMSSQLServerInstalled())
            {
                Thread threadForAddingMessagesToltbTables = new Thread(new ParameterizedThreadStart(threadForAddingMessagesToltbTablesWorkingMethod));
                Thread thread2 = null;
                Thread thread3 = null;

                Thread thread = new Thread(() =>
                {
                    string currentThreadName = Thread.CurrentThread.Name;
                    try
                    {
                        if (_currentDAO.GetAllDataBases().Count > 0)
                        {
                            thread2.Abort();
                            thread3.Abort();
                            _globalInstanceName = string.Empty;
                            alldataBases = _currentDAO.GetAllDataBases();                            
                        }
                        Action act = () => 
                        {
                            threadForAddingMessagesToltbTables.Start(currentThreadName);
                        };
                        SafeInvoke(act, ltbTables);

                    }
                    catch (ThreadAbortException ex)
                    {
                        Action act = () => 
                        {
                            ltbTables.Items.Add($"Message from {currentThreadName}:");
                            ltbTables.Items.Add("ATTEMPT TO DERIVE");
                            ltbTables.Items.Add("SQL SERVER AND INSTANCE NAMES");
                            ltbTables.Items.Add("FROM SqlDataSourceEnumerator TOOK TOO MUCH TIME");
                            ltbTables.Items.Add(ex.Message);
                            ltbTables.Items.Add("------------------------------");
                        };
                        SafeInvoke(act, ltbTables);

                    }
                    catch (Exception ex)
                    {
                        Action act = () => 
                        {
                            ltbTables.Items.Add($"Message from {currentThreadName}:");
                            ltbTables.Items.Add("SOMETHING WENT WRONG");
                        };
                        SafeInvoke(act, ltbTables);
                        FlexibleMessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
                    }
                });
                thread.Name = "thread1";
                thread2 = new Thread(new ParameterizedThreadStart((object instanceName) =>
                {
                    try
                    {
                        Monitor.Enter(this);
                        List<string> innerList = ThreadInnerOperation(instanceName, Thread.CurrentThread.Name);
                        if (innerList.Count > 0)
                        {
                            thread.Abort();
                            thread3.Abort();
                            _globalInstanceName = (string)instanceName;
                            alldataBases = innerList;                            
                        }

                    }
                    finally 
                    {
                        Monitor.Pulse(this);
                        Monitor.Exit(this);
                    }
                    
                }));
                thread2.Name = "thread2";
                thread3 = new Thread(new ParameterizedThreadStart((object instanceName) =>
                {
                    try
                    {
                        Monitor.Enter(this);                        
                        Monitor.Wait(this);
                        List<string> innerList = ThreadInnerOperation(instanceName, Thread.CurrentThread.Name);
                        if (innerList.Count > 0)
                        {
                            thread.Abort();
                            thread2.Abort();
                            _globalInstanceName = (string)instanceName;
                            alldataBases = innerList;
                        }
                    }
                    finally 
                    {
                        Monitor.Exit(this);
                    }                    
                }));
                thread3.IsBackground = true;
                thread3.Name = "thread3";
                thread3.Start(SQL_SERVERS_INSTANCE_NAME_TO_TRY[1]);

                int countMilliseconds = 0;
                System.Windows.Forms.Timer locTimer1 = new System.Windows.Forms.Timer();
                locTimer1.Enabled = false;
                locTimer1.Interval = 10;
                locTimer1.Tick += (object sender, EventArgs e) =>
                {
                    if (countMilliseconds > TIMEOUT)
                    {                        
                        if (thread2.ThreadState == System.Threading.ThreadState.Running)
                        {
                            thread2.Abort();
                            _mssqlUserlInterfacePanel.lblWaitMessage.ForeColor = Color.DarkBlue;
                            countMilliseconds = 0;
                            //thread3.Start("");
                        }
                        if (thread2.ThreadState == System.Threading.ThreadState.Unstarted)
                        {
                            thread.Abort();
                            _mssqlUserlInterfacePanel.lblWaitMessage.ForeColor = Color.DarkRed;
                            countMilliseconds = 0;                            
                            thread2.Start(SQL_SERVERS_INSTANCE_NAME_TO_TRY[0]);
                        }                        

                    }
                    _mssqlUserlInterfacePanel.lblWaitMessage.Text = $"        {lblWaitMessageText}   -={countMilliseconds}=- ";
                    countMilliseconds++;
                    //the following "if" section is the purpose of allthe previously executed threaded code, that had filled the "alldataBases" list with the names of all MSSQL databases names.
                    //The next section is copying the list content to the "cmbDataBases.Items" collection if (actually when - because of the "locTimer1" timer) the condition is true
                    if (!thread.IsAlive && !thread2.IsAlive && !thread3.IsAlive)
                    {
                        locTimer1.Stop();
                        _mssqlUserlInterfacePanel.cmbDataBases.Items.AddRange(alldataBases.ToArray());
                        _mssqlUserlInterfacePanel.cmbDataBases.Text = $"The fetching took {countMilliseconds * 10} ms, chose database from here";
                        _mssqlUserlInterfacePanel.lblWaitMessage.Visible = false;
                        _mssqlUserlInterfacePanel.cmbDataBases.Visible = true;
                    }

                };

                locTimer1.Start();
                thread.Start();
            }
            else
            {
                ///if MSSQL isn't installed ("else"), the application display a message and then close itself
                System.Windows.Forms.Timer locTimer = new System.Windows.Forms.Timer();
                locTimer.Enabled = false;
                locTimer.Interval = 2000;
                locTimer.Tick += (object sender, EventArgs e) =>
                {
                    locTimer.Stop();
                    DialogResult res = MessageBox.Show("Sorry, but this application works with MS SQL databases, so, when MS SQL Server isn't installes, it's pretty useless. Please make an effort and install MS SQL Server, then start the application again.", "MS SQl Server ISN'T INSTALLED", MessageBoxButtons.OK);
                    //this line is closing the application
                    if (res == DialogResult.OK) System.Environment.Exit(0);
                };
                locTimer.Start();
            }
        }


        private void threadForAddingMessagesToltbTablesWorkingMethod(object currentThreadName)
        {
            Action act = () => 
            {
                ltbTables.Items.Add($"Message from {(string)currentThreadName}:");
                ltbTables.Items.Add("ATTEMPT TO DERIVE");
                ltbTables.Items.Add("SQL SERVER AND INSTANCE NAMES");
                ltbTables.Items.Add("FROM SqlDataSourceEnumerator");
                ltbTables.Items.Add($"SUCCEED IN {TIMEOUT * 10} ms OR LESS");
                ltbTables.Items.Add("CONNECTION WAS ESTABLISHED");
                ltbTables.Items.Add("select DB from the dropdown list above");
                ltbTables.Items.Add("------------------------------");
            };
            SafeInvoke(act, ltbTables);
        }

       /// <summary>
       /// Workig methos for the 2nd and 3thd attempting establish connection threads 
       /// </summary>
       /// <param name="instanceName">Name of current Sql Server instance</param>
       /// <param name="currentThreadName">Name of the thread the function run it</param>
       /// <returns></returns>        
        private List<string> ThreadInnerOperation(object instanceName, string currentThreadName)
        {
            List<string> alldataBases = new List<string>();
            try
            {
                alldataBases = _currentDAO.GetAllDataBases2(instanceName.ToString());

                if (ltbTables.InvokeRequired && String.IsNullOrEmpty((string)instanceName)) instanceName = "EMPTY_STRING";

                Action act = () => 
                {                    
                    ltbTables.Items.Add($"Message from {currentThreadName}:");
                    ltbTables.Items.Add("ATTEMPT TO CONNECT TO SQL SERVER");
                    ltbTables.Items.Add($"WITH A SERVER NAME \"{Environment.MachineName}\"");
                    ltbTables.Items.Add($"AND AN INSTANCE NAME \"{instanceName}\"");
                    ltbTables.Items.Add($"SUCCEED IN {TIMEOUT * 10} ms OR LESS");
                    ltbTables.Items.Add("CONNECTION WAS ESTABLISHED");
                    ltbTables.Items.Add("please select DB from the dropdown list above");
                    ltbTables.Items.Add("------------------------------");
                };
                SafeInvoke(act, ltbTables);


            }
            catch (ConnectionFailureException ex)
            {
                string excpMsg = ex.Message;
                int excpIndexOf = excpMsg.IndexOf("server") + "server".Length;
                
                Action act = () => 
                {
                    ltbTables.Items.Add($"Message from {currentThreadName}:");
                    ltbTables.Items.Add($"{excpMsg.Substring(0, excpMsg.Length - (excpMsg.Length - excpIndexOf))}");//ltbTables.Items.Add($"{ex.Message}");
                    ltbTables.Items.Add(excpMsg.Substring(excpIndexOf, excpMsg.Length - excpIndexOf));
                    ltbTables.Items.Add("------------------------------");
                };
                SafeInvoke(act, ltbTables);
            }
            catch (Exception ex)
            {

                Action act = () => 
                {
                    ltbTables.Items.Add($"Message from {currentThreadName}:");
                    ltbTables.Items.Add("SOMETHING WENT WRONG");
                };
                SafeInvoke(act, ltbTables);
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
            }
            return alldataBases;
        }



        private Dictionary<String, String> SetPocoDefaultDataValues()
        {
            Dictionary<String, String> typeValueCorrelation = new Dictionary<String, string>();
            typeValueCorrelation.Add(typeof(Int64).Name, "-9999;");
            typeValueCorrelation.Add(typeof(Int32).Name, "-9999;");
            typeValueCorrelation.Add(typeof(Int16).Name, "-999;");
            typeValueCorrelation.Add(typeof(Decimal).Name, "-9999m;");
            typeValueCorrelation.Add(typeof(float).Name, "-9999f;");
            typeValueCorrelation.Add(typeof(Double).Name, "-9999d;");
            typeValueCorrelation.Add(typeof(Boolean).Name, "false;");
            typeValueCorrelation.Add(typeof(String).Name, "\"-=DEFAULT_STRING=-\";");
            typeValueCorrelation.Add(typeof(DateTime).Name, "DateTime.MinValue;");
            typeValueCorrelation.Add(typeof(Byte[]).Name, "new Byte[] { 0x20 };");
            typeValueCorrelation.Add(typeof(Byte).Name, new Byte().ToString()+";");
            typeValueCorrelation.Add(typeof(Guid).Name, new Guid().ToString()+";");
            typeValueCorrelation.Add(typeof(Object).Name, new Object().ToString()+";");
            typeValueCorrelation.Add(typeof(DateTimeOffset).Name, new DateTimeOffset().ToString()+";");
            return typeValueCorrelation;
        }
        private bool IsMSSQLServerInstalled()
        {
            bool toReturn = false;
            Microsoft.Win32.RegistryKey RK = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\MICROSOFT\Microsoft SQL Server");
            if (RK != null) toReturn = true;

            return toReturn;
        }
        /// <summary>
        /// It's called for thread pool thread from a loop that runs through a collection of names of chosen databases. Because of the need to match the signature of ThreadPool.QueueUserWorkItem delegate, it takes one object parameter, that actually a collection that contains all the data needed to complete its mission.
        /// </summary>
        /// <param name="callBackParameters">"List<object> callBackParameters" => its members also objects that needs casting</param>
        private void GeneratePocosThreadPoolCallBack(object callBackParameters)
        {
            /*
                List<object> callBackParameters = new List<object>();
              0  callBackParametersList.Add(columnNames); //Dictionary<string, string>
              1  callBackParameters.Add(namespaceName); //string
              2  callBackParameters.Add(inheritInterface); //string
              3  callBackParameters.Add(overrideOverload); //string
              4  callBackParameters.Add(pocosFileNames); //List<string>
              5  callBackParametersList.Add(tableName); //string
             */            
            string pocoClassName = (string)((List<object>)callBackParameters)[4];//tableName [ealier was: "pocoClassName = tableName"];
            if (chkSingularizeName.Checked)
            {
                if (pocoClassName.Last() == 's' && !pocoClassName.Contains("ies")) { pocoClassName = pocoClassName.ChopCharsfromTheEnd(1); }
                if (pocoClassName.Last() == 's' && pocoClassName.Contains("ies")) { pocoClassName = pocoClassName.ChopCharsfromTheEnd(3); pocoClassName += "y"; }
            }

            ////////////////////////////////
            Dictionary<string, string> columnNames = _currentDAO.GetColumnNamesInATable((string)((List<object>)callBackParameters)[4]);            
            string poco = "using System;\n\n";
            poco += "namespace " + (string)((List<object>)callBackParameters)[0] + Environment.NewLine + "{" + Environment.NewLine + Environment.NewLine; //namespaceName

            int count = 0;
            string comma = ",";
            //enumeration "property number" code
            if (chkAddEnumeration.Checked)
            {
                poco += $"  public enum {pocoClassName}PropertyNumber\n" + "  {\n";
                foreach (KeyValuePair<string, string> s in columnNames)
                {
                    if (count == columnNames.Count - 1) comma = string.Empty;
                    poco += "           " + s.Key + " = " + count + comma + "\n";
                    count++;
                }
                poco += "  }\n\n";
            }
            //enumeration "property number" code

            poco += $"  public class {pocoClassName}{(string)((List<object>)callBackParameters)[1]}\n" + "   {\n"; //inheritInterface
            foreach (KeyValuePair<string, string> s in columnNames)
            {
                poco += "       public " + s.Value + " " + s.Key + " { get; set; }\n";
            }


            if (chkInsertConstructor.Checked)
            {
                comma = ",";
                count = 0;

                poco += "\n\n";
                poco += $"       public {pocoClassName}(";
                foreach (KeyValuePair<string, string> s in columnNames)
                {
                    if (count > 0)
                    {
                        if (count == columnNames.Count - 1) comma = string.Empty;
                        poco += " " + s.Value + " " + s.Key.ToUpper().FirstLetterToLower() + comma;
                    }// + ",";
                    count++;
                }
                count = 0;
                poco += ")\n       {\n";
                foreach (KeyValuePair<string, string> s in columnNames)
                {
                    if (count > 0)
                        poco += "           " + s.Key + " = " + s.Key.ToUpper().FirstLetterToLower() + ";\n";
                    count++;
                }
                poco += "       }\n";
                poco += "       public " + pocoClassName + "()\n       {\n";
                count = 0;
                Dictionary<String, string> dict = SetPocoDefaultDataValues();
                foreach (KeyValuePair<string, string> s in columnNames)
                {
                    if (count > 0)
                        poco += "           " + s.Key + " = " + SetPocoDefaultDataValues()[s.Value] + "\n";
                    count++;
                }
                poco += "       }\n\n";
            }

            if (chkOverrideOverload.Checked)
            {
                poco += ((string)((List<object>)callBackParameters)[2]).Replace("TYPE_NAME", pocoClassName).Replace("FIRST_COLUMN_NAME", columnNames.Keys.First()); //overrideOverload
            }

            if (chkAddToString.Checked)
            {
                poco += "        public override string ToString()\n        {\n";
                poco += "            string str = string.Empty;\n";
                poco += "            foreach(var s in this.GetType().GetProperties())\n";
                poco += "               str += $\"{ s.Name}: { s.GetValue(this)}\\n\";\n\n";
                poco += "            return str;\n        }\n";
            }

            poco += "\n\n       public void Dispose() { }\n\n";

            poco += "   }\n" + "}\n";
            
            try
            {
                File.WriteAllText($"{_savingPath}\\{pocoClassName}.cs", poco);                
            }
            catch
            {
                SavedPathDontMatchingTheRealPathExceptionResponce();
            }
            //double casting. List of string inside a list of objects.
            //firstly cast the object "callBackParameters" back to List<object> and address it's 4'th member,
            //which is need to be casted to List<strings>.
            //finally perforn adding (.Add()) to the List<string>
            ((List<string>)((List<object>)callBackParameters)[3]).Add($"{pocoClassName}.cs"); //pocosFileNames                        
            Interlocked.Increment(ref _threadsMissionsCount);

            this._fileNamesForMessageWhileCreatingQueue.Enqueue($"{pocoClassName}.cs\n\n");
            if (this._fileNamesForMessageWhileCreatingQueue.Count == MESSAGE_WHILE_GENERATING_ROW_NUM) 
                this._fileNamesForMessageWhileCreatingQueue.Dequeue();
            /*
            if (lblMessageWhileCreating.InvokeRequired)
            {
                lblMessageWhileCreating.Invoke((Action)delegate 
                {
                    lblMessageWhileCreating.Text = $"{_threadsMissionsCount} of {ltbTables.SelectedItems.Count} files were created\n\n";
                    //_helpingList.Clear();
                    _helpingList = new List<string>();
                    _helpingList.AddRange(this._fileNamesForMessageWhileCreatingQueue);                    
                    for(int i = 0; i < _helpingList.Count; i++) lblMessageWhileCreating.Text += _helpingList[i]; 
                });
            }
            else
            {
                lblMessageWhileCreating.Text = $"{_threadsMissionsCount} of {ltbTables.SelectedItems.Count} files were created\n\n";
                _helpingList.Clear();
                _helpingList.AddRange(this._fileNamesForMessageWhileCreatingQueue);
                for (int i = 0; i < _helpingList.Count; i++) lblMessageWhileCreating.Text += _helpingList[i];
            }
            */
            Action act = () => 
            {
                lblMessageWhileCreating.Text = $"{_threadsMissionsCount} of {ltbTables.SelectedItems.Count} files were created\n\n";
                _helpingList.Clear();
                _helpingList.AddRange(this._fileNamesForMessageWhileCreatingQueue);
                for (int i = 0; i < _helpingList.Count; i++) lblMessageWhileCreating.Text += _helpingList[i];
            };
            SafeInvoke(act, lblMessageWhileCreating);
        }

        private bool GeneratePocos(string namespaceName)
        {
            //ThreadPool.SetMaxThreads(ltbTables.SelectedItems.Count, ltbTables.SelectedItems.Count);

            _stopwatch.Start();
            if (!Directory.Exists(_savingPath))
            {
                chkNewSavingPath.Checked = true;
                chkNewSavingPath.Enabled = false;
            }

            List<string> pocosFileNames = new List<string>();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (ltbTables.SelectedItems.Count > 0 && chkNewSavingPath.Checked)
            {
                dialog.SelectedPath = _savingPath;
                dialog.ShowDialog();
                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\_savingPath.txt", dialog.SelectedPath);
                _savingPath = dialog.SelectedPath;
            }

            if (string.IsNullOrEmpty(namespaceName) || string.IsNullOrWhiteSpace(namespaceName) || namespaceName.Equals("namespace of your project")) namespaceName = this.GetType().Namespace;//namespace of your project
            string inheritInterface = " : IDisposable";
            if (chkInheritFromInterface.Checked)
            {
                //inheritInterface = " : ";
                var interfacesNames = Regex.Replace(txtInterfaceName.Text, @"\s+", "").Split(',');
                string ending = ", ";
                inheritInterface += ending;
                for (int i = 0; i < interfacesNames.Length; i++)
                {
                    if (i == interfacesNames.Length - 1) ending = string.Empty;
                    inheritInterface += interfacesNames[i] + ending;

                    string interfaceFile = "using System;\n\n";
                    interfaceFile += "namespace " + namespaceName + Environment.NewLine + "{" + Environment.NewLine;
                    interfaceFile += $"  interface {interfacesNames[i]}\n" + "   {\n";
                    interfaceFile += "   }\n" + "}\n";
                    try
                    {
                        File.WriteAllText($"{_savingPath}\\{interfacesNames[i]}.cs", interfaceFile);
                    }
                    catch
                    {
                        SavedPathDontMatchingTheRealPathExceptionResponce();
                    }
                }
            }

            // declaring variable for overloaded operators and overriden methods
            string overrideOverload = string.Empty;
            if(chkOverrideOverload.Checked)
            {
                string pathToTheFile = $"{Directory.GetCurrentDirectory()}\\_overloadingOperatorsOverridingMethods.txt";
                if (File.Exists(pathToTheFile))
                    overrideOverload = File.ReadAllText(pathToTheFile);

                else
                {
                    chkOverrideOverload.Checked = false;
                    MessageBox.Show("Sorry, but the future can't work because the file \"overloadingOperatorsOverridingMethods.txt\" that contains the methods is missing ): \nThe pocos will be generated without this future");
                }
            }

            //code section inside this "foreach" actually creates a poco, each one in one stroke of the "foreach"
            //"ltbTables.SelectedItems" is a collection of objects (here strings) whcich consists of names of tables selected by user.
            //It's actually a part of the "ltbTables" ListBox component.
            foreach (string tableName in ltbTables.SelectedItems)
            {               
                List<object> callBackParametersList = new List<object>();                
                callBackParametersList.Add(namespaceName); //string
                callBackParametersList.Add(inheritInterface); //string 
                callBackParametersList.Add(overrideOverload); //string
                callBackParametersList.Add(pocosFileNames); //List<string>
                callBackParametersList.Add(tableName); //string
                
                ThreadPool.QueueUserWorkItem(new WaitCallback(GeneratePocosThreadPoolCallBack), callBackParametersList);               

                //CreatePocoThreadPoolCallBack(callBackParametersList);
            }
            chkNewSavingPath.Enabled = true;
            _stopwatch.Stop();
            return CheckIfDone(pocosFileNames);
        }
        private bool CheckIfDone(List<string> fileNames)
        {
            bool toReturn = true;
            foreach (var s in fileNames)            
                if (!File.Exists($"{_savingPath}\\{s}")) toReturn = false;                            

            return toReturn;
        }
        private void SavedPathDontMatchingTheRealPathExceptionResponce()
        {
            _savingPath = null;
            Action act = () => 
            {
                chkNewSavingPath.Checked = true;
            };
            SafeInvoke(act, chkNewSavingPath);

            if (File.Exists($"{Directory.GetCurrentDirectory()}\\_savingPath.txt"))
                File.Delete($"{Directory.GetCurrentDirectory()}\\_savingPath.txt");
        }

        private void btnGeneratePocos_Click(object sender, EventArgs e)
        {
            ltbTables.Enabled = false;
            lblMessageWhileCreating.Visible = true;

            int selectedItemsCount = ltbTables.SelectedItems.Count;          

            System.Windows.Forms.Timer locTimer = new System.Windows.Forms.Timer();
            locTimer.Enabled = false;
            locTimer.Interval = 10;
            bool ifSucseeded = GeneratePocos(txtNameSpace.Text);
            

            locTimer.Tick += (object sender2, EventArgs e2) => 
                {
                    if(_threadsMissionsCount == selectedItemsCount)
                    {
                        locTimer.Stop();
                        lblTimeElapsedForPocosGeneration.Text = _stopwatch.Elapsed.ToString();
                        _WhileGeneratingMesageCanBeClosed = _threadsMissionsCount;
                        _threadsMissionsCount = 0;
                        _fileNamesForMessageWhileCreatingQueue.Clear();
                        ltbTables.Enabled = true;                        
                        if (ifSucseeded)
                        {
                            Process.Start("explorer.exe", _savingPath);
                            MessageBox.Show("Your pocos were generated sucsessfully");
                        }
                        else MessageBox.Show("Sorry, but something went wrong :( boooo");

                        
                    }                    
                };
            locTimer.Start();            
        }

        private void btnMSSQLChooseDataBase_Click(object sender, EventArgs e)
        {
            _currentDAO.SetConnectionString(_mssqlUserlInterfacePanel.cmbDataBases.SelectedItem as string, _globalInstanceName);
            ltbTables.Items.Clear();
            ltbTables.Items.AddRange(_currentDAO.GetAllTableNames().ToArray());
            btnGeneratePocos.Enabled = true;
            txtNameSpace.Text = _mssqlUserlInterfacePanel.cmbDataBases.SelectedItem as string;

            forltbTables.AutoPopDelay = 5000;
            forltbTables.InitialDelay = 1;
            forltbTables.ReshowDelay = 500;
            forltbTables.ShowAlways = true;
            forltbTables.IsBalloon = true;
            forltbTables.SetToolTip(ltbTables, "Use \"Ctrl\"");

            _WhileGeneratingMesageCanBeClosed = ltbTables.SelectedItems.Count;
        }

        private void btnSqlitePickDatabase_Click(object sender, EventArgs e)
        {
            
            _sqliteUserInterfacePanel.SetSqiteDBfilepath();            

        }

        private void btnSqliteGo_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_sqliteUserInterfacePanel.txtPathToDb.Text) || String.IsNullOrWhiteSpace(_sqliteUserInterfacePanel.txtPathToDb.Text)) return;

            string PathToTheDbFile = _sqliteUserInterfacePanel.txtPathToDb.Text;
            if (File.Exists(PathToTheDbFile))
            {
                _currentDAO.SetConnectionString(PathToTheDbFile, _globalInstanceName);

                ltbTables.Items.Clear();
                ltbTables.Items.AddRange(_currentDAO.GetAllTableNames().ToArray());
                btnGeneratePocos.Enabled = true;                
                string sqliteDBSafeFileName = _sqliteUserInterfacePanel.txtPathToDb.Text.Substring(_sqliteUserInterfacePanel.txtPathToDb.Text.LastIndexOf("\\")+1);
                string willBeNamespaceName = sqliteDBSafeFileName.Replace('.', '_');
                if (Int32.TryParse(willBeNamespaceName.First().ToString(), out int firstCharAsNumber))
                    willBeNamespaceName = "namespace_" + willBeNamespaceName;

                txtNameSpace.Text = willBeNamespaceName;
            }
            else
            {
                MessageBox.Show($"The file or path \n{PathToTheDbFile} \ndoesn't exists");
            }

            forltbTables.AutoPopDelay = 5000;
            forltbTables.InitialDelay = 1;
            forltbTables.ReshowDelay = 500;
            forltbTables.ShowAlways = true;
            forltbTables.IsBalloon = true;
            forltbTables.SetToolTip(ltbTables, "Use \"Ctrl\"");

            _WhileGeneratingMesageCanBeClosed = ltbTables.SelectedItems.Count;
        }


        private void SafeInvoke(Action work, Control control)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(work);
            }
            else work();
        }




    }
}
