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
    public partial class ReturnReceiveList_Del : Telerik.WinControls.UI.RadRibbonForm
    {
        public ReturnReceiveList_Del()
        {
            InitializeComponent();
        }
        Telerik.WinControls.UI.RadTextBox RCNo_tt = new Telerik.WinControls.UI.RadTextBox();
        Telerik.WinControls.UI.RadTextBox PRNo_tt = new Telerik.WinControls.UI.RadTextBox();
        int screen = 0;
        public ReturnReceiveList_Del(Telerik.WinControls.UI.RadTextBox RCNoxxx
                    , Telerik.WinControls.UI.RadTextBox PRNoxxx
                )
        {
            InitializeComponent();
            RCNo_tt = RCNoxxx;
            PRNo_tt = PRNoxxx;
            screen = 1;
        }

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
            dtDate1.Value = DateTime.Now;
            dtDate2.Value = DateTime.Now;
            cboStatus.Text = "ทั้งหมด";
            dgvData.AutoGenerateColumns = false;
            GETDTRow();
            DefaultItem();
            //dgvData.ReadOnly = false;
            DataLoad();
            //txtVendorNo.Text = "";
            
        }
        private void DefaultItem()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                cboVendorName.AutoCompleteMode = AutoCompleteMode.Append;
                cboVendorName.DisplayMember = "VendorName";
                cboVendorName.ValueMember = "VendorNo";
                cboVendorName.DataSource =(from ix in db.tb_Vendors.Where(s => s.Active == true) select new { ix.VendorNo,ix.VendorName}).ToList();
                cboVendorName.SelectedIndex = -1;
                cboVendorName.SelectedValue = "";
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
        private void Load_WaitingReceive()  //รอรับเข้า (รอ Receive)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                string VendorNo_ss = "";
                if (!cboVendorName.Text.Equals(""))
                    VendorNo_ss = txtVendorNo.Text;

                int dgvNo = 0;
                bool S = false; 
                //string RCNo = "";
                //string PRNo = "";
                //string CodeNo = "";
                //string ItemName = "";
                //string ItemNo = "";
                //string ItemDescription = "";
                //DateTime? DeliveryDate = null;
                //decimal QTY = 0;
                //decimal BackOrder = 0;
                //decimal RemainQty = 0;
                //string Unit = "";
                //decimal PCSUnit = 0;
                //decimal Leadtime = 0;
                //decimal MaxStock = 0;
                //decimal MinStock = 0;
                //string VendorNo = "";
                //string VendorName = "";
                //DateTime? CreateDate = null;
                //string CreateBy = "";
                //string Status = "รอรับเข้า";


                //var g = (from ix in db.tb_PurchaseRequests select ix).Where(a => a.VendorNo.Contains(VendorNo_ss)
                //    //&& a.Status != "Cancel"
                //    && a.Status == "Waiting"
                //    )
                //    .ToList();
                //if (g.Count() > 0)
                //{

                    var r = (from h in db.tb_PurchaseRequests
                             join d in db.tb_PurchaseRequestLines on h.PRNo equals d.PRNo
                             join i in db.tb_Items on d.CodeNo equals i.CodeNo

                             where //h.Status == "Waiting" //&& d.verticalID == VerticalID
                                Convert.ToDecimal(d.OrderQty ) == Convert.ToDecimal(d.RemainQty)
                                && h.VendorNo.Contains(VendorNo_ss)
                                && d.SS == 1
                             select new
                             {
                                 CodeNo = d.CodeNo,
                                 S = false,
                                 ItemNo = d.ItemName,
                                 ItemDescription = d.ItemDesc,
                                 RCNo = "",
                                 PRNo = d.PRNo,
                                 DeliveryDate = d.DeliveryDate,
                                 QTY = d.OrderQty,
                                 BackOrder = d.RemainQty,
                                 RemainQty = d.RemainQty,
                                 Unit = d.UnitCode,
                                 PCSUnit = d.PCSUnit,
                                 MaxStock = i.MaximumStock,
                                 MinStock = i.MinimumStock,
                                 VendorNo = h.VendorNo,
                                 VendorName = h.VendorName,
                                 CreateBy = h.CreateBy,
                                 CreateDate = h.CreateDate,
                                 Status = "รอรับเข้า"
                             }
               ).ToList();
                    if (r.Count > 0)
                    {
                        dgvNo = dgvData.Rows.Count() + 1;

                        foreach (var vv in r)
                        {
                            dgvData.Rows.Add(dgvNo.ToString(), S, vv.RCNo, vv.PRNo, vv.CodeNo, vv.ItemNo, vv.ItemDescription
                                        , vv.DeliveryDate, vv.QTY, vv.BackOrder, vv.RemainQty, vv.Unit, vv.PCSUnit, vv.MaxStock,
                                        vv.MinStock, vv.VendorNo, vv.VendorName, vv.CreateBy, vv.CreateDate, vv.Status
                                        );
                        }

                    }
                    //var gg = (from ix in db.tb_PurchaseRequestLines select ix)
                    //    .Where(a => a.SS.Equals(true) && (a.PRNo==(StockControl.dbClss.TSt(g.FirstOrDefault().PRNo)))
                    //   && a.OrderQty == a.RemainQty
                    //   && a.OrderQty >0
                    //).ToList();
                    //if (gg.Count() > 0)
                    //{
                    //    foreach (var vv in gg)
                    //    {
                    //        if (!StockControl.dbClss.TSt(vv.DeliveryDate).Equals(""))
                    //            DeliveryDate = Convert.ToDateTime(vv.DeliveryDate);

                    //        decimal.TryParse(StockControl.dbClss.TSt(vv.OrderQty), out QTY);
                    //        decimal.TryParse(StockControl.dbClss.TSt(vv.RemainQty), out BackOrder);
                    //        decimal.TryParse(StockControl.dbClss.TSt(vv.RemainQty), out RemainQty);

                    //        dgvNo = dgvData.Rows.Count() + 1;
                    //        dgvData.Rows.Add(dgvNo.ToString(), S, RCNo,vv.PRNo,vv.CodeNo,vv.ItemName,vv.ItemDesc
                    //            , DeliveryDate, QTY, BackOrder, RemainQty);
                    //    }
                    //}
                //}
            }
        }
        private void Load_Data()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                string VendorNo_ss = "";
                if (!cboVendorName.Text.Equals(""))
                    VendorNo_ss = txtVendorNo.Text;

                int dgvNo = 0;
                bool S = false;
                string RCNo = "";
             
                DateTime inclusiveStart = dtDate1.Value.Date;
                // Include the *whole* of the day indicated by searchEndDate
                DateTime exclusiveEnd = dtDate2.Value.Date.AddDays(1);
                DateTime? DeliveryDate = null;

                string status = cboStatus.Text;
                if (status == "ทั้งหมด")
                    status = "";
                else if (status == "รับเข้าบางส่วน")
                    status = "Partial";
                else if (status == "รับเข้าแล้ว")
                    status = "Completed";
                else
                    status = "";

                
                var r = (from d in db.tb_Receive_Dels
                         join c in db.tb_ReceiveH_Dels on d.RCNo equals c.RCNo
                         // join p in db.tb_PurchaseRequestLines on d.PRID equals p.id
                         join i in db.tb_Items on d.CodeNo equals i.CodeNo

                         where c.VendorNo.Contains(VendorNo_ss)
                             && (((c.RCDate >= inclusiveStart
                                   && c.RCDate < exclusiveEnd)
                                   && cbDate.Checked == true)
                         || (cbDate.Checked == false))
                             //&& (c.RCDate >= inclusiveStart
                             //           && c.RCDate < exclusiveEnd)
                             && d.Status.Contains(status)
                             //&& p.SS == 1
                             && d.PRNo.Trim().ToUpper().Contains(txtPR_PO.Text.Trim().ToUpper())
                             && d.TypeReceive.Trim().ToUpper().Contains(ddlTypeReceive.Text)
                         select new
                         {
                            
                             CodeNo = d.CodeNo,
                             S = false,
                             ItemNo = d.ItemNo,
                             ItemDescription = d.ItemDescription,
                             RCNo = d.RCNo,
                             PRNo = d.PRNo,
                             DeliveryDate = DeliveryDate, //p.DeliveryDate,
                             QTY = d.QTY,
                             BackOrder = d.RemainQty,
                             RemainQty = d.RemainQty,
                             Unit = d.Unit,
                             PCSUnit = d.PCSUnit,
                             MaxStock = i.MaximumStock
                             , MinStock = i.MinimumStock
                            , VendorNo = c.VendorNo
                            , VendorName = c.VendorName
                            , CreateBy = d.CreateBy
                            , CreateDate = d.RCDate
                            , Status =  d.Status
                            , InvNo = c.InvoiceNo
                            , SerialNo = d.SerialNo
                            , LotNo = d.LotNo
                            , ShelfNo = d.ShelfNo
                            , TypeReceive = d.TypeReceive
                         }
                ).ToList();
                //dgvData.DataSource = StockControl.dbClss.LINQToDataTable(r);
                if(r.Count > 0)
                {
                    dgvNo = dgvData.Rows.Count() + 1;
                    string status_temp = "";
                    foreach (var vv in r)
                    {
                        status_temp = vv.Status;
                        if (vv.Status == "Partial")
                            status_temp = "รับเข้าบางส่วน";
                        else
                            status_temp = "รับเข้าแล้ว";

                        dgvData.Rows.Add(dgvNo.ToString(), S, vv.RCNo, vv.PRNo, vv.InvNo ,vv.CodeNo, vv.ItemNo, vv.ItemDescription
                                    ,vv.DeliveryDate, vv.QTY, vv.BackOrder, vv.RemainQty,vv.Unit,vv.PCSUnit,vv.MaxStock,
                                    vv.MinStock,vv.VendorNo,vv.VendorName,vv.LotNo,vv.SerialNo,vv.ShelfNo,vv.CreateBy,vv.CreateDate, status_temp, vv.TypeReceive
                                    );
                    }

                }

                //int rowcount = 0;
                //foreach (var x in dgvData.Rows)
                //{
                //    rowcount += 1;
                //    x.Cells["dgvNo"].Value = rowcount;
                //}


            }
        }
       
        private void DataLoad()
        {
            //dt.Rows.Clear();
            
            try
            {

                this.Cursor = Cursors.WaitCursor;
                dgvData.Rows.Clear();
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    
                    try
                    {
                       
                        Load_Data();
                        

                        int rowcount = 0;
                        foreach (var x in dgvData.Rows)
                        {
                            rowcount += 1;
                            x.Cells["dgvNo"].Value = rowcount;
                            
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }

                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;


            //    radGridView1.DataSource = dt;
        }
        //private bool CheckDuplicate(string code, string Code2)
        //{
        //    bool ck = false;

        //    using (DataClasses1DataContext db = new DataClasses1DataContext())
        //    {
        //        int i = (from ix in db.tb_Models
        //                 where ix.ModelName == code

        //                 select ix).Count();
        //        if (i > 0)
        //            ck = false;
        //        else
        //            ck = true;
        //    }

        //    return ck;
        //}

        
       
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

        private void btnView_Click(object sender, EventArgs e)
        {
            
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
                if (screen.Equals(1))
                {
                    if (!Convert.ToString(dgvData.CurrentRow.Cells["RCNo"].Value).Equals(""))
                    {
                        RCNo_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["RCNo"].Value);
                        this.Close();
                    }
                    else
                    {
                        RCNo_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["RCNo"].Value);
                        PRNo_tt.Text = Convert.ToString(dgvData.CurrentRow.Cells["PRNo"].Value);
                        this.Close();
                    }
                }
                else
                {
                    Receive a = new Receive(Convert.ToString(dgvData.CurrentRow.Cells["RCNo"].Value),
                        Convert.ToString(dgvData.CurrentRow.Cells["PRNo"].Value));
                    a.ShowDialog();
                    //this.Close();
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                

            }
            catch (Exception ex) { }
        }

        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());
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

        private void cboVendorName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cboVendorName.Text.Equals(""))
                txtVendorNo.Text = cboVendorName.SelectedValue.ToString();
            else
                txtVendorNo.Text = "";
        }

        private void MasterTemplate_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            //if (screen.Equals(1))
            //{
            //    if (!Convert.ToString(e.Row.Cells["RCNo"].Value).Equals(""))
            //    {
            //        RCNo_tt.Text = Convert.ToString(e.Row.Cells["RCNo"].Value);
            //        this.Close();
            //    }
            //    else
            //    {
            //        RCNo_tt.Text = Convert.ToString(e.Row.Cells["RCNo"].Value);
            //        PRNo_tt.Text = Convert.ToString(e.Row.Cells["PRNo"].Value);
            //        this.Close();
            //    }
            //}
            //else
            //{
            //    Receive a = new Receive(Convert.ToString(e.Row.Cells["RCNo"].Value),
            //        Convert.ToString(e.Row.Cells["PRNo"].Value));
            //    a.ShowDialog();
            //   // this.Close();
            //}
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                //dt_ShelfTag.Rows.Clear();
                string RCNo = "";
                RCNo = StockControl.dbClss.TSt(dgvData.CurrentRow.Cells["RCNo"].Value);
                PrintPR a = new PrintPR(RCNo, RCNo, "Receive");
                a.ShowDialog();

                //using (DataClasses1DataContext db = new DataClasses1DataContext())
                //{
                //    var g = (from ix in db.sp_R003_ReportReceive(RCNo, DateTime.Now) select ix).ToList();
                //    if (g.Count() > 0)
                //    {

                //        Report.Reportx1.Value = new string[2];
                //        Report.Reportx1.Value[0] = RCNo;
                //        Report.Reportx1.WReport = "ReportReceive";
                //        Report.Reportx1 op = new Report.Reportx1("ReportReceive.rpt");
                //        op.Show();

                //    }
                //    else
                //        MessageBox.Show("not found.");
                //}

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
