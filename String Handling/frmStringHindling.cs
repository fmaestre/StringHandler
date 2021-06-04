using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Net;
using System.Web;
using System.Net.Sockets;
using Microsoft.Win32;
using System.Net.NetworkInformation;
using System.Linq;
using System.Data.Sql;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.IO.Ports;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

//using System.Data.Sql;

//using IronPython.Hosting;
//using IronPython.Runtime;
//using Microsoft.Scripting;
//using Microsoft.Scripting.Hosting;


namespace String_Handling
{
    public partial class frmStringsHandling : Form
    {
        const string _NEWLINE_ = "\r\n";
        const string _TAB_ = "\t";
        private readonly BackgroundWorker worker;
        bool workerCompleted = true;
        string URL = "";
        const string _USER_ = "fmadmin";
        const string _PASSWORD_ = "Admin@cc3ss";
        const string _DOMAIN_ = "Angio";
        const string _DOM_USER = _DOMAIN_ + "\\" + _USER_;

        public void initThreads()
        {
            // Creating and initializing threads 
            Thread a = new Thread(ExThread.thread1);
            a.Start();
        }

        public frmStringsHandling()
        {
            synchronizationContext = SynchronizationContext.Current;
            initThreads();
            InitializeComponent();

            //comp info
            txtCompInfo.Text += Dns.GetHostName();
            txtCompInfo.Text += " ::IP:: " + (GetAddresses() as System.Collections.Generic.List<string>)[0];

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += StartHighLight;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            timeZones_();
            //Defaults
            cmbAxModServer.Text = "Dev";
                     
           // txtZones.Font = new Font(FontFamily.GenericMonospace, txtZones.Font.Size);
        }

        public void timeZones_()
        {
            //foreach (TimeZoneInfo info in TimeZoneInfo.GetSystemTimeZones())
            //{
            //    txtZones.Text += string.Format("{0}*{1}*{2}", info.Id, info.DisplayName, info.BaseUtcOffset) + _NEWLINE_;
            //}


            DateTime localTime = DateTime.Now;

            TimeZoneInfo pacificZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            TimeZoneInfo chinaZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            TimeZoneInfo EuropeCentralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
            TimeZoneInfo ukZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            TimeZoneInfo colZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            TimeZoneInfo bostonZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            string fortmat = "{0} {1} / {2}   {3} [{4}][{5} utc]";

            txtZones.Text = "";

            txtZones.Text += solveZone(pacificZone, fortmat, localTime, "TJ/VC");
            txtZones.Text += solveZone(chinaZone, fortmat, localTime, "Shanghai");
            txtZones.Text += solveZone(EuropeCentralZone, fortmat, localTime, "Germany");
            txtZones.Text += solveZone(ukZone, fortmat, localTime, "UK");
            txtZones.Text += solveZone(colZone, fortmat, localTime, "COL");
            txtZones.Text += solveZone(bostonZone, fortmat, localTime, "Boston");

        }

        private string solveZone(TimeZoneInfo _TimeZoneInfo, string fortmat, DateTime localTime, string _city)
        {
            TimeZoneInfo LocalZone = TimeZoneInfo.Local;

            DateTime dt = TimeZoneInfo.ConvertTime(localTime, LocalZone, _TimeZoneInfo);
            int offSet = int.Parse(_TimeZoneInfo.BaseUtcOffset.ToString().Substring(0, 3).Replace(":", ""));
            int UTC_offSet = int.Parse(_TimeZoneInfo.GetUtcOffset(localTime).ToString().Substring(0, 3).Replace(":", ""));
            return string.Format(fortmat,
                                           _city.PadRight(8,' '),
                                           _TimeZoneInfo.IsDaylightSavingTime(localTime) ? "DL" : "ST",
                                           dt.ToShortTimeString().PadRight(8,' '),
                                           dt.ToShortDateString(),
                                           offSet,
                                           UTC_offSet
                                           ) + _NEWLINE_;


        }

        public static IEnumerable<string> GetAddresses()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
        }
        public static IPAddress GetIPAddress(string hostName)
        {
            Ping ping = new Ping();
            var replay = ping.Send(hostName);

            if (replay.Status == IPStatus.Success)
            {
                return replay.Address;
            }
            return null;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            cmbLis.Text = "'";
            linkLabel1.Text = "";
            linkLabel1.Links.Add(0, 14, "");
            linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);

            DPsessionDate.Value = DateTime.Today;

            
            int count = 0;
            await Task.Run(() =>
            {
                for (var i = 0; ; i++)
                {
                    UpdateUI(i);
                    count = i;
                }
            });


