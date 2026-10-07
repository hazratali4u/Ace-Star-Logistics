using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommon.Classes;

public partial class Forms_LoadPassEntry : System.Web.UI.Page
{
    readonly PurchaseController _mPurchase = new PurchaseController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            FillRepeater();
            Session.Add("SaleInvoiceID",-1);
            ltrAdd.Text = "<a href='frmLoadPass.aspx?Status=" + false + "&LevelType=3&LevelID=" + Request.QueryString["LevelID"] + "'>Add Load Pass</a>";
          
        }

    }
    protected void rPurchaseOrder_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Load")
        {
            long saleInvoiceId = Convert.ToInt64(e.CommandArgument);
           // long SALE_INVOICE_ID;
           // HiddenField HFSaleID = (HiddenField)e.Item.FindControl("hfSaleInvoiceId");
             Session.Add("SaleInvoiceID", saleInvoiceId);
           //  Session.Add("SaleInvoiceID", HFSaleID.Value);
            Response.Redirect("~/Forms/frmLoadPass.aspx?Status=" + false + "&LevelType=3&LevelID=" + Request.QueryString["LevelID"]);
           // if (mPurchase.UpdatePurchaseOrder(SALE_ORDER_ID, 1))//TypeID = 1 for Close
            //{
            //    ScriptManager.RegisterStartupScript(this,  GetType(), "msg", "javascript:alert('PO closed successfully.');", true);
            //    FillRepeater();
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this,  GetType(), "msg", "javascript:alert('Some error occured.');", true);
            //}
        }
    }

    private void FillRepeater()
    {
        rPurchaseOrder.DataSource = null;
        rPurchaseOrder.DataBind();
        lblTotalNoOfRecords.Text = "";
        lblCurrentPageNo.Text = "";
        lblTotalNoOfPages.Text = "";
        PagedDataSource pagedResults = new PagedDataSource();
        pagedResults.AllowPaging = true;
        pagedResults.PageSize = 20;

        DataTable dt = _mPurchase.SelectPurchaseOrderDocumentNo2(Constants.IntNullValue, Convert.ToDateTime(Session["CurrentWorkDate"]));

        pagedResults.DataSource = dt.DefaultView;
        if (pagedResults.Count > 0)
        {
            pagedResults.CurrentPageIndex = CurrentPage;
            linkbtnnext.Enabled = !pagedResults.IsLastPage;
            linkbtnprev.Enabled = !pagedResults.IsFirstPage;
            rPurchaseOrder.DataSource = pagedResults;
            rPurchaseOrder.DataBind();

            linkbtnnext.Visible = true;
            linkbtnprev.Visible = true;
            lblTotalNoOfPages.Visible = true;
            lblCurrentPageNo.Visible = true;
            lblTotalNoOfRecords.Visible = true;
            lblDummy.Visible = true;
            lblOf.Visible = true;
            lblTotalNoOfRecords.Text = dt.Rows.Count.ToString();
            lblCurrentPageNo.Text = (CurrentPage + 1).ToString();
            lblTotalNoOfPages.Text = Convert.ToString(Math.Ceiling(Convert.ToDecimal(dt.Rows.Count) / Convert.ToDecimal(20)));

        }
        else if (pagedResults.Count == 0)
        {
            linkbtnnext.Visible = false;
            linkbtnprev.Visible = false;
            lblTotalNoOfPages.Visible = false;
            lblCurrentPageNo.Visible = false;
            lblTotalNoOfRecords.Visible = false;
            lblOf.Visible = false;
            lblDummy.Visible = false;
            lblTotalNoOfRecords.Text = "";
        }
    }

    private int CurrentPage
    {
        get
        {
            object objview =  ViewState["_CurrentPage"];
            if (objview == null)
                return 0;
            else
                return (int)objview;
        }
        set
        {
             ViewState["_CurrentPage"] = value;
        }
    }

    protected void linkbtnprev_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        FillRepeater();
    }

    protected void linkbtnnext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        FillRepeater();
    }
}