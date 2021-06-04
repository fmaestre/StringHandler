using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace String_Handling
{
    public partial class frmStringsHandling : Form
    {
        public frmStringsHandling()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbLis.Text = "'";
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            string strCadena, strParse="",strResult="";
            int i,j;
            j =0;

            try
            {
                if (txtEntries.Text != "")
                {
                    strCadena = this.txtEntries.Text;
                    for (i = 0; i <= strCadena.Length-1; i++)
                    {
                        //j = strCadena.IndexOf('\r');
                        if (strCadena.Substring(i, 1).ToString() == '\r'.ToString())
                        {
                            if (strParse.Trim() != "")
                            {
                                strResult = (strResult + cmbLis.Text + strParse.Trim() + cmbLis.Text).Trim() + ",";
                            }
                            strParse = "";
                            j = 1;
                        }
                        else
                        {
                            if (strCadena.Substring(i, 1).ToString() != '\n'.ToString()) 
                            {
                               strParse = strParse + strCadena.Substring(i,1);
                            }

                            j= 0;
                        }
                    } //end for

                    if ((j == 0) & (strParse != '\n'.ToString()) & (strParse != ""))
                    {
                        strResult = (strResult + cmbLis.Text + strParse.Trim() + cmbLis.Text).Trim();
                     }
                     if (strResult.Substring(strResult.Length -1 ,1).ToString() == ",".ToString()) {
                        strResult = strResult.Substring(0,strResult.Length-1);
                     }
                     
                    txtResults.Text = "IN (" + strResult + ")";

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
                    MessageBox.Show(ex.ToString());
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
                                strResult = (strResult + " Select " + cmbLis.Text + strParse.Trim() + cmbLis.Text).Trim() + " From Dual Union ";
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

                    txtResults.Text = "( " + strResult + " )" ;

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
                    MessageBox.Show(ex.ToString());
                }

            }
        }

        
    }
}