            txtAsync.Text = @"Counter " + count;
        }
        private readonly SynchronizationContext synchronizationContext;
        private DateTime previousTime = DateTime.Now;

        public void UpdateUI(int value)
        {
            var timeNow = DateTime.Now;

            if ((DateTime.Now - previousTime).Milliseconds <= 200) return;
            
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                int x = (int)o;
                if (x % 3 == 0)
                    txtAsync.ForeColor = Color.DimGray;
                else if (x % 4 == 0)
                    txtAsync.ForeColor = Color.Gainsboro;
                else if (x % 5 == 0)
                    txtAsync.ForeColor = Color.DarkGray;
                else if (x % 2 == 0)
                    txtAsync.ForeColor = Color.Black;
                else
                    txtAsync.ForeColor = Color.DeepSkyBlue;
                //
                Random random = new Random(DateTime.Now.Millisecond);
                int stgg = (random.Next(31, 254 + 1) % 255);
                //txtResults2.Text = stgg.ToString();
                txtAsync.Text = ((char)stgg).ToString();
                

                timeZones_();
            }), value);

            previousTime = timeNow;
        }
        void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
        string getCommaSeparated(string prmSeparator, string prmEnclose)
        {
            string strCadena, strParse = "", strResult = "";
            int i, j;
            j = 0;

            if (txtEntries.Text != "")
            {
                strCadena = this.txtEntries.Text;
                for (i = 0; i <= strCadena.Length - 1; i++)
                {
                    //j = strCadena.IndexOf('\r');
                    if (strCadena.Substring(i, 1).ToString() == '\r'.ToString())
                    {
                        if (strParse.Trim() != "")
                        {
                            strResult = (strResult + prmEnclose + strParse.Trim() + prmEnclose).Trim() + prmSeparator;
                        }
                        strParse = "";
                        j = 1;
                    }
                    else
                    {
                        if (strCadena.Substring(i, 1).ToString() != '\n'.ToString())
                        {
                            strParse = strParse + strCadena.Substring(i, 1);
                        }

                        j = 0;
                    }
                } //end for

                if ((j == 0) & (strParse != '\n'.ToString()) & (strParse != ""))
                {
                    strResult = (strResult + prmEnclose + strParse.Trim() + prmEnclose).Trim();
                }
                if (strResult.Substring(strResult.Length - 1, 1).ToString() == prmSeparator.ToString())
                {
                    strResult = strResult.Substring(0, strResult.Length - 1);
                }

                //txtResults2.Text =    "IN (" + strResult + ")";


            }
            else
            {
                throw new ArgumentNullException();
            }

            return strResult;
        }
        private void btnIn_Click(object sender, EventArgs e)
        {
            try
            {

                txtResults2.Text = txtIN.Text + " " + txtP1.Text + getCommaSeparated(",", cmbLis.Text) + txtP2.Text;

            }
            catch (Exception ex)
            {
                if (!(ex == null))
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
        private void btnDualUnion_Click(object sender, EventArgs e)
        {
            string strCadena, strParse = "", strResult = "";
            int i, j;
            j = 0;

            try
            {
                if (txtEntries.Text != "")
                {
                    strCadena = this.txtEntries.Text;
                    for (i = 0; i <= strCadena.Length - 1; i++)
                    {
                        //j = strCadena.IndexOf('\r');
                        if (strCadena.Substring(i, 1).ToString() == '\r'.ToString())
                        {
                            if (strParse.Trim() != "")
                            {
                                strResult = (strResult + " Select " + cmbLis.Text + strParse.Trim() + cmbLis.Text).Trim() + " as col1 From Dual Union ";
                            }
                            strParse = "";
                            j = 1;
                        }
                        else
                        {
                            if (strCadena.Substring(i, 1).ToString() != '\n'.ToString())
                            {
                                strParse = strParse + strCadena.Substring(i, 1);
                            }

                            j = 0;
                        }
                    } //end for

                    if ((j == 0) & (strParse != '\n'.ToString()) & (strParse != ""))
                    {
                        strResult = (strResult + " Select " + cmbLis.Text + strParse.Trim() + cmbLis.Text).Trim() + " From Dual ";
                    }
                    if (strResult.Trim().Substring(strResult.Length - 6, 5).ToString().Trim() == "Union".ToString())
                    {
                        strResult = strResult.Trim().Substring(0, strResult.Trim().Length - 5);
                    }

                    if (!chkDual.Checked) strResult = strResult.Replace("From Dual", "");

                    txtResults2.Text = "( " + strResult + " )";

                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                if (!(ex == null))
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
        public static string[,] importFile(System.Data.OleDb.OleDbDataReader reader)
        {
            string[,] x = new string[40000, reader.FieldCount];
            int j = -1;
            while (reader.Read())
            {
                j++;
                for (int i = 0; i <= reader.FieldCount - 1; i++)
                {
                    if (j == 0)
                    {
                        var a = reader.GetName(i);
                        x[j, i] = a;
                    }

                    if (reader[i].GetType() == typeof(string))
                        x[j + 1, i] = reader.GetString(i);
                    else if (reader[i].GetType() == typeof(int))
                        x[j + 1, i] = reader.GetInt32(i).ToString();
                    else if (reader[i].GetType() == typeof(double))
                        x[j + 1, i] = reader.GetDouble(i).ToString();
                    else if (reader[i].GetType() == typeof(DateTime))
                        x[j + 1, i] = reader.GetDateTime(i).ToString();
                    else if (reader[i].GetType() != typeof(DBNull))
                        throw new Exception("No supported yet:" + reader[i].GetType().Name);
                }
            }

            reader.Close();
            con.Close();

            return x;

        }
        private string getUpdate(string[,] a, int pkq)
        {
            string _y = " ";

            for (int i = 0; i <= a.GetUpperBound(0); i++)
            {
                string _x = "";
                string _x2 = "";

                for (int j = 0; j <= a.GetUpperBound(1); j++)
                {
                    if (string.IsNullOrEmpty(a[i, j])) continue;
                    if (a[0, j] == a[i, j]) break;

                    if (j <= pkq - 1) _x += a[0, j] + " = '" + a[i, j].TrimEnd() + "' AND ";
                    if (j > pkq - 1) _x2 += a[0, j] + " = '" + a[i, j].TrimEnd() + "' , ";
                }

                if (!string.IsNullOrEmpty(_x2))
                    _y += "Update " + txtTableName.Text + " SET " + _x2.Substring(0, _x2.Length - 2) + " WHERE " + _x.Substring(0, _x.Length - 4) + " \n";
            }


            return _y;
        }
        private string getInsert(string[,] a)
        {
            string _y = " ";

            for (int i = 0; i <= a.GetUpperBound(0); i++)
            {
                string _x = "";
                string _x2 = "";

                for (int j = 0; j <= a.GetUpperBound(1); j++)
                {
                    if (string.IsNullOrEmpty(a[i, j])) continue;
                    if (a[0, j] == a[i, j]) break;

                    _x += a[0, j] + " , ";
                    _x2 += "'" + a[i, j].TrimEnd() + "' , ";
                }

                if (!string.IsNullOrEmpty(_x2))
                    _y += "Insert into " + txtTableName.Text + " ( " + _x.Substring(0, _x.Length - 2) + ") " + " VALUES ( " + _x2.Substring(0, _x2.Length - 2) + ") \n";
            }
            return _y;
        }
        private string getDelete(string[,] a, int pkq)
        {
            string _y = " ";

            for (int i = 0; i <= a.GetUpperBound(0); i++)
            {
                string _x = "";
                //string _x2 = "";

                for (int j = 0; j <= a.GetUpperBound(1); j++)
                {
                    if (string.IsNullOrEmpty(a[i, j])) continue;
                    if (a[0, j] == a[i, j]) break;

                    if (j <= pkq - 1) _x += a[0, j] + " = '" + a[i, j].TrimEnd() + "' AND ";
                    //if (j > pkq - 1) _x2 += a[0, j] + " = '" + a[i, j] + "' , ";
                }

                if (!string.IsNullOrEmpty(_x))
                    _y += "Delete From " + txtTableName.Text + " WHERE " + _x.Substring(0, _x.Length - 4) + " \n";
            }
            return _y;
        }
        private void btnLoadExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string[,] a = importFile(importFromExcel(getFileName(1), "Sheet1"));

                int pkq = int.Parse(txtpk.Text == "" ? "0" : txtpk.Text);

                if (rUpd.Checked)
                    txtResults2.Text = getUpdate(a, pkq);
                else if (rIns.Checked)
                    txtResults2.Text = getInsert(a);
                else if (rDel.Checked)
                    txtResults2.Text = getDelete(a, pkq);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        static OleDbConnection con;
        public static OleDbDataReader importFromExcel(string Path, string SheetName)
        {
            /*
                http://www.connectionstrings.com/excel
                Standard alternative
                Try this one if the one above is not working. Some reports that Excel 2003 need the exta OLEDB; section in the beginning of the string.
                OLEDB;Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\MyExcel.xls;
                Extended Properties="Excel 8.0;HDR=Yes;IMEX=1";
                Important note!
                The quota " in the string needs to be escaped using your language specific escape syntax.
                c#, c++   \"
                VB6, VBScript   ""
                xml (web.config etc)   &quot;
                or maybe use a single quota '.
                "HDR=Yes;" indicates that the first row contains columnnames, not data. "HDR=No;" indicates the opposite.
                "IMEX=1;" tells the driver to always read "intermixed" (numbers, dates, strings etc) data columns as text. Note that this option might affect excel sheet write access negative.
                SQL syntax "SELECT [Column Name One], [Column Name Two] FROM [Sheet One$]". I.e. excel worksheet name followed by a "$" and wrapped in "[" "]" brackets.
                Check out the [HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Jet\4.0\Engines\Excel] located registry REG_DWORD "TypeGuessRows". That's the key to not letting Excel use only the first 8 rows to guess the columns data type. Set this value to 0 to scan all rows. This might hurt performance. Please also note that adding the IMEX=1 option might cause the IMEX feature to set in after just 8 rows. Use IMEX=0 instead to be sure to force the registry TypeGuessRows=0 (scan all rows) to work.
                If the Excel workbook is protected by a password, you cannot open it for data access, even by supplying the correct password with your connection string. If you try, you receive the following error message: "Could not decrypt file."
            */
            string connectionString = "Provider=Microsoft.Jet.OleDb.4.0; data source=" + Path + "; Extended Properties=Excel 8.0;";//HDR=Yes;IMEX=1;";

            string selectString = "SELECT * FROM [" + SheetName + "$]";
            con = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand(selectString, con);
            con.Open();
            try
            {
                OleDbDataReader theData = cmd.ExecuteReader();
                return theData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //con.Close();
                //con.Dispose();
            }
        }
        public static string getFileName(int FileType)
        {
            string imagename = string.Empty;
            FileDialog fldlg = new OpenFileDialog();
            //specify your own initial directory
            fldlg.InitialDirectory = @":C\";
            //this will allow only those file extensions to be added
            if (FileType == 0)
                fldlg.Filter = "Image File (*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*.gif";
            else
                if (FileType == 1)
                fldlg.Filter = "File (*.xls;*.xlsx)|*.xls;xlsx";

            if (fldlg.ShowDialog() == DialogResult.OK)
            {
                return fldlg.FileName;
            }

            return "";

        }
        //private ScriptEngine pyEngine = null;
        //private ScriptRuntime pyRuntime = null;
        //private ScriptScope pyScope = null;

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    if (pyEngine == null)
        //    {
        //        pyEngine = Python.CreateEngine();
        //        pyScope = pyEngine.CreateScope();
        //        pyScope.SetVariable("txt1", txtEntries);
        //        pyScope.SetVariable("txt2", txtResults2);
        //    }

        //    CompileSourceAndExecute(CreatePythonScript1());
        //    CompileSourceAndExecute(txtexe.Text);
        //}


        //private void CompileSourceAndExecute(String code)
        //{
        //    try
        //    {
        //        ScriptSource source = pyEngine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
        //        CompiledCode compiled = source.Compile();
        //        // Executes in the scope of Python
        //        compiled.Execute(pyScope);
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //    }
        //}
        private String CreatePythonScript1()
        {
            String result = "";
            string[] lines =
                {
                    @"def DoIt1(logObj):",
                    @"   logObj.AddInfo('Executed in a function call using log object input.')"
                };
            result = String.Join("\r", lines);

            result = txtEntries.Text.Replace("\n", "");
            return result;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string model = txtEntries.Text,
                    ranameModel = "Edit-AXModelManifest –Model {0} –ManifestProperty \"Version = {1}.{2}.{3}.1\"",
                    unInstallModel = "Uninstall-AXModel –Model \"{0}\" -NoPrompt",
                    changeDisplayNameModel = "Edit-AXModelManifest –Model {0} –ManifestProperty \"DisplayName={1} *NewDisplayHere*\"",
                    changeDescrNameModel = "Edit-AXModelManifest –Model {0} –ManifestProperty \"Description=*NewDescriptionHere*\"",
                    unInstallMoldelsHolder = "",
                    changeDisplayNameModelHolder = "",
                    changeDescrNameModelHolder = "",
                    syear = DPsessionDate.Value.Year.ToString(),
                    smonth = DPsessionDate.Value.Month.ToString(),
                    smonthN = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(DPsessionDate.Value.Month),
                    sday = DPsessionDate.Value.ToString("dd"),
                    version = "Version:{0}.{1}.{2}.1",
                    ren = @"ren \\cavan01tsw004d\ax\Dev\Mods\Models\{0}.axmodel {0}_FM_{1}{2}{3}.axmodel",
                    renLabels = @"ren \\cavan01tsw004d\ax\Dev\Labels\{0}.ald {0}_FM_{1}{2}{3}.ald"
                    ;
            //2020Apr28
            string[] vModel;
            int len;
            bool iRLC = false;


            if (model == "") return;
            if (model.Contains(","))
                vModel = model.Split(',');
            else
                vModel = model.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            len = vModel.Length;
            string ln, fileName, sModel, sModelsMigrated = "", sRenames = "", sModelFileName = "", sRenameLabelUS = "", sRenameLabelGB = "", sRenameLabelDE = "";
            int beginOfFname = 0;

            txtResults2.Text = "";

            for (int f = 0; f < 2; f++)
            {
                
                fileName = f == 0 ? @"\\cavan01tsw004d\ax\Dev\Mods\AXModelExport.ps1" :
                                    @"\\cavan01tsw004d\ax\Dev\Mods\AXModelInstall.ps1";



                if (f == 1)
                    txtResults2.Text += "\r\n";

                using (StreamReader file = new StreamReader(fileName))
                {
                    while ((ln = file.ReadLine()) != null)
                    {
                        for (int i = 0; i < len; i++)
                        {
                            sModel = vModel[i];
                            sModel = sModel.Replace("\r", "").Replace("\n", "").Replace(" ", "");
                            
                            if (ln.Contains("Edit-AXModelManifest")) continue;

                            if (ln.Contains(sModel))
                            {
                                if (f == 0) //AXModelExport
                                {
                                    if (chkIncludedExport.Checked)  //user only wants Install scripts                                        
                                       txtResults2.Text += string.Format(ranameModel, sModel, syear, smonth, sday) + _NEWLINE_;

                                    unInstallMoldelsHolder += string.Format(unInstallModel, sModel) + _NEWLINE_;
                                    changeDisplayNameModelHolder += string.Format(changeDisplayNameModel, sModel, sModel) + _NEWLINE_;
                                    changeDescrNameModelHolder += string.Format(changeDescrNameModel, sModel) + _NEWLINE_;
                                    sModelsMigrated += string.Format("{0}[{1}]{2}", sModel, string.Format(version, syear, smonth, sday), _NEWLINE_);

                                    //phical file renaming
                                    beginOfFname = ln.LastIndexOf(@"\") + 1;
                                    sModelFileName = ln.Substring(beginOfFname, ln.LastIndexOf(@".") - beginOfFname);
                                    sRenames += string.Format(ren, sModelFileName, syear, smonthN, sday) + _NEWLINE_;
                                    if (!iRLC)
                                    {
                                        sRenameLabelUS += string.Format(renLabels, "AxANGen-us", syear, smonthN, sday);
                                        sRenameLabelGB += string.Format(renLabels, "AxANGen-gb", syear, smonthN, sday);
                                        sRenameLabelDE += string.Format(renLabels, "AxANGde", syear, smonthN, sday);
                                        iRLC = true;
                                    }

                                }


                                ln = ln.Replace(@"\Dev\", @"\" + cmbAxModServer.Text + @"\");
                                ln = ln.Replace(@"\Models\", @"\Models\" + txtAxModPrefix.Text + (txtAxModPrefix.Text == "" ? "" : @"_"));
                                txtResults2.Text += ln + _NEWLINE_;



                                break;
                            }
                        }
                    }
                    file.Close();
                }
            }

            //if (sModelsMigrated.Length > 0)  //remove last comma
            //    sModelsMigrated = sModelsMigrated.Substring(0, sModelsMigrated.Length - 1);


            txtResults2.Text += _NEWLINE_ + sRenames;
            txtResults2.Text += _NEWLINE_ + sRenameLabelUS;
            txtResults2.Text += _NEWLINE_ + sRenameLabelGB;
            txtResults2.Text += _NEWLINE_ + sRenameLabelDE;

            txtResults2.Text += _NEWLINE_ + _NEWLINE_ + _NEWLINE_ + _NEWLINE_;



            txtResults2.Text += @"The requested AX modification has been migrated to the Test environment. " + _NEWLINE_ +
                                 "Please test the modification (or designate others to perform the test) and then update this ticket to indicate " + _NEWLINE_ +
                                 "that the modification meets all required business/user requirements. You may have already written a unit test " + _NEWLINE_ +
                                 "script or validation script that you can use to test the modification. Test evidence should be attached to the ticket. " + _NEWLINE_ +
                                 "If the test evidence involves many files or very large files, then please request a test folder to be created." + _NEWLINE_;
            txtResults2.Text += _NEWLINE_;
            txtResults2.Text += "The following models were migrated: " + _NEWLINE_ + sModelsMigrated + _NEWLINE_;

            txtResults2.Text += _NEWLINE_ + _NEWLINE_ + _NEWLINE_ + _NEWLINE_;

            txtResults2.Text += "The change was migrated to Production. If there are no issues or concerns with the change then the Client or IT Lead should now close the ticket.";
            txtResults2.Text += _NEWLINE_ + _NEWLINE_;
            txtResults2.Text += "The following models were migrated: " + _NEWLINE_ + sModelsMigrated + _NEWLINE_ + _NEWLINE_ + _NEWLINE_;

            txtResults2.Text += "***COMPLETED * **" + _NEWLINE_;
            txtResults2.Text += "The AX system is now available. You may log back into the system and production scanning can be resumed.";
            txtResults2.Text += _NEWLINE_ + _NEWLINE_;
            txtResults2.Text += "The following models were migrated: " + _NEWLINE_ + _NEWLINE_ + _NEWLINE_;


            txtResults2.Text += _NEWLINE_ + _NEWLINE_;
            txtResults2.Text += "Modification was made directly in Production as an Emergency change." + _NEWLINE_;
            txtResults2.Text += "It will be proper migrated in Next scheduled code migration." + _NEWLINE_;
            txtResults2.Text += "If there are no issues or concerns with the change then please leave a comment in this ticket." + _NEWLINE_;

            txtResults2.Text += _NEWLINE_ + _NEWLINE_;
            txtResults2.Text += "***UTILS*****" + _NEWLINE_ + _NEWLINE_;
            txtResults2.Text += unInstallMoldelsHolder + _NEWLINE_ + _NEWLINE_;
            txtResults2.Text += changeDisplayNameModelHolder + _NEWLINE_ + _NEWLINE_;
            txtResults2.Text += changeDescrNameModelHolder + _NEWLINE_ + _NEWLINE_;

            txtResults2.Text += "***SSRS DEPLOY {Depend on instance date format}*****" + _NEWLINE_ + _NEWLINE_;
            txtResults2.Text += "NA!" + _NEWLINE_ + _NEWLINE_;
            txtResults2.Text += @"Publish-AXReport -ModifiedAfter MM/DD/YYYY -ReportName *" + _NEWLINE_ + _NEWLINE_;
            txtResults2.Text += "TAU!" + _NEWLINE_ + _NEWLINE_;
            txtResults2.Text += @"Publish-AXReport -ModifiedAfter DD/MM/YYYY -ReportName *" + _NEWLINE_ + _NEWLINE_;
            txtResults2.Text += "GER!" + _NEWLINE_ + _NEWLINE_;
            txtResults2.Text += @"Publish-AXReport -ModifiedAfter DD/MM/YYYY -ReportName *" + _NEWLINE_ + _NEWLINE_;



            txtResults2.Text += "***APP Compile on CMD*****" + _NEWLINE_ + _NEWLINE_;
            var strComplie    = @"CD\" + _NEWLINE_ +
                                @"CD C:\Program Files\Microsoft Dynamics AX\60\Server\{0}\bin"+ _NEWLINE_ +
                                @"AxBuild xppcompileall /aos=01 /layer=cus /workers=10" + _NEWLINE_ + _NEWLINE_;
            txtResults2.Text += string.Format(strComplie,"AX_TEST");
            txtResults2.Text += string.Format(strComplie, "AX_PROD");
            txtResults2.Text += string.Format(strComplie, "AX_PREPROD");
            txtResults2.Text += string.Format(strComplie, "AX_DEV");
            txtResults2.Text += string.Format(strComplie, "AX_TEST3");
            txtResults2.Text += string.Format(strComplie, "AX_STAGING");



        }
        private void button2_Click(object sender, EventArgs e)
        {
            string txt = txtEntries.Text;
            string PT = "PARTITION = 5637144576";
            string PT2 = "PARTITION#2 = 5637144576";
            string DA = "DATAAREAID = '220'";
            string i_ = "";

            txt = txt.Replace("PARTITION = ?", PT);
            txt = txt.Replace("PARTITION=?", PT);
            txt = txt.Replace("DATAAREAID=?", DA);
            txt = txt.Replace("PARTITION#2 = ?", PT2);
            txt = txt.Replace("DATAAREAID = ?", DA);
            txt = txt.Replace("PARTITION=@P1", PT);
            for (int i = 0; i <= 11; i++)
            {
                i_ = i.ToString();
                txt = txt.Replace("PARTITION=@P" + i_, PT);
                txt = txt.Replace("PARTITION#2=@P" + i_, PT);
                txt = txt.Replace("PARTITION#3=@P" + i_, PT);
                txt = txt.Replace("PARTITION#4=@P" + i_, PT);
                txt = txt.Replace("PARTITION#5=@P" + i_, PT);
                txt = txt.Replace("PARTITION#6=@P" + i_, PT);
                txt = txt.Replace("PARTITION#7=@P" + i_, PT);
                txt = txt.Replace("PARTITION#8=@P" + i_, PT);
                txt = txt.Replace("PARTITION#9=@P" + i_, PT);
                txt = txt.Replace("PARTITION#10=@P" + i_, PT);
                txt = txt.Replace("PARTITION#11=@P" + i_, PT);
                txt = txt.Replace("DATAAREAID=@P" + i_, DA);
                txt = txt.Replace("DATAAREAID#2=@P" + i_, DA);
                txt = txt.Replace("DATAAREAID#3=@P" + i_, DA);
                txt = txt.Replace("DATAAREAID#4=@P" + i_, DA);
                txt = txt.Replace("DATAAREAID#5=@P" + i_, DA);
                txt = txt.Replace("DATAAREAID#6=@P" + i_, DA);
                txt = txt.Replace("DATAAREAID#7=@P" + i_, DA);
                txt = txt.Replace("DATAAREAID#8=@P" + i_, DA);
                txt = txt.Replace("DATAAREAID#9=@P" + i_, DA);
                txt = txt.Replace("DATAAREAID#10=@P" + i_, DA);
                txt = txt.Replace("DATAAREAID#11=@P" + i_, DA);

            }

            txtResults2.Text = txt;
        }
        private void btnUnique_Click(object sender, EventArgs e)
        {
            try
            {
                string values = getCommaSeparated(",", "");
                string[] vec = values.Split(',');
                string val;

                txtResults2.Text = "";

                List<string> l = new List<string>();

                for (int i = 0; i < vec.Length; i++)
                {
                    val = vec[i];
                    if (l.Exists(x => x == val)) continue;

                    l.Add(vec[i]);

                }

                vec = l.ToArray();

                for (int i = 0; i < vec.Length; i++)
                {
                    txtResults2.Text += vec[i] + "\r\n";
                }
            }
            catch (Exception ex)
            {
                if (!(ex == null))
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
        private void btnClearDefault_Click(object sender, EventArgs e)
        {
            txtIN.Text = "";
            txtP1.Text = "";
            txtP2.Text = "";
            cmbLis.Text = "";
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtIN.Text = "IN";
            txtP1.Text = "(";
            txtP2.Text = ")";
            cmbLis.Text = "'";
        }
        private void btnClipboard_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(txtResults2.Text);
        }
        private void btnAxLog_Click(object sender, EventArgs e)
        {

            List<string> vStatuses = new List<string>();
            List<string> fileContent = new List<string>();
            string ln, status, fileName = @"\\cavan01tsw004d\ax\Dev\Mods\PendingMods.txt";
            int posA, posB, i = -1;
            bool previousCCA_Done = false;

            txtResults2.Text = "";
            using (StreamReader file = new StreamReader(fileName))
            {
                //get statuses
                while ((ln = file.ReadLine()) != null)
                {
                    fileContent.Add(ln);
                    if (ln.Contains("(") && ln.Contains(")"))
                    {
                        posA = ln.IndexOf('(');
                        posB = ln.IndexOf(')', posA);
                        
                        if (posB == -1) continue;

                        status = ln.Substring(posA + 1, posB - (posA + 1));

                        if (!vStatuses.Contains(status.ToUpper()))
                            vStatuses.Add(status.ToUpper());
                    }
                    if (ln.Contains("===== "))
                        break;
                }
                file.Close();

                //show grouped 
                foreach (var statusValue in vStatuses)
                {
                    txtResults2.Text += "***" + statusValue + "\r\n";
                    foreach (var str in fileContent)
                    {
                        if (str.ToUpper().Contains("(" + statusValue + ")"))
                        {
                            txtResults2.Text += "\t\t\t" + str + "\r\n";
                        }
                    }
                }


                txtResults2.Text += "" + "\r\n";
                txtResults2.Text += "" + "\r\n";
                txtResults2.Text += "" + "\r\n";
                txtResults2.Text += "" + "\r\n";
                txtResults2.Text += "-------------------------------------------------------------------------------------" + "\r\n";
                txtResults2.Text += "------------------------------MODS TO APPLY IN PROD----------------------------------" + "\r\n";
                txtResults2.Text += "-------------------------------------------------------------------------------------" + "\r\n";
                //ONLY CCA DONE
                foreach (var str in fileContent)
                {
                    if (str.Contains("(") && str.Contains(")"))  //have a status
                    {
                        //previousCCA_Done = (str.ToUpper().Contains("(" + "CCA DONE" + ")")); //contain a (CCA DONE)????
                        previousCCA_Done = str.ToUpper().Contains("(" + "CCA DONE"); //contain a (CCA DONE*????
                    }

                    if (previousCCA_Done)
                        txtResults2.Text += str.Replace(">>>","###") + "\r\n";
                }


                txtResults2.Text += "" + "\r\n";
                txtResults2.Text += "" + "\r\n";
                txtResults2.Text += "" + "\r\n";
                txtResults2.Text += "" + "\r\n";
                txtResults2.Text += "-------------------------------------------------------------------------------------" + "\r\n";
                txtResults2.Text += "------------------------------MODS TO APPLY IN TEST-------------------------------" + "\r\n";
                txtResults2.Text += "-------------------------------------------------------------------------------------" + "\r\n";
                //ONLY CCA DONE
                foreach (var str in fileContent)
                {
                    if (str.Contains("(") && str.Contains(")"))  //have a status
                    {
                        //previousCCA_Done = (str.ToUpper().Contains("(" + "CCA DONE" + ")")); //contain a (CCA DONE)????
                        previousCCA_Done = str.ToUpper().Contains("(" + "IN DEV*"); //contain a (CCA DONE*????
                    }

                    if (previousCCA_Done)
                        txtResults2.Text += str.Replace(">>>", "###") + "\r\n";
                }

                /*
                txtResults2.Text += "" + "\r\n";
                txtResults2.Text += "" + "\r\n";
                txtResults2.Text += "" + "\r\n";
                txtResults2.Text += "" + "\r\n";
                txtResults2.Text += "-------------------------------------------------------------------------------------" + "\r\n";
                txtResults2.Text += "------------------------------MODS PEND CCA-------------------------------" + "\r\n";
                txtResults2.Text += "-------------------------------------------------------------------------------------" + "\r\n";
                //ONLY CCA DONE
                foreach (var str in fileContent)
                {
                    if (str.Contains("(") && str.Contains(")"))  //have a status
                    {
                        //previousCCA_Done = (str.ToUpper().Contains("(" + "CCA DONE" + ")")); //contain a (CCA DONE)????
                        previousCCA_Done = str.ToUpper().Contains("(" + "IN TEST"); //contain a (CCA DONE*????
                    }

                    if (previousCCA_Done)
                        txtResults2.Text += str.Replace(">>>", "###") + "\r\n";
                }
                */
            }
        }


        private void StatusServices()
        {
            string[] command = new string[20], commandStop = new string[20];
            string fmt = @"sc \\{0} query {1}";
            string sch = @"schtasks /S {0} /Query  /TN {1}";
            string _AOS1_ = "", _AOS2_ = "", _SSRS_ = "", _COM_ = "";
            string strStartCommand = "", strStopCommand = "";
            bool runAsAdmin = (ModifierKeys & Keys.Shift) == Keys.Shift;
            int iTop = int.Parse(txtTop.Text);

            string _PREPROD_ = "CAVAN01ASW011D",
                     _STAGING_ = "CAVAN01ASW014D",
                     _TEST3_ = "VANDBW011D",
                     _DEV_ = "CAVAN01DBW009D",
                     _TEST_AOS = "CAVAN01ASW012D",
                     _TEST_SSRS_ = "CAVAN01DBW010D",
                     _CH_TEST_AOS = "SHA1ASW003D",
                     _CH_TEST_SSRS = "SHA1ASW002D",
                     _CH_PROD_AOS = "SHA1ASW003P",
                     _CH_PROD_SSRS = "sha1asw002p",
                     _PROD_AOS_1 = "CAVAN01ASW010P",
                     _PROD_AOS_2 = "VANASW012P",
                     _PROD_SSRS = "CAVAN01DBW005P",
                     _PROD_COM = "VANDBW002P",
                     _ATLAS_ = "\"Atlas 5.1.3578 AX2012R3 ATLAS-{0} Service\"",
                     _COMAX_ = "\"ComAX_{0}\"",
                     _UK_TEST_AOS = "TAUASW001D",
                     _UK_TEST_SSRS = "TAUDBW003D",
                     _UK_PROD_AOS = "tauasw003p",
                     _UK_PROD_SSRS = "taudbw005p",
                     _GE_TEST_AOS = "JETASW001D",
                     _GE_TEST_SSRS = "JETDBW003D",
                     _GE_PROD_AOS = "jetasw001p",
                     _GE_PROD_SSRS = "jetdbw002p"
                     ;

            btnStatusServices.Enabled = false;
            txtResults2.Text = "";



            switch (cmbServers.Text)
            {
                case "PROD-NA":
                    _AOS1_ = _PROD_AOS_1;
                    _AOS2_ = _PROD_AOS_2;
                    _SSRS_ = _PROD_SSRS;
                    _COM_ = _PROD_COM;
                    _ATLAS_ = string.Format(_ATLAS_, "PROD");
                    _COMAX_ = string.Format(_COMAX_, "Production");
                    break;
                case "PROD-CH":
                    _AOS1_ = _CH_PROD_AOS;
                    _AOS2_ = _CH_PROD_AOS;
                    _SSRS_ = _CH_PROD_SSRS;
                    _COM_ = "";// _PROD_COM;
                    _ATLAS_ = string.Format(_ATLAS_, "PROD-CN");
                    _COMAX_ = ""; // string.Format(_COMAX_, "Production");
                    break;
                case "PROD-UK":
                    _AOS1_ = _UK_PROD_AOS;
                    _AOS2_ = _UK_PROD_AOS;
                    _SSRS_ = _UK_PROD_SSRS;
                    _COM_ = "";// _PROD_COM;
                    _ATLAS_ = string.Format(_ATLAS_, "TEST-TAU");
                    _COMAX_ = ""; // string.Format(_COMAX_, "Production");
                    break;
                case "PROD-GER":
                    _AOS1_ = _GE_PROD_AOS;
                    _AOS2_ = _GE_PROD_AOS;
                    _SSRS_ = _GE_PROD_SSRS;
                    _COM_ = "";// _PROD_COM;
                    _ATLAS_ = string.Format(_ATLAS_, "PROD-JET");
                    _COMAX_ = ""; // string.Format(_COMAX_, "Production");
                    break;
                case "TEST-CH":
                    _AOS1_ = _CH_TEST_AOS;
                    _AOS2_ = _CH_TEST_AOS;
                    _SSRS_ = _CH_TEST_SSRS;
                    _ATLAS_ = string.Format(_ATLAS_, "TEST-CN");
                    _COMAX_ = string.Format(_COMAX_, "Staging");
                    break;
                case "TEST-UK":
                    _AOS1_ = _UK_TEST_AOS;
                    _AOS2_ = _UK_TEST_AOS;
                    _SSRS_ = _UK_TEST_SSRS;
                    _ATLAS_ = string.Format(_ATLAS_, "TEST-TAU");
                    _COMAX_ = string.Format(_COMAX_, "Staging");
                    break;
                case "TEST-GE":
                    _AOS1_ = _GE_TEST_AOS;
                    _AOS2_ = _GE_TEST_AOS;
                    _SSRS_ = _GE_TEST_SSRS;
                    _ATLAS_ = string.Format(_ATLAS_, "TEST-JET");
                    _COMAX_ = string.Format(_COMAX_, "Staging");
                    break;
                case "TEST-NA":
                    _AOS1_ = _TEST_AOS;
                    _AOS2_ = _TEST_AOS;
                    _SSRS_ = _TEST_SSRS_;
                    _COM_ = _PROD_COM;
                    _ATLAS_ = string.Format(_ATLAS_, "TEST");
                    _COMAX_ = string.Format(_COMAX_, "Staging");
                    break;
                case "DEV-NA":
                    _AOS1_ = _DEV_;
                    _AOS2_ = _DEV_;
                    _SSRS_ = _DEV_;
                    _COM_ = _PROD_COM;
                    _ATLAS_ = string.Format(_ATLAS_, "DEV");
                    _COMAX_ = string.Format(_COMAX_, "Dev");
                    break;
                case "TEST3-NA":
                    _AOS1_ = _TEST3_;
                    _AOS2_ = _TEST3_;
                    _SSRS_ = _TEST3_;
                    _COM_ = _PROD_COM;
                    _ATLAS_ = string.Format(_ATLAS_, "TEST");
                    _COMAX_ = string.Format(_COMAX_, "Staging");
                    break;
                case "PREPROD-NA":
                    _AOS1_ = _PREPROD_;
                    _AOS2_ = _PREPROD_;
                    _SSRS_ = _PREPROD_;
                    _COM_ = _PROD_COM;
                    _ATLAS_ = string.Format(_ATLAS_, "TEST");
                    _COMAX_ = string.Format(_COMAX_, "Staging");
                    break;
                case "STAGING-NA":
                    _AOS1_ = _STAGING_;
                    _AOS2_ = _STAGING_;
                    _SSRS_ = _STAGING_;
                    _COM_ = _PROD_COM;
                    _ATLAS_ = string.Format(_ATLAS_, "TEST");
                    _COMAX_ = string.Format(_COMAX_, "Staging");
                    break;
                default: break;
            }

            command[0] = string.Format(fmt, _AOS1_, "AOS60$01");
            command[1] = string.Format(fmt, _AOS2_, "AOS60$01");
            command[2] = string.Format(fmt, _AOS1_, _ATLAS_);
            command[3] = string.Format(fmt, _AOS1_, "MR2012ApplicationService");
            command[4] = string.Format(fmt, _AOS1_, "MR2012ProcessService");
            command[5] = string.Format(fmt, _SSRS_, "MSSQLServerOLAPService");
            command[6] = string.Format(fmt, _SSRS_, "ReportServer");
            command[7] = string.Format(fmt, _SSRS_, "RFSmart5_Monitor");
            command[8] = string.Format(fmt, _SSRS_, "RFSmart5_Printing");
            command[9] = string.Format(fmt, _SSRS_, "RFSmart5_Telnetd");
            command[10] = string.Format(sch, _COM_, _COMAX_);

            for (int i = 0; i < command.Length; i++)
            {
                if (iTop > 0 && i + 1 > iTop) //skip if top has been reached.
                    break;

                if (command[i] == null) continue;
                try
                {

                    bool isSchedTack = command[i].Contains("Query");
                    if (isSchedTack)
                    {
                        strStartCommand += command[i].Replace("Query", "Run") + _NEWLINE_;
                        strStopCommand += command[i].Replace("Query", "End") + _NEWLINE_;
                    }
                    else
                    {
                        strStartCommand += command[i].Replace("query", "start") + _NEWLINE_;
                        strStopCommand += command[i].Replace("query", "stop") + _NEWLINE_;
                    }

                    if (runAsAdmin)
                        txtResults2.Text += runAsCmd(command[i]) + _NEWLINE_;
                    else
                        txtResults2.Text += runCmd(command[i]) + _NEWLINE_;

                }
                catch (Exception ex)
                {
                    txtResults2.Text += "*********** ERROR ************** " + command[i] + _NEWLINE_;
                }
                Application.DoEvents();
            }

            //add start commands
            txtResults2.Text += _NEWLINE_ + "~~ START COMMANDS ~~" + _NEWLINE_;
            txtResults2.Text += strStartCommand;
            txtResults2.Text += _NEWLINE_ + "~~ STOP COMMANDS ~~" + _NEWLINE_;
            txtResults2.Text += strStopCommand;

            btnStatusServices.Enabled = true;
            System.Media.SystemSounds.Exclamation.Play();
        }
        private void btnStatusServices_Click(object sender, EventArgs e)
        {
            StatusServices();
            
        }

        string runCmd(string command)
        {
            
            // create the ProcessStartInfo using "cmd" as the program to be run, and "/c " as the parameters.
            // Incidentally, /c tells cmd that we want it to execute the command that follows, and then exit.
            System.Diagnostics.ProcessStartInfo procStartInfo =
                new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

            // The following commands are needed to redirect the standard output.
            // This means that it will be redirected to the Process.StandardOutput StreamReader.
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true; // Do not create the black window.            
            System.Diagnostics.Process proc = new System.Diagnostics.Process(); // Now we create a process, assign its ProcessStartInfo and start it
            proc.StartInfo = procStartInfo;
            proc.Start();

            string result = proc.StandardOutput.ReadToEnd(); // Get the output into a string


            string[] vec;
            vec = result.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (vec.Length == 4)
                return vec[3] + " >>> " + command;

            return vec[2] + " >>> " + command;
        }
        string runAsCmd(string command)
        {           
            // create the ProcessStartInfo using "cmd" as the program to be run, and "/c " as the parameters.
            // Incidentally, /c tells cmd that we want it to execute the command that follows, and then exit.
            System.Security.SecureString ssPwd = new System.Security.SecureString();
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", " /c " + command);


            procStartInfo.Domain = _DOMAIN_;
            procStartInfo.UserName = _USER_;
            string password = _PASSWORD_;
            for (int x = 0; x < password.Length; x++)
            {
                ssPwd.AppendChar(password[x]);
            }
            procStartInfo.Password = ssPwd;
            // The following commands are needed to redirect the standard output.
            // This means that it will be redirected to the Process.StandardOutput StreamReader.
            procStartInfo.RedirectStandardOutput = true;

            procStartInfo.UseShellExecute = false;

            procStartInfo.CreateNoWindow = true; // Do not create the black window.            
            procStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            System.Diagnostics.Process proc = new System.Diagnostics.Process(); // Now we create a process, assign its ProcessStartInfo and start it
            proc.StartInfo = procStartInfo;

            proc.Start();
            
            string result = proc.StandardOutput.ReadToEnd(); // Get the output into a string


            string[] vec;
            vec = result.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (vec.Length == 4)
                return vec[3] + " >>> " + command;
            if (vec.Length == 2)
                return vec[1] + " >>> " + command;

            return vec[2] + " >>> " + command;
        }
        private void cmdHelpDesk_Click(object sender, EventArgs e)
        {
            try
            {

                txtResults2.Text = "";
                Application.DoEvents();

                string sStaus = CmbHelpDesk.Text.Split('.')[0];
                int limit = 500;
                string sLink = "";
                string sKeyword = txtKeyword.Text;

                if (sKeyword != "")
                    sLink = string.Format(@"http://helpdesk/tickets.php?keyword={0}", sKeyword);
                else
                    sLink = string.Format(@"http://helpdesk/tickets.php?limit={0}&&status={1}", limit, sStaus);


                string url = sLink; //@"http://helpdesk/tickets.php?keyword=&from_date=&to_date=&completed_from_date=&completed_to_date=&cat_id=&service_id=48&action_id=&assigned=&userid=&originator=&priority=&location=&status=6&source=&followup=&change_control=&change_type=&change_risk=&show_comments=on";
                String userName = "fmaestre";
                String passWord = getPassword(); // "Del1al.05";
                using (WebClient client = new WebClient())
                {
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + passWord));
                    client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
                    client.Encoding = Encoding.Default;
                    var result = client.DownloadString(url);
                    result = HttpUtility.HtmlDecode(result);
                    var ss = parseHelpDesk(result);

                    txtResults2.Text += ss + _NEWLINE_;
                    //Console.WriteLine(ss);
                    //Console.ReadLine();
                    if (sKeyword != "")
                    {
                        highlight(sKeyword);
                    }


                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
        string parseHelpDesk(string html)
        {
            int i = 0;
            var ss = "";
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            var htmlNodes = doc.DocumentNode.SelectNodes("//table/tr");
            foreach (var x in htmlNodes)
            {
                var tds = x.SelectNodes("td");
                string str = "";
                foreach (var d in tds)
                    str += String.Format("{0},", d.InnerHtml.Replace(",", " "));

                if (!str.StartsWith("<span class")
                    &&
                    !str.StartsWith("<center")
                    &&
                    !str.StartsWith("<table")
                    &&
                    !str.StartsWith("<img")
                    &&
                    !str.StartsWith("<input")
                    &&
                    !str.StartsWith("Ticket")
                    &&
                    !str.StartsWith("<a")
                    )
                {
                    string str0 = "";

                    var vec = str.Split(',');
                    int col = 0;
                    foreach (var s0 in vec)
                    {
                        var doc0 = new HtmlAgilityPack.HtmlDocument();
                        doc0.LoadHtml(s0);
                        HtmlAgilityPack.HtmlNodeCollection tds0;
                        var sout = s0;
                        col++;

                        //if (i == 1)
                        //    i = i;

                        if (col == 15)
                        {
                            tds0 = doc0.DocumentNode.SelectNodes("//b");
                            sout = sout.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                            sout = sout.Replace(",", ";");

                            if (tds0 != null)
                            {
                                sout = cleartags("<b", ">", sout, tds0);
                                sout = cleartags("</b", ">", sout, tds0);
                            }
                            doc0.LoadHtml(sout);
                            tds0 = doc0.DocumentNode.SelectNodes("br");
                            if (tds0 != null)
                                sout = cleartags("<b", "r>", sout, tds0);

                            doc0.LoadHtml(sout);
                            tds0 = doc0.DocumentNode.SelectNodes("font");
                            if (tds0 != null)
                            {
                                sout = cleartags("<font", ">", sout, tds0);
                                sout = cleartags("</font", ">", sout, tds0);
                            }
                            tds0 = null;
                        }
                        else if (s0.StartsWith("<a"))
                        {
                            tds0 = doc0.DocumentNode.SelectNodes("a");
                            if (i > 0 && !tds0[0].InnerHtml.StartsWith("#"))
                            {
                                sout = sout.Substring(sout.LastIndexOf("</a>") + 4);
                                if (sout.Trim() == "")
                                    sout = cleartags("<a", "</a>", s0, tds0);

                                tds0 = null;
                            }
                        }
                        else if (s0.StartsWith("<img"))
                        {
                            sout = s0.Replace(s0.Substring(0, s0.IndexOf('>') + 1), "");
                            tds0 = doc0.DocumentNode.SelectNodes("a");
                            if (tds0 != null)
                            {
                                var souttmp = sout.Substring(sout.LastIndexOf("</a>") + 4);
                                if (souttmp.Trim() == "")
                                {
                                    sout = cleartags("<a", "</a>", sout, tds0);
                                }
                                else
                                {
                                    sout = souttmp;
                                }
                            }
                            else if (sout.IndexOf("...") > -1)
                            {
                                sout = sout.Substring(0, sout.IndexOf("...") + 3);
                            }
                            tds0 = null;
                        }
                        else
                        {
                            tds0 = null;
                            int xi = s0.IndexOf("<a href");
                            if (xi > -1)
                                sout = sout.Substring(0, xi);
                        }
                        str0 += String.Format("{0},", tds0 != null ? tds0[0].InnerText : sout);

                    }
                    i++;


                    ss += str0 + _NEWLINE_;
                }
            }
            return ss;
        }






        string cleartags(string tagOpen, string tagClose, string toCLean, HtmlAgilityPack.HtmlNodeCollection nodes)
        {
            string sout = toCLean;
            int open = 0, close = 0;
            foreach (var t in nodes)
            {
                open = sout.IndexOf(tagOpen);
                if (open == -1) continue;
                close = sout.IndexOf(tagClose, open) + tagClose.Length;
                sout = sout.Replace(sout.Substring(open, close - open), "");
            }
            return sout;
        }


        void highlight(string _test)
        {
            string[] words = _test.Split(',');
            int startindex;
            int wordLen;
            int textLen = txtResults2.TextLength;
            foreach (string word in words)
            {
                startindex = 0;
                wordLen = word.Length;
                while (startindex < textLen)
                {
                    int wordstartIndex = txtResults2.Find(word, startindex, RichTextBoxFinds.None);
                    if (wordstartIndex != -1)
                    {
                        txtResults2.SelectionStart = wordstartIndex;
                        txtResults2.SelectionLength = word.Length;
                        txtResults2.SelectionBackColor = Color.Yellow;
                    }
                    else
                        break;
                    startindex = wordstartIndex + wordLen;
                }
            }
        }


        void clearHighLight()
        {
            txtResults2.SelectionStart = 0;
            txtResults2.SelectAll();
            txtResults2.SelectionBackColor = txtResults2.BackColor;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {

            //if (chkFilter.Checked)
            //{
            //     filterLines(savedResults, txtFilter.Text);
            //}
            // clearHighLight();
            // highlight(txtFilter.Text);



            FindOrFilter();

        }

        void filterLines(string _lines, string _value)
        {
            string value = _value.ToUpper();
            string[] lines = _lines.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            txtResults2.Text = "";
            foreach (var s in lines)
            {
                if (s.ToUpper().Contains(value))
                {
                    txtResults2.Text += s + _NEWLINE_;
                }
            }

        }
        string savedResults;
        private void txtFilter_Enter(object sender, EventArgs e)
        {
            savedResults = txtResults2.Text;
        }



        private void StartHighLight(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgWorker = (BackgroundWorker)sender;
            System.Windows.Forms.RichTextBox th_txtResults;
            object[] xObj = e.Argument as object[];
            string[] words = xObj[1].ToString().Split(',');
            int startindex;
            int wordLen;
            int[] outObj = new int[2];

            string indexes = "";

            th_txtResults = new RichTextBox();

            th_txtResults.Text = xObj[0].ToString();

            int textLen = th_txtResults.TextLength;
            foreach (string word in words)
            {

                startindex = 0;
                wordLen = word.Length;
                while (startindex < textLen)
                {
                    int wordstartIndex = th_txtResults.Find(word, startindex, RichTextBoxFinds.None);
                    if (wordstartIndex != -1)
                    {
                        outObj[0] = wordstartIndex;
                        outObj[1] = wordLen;
                        startindex = wordstartIndex + wordLen;

                        indexes += string.Format("{0}{1}", wordstartIndex, ",");
                    }
                    else
                        break;

                }

                e.Result = indexes;
            }

        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            //txtEntries.Text = e.Result.ToString();
            string[] pos = e.Result.ToString().Split(',');
            clearHighLight();
            foreach (var p in pos)
            {
                if (p != "")
                {
                    txtResults2.SelectionStart = int.Parse(p);
                    txtResults2.SelectionLength = txtFilter.Text.Length;
                    txtResults2.SelectionBackColor = Color.Yellow;


                }
            }

            //create link for tickets
            var splitLedLines = txtResults2.Text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string tickets = "", helpDeskLink = "http://helpdesk/ticket.php?ticket={0}";

            int idx, TicketLen = 5;
            foreach (var line in splitLedLines)
            {
                //if (line.Length < 5) continue;
                idx = line.IndexOf("#");
                if (idx++ == -1) continue;
                try
                {
                    tickets += string.Format(helpDeskLink, line.Substring(idx, TicketLen)) + _NEWLINE_;
                }
                catch { }


            }
            txtEntries.Text += tickets + _NEWLINE_;

            //System.Threading.Thread.Sleep(100);
            workerCompleted = true;

        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            /*
            int[] outObj = e.UserState as int[];
            int wordstartIndex = outObj[0];
            int len = outObj[1];
            
            txtResults2.SelectionStart = wordstartIndex;
            txtResults2.SelectionLength = len;
            txtResults2.SelectionBackColor = Color.Yellow;

            txtEntries.Text += string.Format("{0},{1}{2}", wordstartIndex, len, _NEWLINE_);
            */


        }

        private void FindOrFilter()
        {

            if (txtFilter.Text.Length == 1) return;

            if (chkFilter.Checked)
                filterLines(savedResults, txtFilter.Text);

            object[] _obj = new Object[2];
            _obj[0] = chkFilter.Checked ? txtResults2.Text : savedResults;
            _obj[1] = txtFilter.Text;


            if (workerCompleted)
            {
                txtEntries.Text = "";
                workerCompleted = false;
                worker.RunWorkerAsync(_obj);
            }
        }

        private void btnUrl_Click(object sender, EventArgs e)
        {
            try
            {
                string _url = "";
                if (txtEntries.SelectedText != "")
                    System.Diagnostics.Process.Start("chrome.exe", txtEntries.SelectedText);
                else if (txtResults2.SelectedText != "")
                {
                    _url = URL + txtResults2.SelectedText;
                    System.Diagnostics.Process.Start("iexplore.exe", _url);
                    System.Windows.Forms.Clipboard.SetText(_url);
                }
            }
            catch
            { }
        }

        private async void btnPingPorts_Click(object sender, EventArgs e)
        {
            txtResults2.Text = "";
            string sResult;
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string host = txtEntries.Text;
            dic.Add("467", 467);
            dic.Add("436", 436);
            dic.Add("SMTP", 25);
            dic.Add("SMTP - OUT- SSL", 465);
            dic.Add("SMTP - OUT2- SSL", 587);
            dic.Add("POP3", 995);
            dic.Add("POP3 - 110", 110);
            dic.Add("IMAP", 993);
            dic.Add("IMAP - 143", 143);
            dic.Add("80", 80);
            dic.Add("Default Instance", 1433);
            dic.Add("DAC Default Instance", 1434);
            dic.Add("DTS / SSIS", 3882);
            dic.Add("SSAS", 2393);
            dic.Add("SSAS_", 2394);
            dic.Add("SSAS__", 2725);
            dic.Add("SSAS___", 2383);
            dic.Add("Browser SSAS", 2382);
            dic.Add("RDP", 3389);
            dic.Add("SSL", 443);
            dic.Add("HTTPS endpoint", 443);
            dic.Add("iSCSI", 3260);
            dic.Add("iSCSI_", 860);
            dic.Add("SQL Agent File Copy", 135);
            dic.Add("File copy on UNC shares", 137);
            dic.Add("File copy on UNC shares_", 138);
            dic.Add("File copy on UNC shares__", 139);
            dic.Add("File copy on UNC shares___", 445);
            dic.Add("SQL Debugger", 135);
            dic.Add("Replication", 1433);
            dic.Add("Cluster Service", 3343);
            dic.Add("RPC", 135);
            dic.Add("Filestream", 139);
            dic.Add("Filestream_)", 445);
            dic.Add("SSIS", 135);
            dic.Add("WMI", 135);

            if (chkPingAll.Checked)
            {
                /*for (int i=1; i<= 65535; i++)
                {
                    if (!chkPingAll.Checked) break; //stop

                    sResult = await pingPort(host, i);
                    txtResults2.Text += string.Format("{0} pinging host: {1}:{2} -- {3} -- {4}", sResult, host, i, "", _NEWLINE_);
                    Application.DoEvents();
                }*/
                string rst = "";

                rst += DateTime.Now.ToString("HH-mm") + _NEWLINE_;

                List<Task<string>> tasks = new List<Task<string>>();               
                for (int i = 1; i <= 65535; i++)
                {
                    int port = i;
                    Task<string> t = Task<string>.Factory.StartNew(() => { return pingPort(host, port); });
                    tasks.Add(t);
                }

                //Task.WaitAll(tasks.ToArray());
                var r = Task.WhenAll(tasks.ToArray());

                try {r.Wait();}catch { }

                if (r.Status == TaskStatus.RanToCompletion)
                {
                    
                    //string tmp = "";
                    foreach (var st in tasks)
                    {
                        //tmp = st.Result.Result;
                        //if (tmp.Contains("Errror")) continue;                    
                        rst += st.Result + _NEWLINE_;
                    }
                    
                    rst += DateTime.Now.ToString("HH-mm") + _NEWLINE_;

                    txtResults2.Text += rst;
                    
                }
            }
            else
            {

                foreach (var x in dic)
                {

                    sResult = pingPort(host, x.Value);
                    txtResults2.Text += string.Format("{0} pinging host: {1}:{2} -- {3} -- {4}", sResult, host, x.Value, x.Key, _NEWLINE_);
                    Application.DoEvents();
                }

                // now UDP
                dic.Clear();
                dic.Add("SQL Browser / SQL Server Resolution Protocol", 1434);
                dic.Add("Cluster Admin", 137);
                dic.Add("IPsec traffic", 500);
                dic.Add("IPsec traffic_", 4500);

                foreach (var x in dic)
                {
                    sResult = "";
                    try
                    {
                        using (var client = new UdpClient(host, x.Value))
                        {
                            sResult = "OK [UDP]";
                        }
                    }
                    catch (SocketException)
                    {
                        sResult = "*****Errror [UDP]*";
                    }


                    txtResults2.Text += string.Format("{0} pinging host: {1}:{2} -- {3} -- {4}", sResult, host, x.Value, x.Key, _NEWLINE_);
                    Application.DoEvents();
                }
            }


            txtResults2.Text += "End!";
        }
    
        string pingPort(string host, int port)
        {
            //await Task.Delay(1);
            
            var sResult = "";
            try
            {
                using (var client = new TcpClient(host, port))
                {
                    sResult = string.Format("OK [TCP:{0}]", port);                    
                }
            }
            catch (SocketException)
            {
                sResult = string.Format("*****Errror [TCP:{0}]*", port); //"*****Errror [TCP]*";
            }

            return sResult;
        }

        string msgPort(string host, int port = 467, string msg = "")
        {
            var sResult = "";
            try
            {
                using (var client = new TcpClient(host, port))
                {
                    Byte[] message = System.Text.Encoding.ASCII.GetBytes(msg + "\r");
                    NetworkStream stream = client.GetStream();
                    stream.Write(message, 0, message.Length);
                    stream.Close();
                    client.Close();
                }
            }
            catch (SocketException)
            {
                sResult = "*****Errror [TCP]*";
            }

            return sResult;
        }

        string pingPort(string host, int port, int timeout)
        {
            var client = new TcpClient();
            var result = client.BeginConnect(host, port, null, null);

            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(timeout));

            if (!success)
            {
                //throw new Exception("Failed to connect.");
                return "*****Error [TCP]*";
            }

            // we have connected
            return "OK [TCP]";
        }

        private void btnALT_Click(object sender, EventArgs e)
        {
            txtResults2.Text = "";
            string sResult = "";
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("a", "á, â, ä, à, å");
            dic.Add("e", "é, ê, ë, è");
            dic.Add("i", "í");
            dic.Add("o", "ó, ô, ö, ò");
            dic.Add("u", "ú, û, ù, ü");
            dic.Add("n", "ñ");
            dic.Add("?", "¿, ?");
            dic.Add("!", "!, ¡");

            string lt = txtEntries.Text;

            foreach (var x in dic)
            {
                if (x.Key == lt)
                {
                    sResult = x.Value;
                    break;
                }

                sResult += x.Value + _NEWLINE_;

            }
            txtResults2.Text += sResult == "" ? "Not found" : sResult;
            if (sResult != "")
                System.Windows.Forms.Clipboard.SetText(sResult.Substring(0, 1));
        }

        private void btnAxSpecs_Click(object sender, EventArgs e)
        {
            URL = "http://cavan01asw003p/Validation/Validation Documents/AX Validation/3. Design/Modifications/Specs/";
            string _u1 = "http://cavan01asw003p/Validation/Validation%20Documents/Forms/AllItems.aspx?";
            string url = _u1 + @"RootFolder=%2FValidation%2FValidation%20Documents%2FAX%20Validation%2F3%2E%20Design%2FModifications%2FSpecs&FolderCTID=0x012000284018D5096D0A43B0F579B3DB589340&View={0AA18543-7279-4EEF-B933-5BB697017049}&InitialTabId=Ribbon%2EDocument&VisibilityContext=WSSTabPersistence"
                   , token = "PO-132%20Purchase%20Agreement%20Form%2edocx&p_ID=2354"
                   , url2 = _u1 + "Paged=TRUE&p_SortBehavior=0&p_FileLeafRef="+ token +"&RootFolder=%2fValidation%2fValidation%20Documents%2fAX%20Validation%2f3%2e%20Design%2fModifications%2fSpecs&PageFirstRow=101&&View={0AA18543-7279-4EEF-B933-5BB697017049}&InitialTabId=Ribbon%2EDocument&VisibilityContext=WSSTabPersistence";
            txtResults2.Text = "";
            using (WebClient client = new WebClient())
            {
                client.UseDefaultCredentials = true;
                var result = client.DownloadString(url);
                result = HttpUtility.HtmlDecode(result);
                //var ss = parseSpecs(result);

                txtResults2.Text += parseSpecs(result) + _NEWLINE_;

                result = client.DownloadString(url2);
                result = HttpUtility.HtmlDecode(result);
                //ss = parseSpecs(result);

                txtResults2.Text += parseSpecs(result) + _NEWLINE_;
            }
        }

        string parseSpecs(string str)
        {
            string sOut = "";
            string[] v = str.Split(new string[] { "icon\"><a onfocus=\"OnLink(this)\" href=\"/Validation/Validation Documents/AX Validation/3. Design/Modifications/Specs/" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in v)
            {
                sOut += s.Substring(0, s.IndexOf("\"")) + _NEWLINE_;
            }
            return sOut;



        }

        private void btnDBs_Click(object sender, EventArgs e)
        {

            btnDBs.Enabled = false;
            string[] splt;
            string host;
            string fmt = @"sc \\{0} query {1}";
            string sPinq = "", sServ = "";
            using (PowerShell pw = PowerShell.Create())
            {
                var script = "$username = \"" + _DOM_USER + "\"; " +
                             "$password = \"" + _PASSWORD_ + "\"  | ConvertTo-SecureString -asPlainText -Force; " +
                             "$credential = New-Object System.Management.Automation.PSCredential($username,$password); ";
                script += "Get-ADComputer -Credential $credential -Filter { OperatingSystem -Like \"*server*\"}";


                pw.AddScript(script);

                txtResults2.Text = "";
                txtEntries.Text = "";

                Collection<PSObject> PSOutput = pw.Invoke();

                if (pw.Streams.Error.Count > 0)
                {
                    txtResults2.Text = pw.Streams.Error.ToString();
                }
                foreach (PSObject outputItem in PSOutput)
                {

                    if (outputItem != null)
                    {

                        splt = outputItem.BaseObject.ToString().Split('=');
                        host = splt[1].Split(',')[0];

                        if (txtScannServ.Text != "*")
                        {
                            if (!(host.ToUpper().Contains(txtScannServ.Text.ToUpper()))) continue;
                        }

                        txtEntries.Text += host + _NEWLINE_;
                        Application.DoEvents();

                        sPinq = pingPort(host, 1433, 1);
                        sServ = "";
                        if (!sPinq.Contains("Error"))
                        {
                            sServ = runAsCmd(string.Format(fmt, host, "MSSQLSERVER"));
                        }
                        txtResults2.AppendText(host + " ::SQL:: " + sPinq + sServ + _NEWLINE_); //sqlserver

                        txtResults2.SelectionStart = txtResults2.Text.Length;
                        txtResults2.ScrollToCaret();

                        Application.DoEvents();
                    }
                }

            }

            btnDBs.Enabled = true;

        }

        private void cmbServers_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbServers.Text.StartsWith("PROD"))
            {
                cmbServers.ForeColor = Color.Red;
                txtResults2.BackColor = Color.Red;
                txtResults2.ForeColor = Color.White;

                MessageBox.Show("PROD!!!!!!!!!");
            }
            else
            {
                cmbServers.ForeColor = Color.Black;
                txtResults2.ForeColor = Color.Lime;
                txtResults2.BackColor = ColorTranslator.FromHtml("#646464");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var d = TerminalTools.TermServicesManager.ListSessions(cmbTsServers.Text);

                txtResults2.Text = string.Format("QWINSTA/server:{0}", cmbTsServers.Text);

                txtResults2.Text += "Session ---- Status ---- UserName" + _NEWLINE_;
                Application.DoEvents();
                foreach (var z in d)
                {
                    txtResults2.Text += z.ToString().Replace("#", "%") + _NEWLINE_;
                }

                txtResults2.Text += "" + _NEWLINE_;
                txtResults2.Text += "" + _NEWLINE_;
                txtResults2.Text += "************** RESET ***************" + _NEWLINE_;

                foreach (var z in d)
                {
                    if (z.Info.UserName.ToUpper().Contains("ADMIN") || z.Info.UserName.ToUpper().Contains("MAESTRE"))
                        continue;

                    txtResults2.Text += string.Format("reset session {0} /server:{1}", z.SessionId, cmbTsServers.Text) + _NEWLINE_;
                }


                txtResults2.Text += "" + _NEWLINE_;
                txtResults2.Text += "" + _NEWLINE_;
                txtResults2.Text += "************** NOTIFY ***************" + _NEWLINE_;

                string strMsg = "Esto es un recordatorio que el sistema AX está programado para mantenimiento y tienen que salir de AX en 10 minutos. " + _NEWLINE_ +
                                "Se calcula que el mantenimiento tardara 2 horas, después podrán entrar al sistema y continuar operaciones. Se les mandara un correo cuando el sistema esté disponible. " + _NEWLINE_ +
                                "- - - - - - - - - - - -" + _NEWLINE_ +
                                "This is a reminder that the AX system is scheduled for maintenance and that you must log out of AX within the next 10 minutes. " + _NEWLINE_ +
                                "This work is expected to be completed in 2 hours at which time you may log back into the system and production scanning can resume.An email will be sent when the system is available. ";

                txtResults2.Text += string.Format("msg /server:{0} {1}", cmbTsServers.Text, /*z.Info.UserName*/ "*") + _NEWLINE_;


                txtResults2.Text += "" + _NEWLINE_;
                txtResults2.Text += "" + _NEWLINE_;
                txtResults2.Text += "************** MSG {PROD}***************" + _NEWLINE_;

                txtResults2.Text += strMsg + _NEWLINE_;

                strMsg = "Esto es un recordatorio que el sistema AX *TEST* está programado para mantenimiento y tienen que salir de AX en 10 minutos. ";
                txtResults2.Text += "" + _NEWLINE_;
                txtResults2.Text += "" + _NEWLINE_;
                txtResults2.Text += "************** MSG {TEST}***************" + _NEWLINE_;

                txtResults2.Text += strMsg + _NEWLINE_;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        string _keyName = @"SOFTWARE\StringHandler";
        string _EncKey = "&UH1$W@M";
        const string _PASS_ = "pass";
        private void btnPass_Click(object sender, EventArgs e)
        {

            RegistryKey key = Registry.CurrentUser.OpenSubKey(_keyName, true);

            if (key == null)
                key = Registry.CurrentUser.CreateSubKey(_keyName);

            key.SetValue(_PASS_, Encrypt.EncryptData(txtPassword.Text, _EncKey));
        }

        private string getPassword()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(_keyName);

            string val = key.GetValue(_PASS_).ToString();
            return Encrypt.DecryptData(val, _EncKey);
        }

        private void txtZones_Click(object sender, EventArgs e)
        {
            timeZones_();
        }

        private void btnPingIP_Click(object sender, EventArgs e)
        {
            try
            {
                Ping myPing = new Ping();
                PingReply reply = myPing.Send("vandbw001p", 1000);
                if (reply != null)
                {

                    txtResults2.Text =
                                     "Status :  " + reply.Status + " \n Time : " + reply.RoundtripTime.ToString() + " \n Address : " + reply.Address;
                }
            }
            catch
            {
                Console.WriteLine("ERROR: You have Some TIMEOUT issue");
            }
        }

        private void btnInvert_Click(object sender, EventArgs e)
        {
            try
            {
                string values = getCommaSeparated("|", ""); //use Pipe as wildcard
                string[] vec = values.Split('|');
                string[] vValues;
                string val;
                string spaces = txtEntries.Text.Split('"')[0].ToString();


                txtResults2.Text = "";

                List<string> l = new List<string>();

                for (int i = 0; i < vec.Length; i++)
                {
                    val = vec[i].Replace(",", ""); //clear comma
                    vValues = val.Split(':');

                    val = vValues[1] + ":" + vValues[0]; //swap 

                    if (i + 1 < vec.Length) //the last one do not need comma
                        val += ","; //add comma to end

                    l.Add(val);
                }

                vec = l.ToArray();

                for (int i = 0; i < vec.Length; i++)
                {
                    txtResults2.Text += spaces + vec[i];

                    if (i + 1 < vec.Length)
                        txtResults2.Text += "\r\n"; ;
                }
            }
            catch (Exception ex)
            {
                if (!(ex == null))
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        private string hashIt(string pass, int _iterations)
        {
            string password = pass;
            //STEP 1 Create the salt value with a cryptographic PRNG:
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            //STEP 2 Create the Rfc2898DeriveBytes and get the hash value:
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, _iterations);
            byte[] hash = pbkdf2.GetBytes(20);
            //STEP 3 Combine the salt and password bytes for later use:
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            //STEP 4 Turn the combined salt+hash into a string for storage            
            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            return savedPasswordHash;
        }


        private string verifyHash(string EnteredPass, string savedPasswordHash, int _iterations)
        {
            /* Fetch the stored value */
            //string savedPasswordHash = DBContext.GetUser(u => u.UserName == user).Password;
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(EnteredPass, salt, _iterations);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    throw new UnauthorizedAccessException();

            return EnteredPass;

        }
        private void bthHash_Click(object sender, EventArgs e)
        {
            txtResults2.Text = hashIt(txtPassword.Text, 8500);
        }
        private void btnVerifyHash_Click(object sender, EventArgs e)
        {
            txtResults2.Text += _NEWLINE_ + verifyHash(txtPassword.Text, txtResults2.Text, 8500);
        }

        private void btnCompareDTS_Click(object sender, EventArgs e)
        {
            const string NA_Server = "CAVAN01DBW001P",
                            UK_Server = "taudbw005p",
                            GER_Server = "jetdbw002p",
                            NA_Server_DEV = "CAVAN01BPW001D",
                            NA_Server_TEST = "CAVAN01DBW010D",
                            UK_Server_TEST = "TAUDBW003d",
                            GER_Server_TEST = "jetdbw003d";
            //const string JobPath = @"\BPM\Jobs";
            const string DtsPath = @"\BPM\Prod\Sales\Src\Pkgs";
            const string DtsPathDEV = @"\BPM\Dev\Visual Studio 2005\Projects\BPM\BPMPkg";
            const string nav = @"\\";
            string option, targetA, targetB;
            List<string> resultA, resultB;
            string resultCompare = "";
            string FileNameA, FileNameB;
            bool isDevA = false;
            bool isDevB = false;

            option = txtTargetCompare.Text;
            showTargetCompares();
            switch (option)
            {
                case "0":
                    return;
                case "1":
                    targetA = UK_Server; targetB = NA_Server;
                    break;
                case "3":
                    targetA = UK_Server_TEST; targetB = NA_Server;
                    break;
                case "4":
                    targetA = NA_Server; targetB = UK_Server_TEST;
                    break;
                case "5":
                    isDevA = true;
                    targetA = NA_Server_DEV; targetB = NA_Server;
                    break;
                case "6":
                    isDevB = true;
                    targetA = NA_Server; targetB = NA_Server_DEV;
                    break;
                case "7":
                    targetA = GER_Server; targetB = NA_Server;
                    break;
                default:
                    return;
            }

            txtResults2.Text = "";


            //targetDirectory = nav + UK_Server_TEST + DtsPath;
            resultA = processDir(nav + targetA + (isDevA ? DtsPathDEV : DtsPath));
            resultB = processDir(nav + targetB + (isDevB ? DtsPathDEV : DtsPath));

            bool skipEmptySpaces = chkSkipSpaced.Checked;
            bool isSpacedFileName = false;

            foreach (var s in resultA)
            {
                FileNameA = s.Split(new string[] { " " }, StringSplitOptions.None)[0];
                isSpacedFileName = !FileNameA.Contains(".dtsx");
                if (skipEmptySpaces && isSpacedFileName) continue;

                if (!resultB.Exists(x => x == s))
                {
                    FileNameB = resultB.Find(x => x.Contains(FileNameA));

                    resultCompare += string.Format("{0} *******NOTFOUND ==> {1} {2}", s, FileNameB, _NEWLINE_);
                }
            }


            txtResults2.Text = resultCompare + txtResults2.Text;

        }

        private void showTargetCompares()
        {
            txtEntries.Text = "(1) UK vs NA" + _NEWLINE_ +
                              "(2) UKt vs NAt" + _NEWLINE_ +
                              "(3) UKt vs NA" + _NEWLINE_ +
                              "(4) NA vs UKt" + _NEWLINE_ +
                              "(5) DEV vs NA" + _NEWLINE_ +
                              "(6) NA vs DEV" + _NEWLINE_ +
                              "(7) GER vs NA" + _NEWLINE_
                ;
        }

        private List<string> processDir(string targetDirectory, string wildcard = "ImportAX*")
        {
            txtResults2.Text += "******************************************************************************************" + _NEWLINE_;
            txtResults2.Text += "******************" + targetDirectory + "******************" + _NEWLINE_;
            txtResults2.Text += "******************************************************************************************" + _NEWLINE_;

            List<string> result = new List<string>();
            string[] fileEntries = Directory.GetFiles(targetDirectory, wildcard);
            foreach (string fileName in fileEntries)
            {
                var lastModified = System.IO.File.GetLastWriteTime(fileName).ToString("dd/MM/yy HH:mm:ss");
                string foundFile = string.Format("{0} {1}", fileName.Replace(targetDirectory + @"\", ""), lastModified);
                txtResults2.Text += foundFile + _NEWLINE_;
                result.Add(foundFile);
            }

            return result;
        }

        private void btnAxPaths_Click(object sender, EventArgs e)
        {
            string strDeleteALD_files = "<Lables Files \t\t==>" + _NEWLINE_ + @"CD C:\Program Files\Microsoft Dynamics AX\60\Server\AX_{0}\bin\Application\Appl\Standard\"
                                                                + _NEWLINE_ + "[a: application][l: label][c: cache][d: data][i: index][t: temporary]"
                                                                + _NEWLINE_ + "DEL *.ALD"
                                                                + _NEWLINE_ + "DEL *.ALC"
                                                                + _NEWLINE_ + "DEL *.ALI"
                                                                + _NEWLINE_ + "DEL *.ALT",
            strXppIL = "<delete only files for Full Cyl \t==>" + _NEWLINE_ + @"C:\Program Files\Microsoft Dynamics AX\60\Server\AX_{0}\bin\XppIl",
            strAUC = @"del C:\Users\{1}\AppData\Local\*.AUC",
            strAxAAlog = "<App log \t\t\t==>" + @"C:\Program Files\Microsoft Dynamics AX\60\Server\AX_{0}\Log",
            strSource = "==============AOS SERVER=AX_{0}===================" + _NEWLINE_ +
                                                strDeleteALD_files + _NEWLINE_ +
                                                            strXppIL + _NEWLINE_ +
                                                            strAxAAlog + _NEWLINE_ +
                                         "====================TS=======================" + _NEWLINE_ +
                                                            strAUC + _NEWLINE_,
            strResult0 = string.Format(strSource, "DEV", "*UserName*"),
            strResult1 = string.Format(strSource, "TEST", "*UserName*"),
            strResult2 = string.Format(strSource, "PREPROD", "*UserName*"),
            strResult3 = string.Format(strSource, "PROD", "*UserName*")
            ;

            txtResults2.Text = "" + strResult0 +
                                    strResult1 +
                                    strResult2 +
                                    strResult3;

        }

        System.Net.Sockets.TcpClient clientSocket;
        private void button4_Click(object sender, EventArgs e)
        {
            if (!chkSocket.Checked)
            {

                try
                {
                    msgPort(txtIP.Text, int.Parse(txtPort.Text), txtEntries.Text.Replace("\n", ""));
                }
                catch
                {
                    MessageBox.Show("port not ok");
                }
            }
            else
            {
                /*
                try
                {
                    clientSocket = new System.Net.Sockets.TcpClient();

                    txtResults2.Text = "" + "Client Started" + _NEWLINE_;
                    clientSocket.Connect(txtIP.Text, int.Parse(txtPort.Text));
                    txtResults2.Text += "Client Socket Program - Server[" + txtIP.Text + ":" + txtPort.Text + "] Connected ..." + _NEWLINE_;

                    socketHandling();
                    //Thread t = new Thread(new ThreadStart(socketHandling));
                    //started = true;
                    //t.Start();
                }
                catch(Exception xc)
                {
                    txtResults2.Text += xc.Message + _NEWLINE_;
                }
                */

                ClientSocket cs = new ClientSocket();
                cs.ConnectToServer(txtIP.Text, int.Parse(txtPort.Text));
                cs.OnDataReceived += Cs_OnDataReceived;

            }
        }

        private void Cs_OnDataReceived(byte[] data, int bytesRead)
        {
            string returndata = Encoding.UTF8.GetString(data);
            //txtResults2.Text += returndata + _NEWLINE_;
            AddText(returndata + _NEWLINE_);
        }

        public void AddText(string r)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => this.AddText(r)));
            }
            else
            {
               
                this.txtResults2.Text += r;
                
            }
        }

        //socket
        private void socketHandling()
        {
            try
            {

             NetworkStream serverStream = clientSocket.GetStream();
                //byte[] outStream = System.Text.Encoding.ASCII.GetBytes("" + "$");
                //serverStream.Write(outStream, 0, outStream.Length);
                //serverStream.Flush();
               if(serverStream.CanRead)
                {
                    byte[] bytes = new byte[clientSocket.ReceiveBufferSize];

                    // Read can return anything from 0 to numBytesToRead.
                    // This method blocks until at least one byte is read.
                    serverStream.Read(bytes, 0, (int)clientSocket.ReceiveBufferSize);

                    // Returns the data received from the host to the console.
                    string returndata = Encoding.UTF8.GetString(bytes);
                    txtResults2.Text += returndata + _NEWLINE_;
                }

                serverStream.Close();

                //byte[] inStream = new byte[10025];
                //serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                //string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                //txtResults2.Text += returndata + _NEWLINE_;
            }
            catch(Exception e)
            {
                txtResults2.Text += e.Message + _NEWLINE_;
            }
            

        }
        TcpListener server = null;
        TcpClient client = null;
        private void btnSocketServer_Click(object sender, EventArgs e)
        {

            if (server != null) { client.Close(); server.Stop(); server = null; return; }
           

            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = int.Parse(txtPort.Text);
                IPAddress localAddr = IPAddress.Parse(txtIP.Text);

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    txtResults2.Text = "Waiting for a connection... " + _NEWLINE_;
                    Application.DoEvents();
                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    client = server.AcceptTcpClient();
                    txtResults2.Text = "Connected " + _NEWLINE_;

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    /* int i;
                     // Loop to receive all the data sent by the client.
                     while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                     {
                         // Translate data bytes to a ASCII string.
                         data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                         txtResults2.Text = string.Format("Received: {0}", data) + _NEWLINE_;  

                         // Process the data sent by the client.
                         data = data.ToUpper();

                         byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                         // Send back a response.
                         stream.Write(msg, 0, msg.Length);
                         txtResults2.Text = string.Format("Sent: xxx{0}", data) + _NEWLINE_;

                     }
                     */
                    //
                    string msg = "";
                    while (true)
                    {
                        Application.DoEvents();
                        if (msg == txtEntries.Text) continue;
                        msg = txtEntries.Text;
                        if (msg == "") continue;

                        byte[] bytesm = System.Text.Encoding.ASCII.GetBytes(msg);
                        stream.Write(bytesm, 0, bytesm.Length);
                        SETtxtResults2(string.Format("Sent: {0}", msg) + _NEWLINE_);
                        Application.DoEvents();
                        Thread.Sleep(100);
                    }
                    // Shutdown and end connection
                    //client.Close();
                }
            }
            catch (SocketException ex)
            {
                txtResults2.Text = string.Format("SocketException: {0}", ex) + _NEWLINE_;
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        }

        public void SETtxtResults2(string r)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => this.AddText(r)));
            }
            else
            {
                this.txtResults2.Text += r;
            }
        }

        //listen port
        private void cmdListen_Click(object sender, EventArgs e)
        {
            if (chkSocket.Checked) { socketHandling(); return; }

            txtResults2.Text = "";
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = int.Parse(txtPort.Text);
                IPAddress localAddr = IPAddress.Parse(txtIP.Text);

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    
                    txtResults2.Text += "Waiting for a connection... " + _NEWLINE_;

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    txtResults2.Text += "Connected!" + _NEWLINE_;
                    

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;
                    Application.DoEvents();
                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        txtResults2.Text += string.Format("Received: {0}", data) + _NEWLINE_;
                        Application.DoEvents();

                        /*
                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        txtResults2.Text += string.Format("Sent: {0}", data) + _NEWLINE_;
                        */
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException ex)
            {
                txtResults2.Text += string.Format("SocketException: {0}", ex) + _NEWLINE_;
                
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            //Console.WriteLine("\nHit enter to continue...");
            //Console.Read();

        }

        private void cmdListenCOM_Click(object sender, EventArgs e)
        {
            txtResults2.Text = "";
            SerialPort mySerialPort = new SerialPort(txtCom.Text);

            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.XOnXOff;
            mySerialPort.RtsEnable = true;
            mySerialPort.DtrEnable = true;

            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(MySerialPort_DataReceived);


            mySerialPort.Open();


            //mySerialPort.Close();

        }

        delegate void SetTextCallback(string text);
        private void MySerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();

            SetTextCallback d = new SetTextCallback(SetText);
            this.Invoke(d, new object[] { indata });
            //txtResults2.Text += indata + _NEWLINE_;
        }        
        private void SetText(string text)
        {
            txtResults2.Text += text + _NEWLINE_;

            if (!chkShowUnicode.Checked) return;
            //Unicode representation
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            txtResults2.Text +=  BitConverter.ToString(bytes) + _NEWLINE_;
        }

        private void btnFormatPair_Click(object sender, EventArgs e)
        {
            try
            {
                string values = txtEntries.Text;
                string[] vec = values.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                txtResults2.Text = "";
                int maxLength = 0;
                foreach (var s in vec)
                {
                    if ((s.Length) > maxLength)
                        maxLength = s.Length;

                }
                foreach (var s in vec)
                {
                    string spaces = new string(' ', maxLength + 1 - s.Length);

                    if (s == "NAME:")
                    {
                        txtResults2.Text += _NEWLINE_;
                        spaces = "";
                    }
                    else if (s == "VALUE:") spaces = "";
                    else if (s == "TYPE:") spaces = "";


                    txtResults2.Text += s + spaces;
                }
            }
            catch (Exception xx)
            {
                MessageBox.Show(xx.Message);
            }
        }

        /// <summary>
        /// just detect if unicodes in a string.
        /// https://en.wikipedia.org/wiki/List_of_Unicode_characters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnicode_Click(object sender, EventArgs e)
        {
            var entry = txtEntries.Text;
            var entry2 = entry;
            byte[] bytes = Encoding.UTF8.GetBytes(entry);
            txtResults2.Text = BitConverter.ToString(bytes);


            
            txtResults2.Text += _NEWLINE_;
            txtResults2.Text += "//To Literal;";
            txtResults2.Text += _NEWLINE_;
            txtResults2.Text += Program.ToLiteral(entry);

            txtResults2.Text += _NEWLINE_;
            //remove when 
            // Code     Decimal Description             Abbreviation
            // U+0002	2	    Start of Text	        STX
            // U+0003	3	    End-of-text character	ETX
            // U+0011  17       Device Control 1        DC1
            txtResults2.Text += "//remove STX && ETX";
            txtResults2.Text += _NEWLINE_;
            bytes = bytes.Where(x => x != 2 && x != 3 && x != 17).ToArray();
            txtResults2.Text += BitConverter.ToString(bytes);

            txtResults2.Text += _NEWLINE_;
            //covert bytes back to string
            txtResults2.Text += "//covert bytes back to string";
            txtResults2.Text += _NEWLINE_;
            txtResults2.Text += System.Text.Encoding.Default.GetString(bytes);

            txtResults2.Text += "//To Literal;";
            txtResults2.Text += _NEWLINE_;
            txtResults2.Text += Program.ToLiteral(System.Text.Encoding.Default.GetString(bytes));

            txtResults2.Text += _NEWLINE_;
            //special ones?
            //clear them
            txtResults2.Text += "//special ones? clear them";
            txtResults2.Text += _NEWLINE_;
            txtResults2.Text +=  entry2.Replace("\u0002", "").Replace("\u0003", "").Replace("\u0011", "");
        }

        private void chkSocket_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSocket.Checked)
            {
                button4.Text = "Conn Cln";
                cmdListen.Text = "Get Strm";
                btnSocketServer.Enabled = true;
            }
            else
            {
                button4.Text = "Send=>";
                cmdListen.Text = "<=Listen";
                btnSocketServer.Enabled = false;
            }
        }

        private void btnTicket_Click(object sender, EventArgs e)
        {
            try
            {
            
                string _url = @"http://helpdesk/ticket.php?ticket=";
                if (txtResults2.SelectedText != "")
                    System.Diagnostics.Process.Start("chrome.exe", _url + txtResults2.SelectedText.Trim());                
                
            }
            catch
            { }
        }

    }

    public class ExThread
    {
        // Static method for thread a 
        public static async void thread1()
        {
            while (1==1)
            {
                try
                {              
                    await Task.Delay(60000);
                    var path = Application.StartupPath + @"\";
                    var g = System.IO.File.ReadAllText(path + "clock" + ".txt");
                    System.IO.File.WriteAllText(path + "clock" + ".txt", g + DateTime.Now.ToLongDateString());
                }
                catch { }
            }
        }

      
    }

}