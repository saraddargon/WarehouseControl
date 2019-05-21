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
    public partial class CalculatePlanning : Telerik.WinControls.UI.RadRibbonForm
    {
        public CalculatePlanning()
        {
            InitializeComponent();
        }

        //private int RowView = 50;
        //private int ColView = 10;
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
            //dt.Columns.Add(new DataColumn("YYYY", typeof(int)));
            //dt.Columns.Add(new DataColumn("MMM", typeof(int)));
            //dt.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            //dt.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
            //dt.Columns.Add(new DataColumn("ForeCastQty", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("Toolife_spc", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("SumQty", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("ExtendQty", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("UsePerDay", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("LeadTime", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("KeepStock", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("AddErrQty", typeof(decimal)));
            //dt.Columns.Add(new DataColumn("OrderQty", typeof(decimal)));

           
        }
        int crow = 99;
        private void Unit_Load(object sender, EventArgs e)
        {
            radGridView1.ReadOnly = true;
            radGridView1.AutoGenerateColumns = false;
            GETDTRow();
            for (int i = 2017; i < DateTime.Now.Year + 10; i++)
            {
                cboYear.Items.Add(i.ToString());

            }
            //DefaultItem();
            cboYear.Text = DateTime.Now.Year.ToString();
            cboMonth.Text = dbClss.getMonthRevest(DateTime.Now.Month);
            DataLoad();

            crow = 0;
        }
        private void DefaultItem()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                cboModelName.AutoCompleteMode = AutoCompleteMode.Append;
                cboModelName.DisplayMember = "ModelName";
                cboModelName.ValueMember = "ModelName";
                cboModelName.DataSource = (from ix in db.tb_Models.Where(s => s.ModelActive == true)select new {ix.ModelName,ix.ModelDescription }).ToList();
                cboModelName.SelectedIndex = 0;


                try
                {

                    for(int i=2017;i<2030;i++)
                    {
                        cboYear.Items.Add(i.ToString());

                    }

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
            
            
            try
            {

                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {                    
                    //dt = ClassLib.Classlib.LINQToDataTable(db.tb_Units.ToList());
                    try
                    {
                        int year1 = 2017;
                        int.TryParse(cboYear.Text, out year1);
                        /*
                        var gd = (from ix in db.tb_ForcastCalculates
                                  where ix.MMM == dbClss.getMonth(cboMonth.Text) && ix.YYYY == year1
                                  select new { ix.YYYY, ix.MMM, Month = dbClss.getMonthRevest(ix.MMM)
                                  , ix.CodeNo
                                  , ItemDescription =db.tb_Items.Where(s => s.CodeNo == ix.CodeNo).Select(o => o.ItemDescription).FirstOrDefault()
                                  ,ix.ForeCastQty,ix.Toolife_spc,ix.SumQty,ix.ExtendQty,ix.UsePerDay
                                  ,
                                      ix.StockQty
                                  ,
                                      ix.RemainOrder
                                  //,  ix.UsedPerMonth
                                  //,  ix.Hold
                                  ,
                                      ix.ReOrderPoint
                                  ,
                                      ix.LeadTime,ix.KeepStock,ix.AddErrQty,ix.OrderQty}).ToList();
                        //MessageBox.Show(gd.Count().ToString());
                        */
                        var g3 = (from ix in db.sp_SelectProduction_ListForcast(year1, dbClss.getMonth(cboMonth.Text)) select ix).ToList();

                        radGridView1.DataSource = g3;

                        int rowcount = 0;
                        foreach (var x in radGridView1.Rows)
                        {
                            rowcount += 1;
                            x.Cells["dgvNo"].Value = rowcount;
                            x.Cells["dgvCodeTemp"].Value = x.Cells["YYYY"].Value.ToString();
                            x.Cells["dgvCodeTemp2"].Value = x.Cells["CodeNo"].Value.ToString();
                            x.Cells["dgvCodeTemp3"].Value = x.Cells["MMM"].Value.ToString();
                            //  MessageBox.Show("ss");
                            // x.Cells["ModelName"].ReadOnly = true;
                            //x.Cells["YYYY"].ReadOnly = true;
                            //x.Cells["MMM"].ReadOnly = true;
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }

                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;


            //    radGridView1.DataSource = dt;
        }
        private bool CheckDuplicate(string code, string Code2)
        {
            bool ck = false;

            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                int i = (from ix in db.tb_Models
                         where ix.ModelName == code

                         select ix).Count();
                if (i > 0)
                    ck = false;
                else
                    ck = true;
            }

            return ck;
        }

        private bool AddUnit()
        {
            return false;
          
            bool ck = false;
            int C = 0;
            try
            {


                radGridView1.EndEdit();
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    foreach (var g in radGridView1.Rows)
                    {
                        if (!Convert.ToString(g.Cells["ModelName"].Value).Equals("")
                            )
                        {
                            if (Convert.ToString(g.Cells["dgvC"].Value).Equals("T"))
                            {
                                //int yyyy = 0;
                                //int mmm = 0;
                                //decimal wk = 0;
                                //int.TryParse(Convert.ToString(g.Cells["YYYY"].Value), out yyyy);
                                //int.TryParse(Convert.ToString(g.Cells["MMM"].Value), out mmm);
                                //decimal.TryParse(Convert.ToString(g.Cells["WorkDays"].Value), out wk);
                                DateTime? d = null;
                                DateTime d1 = DateTime.Now;
                                if (Convert.ToString(g.Cells["dgvCodeTemp"].Value).Equals(""))
                                {

                                    tb_Model u = new tb_Model();
                                    u.ModelName = Convert.ToString(g.Cells["ModelName"].Value);
                                    u.ModelDescription = Convert.ToString(g.Cells["ModelDescription"].Value);
                                    u.ModelActive = Convert.ToBoolean(Convert.ToString(g.Cells["ModelActive"].Value));
                                    u.LineName = Convert.ToString(g.Cells["LineName"].Value);
                                    u.MCName = Convert.ToString(g.Cells["MCName"].Value);
                                    u.Limit = Convert.ToBoolean(g.Cells["Limit"].Value);
                                    if (DateTime.TryParse(Convert.ToString(g.Cells["ExpireDate"].Value), out d1))
                                    {
                                        d = dbClss.ChangeFormat(Convert.ToString(g.Cells["ExpireDate"].Value));
                                        //Convert.ToDateTime(Convert.ToString(g.Cells["ExpireDate"].Value));

                                    }
                                    u.ExpireDate = d;


                                    db.tb_Models.InsertOnSubmit(u);
                                    db.SubmitChanges();
                                    C += 1;
                                    dbClss.AddHistory(this.Name, "เพิ่ม", "Insert Model [" + u.ModelName + "]", "");

                                }
                                else
                                {

                                    var u = (from ix in db.tb_Models
                                             where ix.ModelName == Convert.ToString(g.Cells["dgvCodeTemp"].Value)

                                             select ix).First();

                                    u.ModelDescription = Convert.ToString(g.Cells["ModelDescription"].Value);
                                    u.ModelActive = Convert.ToBoolean(Convert.ToString(g.Cells["ModelActive"].Value));
                                    u.LineName = Convert.ToString(g.Cells["LineName"].Value);
                                    u.MCName = Convert.ToString(g.Cells["MCName"].Value);
                                    u.Limit = Convert.ToBoolean(g.Cells["Limit"].Value);

                                    if (DateTime.TryParse(Convert.ToString(g.Cells["ExpireDate"].Value), out d1))
                                    {
                                        d = dbClss.ChangeFormat(Convert.ToString(g.Cells["ExpireDate"].Value));
                                        //Convert.ToDateTime(Convert.ToString(g.Cells["ExpireDate"].Value));

                                    }
                                    u.ExpireDate = d;

                                    C += 1;

                                    db.SubmitChanges();
                                    dbClss.AddHistory(this.Name, "แก้ไข", "Update Model [" + u.ModelName + "]", "");

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("AddUnit", ex.Message, this.Name);
            }

            if (C > 0)
                MessageBox.Show("บันทึกสำเร็จ!");

            return ck;
        }
        private bool DeleteUnit()
        {
            return false;
            bool ck = false;

            int C = 0;
            try
            {

                if (row >= 0)
                {
                    string CodeDelete = Convert.ToString(radGridView1.Rows[row].Cells["ModelName"].Value);
                    string CodeTemp = Convert.ToString(radGridView1.Rows[row].Cells["dgvCodeTemp"].Value);
                    string CodeTemp2 = Convert.ToString(radGridView1.Rows[row].Cells["dgvCodeTemp2"].Value);
                    radGridView1.EndEdit();
                    if (MessageBox.Show("ต้องการลบรายการ ( " + CodeDelete + " ) หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {

                            if (!CodeDelete.Equals(""))
                            {
                                if (!CodeTemp.Equals(""))
                                {

                                    var unit1 = (from ix in db.tb_Models
                                                 where ix.ModelName == Convert.ToString(CodeTemp)

                                                 select ix).ToList();
                                    foreach (var d in unit1)
                                    {
                                        db.tb_Models.DeleteOnSubmit(d);
                                        dbClss.AddHistory(this.Name, "ลบรายการ ModelName", "Delete Model [" + d.ModelName + "]", "");
                                    }
                                    C += 1;



                                    db.SubmitChanges();
                                }
                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbClss.AddError("DeleteUnit", ex.Message, this.Name);
            }

            if (C > 0)
            {
                MessageBox.Show("ลบรายการ สำเร็จ!");
            }




            return ck;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            //return;
            //radGridView1.ReadOnly = false;
            //radGridView1.AllowAddNewRow = false;
            //radGridView1.Rows.AddNew();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            radGridView1.ReadOnly = true;
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            radGridView1.AllowAddNewRow = false;
            DataLoad();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            radGridView1.ReadOnly = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            radGridView1.AllowAddNewRow = false;
            //DataLoad();
        }
        private void Calculate()
        {

            MessageBox.Show("คำนวณเรียบร้อยแล้ว !");
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ต้องการคำนวน หรือไม่?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Calculate();
                //
                StockControl.Form2 f2 = new Form2();
                f2.ShowDialog();
                DataLoad();
            }
        }

        private void radGridView1_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            try
            {
                radGridView1.Rows[e.RowIndex].Cells["dgvC"].Value = "T";
                
                //string TM1 = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["ModelName"].Value);
                ////string TM2 = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["MMM"].Value);
                //string Chk = Convert.ToString(radGridView1.Rows[e.RowIndex].Cells["dgvCodeTemp"].Value);
                //if (Chk.Equals("") && !TM1.Equals(""))
                //{

                //    if (!CheckDuplicate(TM1, Chk))
                //    {
                //        MessageBox.Show("ข้อมูล รายการซ้า");
                //        radGridView1.Rows[e.RowIndex].Cells["ModelName"].Value = "";
                //        //  radGridView1.Rows[e.RowIndex].Cells["MMM"].Value = "";
                //        //  radGridView1.Rows[e.RowIndex].Cells["UnitCode"].IsSelected = true;

                //    }
                //}


            }
            catch (Exception ex) { }
        }

        private void Unit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());
        }

        private void radGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // MessageBox.Show(e.KeyCode.ToString());

            if (e.KeyData == (Keys.Control | Keys.S))
            {
                if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    return;
                    AddUnit();
                    DataLoad();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            return;
            DeleteUnit();
            DataLoad();

        }

        int row = -1;
        private void radGridView1_CellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            row = e.RowIndex;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //dbClss.ExportGridCSV(radGridView1);
            dbClss.ExportGridXlSX(radGridView1);
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
            radGridView1.EnableFiltering = true;
        }

        private void btnUnfilter1_Click(object sender, EventArgs e)
        {
            radGridView1.EnableFiltering = false;
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
            if (e.CellElement.ColumnInfo.Name == "ModelName")
            {
                if (e.CellElement.RowInfo.Cells["ModelName"].Value != null)
                {
                    if (!e.CellElement.RowInfo.Cells["ModelName"].Value.Equals(""))
                    {
                        e.CellElement.DrawFill = true;
                        // e.CellElement.ForeColor = Color.Blue;
                        e.CellElement.NumberOfColors = 1;
                        e.CellElement.BackColor = Color.WhiteSmoke;
                    }

                }
            }
        }

        private void txtModelName_TextChanged(object sender, EventArgs e)
        {
            DataLoad();
        }

        private void radPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboModelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (crow == 0)
            //    DataLoad();
        }

        private void cboYear_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (crow == 0)
                DataLoad();
        }

        private void radButtonElement1_Click(object sender, EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                if (MessageBox.Show("ต้องการกำหนดค่าสั้งซื้อใหม่ หรือไม่?", "Apply", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        int yyyy = DateTime.Now.Year;
                        int month = DateTime.Now.Month;
                        yyyy = Convert.ToInt32(cboYear.Text);
                        month = dbClss.getMonth(cboMonth.Text);
                        //db.sp_SelectProduction_Update(yyyy, month);
                        var db2 = (from ix in db.sp_SelectProduction_ListForcast(yyyy, month) select ix).ToList();
                        if (db2.Count > 0)
                        {
                            foreach (var r in db2)
                            {

                                if(!Convert.ToBoolean(r.Hold))
                                    db.sp_SelectProduction_UpdateToItem(r.CodeNo, r.KeepStock, r.ForeCastQty, r.ForeCastQty);
                               
                            }

                        }
                        dbClss.AddHistory(this.Name, "Apply", "อัพเดตจุดสั่งซื้อ โดย " + ClassLib.Classlib.User, "");
                        this.Cursor = Cursors.Default;
                    }
                    MessageBox.Show("Apply เรียบร้อยแล้ว!");
                }
            }
            catch { }
            this.Cursor = Cursors.Default;
        }

        private void radButtonElement4_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (MessageBox.Show("ต้องการบันทึก หรือไม่?", "Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    try
                    {


                        decimal OrderPoint = 0;
                        decimal Maxstock = 0;
                        bool hold = false;
                        int id = 0;

                        foreach (var x in radGridView1.Rows)
                        {
                            if (x.Cells["dgvC"].Value.Equals("T"))
                            {

                                int.TryParse(x.Cells["id"].Value.ToString(), out id);
                                hold = Convert.ToBoolean(x.Cells["Hold"].Value);
                                decimal.TryParse(Convert.ToString(x.Cells["ForeCastQty"].Value), out OrderPoint);
                                decimal.TryParse(Convert.ToString(x.Cells["KeepStock"].Value), out Maxstock);
                                db.sp_SelectProduction_updateHold(id, hold, Maxstock, OrderPoint);
                            }

                        }

                        MessageBox.Show("บันทึกเรียบร้อย");
                    }
                    catch { }
                }
            }
            this.Cursor = Cursors.Default;
        
        }

        private void frezzRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (radGridView1.Rows.Count > 0)
                {

                    int Row = 0;
                    Row = radGridView1.CurrentRow.Index;
                    dbClss.Set_Freeze_Row(radGridView1, Row);

                    //foreach (var rd in radGridView1.Rows)
                    //{
                    //    if (rd.Index <= Row)
                    //    {
                    //        radGridView1.Rows[rd.Index].PinPosition = PinnedRowPosition.Top;
                    //    }
                    //    else
                    //        break;
                    //}
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void frezzColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (radGridView1.Columns.Count > 0)
                {

                    int Col = 0;
                    Col = radGridView1.CurrentColumn.Index;
                    dbClss.Set_Freeze_Column(radGridView1, Col);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void unFrezzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                dbClss.Set_Freeze_UnColumn(radGridView1);
                dbClss.Set_Freeze_UnRows(radGridView1);
                //foreach (var rd in radGridView1.Rows)
                //{
                //    radGridView1.Rows[rd.Index].IsPinned = false;
                //}
                //foreach (var rd in radGridView1.Columns)
                //{
                //    radGridView1.Columns[rd.Index].IsPinned = false;                   
                //}

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
