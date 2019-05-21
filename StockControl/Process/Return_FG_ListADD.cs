﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using Telerik.WinControls.UI;

namespace StockControl
{
    public partial class Return_FG_ListADD : Telerik.WinControls.UI.RadRibbonForm
    {
        public Return_FG_ListADD()
        {
            InitializeComponent();
        }
       
        public Return_FG_ListADD(Telerik.WinControls.UI.RadTextBox DocNo_tt2
                    , Telerik.WinControls.UI.RadTextBox Refid_tt2
            , Telerik.WinControls.UI.RadTextBox CodeNo_tt2
            , Telerik.WinControls.UI.RadTextBox ItemNo_tt2
            , Telerik.WinControls.UI.RadTextBox ItemDesc_tt2
            , Telerik.WinControls.UI.RadTextBox Qty_tt2
            , Telerik.WinControls.UI.RadTextBox Unit_tt2
            , Telerik.WinControls.UI.RadTextBox UnitCost_tt2

            )
        {
            InitializeComponent();
            DocNo_tt = DocNo_tt2;
            CodeNo_tt = CodeNo_tt2;
            Refid_tt = Refid_tt2;
            ItemNo_tt = ItemNo_tt2;
            ItemDesc_tt = ItemDesc_tt2;
            Qty_tt = Qty_tt2;
            Unit_tt = Unit_tt2;
            UnitCost_tt = UnitCost_tt2;

            screen = 1;

        }
        Telerik.WinControls.UI.RadTextBox DocNo_tt = new Telerik.WinControls.UI.RadTextBox();
        Telerik.WinControls.UI.RadTextBox CodeNo_tt = new Telerik.WinControls.UI.RadTextBox();
        Telerik.WinControls.UI.RadTextBox Refid_tt = new Telerik.WinControls.UI.RadTextBox();
        Telerik.WinControls.UI.RadTextBox ItemNo_tt = new Telerik.WinControls.UI.RadTextBox();
        Telerik.WinControls.UI.RadTextBox ItemDesc_tt = new Telerik.WinControls.UI.RadTextBox();
        Telerik.WinControls.UI.RadTextBox Qty_tt = new Telerik.WinControls.UI.RadTextBox();
        Telerik.WinControls.UI.RadTextBox Unit_tt = new Telerik.WinControls.UI.RadTextBox();
        Telerik.WinControls.UI.RadTextBox UnitCost_tt = new Telerik.WinControls.UI.RadTextBox();

        int screen = 0;
        DataTable dt = new DataTable();
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HistoryView hw = new HistoryView(this.Name);
            this.Cursor = Cursors.Default;
            hw.ShowDialog();
        }

        private void radRibbonBar1_Click(object sender, EventArgs e)
        {

        }
        private void GETDTRow()
        {

            //dt.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            //dt.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
            //dt.Columns.Add(new DataColumn("Order", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("BackOrder", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("StockQty", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("UnitBuy", typeof(string)));
            //dt.Columns.Add(new DataColumn("PCSUnit", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("LeadTime", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("MaxStock", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("MinStock", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("VendorNo", typeof(string)));
            //dt.Columns.Add(new DataColumn("VendorName", typeof(string)));



        }
  
        private void Unit_Load(object sender, EventArgs e)
        {

            //radGridView1.ReadOnly = true;
            dgvData.AutoGenerateColumns = false;
            //GETDTRow();
            // DefaultItem();
            dtDate1.Value = DateTime.Now;
            dtDate2.Value = DateTime.Now;

            DataLoad();

         
        }
        private void DefaultItem()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //cboVendor.AutoCompleteMode = AutoCompleteMode.Append;
                //cboVendor.DisplayMember = "VendorName";
                //cboVendor.ValueMember = "VendorNo";
                //cboVendor.DataSource =(from ix in db.tb_Vendors.Where(s => s.Active == true) select new { ix.VendorNo,ix.VendorName}).ToList();
                //cboVendor.SelectedIndex = -1;

