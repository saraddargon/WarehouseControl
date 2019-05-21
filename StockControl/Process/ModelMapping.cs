﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
namespace StockControl
{
    public partial class ModelMapping : Telerik.WinControls.UI.RadRibbonForm
    {
        public ModelMapping()
        {
            InitializeComponent();
        }

        //private int RowView = 50;
        //private int ColView = 10;
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
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
            dt.Columns.Add(new DataColumn("ModelName", typeof(string)));
            dt.Columns.Add(new DataColumn("PartName", typeof(string)));
            dt.Columns.Add(new DataColumn("PartNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Process", typeof(string)));
            dt.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt.Columns.Add(new DataColumn("ToolLife", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Corner", typeof(int)));
            dt.Columns.Add(new DataColumn("QtyPerPCS", typeof(decimal)));
            dt.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
            dt.Columns.Add(new DataColumn("Remark", typeof(string)));
            dt.Columns.Add(new DataColumn("id", typeof(int)));

            dt2.Columns.Add(new DataColumn("ModelName", typeof(string)));
            dt2.Columns.Add(new DataColumn("PartName", typeof(string)));
            dt2.Columns.Add(new DataColumn("PartNo", typeof(string)));
            dt2.Columns.Add(new DataColumn("Process", typeof(string)));
            dt2.Columns.Add(new DataColumn("CodeNo", typeof(string)));
            dt2.Columns.Add(new DataColumn("ToolLife", typeof(decimal)));
            dt2.Columns.Add(new DataColumn("Corner", typeof(int)));
            dt2.Columns.Add(new DataColumn("QtyPerPCS", typeof(decimal)));
            dt2.Columns.Add(new DataColumn("ItemDescription", typeof(string)));
            dt2.Columns.Add(new DataColumn("Remark", typeof(string)));
           // dt2.Columns.Add(new DataColumn("id", typeof(int)));


        }
        private void Unit_Load(object sender, EventArgs e)
        {
            RMenu3.Click += RMenu3_Click;
            RMenu4.Click += RMenu4_Click;
            RMenu5.Click += RMenu5_Click;
            RMenu6.Click += RMenu6_Click;
            RFrezzRow.Click += RFrezzRow_Click;
            RFrezzColumn.Click += RFrezzColumn_Click;
            RUnFrezz.Click += RUnFrezz_Click;

            radGridView1.ReadOnly = true;
            radGridView1.AutoGenerateColumns = false;
            GETDTRow();
            DataItem();

            DataLoad();
        }
        private void DataItem()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                try
                {


                    txtModelName.AutoCompleteMode = AutoCompleteMode.Append;
                    txtModelName.DisplayMember = "ModelName";
                    txtModelName.ValueMember = "ModelName";
                    var g = (from ix in db.tb_Models.Where(s => s.ModelActive == true) select new { ix.ModelName, ix.ModelDescription }).ToList();
                    txtModelName.DataSource = g; 
                   if(g.Count>0)
                       txtModelName.SelectedIndex = 0;
                }
                catch { }

             
            }
        }
        private void RMenu6_Click(object sender, EventArgs e)
        {
            DeleteUnit();
            DataLoad();
        }

        private void RMenu5_Click(object sender, EventArgs e)
        {
            EditClick(); 
        }

        private void RMenu4_Click(object sender, EventArgs e)
        {
            ViewClick();
        }

