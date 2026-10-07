using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommon.Classes;

/// <summary>
/// Form For Stock Reconciliation Report
/// </summary>
public partial class Forms_RptStockReconcilation : System.Web.UI.Page
{
    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();
            LoadPrincipal();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        }
    }

    /// <summary>
    /// Loads Principals To Principal Combo
    /// </summary>
    private void LoadPrincipal()
    {
        var pController = new SKUPriceDetailController();
        DataTable mDt = pController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
        DrpPrincipal.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));       
        clsWebFormUtil.FillDropDownList(DrpPrincipal, mDt, 0, 1);
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        var dController = new DistributorController();
        DataTable dt = dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
      //  drpDistributor.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));       
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2);
    }

    /// <summary>
    /// Shows Stock Reconciliation in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        var mController = new DocumentPrintController();
        var rptInventoryCtl = new RptInventoryController();
        var crpReport = new SAMSBusinessLayer.Reports.StockReConsilation();
        var dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedValue));
        var ds = rptInventoryCtl.SelectPrincipalStockReconcilation(int.Parse(drpDistributor.SelectedValue), int.Parse(DrpPrincipal.SelectedValue), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text), int.Parse(Session["UserId"].ToString()),ddlType.SelectedIndex,Convert.ToInt32(rblRate.SelectedValue));
        crpReport.SetDataSource(ds);
        crpReport.Refresh();


        crpReport.SetParameterValue("division", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
        crpReport.SetParameterValue("fromdate", txtStartDate.Text);
        crpReport.SetParameterValue("todate", txtEndDate.Text );
        crpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        crpReport.SetParameterValue("Price", rblRate.SelectedItem.Text);
        crpReport.SetParameterValue("ReportType", "Stock Reconciliation Report ( " + ddlType.SelectedItem.Text + " )");

        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", 0);  
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);            
    }

    /// <summary>
    /// Shows Stock Reconciliation in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        var mController = new DocumentPrintController();
        var rptInventoryCtl = new RptInventoryController();
        var crpReport = new SAMSBusinessLayer.Reports.StockReConsilation();
        var dt = mController.SelectReportTitle(int.Parse(drpDistributor.SelectedValue));
        var ds = rptInventoryCtl.SelectPrincipalStockReconcilation(int.Parse(drpDistributor.SelectedValue), int.Parse(DrpPrincipal.SelectedValue), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text), int.Parse(Session["UserId"].ToString()),ddlType.SelectedIndex,Convert.ToInt32(rblRate.SelectedValue));
        crpReport.SetDataSource(ds);
        crpReport.Refresh();


        crpReport.SetParameterValue("division", drpDistributor.SelectedItem.Text);
        crpReport.SetParameterValue("Principal", DrpPrincipal.SelectedItem.Text);
        crpReport.SetParameterValue("fromdate", txtStartDate.Text);
        crpReport.SetParameterValue("todate", txtEndDate.Text);
        crpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
        crpReport.SetParameterValue("Price", rblRate.SelectedItem.Text);
        crpReport.SetParameterValue("ReportType", "Stock Reconciliation Report ( " + ddlType.SelectedItem.Text + " )");

        string path = Configuration.GetAppInstallationPath() + "\\ExportedFile.xls";

        crpReport.SetDatabaseLogon("sa", "Laislabonitamac2065");

        crpReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, path);

        System.IO.FileInfo file = new System.IO.FileInfo(path);

        if (file.Exists)
        {
            Response.Clear();

            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

            Response.AddHeader("Content-Length", file.Length.ToString());

            Response.ContentType = "application/octet-stream";

            Response.WriteFile(file.FullName);

            Response.End();

        }
        else
        {
            Response.Write("This file does not exist.");
        }        
    }
}