                try
                {

               

                    //GridViewMultiComboBoxColumn col = (GridViewMultiComboBoxColumn)radGridView1.Columns["CodeNo"];
                    //col.DataSource = (from ix in db.tb_Items.Where(s => s.Status.Equals("Active")) select new { ix.CodeNo, ix.ItemDescription }).ToList();
                    //col.DisplayMember = "CodeNo";
                    //col.ValueMember = "CodeNo";

                    //col.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDown;
                    //col.FilteringMode = GridViewFilteringMode.DisplayMember;

                    // col.AutoSizeMode = BestFitColumnMode.DisplayedDataCells;
                }
                catch { }

                //col.TextAlignment = ContentAlignment.MiddleCenter;
                //col.Name = "CodeNo";
                //this.radGridView1.Columns.Add(col);

                //this.radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                //this.radGridView1.CellEditorInitialized += radGridView1_CellEditorInitialized;
            }
        }
        private void DataLoad()
        {
            dgvData.Rows.Clear();
            
            try
            {
                DateTime inclusiveStart = dtDate1.Value.Date;
                // Include the *whole* of the day indicated by searchEndDate
                DateTime exclusiveEnd = dtDate2.Value.Date.AddDays(1);

                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                     var g = (from i in db.tb_Sell_FG_Details
                               join s in db.tb_Sell_FGs on i.DocNo equals s.DocNo
                              join t in db.tb_Items on i.CodeNo equals t.CodeNo

                              where i.Status != "Cancel"                                  
                                  && i.DocNo.Contains(txtDocNo.Text.Trim())
                                  && s.CSTM_Name.Contains(txtCSTM_Name.Text.Trim())
                                  && i.CodeNo.Contains(txtCodeNo.Text.Trim())
                                   && (((s.DocNoDate >= inclusiveStart
                                   && s.DocNoDate < exclusiveEnd)
                                   && cbDate.Checked == true)
                                    || (cbDate.Checked == false))
                              select new
                              {
                                  DocNo = i.DocNo,
                                  CodeNo = i.CodeNo,
                                  ItemNo = t.ItemNo,
                                  ItemDesc = t.ItemDescription,
                                  id = i.id,
                                  CSTM_Name = s.CSTM_Name,
                                  CSTM_Address = s.CSTM_Address,                                  
                                  Qty = i.Qty,                                 
                                  Unit = t.UnitShip,
                                  Remark = i.Remark,
                                  Status = i.Status,
                                  DocBy = s.DocBy,
                                  DocNoDate = s.DocNoDate,
                                  UnitCost = i.UnitCost,
                                  Amount = i.Amount

                              }
                            ).ToList();
                    dgvData.DataSource = g;

                    int rowcount = 0;
                    foreach (var x in dgvData.Rows)
                    {
                        rowcount += 1;
                        x.Cells["dgvNo"].Value = rowcount;
                    }
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;


            //    radGridView1.DataSource = dt;
        }
      
        private void btnCancel_Click(object sender, EventArgs e)
        {
          
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            return;
            dgvData.ReadOnly = false;
            dgvData.AllowAddNewRow = false;
            dgvData.Rows.AddNew();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            dgvData.ReadOnly = false;
           // btnEdit.Enabled = false;
            btnPrint.Enabled = true;
            dgvData.AllowAddNewRow = false;
            //DataLoad();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvData.Rows.Count <= 0)
                    return;

                if (screen.Equals(1))
                {
                    //if (!Convert.ToString(dgvData.CurrentRow.Cells["DocNo"].Value).Equals(""))
                    //{
                    DocNo_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["DocNo"].Value);
                    CodeNo_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["CodeNo"].Value);
                    Refid_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["id"].Value);
                    ItemNo_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["ItemNo"].Value);
                    ItemDesc_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["ItemDesc"].Value);
                    Qty_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["Qty"].Value);
                    Unit_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["Unit"].Value);
                    UnitCost_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["UnitCost"].Value);

                    //SHNo_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["SHNo"].Value);
                    this.Close();
                    //}
                    //else
                    //{
                    //    SHNo_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["SHNo"].Value);
                    //    CodeNo_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["CodeNo"].Value);
                    //    this.Close();
                    //}
                }
                //else
                //{
                //    if (dgvData.Rows.Count > 0)
                //    {

                //        Shipping_FG a = new Shipping_FG(Convert.ToString(dgvData.CurrentRow.Cells["SHNo"].Value)
                //            //Convert.ToString(dgvData.CurrentRow.Cells["CodeNo"].Value)
                //            );
                //        a.ShowDialog();
                //       // this.Close();
                //    }
                //    else
                //    {
                //        Shipping_FG a = new Shipping_FG();
                //        a.ShowDialog();                       
                //    }
                //}

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {

        }

        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());
        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
           
        }

      

        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //dbClss.ExportGridCSV(radGridView1);
            dbClss.ExportGridXlSX(dgvData);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                return;
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "Spread Sheet files (*.csv)|*.csv|All files (*.csv)|*.csv";
                if (op.ShowDialog() == DialogResult.OK)
                {

                    using (TextFieldParser parser = new TextFieldParser(op.FileName, Encoding.GetEncoding("windows-874")))
                    //using (TextFieldParser parser = new TextFieldParser(op.FileName))
                    {
                        dt.Rows.Clear();
                        DateTime? d = null;
                        DateTime d1 = DateTime.Now;
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        int a = 0;
                        int c = 0;
                        while (!parser.EndOfData)
                        {
                            //Processing row
                            a += 1;
                            DataRow rd = dt.NewRow();
                            // MessageBox.Show(a.ToString());
                            string[] fields = parser.ReadFields();
                            c = 0;
                            foreach (string field in fields)
                            {
                                c += 1;
                                //TODO: Process field

                                if (a > 1)
                                {
                                    if (c == 1)
                                        rd["ModelName"] = Convert.ToString(field).Trim();
                                    else if (c == 2)
                                        rd["ModelDescription"] = Convert.ToString(field);
                                    else if (c == 3)
                                        rd["ModelActive"] = Convert.ToBoolean(field);
                                    else if (c == 4)
                                        rd["LineName"] = Convert.ToString(field).Trim();
                                    else if (c == 5)
                                        rd["MCName"] = Convert.ToString(field);
                                    else if (c == 6)
                                        rd["Limit"] = Convert.ToBoolean(field);
                                    else if (c == 7)
                                    {
                                        if (DateTime.TryParse(Convert.ToString(field), out d1))
                                        {
                                            rd["ExpireDate"] = Convert.ToDateTime(field);

                                        }
                                        else
                                        {
                                            rd["ExpireDate"] = d;
                                        }
                                    }

                                }
                                else
                                {
                                    if (c == 1)
                                        rd["ModelName"] = "";
                                    else if (c == 2)
                                        rd["ModelDescription"] = "";
                                    else if (c == 3)
                                        rd["ModelActive"] = false;
                                    else if (c == 4)
                                        rd["LineName"] = "";
                                    else if (c == 5)
                                        rd["MCName"] = "";
                                    else if (c == 6)
                                        rd["Limit"] = false;
                                    else if (c == 7)
                                        rd["ExpireDate"] = d;




                                }


                            }
                            dt.Rows.Add(rd);

                        }
                    }
                    if (dt.Rows.Count > 0)
                    {

                        dbClss.AddHistory(this.Name, "Import", "Import file CSV in to System", "");
                        ImportData();
                        MessageBox.Show("Import Completed.");

                        DataLoad();
                    }

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dt.Rows.Clear(); }
        }

        private void ImportData()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    foreach (DataRow rd in dt.Rows)
                    {
                        if (!rd["ModelName"].ToString().Equals(""))
                        {


                            var x = (from ix in db.tb_Models where ix.ModelName == rd["ModelName"].ToString().Trim() select ix).FirstOrDefault();


                            DateTime? d = null;
                            DateTime d1 = DateTime.Now;
                            if (x == null)
                            {
                                tb_Model u = new tb_Model();
                                u.ModelName = rd["ModelName"].ToString().Trim();
                                u.ModelDescription = rd["ModelDescription"].ToString().Trim();
                                u.ModelActive = Convert.ToBoolean(rd["ModelActive"].ToString());
                                u.LineName = rd["LineName"].ToString().Trim();
                                u.MCName = rd["MCName"].ToString().Trim();
                                u.Limit = Convert.ToBoolean(rd["Limit"].ToString());
                                if (DateTime.TryParse(rd["ExpireDate"].ToString(), out d1))
                                {

                                    u.ExpireDate = Convert.ToDateTime(rd["ExpireDate"].ToString());
                                }
                                else
                                {
                                    u.ExpireDate = d;
                                }
                                db.tb_Models.InsertOnSubmit(u);
                                db.SubmitChanges();
                            }
                            else
                            {
                                x.ModelName = rd["ModelName"].ToString().Trim();
                                x.ModelDescription = rd["ModelDescription"].ToString().Trim();
                                x.ModelActive = Convert.ToBoolean(rd["ModelActive"].ToString());
                                x.LineName = rd["LineName"].ToString().Trim();
                                x.MCName = rd["MCName"].ToString().Trim();
                                x.Limit = Convert.ToBoolean(rd["Limit"].ToString());
                                if (DateTime.TryParse(rd["ExpireDate"].ToString(), out d1))
                                {

                                    x.ExpireDate = Convert.ToDateTime(rd["ExpireDate"].ToString());
                                }
                                else
                                {
                                    x.ExpireDate = d;
                                }


                                db.SubmitChanges();

                            }



                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("InportData", ex.Message, this.Name);
            }
        }

        private void btnFilter1_Click(object sender, EventArgs e)
        {
            dgvData.EnableFiltering = true;
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
            dgvData.EnableFiltering = false;
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void radGridView1_Click(object sender, EventArgs e)
        {

        }

        private void chkActive_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {

        }

        private void radButton1_Click_1(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void radGridView1_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            //if (e.CellElement.ColumnInfo.Name == "ModelName")
            //{
            //    if (e.CellElement.RowInfo.Cells["ModelName"].Value != null)
            //    {
            //        if (!e.CellElement.RowInfo.Cells["ModelName"].Value.Equals(""))
            //        {
            //            e.CellElement.DrawFill = true;
            //            // e.CellElement.ForeColor = Color.Blue;
            //            e.CellElement.NumberOfColors = 1;
            //            e.CellElement.BackColor = Color.WhiteSmoke;
            //        }

            //    }
            //}
        }

        private void txtModelName_TextChanged(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void cboModelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (crow == 0)
            //    DataLoad();
        }

        private void cboYear_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
          
        }

        private void radCheckBox1_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            //if(radCheckBox1.Checked)
            //{
            //    foreach(var rd in radGridView1.Rows)
            //    {
            //        rd.Cells["S"].Value = true;
            //    }
            //}else
            //{
            //    foreach (var rd in radGridView1.Rows)
            //    {
            //        rd.Cells["S"].Value = false;
            //    }
            //}
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string SHNo = "";
                if (dgvData.Rows.Count > 0)
                    SHNo = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["ShippingNo"].Value);

                PrintPR a = new PrintPR(SHNo, SHNo, "Shipping");
                a.ShowDialog();
            }
            catch { }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void MasterTemplate_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if(e.RowIndex>0)
                btnSave_Click(null, null);
            //try
            //{
            //    if (screen.Equals(1))
            //    {
            //        if (!Convert.ToString(e.Row.Cells["SHNo"].Value).Equals(""))
            //        {
            //            SHNo_tt.Text = Convert.ToString(e.Row.Cells["SHNo"].Value);
            //            this.Close();
            //        }
            //        else
            //        {
            //            SHNo_tt.Text = Convert.ToString(e.Row.Cells["SHNo"].Value);
            //            CodeNo_tt.Text = Convert.ToString(e.Row.Cells["CodeNo"].Value);
            //            this.Close();
            //        }
            //    }
            //    else
            //    {
            //        Shipping_FG a = new Shipping_FG(Convert.ToString(e.Row.Cells["SHNo"].Value)
            //           // ,Convert.ToString(e.Row.Cells["CodeNo"].Value)
            //            );
            //        a.ShowDialog();
            //        //this.Close();
            //    }

            //}
            //catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void frezzRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvData.Rows.Count > 0)
                {

                    int Row = 0;
                    Row = dgvData.CurrentRow.Index;
                    dbClss.Set_Freeze_Row(dgvData, Row);


                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void frezzColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvData.Columns.Count > 0)
                {

                    int Col = 0;
                    Col = dgvData.CurrentColumn.Index;
                    dbClss.Set_Freeze_Column(dgvData, Col);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void unFrezzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                dbClss.Set_Freeze_UnColumn(dgvData);
                dbClss.Set_Freeze_UnRows(dgvData);


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