        private void RMenu3_Click(object sender, EventArgs e)
        {
            NewClick();
        }
        private void DataLoad()
        {
            dt.Rows.Clear();
            try
            {
                radGridView1.AutoGenerateColumns = false;
                this.Cursor = Cursors.WaitCursor;
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    try
                    {
                      //  radGridView1.DataSource = null;




                        if (!txtModelName.Text.Equals(""))
                        {

                            var gd1 = (from a in db.tb_Mappings
                                       where a.ModelName.Contains(txtModelName.Text)
                                       select new
                                       {
                                           a.ModelName,
                                           a.CodeNo,
                                           a.Remark,
                                           a.QtyPerPCS,
                                           a.ToolLife,
                                           a.PartName,
                                           a.PartNo,
                                           a.Process,
                                           a.Corner,
                                           a.id,
                                           ItemDescription = (from b in db.tb_Items.Where(s => s.CodeNo.Trim().Equals(a.CodeNo)) select b.ItemDescription).FirstOrDefault()
                                       }).ToList();
                            dt.Rows.Clear();
                            dt = dbClss.LINQToDataTable(gd1);
                            radGridView1.DataSource = dt;
                        }
                        else
                        {

                            var gd2 = (from a in db.tb_Mappings
                                       select new
                                       {
                                           a.ModelName,
                                           a.CodeNo,
                                           a.Remark,
                                           a.QtyPerPCS,
                                           a.ToolLife,
                                           a.PartName,
                                           a.PartNo,
                                           a.Process,
                                           a.Corner,
                                           a.id,
                                           ItemDescription = (from b in db.tb_Items.Where(s => s.CodeNo.Trim().Equals(a.CodeNo)) select b.ItemDescription).FirstOrDefault()
                                       }).ToList();
                            dt.Rows.Clear();
                            dt = dbClss.LINQToDataTable(gd2);
                            radGridView1.DataSource = dt;
                        }

                        int ck = 0;
                        foreach (var x in radGridView1.Rows)
                        {
                            x.Cells["dgvCodeTemp"].Value = x.Cells["ModelName"].Value.ToString();
                            x.Cells["dgvCodeTemp2"].Value = x.Cells["CodeNo"].Value.ToString();

                            //x.Cells["ModelName"].ReadOnly = true;
                            //x.Cells["CodeNo"].ReadOnly = true;
                            x.Cells["ItemDescription"].ReadOnly = true;
                            //x.Cells["MMM"].ReadOnly = true;

                            if (row >= 0 && row == ck && radGridView1.Rows.Count > 0)
                            {
                                x.ViewInfo.CurrentRow = x;
                            }
                            ck += 1;
                        }

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }

                }
            }
            catch { }
            this.Cursor = Cursors.Default;


