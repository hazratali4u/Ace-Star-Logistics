using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommon.Classes;

public partial class Forms_Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadDistributor();
            LoadPrincipal();
            LoadArea();
            LoadOrderBooker();
            LoadDeliveryman();
            DateTime x = DateTime.Parse(Session["CurrentWorkDate"].ToString());
            txtFromDate.Text = x.ToString("dd-MMM-yyyy");
        }
       
    }
    private void LoadDistributor()
    {
        int x = int.Parse(Session["CompanyId"].ToString());
        var dController = new DistributorController();
        var dt = dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(drpDistributor, dt, 0, 2, true);

    }
    private void LoadPrincipal()
    {

        var pController = new SKUPriceDetailController();
        var mDt = pController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
        clsWebFormUtil.FillDropDownList(DrpPrincipal, mDt, 0, 1, true);
    }
    private void LoadArea()
    {
        if (drpDistributor.Items.Count > 0)
        {
            DrpRoute.Items.Clear();
            var mController = new DistributorAreaController();
            var dt = mController.SelectDist_Area(Constants.LongNullValue, Constants.DateNullValue, Constants.DateNullValue, int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, null, null);
            DrpRoute.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(DrpRoute, dt, 0, 6);
        }
        else
        {
            DrpRoute.Items.Clear();
        }
    }
    private void LoadOrderBooker()
    {
        if (drpDistributor.Items.Count > 0 && DrpRoute.Items.Count > 0 && DrpPrincipal.Items.Count > 0)
        {
            DrpOrderBooker.Items.Clear();
            var mDController = new SaleForceController();
            var mDt = mDController.SelectSaleForceAssignedArea(Constants.SALES_FORCE_ORDERBOOKER, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpRoute.SelectedValue.ToString()), int.Parse(Session["CompanyId"].ToString()), Convert.ToInt32(DrpPrincipal.SelectedValue));
            DrpOrderBooker.Items.Add(new ListItem("All", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDropDownList(DrpOrderBooker, mDt, 0, 3);
        }
        else
        {
            DrpOrderBooker.Items.Clear();
        }
    }
    private void LoadDeliveryman()
    {
        if (drpDistributor.Items.Count > 0 && DrpRoute.Items.Count > 0)
        {
            var mDController = new SaleForceController();
            var mDt = mDController.SelectSaleForceAssignedArea(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpRoute.SelectedValue.ToString()), int.Parse(Session["CompanyId"].ToString()));
            clsWebFormUtil.FillDropDownList(DrpDeliveryMan, mDt, 0, 3, true);
        }
        else
        {
            DrpDeliveryMan.Items.Clear();
        }
    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        try
        {
            var dPrint = new DocumentPrintController();
            var rptSaleCtl = new RptSaleController();
            var dt = dPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue));

            string fromDate = null;
          //  string ToDate = null;

            DataSet ds = null;

            DateTime parsedDateFromdate = DateTime.Parse(txtFromDate.Text);
            fromDate = parsedDateFromdate.ToShortDateString();
         


          ds = rptSaleCtl.LoadPassEntry2(int.Parse(DrpPrincipal.SelectedValue), int.Parse(drpDistributor.SelectedValue), Convert.ToDateTime(fromDate + " 00:00:00"), int.Parse(DrpOrderBooker.SelectedValue.ToString()), int.Parse(DrpRoute.SelectedValue.ToString()), int.Parse(DrpDeliveryMan.SelectedValue.ToString()));
                                                       
            
            
            var crpReport = new SAMSBusinessLayer.Reports.CrpLoadPassEntry();
            crpReport.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("Rout_ID", DrpRoute.SelectedItem.Text);
            crpReport.SetParameterValue("OrderBooker", DrpOrderBooker.SelectedItem.Text);
   
            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", 0);
            string url = "'Default.aspx'";
            string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
            
           
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }
    protected void DrpDeliveryMan_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDeliveryman();
        LoadOrderBooker();
        LoadArea();
    }
    protected void DrpRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDeliveryman();
        LoadOrderBooker();
    }
    protected void DrpOrderBooker_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void DrpPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadOrderBooker();
    }
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        try
        {
            var dPrint = new DocumentPrintController();
            var rptSaleCtl = new RptSaleController();
            var dt = dPrint.SelectReportTitle(int.Parse(drpDistributor.SelectedValue.ToString()));

            string fromDate = null;
        

            DataSet ds = null;

            DateTime parsedDateFromdate = DateTime.Parse(txtFromDate.Text);
            fromDate = parsedDateFromdate.ToShortDateString();



            ds = rptSaleCtl.LoadPassEntry2(int.Parse(DrpPrincipal.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), Convert.ToDateTime(fromDate + " 00:00:00"), int.Parse(DrpOrderBooker.SelectedValue.ToString()), int.Parse(DrpRoute.SelectedValue.ToString()), int.Parse(DrpDeliveryMan.SelectedValue.ToString()));



            var crpReport = new SAMSBusinessLayer.Reports.CrpLoadPassEntry();
            crpReport.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
            crpReport.SetDataSource(ds);
            crpReport.Refresh();

            crpReport.SetParameterValue("Rout_ID", DrpRoute.SelectedItem.Text);
            crpReport.SetParameterValue("OrderBooker", DrpOrderBooker.SelectedItem.Text);



            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", 1);
            string url = "'Default.aspx'";
            string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }
}