            //    radGridView1.DataSource = dt;
        }
        private void DataLoad1()
        {
           
           

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
            bool ck = false;
            int C = 0;
            try
            {


                radGridView1.EndEdit();
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {
                    foreach (var g in radGridView1.Rows)
                    {
                        if (!Convert.ToString(g.Cells["id"].Value).Equals("")
                            )
                        {
                            if (Convert.ToString(g.Cells["dgvC"].Value).Equals("T"))
                            {
                                
                                DateTime? d = null;
                                DateTime d1 = DateTime.Now;
                                string id = Convert.ToString(g.Cells["id"].Value);
                                if (Convert.ToString(g.Cells["id"].Value).Equals(""))
                                {
                                    /*
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
                                    */

                                }
                                else
                                {
                                    int id2 = 0;
                                    int.TryParse(id, out id2);
                                    decimal tl = 0;
                                    decimal qty = 0;
                                    int co = 1;
                                    int.TryParse(Convert.ToString(g.Cells["Corner"].Value), out co);
                                    decimal.TryParse(Convert.ToString(g.Cells["ToolLife"].Value), out tl);
                                    decimal.TryParse(Convert.ToString(g.Cells["QtyPerPCS"].Value), out qty);
                                    var u = (from ix in db.tb_Mappings
                                             where ix.id == id2
                                             select ix).First();
                                    u.ToolLife = tl;
                                    u.QtyPerPCS = qty;
                                    u.Remark = Convert.ToString(g.Cells["Remark"].Value);
                                    u.PartName = Convert.ToString(g.Cells["PartName"].Value);
                                    u.PartNo = Convert.ToString(g.Cells["PartNo"].Value);
                                    u.Process = Convert.ToString(g.Cells["Process"].Value);
                                    u.ModelName = Convert.ToString(g.Cells["ModelName"].Value);
                                    u.CodeNo = Convert.ToString(g.Cells["CodeNo"].Value);
                                    u.Corner = co;


                                    C += 1;

                                    db.SubmitChanges();
                                    dbClss.AddHistory(this.Name, "แก้ไข", "Update  [ Model=" + u.ModelName+",PartName="+u.PartName+",Qty="+ u.QtyPerPCS.ToString() +"TL="+u.ToolLife.ToString()+ "]", "");

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
            {
               
                if (radGridView1.Rows.Count == 1)
                    row = 0;
                MessageBox.Show("บันทึกสำเร็จ!");
            }

            return ck;
        }
        private bool DeleteUnit()
        {
            bool ck = false;

            int C = 0;
            try
            {

                if (row >= 0)
                {
                    string CodeDelete = Convert.ToString(radGridView1.Rows[row].Cells["ModelName"].Value);
                    string CodeTemp = Convert.ToString(radGridView1.Rows[row].Cells["CodeNo"].Value);
                    string CodeTemp2 = Convert.ToString(radGridView1.Rows[row].Cells["dgvCodeTemp2"].Value);
                    string id = Convert.ToString(radGridView1.Rows[row].Cells["id"].Value);
                    radGridView1.EndEdit();
                    if (MessageBox.Show("ต้องการลบรายการ ( " + CodeDelete+","+ CodeTemp + " ) หรือไม่ ?", "ลบรายการ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {

                            if (!CodeDelete.Equals(""))
                            {
                                if (!CodeTemp.Equals(""))
                                {
                                    int id2 = 0;
                                    int.TryParse(id, out id2);
                                    var unit1 = (from ix in db.tb_Mappings
                                                 where ix.id == id2

                                                 select ix).ToList();
                                    foreach (var d in unit1)
                                    {
                                        db.tb_Mappings.DeleteOnSubmit(d);
                                        dbClss.AddHistory(this.Name, "ลบรายการ PartName", "Delete PrtName [" + d.ModelName+","+ CodeTemp + "]", "");
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
                row = row - 1;
                if (radGridView1.Rows.Count == 1)
                    row = 0;
                else if (row < 0 && radGridView1.Rows.Count > 1)
                    row = 0;
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
            NewClick();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            ViewClick();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditClick();
        }
        private void NewClick()
        {
            radGridView1.ReadOnly = false;
            radGridView1.AllowAddNewRow = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            MappingAdd md = new MappingAdd();
            md.ShowDialog();
            row = radGridView1.Rows.Count - 1;
            if (row < 0)
                row = 0;
            DataLoad();
          //  radGridView1.Rows.AddNew();
        }
        private void EditClick()
        {
            radGridView1.ReadOnly = false;
            btnEdit.Enabled = false;
            btnView.Enabled = true;
            radGridView1.AllowAddNewRow = false;
        }
        private void ViewClick()
        {
            radGridView1.ReadOnly = true;
            btnView.Enabled = false;
            btnEdit.Enabled = true;
            radGridView1.AllowAddNewRow = false;
            DataLoad();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ต้องการบันทึก ?", "บันทึก", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                AddUnit();
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
                    AddUnit();
                    DataLoad();
                }
            }
            else if(e.KeyData == (Keys.Control | Keys.N))
            {
                radGridView1.ReadOnly = true;
                NewClick();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

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
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "Spread Sheet files (*.csv)|*.csv|All files (*.csv)|*.csv";
                if (op.ShowDialog() == DialogResult.OK)
                {


                    using (TextFieldParser parser = new TextFieldParser(op.FileName, Encoding.GetEncoding("windows-874")))
                    {
                        dt2.Rows.Clear();
                        DateTime? d = DateTime.Now;
                        DateTime d1 = DateTime.Now;
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        int a = 0;
                        int c = 0;
                        decimal tl = 0;
                        decimal qty = 0;
                        int co = 1;
                        while (!parser.EndOfData)
                        {
                            //Processing row
                            a += 1;
                            DataRow rd = dt2.NewRow();
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
                                        rd["PartName"] = Convert.ToString(field);
                                    else if (c == 3)
                                        rd["PartNo"] = Convert.ToString(field);
                                    else if (c == 4)
                                        rd["Process"] = Convert.ToString(field).Trim();
                                    else if (c == 5)
                                    {
                                        tl = 0;
                                        decimal.TryParse(Convert.ToString(field).Replace(",","").ToString(), out tl);
                                        rd["ToolLife"] = tl;
                                    }
                                    else if (c == 6)
                                    {
                                        co = 1;
                                        int.TryParse(Convert.ToString(field), out co);
                                        rd["Corner"] = co;
                                    }
                                       
                                    else if(c==7)
                                    {
                                        rd["CodeNo"] = Convert.ToString(field);
                                       
                                    }
                                    else if(c==8)
                                    {
                                        
                                    }
                                    else if (c == 9)
                                    {
                                        qty = 0;
                                        decimal.TryParse(Convert.ToString(field).Replace(",", "").Trim(), out qty);
                                        rd["QtyPerPCS"] = qty;
                                    }
                                    else if(c==10)
                                        rd["Remark"] = Convert.ToString(field);

                                }
                                else
                                {
                                    if (c == 1)
                                        rd["ModelName"] = "";
                                    else if (c == 2)
                                        rd["PartName"] = "";
                                    else if (c == 3)
                                        rd["PartNo"] = "";
                                    else if (c == 4)
                                        rd["Process"] = "";
                                    else if (c == 5)
                                        rd["ToolLife"] = 1;
                                    else if (c == 6)
                                        rd["Corner"] = 1;
                                    else if (c == 7)
                                        rd["CodeNo"] = "";
                                    else if (c==8)
                                    {
                                        
                                    }
                                    else if (c == 9)
                                    {
                                        rd["QtyPerPCS"] = 0;
                                    }
                                    
                                    else if (c == 10)
                                        rd["Remark"] = "";




                                }


                            }
                            dt2.Rows.Add(rd);

                        }
                    }
                    if (dt2.Rows.Count > 0)
                    {

                        dbClss.AddHistory(this.Name, "Import", "Import file CSV in to System", "");
                        ImportData();
                        MessageBox.Show("Import Completed.");

                        DataLoad();
                    }

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); dt2.Rows.Clear(); }
        }

        private void ImportData()
        {
            try
            {
                using (DataClasses1DataContext db = new DataClasses1DataContext())
                {

                    foreach (DataRow rd in dt2.Rows)
                    {
                        if (!rd["ModelName"].ToString().Equals("") && !rd["CodeNo"].ToString().Equals(""))
                        {

                            string Process = rd["Process"].ToString().Trim();
                            string CodeNo = rd["CodeNo"].ToString().Trim();
                            string Model = rd["ModelName"].ToString().Trim();
                            string PartName = rd["PartName"].ToString().Trim();
                            string PartNo = rd["PartNo"].ToString().Trim();
                            string Remark = rd["Remark"].ToString().Trim();
                            decimal qty = 0;
                            decimal tl = 1;
                            int co = 1;
                            int.TryParse(rd["Corner"].ToString(), out co);
                            decimal.TryParse(rd["QtyPerPCS"].ToString().Trim(), out qty);
                            decimal.TryParse(rd["ToolLife"].ToString().Trim(), out tl);
                            var x = (from ix in db.tb_Mappings where ix.ModelName.Equals(Model)
                                     && ix.PartName.Equals(PartName) && ix.CodeNo.Equals(CodeNo) && ix.Process.Equals(Process)
                                     select ix).FirstOrDefault();


                            DateTime? d = null;
                            DateTime d1 = DateTime.Now;
                            if (x == null)
                            {
                                tb_Mapping u = new tb_Mapping();
                                u.ModelName = Model;
                                u.PartName = PartName;
                                u.PartNo = PartNo;
                                u.Process = Process;
                                u.Remark = Remark;
                                u.CodeNo = CodeNo;
                                u.QtyPerPCS = qty;
                                u.ToolLife = tl;
                                u.Corner = co;
                                
                                db.tb_Mappings.InsertOnSubmit(u);
                                db.SubmitChanges();
                            }
                            else
                            {
                                x.ModelName = Model;
                                x.PartName = PartName;
                                x.PartNo = PartNo;
                                x.Process = Process;
                                x.Remark = Remark;
                                x.CodeNo = CodeNo;
                                x.QtyPerPCS = qty;
                                x.ToolLife = tl;
                                x.Corner = co;
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
           
        }

        private void txtModelName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                DataLoad();
            }
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
               
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void RFrezzRow_Click(object sender, EventArgs e)
        {
            frezzRowToolStripMenuItem_Click(null, null);
        }
        private void RFrezzColumn_Click(object sender, EventArgs e)
        {
            frezzColumnToolStripMenuItem_Click(null, null);
        }
        private void RUnFrezz_Click(object sender, EventArgs e)
        {
            unFrezzToolStripMenuItem_Click(null, null);
        }
    
    }
}
