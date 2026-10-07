using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommon.Classes;

/// <summary>
/// Form To Take Order, Invoice And Sale Return(Step2)
/// </summary>
public partial class Forms_frmLoadPassIssue : Page
{

    #region Variables

    DataTable PurchaseSKU;
    static DataTable PurchaseSKU1;
    DataTable dtFreeSKU;
    private int mTownId;
    private static int RowId;
    private static int UnitType;
    private static int OrderNo;
    private static bool IsDamage = false;

    #endregion

    /// <summary>
    /// Page_Load Function Populates All Combos, Grids And ListBox On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    PurchaseController mPurchase = new PurchaseController();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            btnSaveOrder.Enabled = false;
            if (this.Session["SaleInvoiceID"] != null)
            {
                if (int.Parse(this.Session["SaleInvoiceID"].ToString()) != -1)
                {
                    btnSaveOrder.Text = "UPDATE";
                    int SaleInvoiceID = int.Parse(this.Session["SaleInvoiceID"].ToString());

                    #region SALE Order MATER

                    DataControl dc = new DataControl();
                    DataTable dt = mPurchase.SelectPurchaseOrderDocumentNo2(Constants.LongNullValue,
                                                                            Constants.IntNullValue, 0, SaleInvoiceID);
                    clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 1, 2, true);
                    clsWebFormUtil.FillDropDownList(this.DrpPrincipal, dt, 5, 6, true);
                    clsWebFormUtil.FillDropDownList(this.DrpRoute, dt, 3, 4, true);
                    clsWebFormUtil.FillDropDownList(this.DrpDeliveryMan, dt, 7, 8, true);
                    clsWebFormUtil.FillDropDownList(this.DrpOrderBooker, dt, 9, 10, true);

                    txtnumGrossSale.Text =
                        Math.Round(Convert.ToDecimal(dc.chkNull_0(dt.Rows[0]["TOTAL_AMOUNT"].ToString())), 2).ToString();
                    txtUnclaimAbleDiscount.Text =
                        Math.Round(Convert.ToDecimal(dc.chkNull_0(dt.Rows[0]["UNCLAIMABLE_AMOUNT"].ToString())), 2)
                            .ToString();
                    txtWholeSaleDiscount.Text =
                        Math.Round(Convert.ToDecimal(dc.chkNull_0(dt.Rows[0]["WHOLESALE_DISCOUNT"].ToString())), 2)
                            .ToString();
                    txtGSTAmount.Text =
                        Math.Round(Convert.ToDecimal(dc.chkNull_0(dt.Rows[0]["GST_AMOUNT"].ToString())), 2).ToString();
                    txtTSTAmount.Text =
                        Math.Round(Convert.ToDecimal(dc.chkNull_0(dt.Rows[0]["TST_AMOUNT"].ToString())), 2).ToString();
                    txtTaxCollected.Text =
                        Math.Round(Convert.ToDecimal(dc.chkNull_0(dt.Rows[0]["TAX_COLLECTED"].ToString())), 2)
                            .ToString();
                    txtNetAmount.Text =
                        Math.Round(Convert.ToDecimal(dc.chkNull_0(dt.Rows[0]["TOTAL_NET_VALUE"].ToString())), 2)
                            .ToString();
                    txtBRD.Text =
                        Math.Round(Convert.ToDecimal(dc.chkNull_0(dt.Rows[0]["BRD"].ToString())), 2).ToString();
                    txtVisit.Text = dt.Rows[0]["VISIT"].ToString();
                    txtProductiveCall.Text = dc.chkNull_0(dt.Rows[0]["PRODUCTIVE_CALL"].ToString());
                    txtOpeningReading.Text = dc.chkNull_0(dt.Rows[0]["OPENING_READING"].ToString());
                    txtCloseReading.Text = dc.chkNull_0(dt.Rows[0]["CLOSING_READING"].ToString());
                    txtBillNoFrom.Text = dc.chkNull_0(dt.Rows[0]["BILL_NO_FROM"].ToString());
                    txtBillNoTo.Text = dc.chkNull_0(dt.Rows[0]["BILL_NO_TO"].ToString());
                    txtOtherDiscounts.Text =
                        Math.Round(Convert.ToDecimal(dc.chkNull_0(dt.Rows[0]["OTHER_DISCOUNT"].ToString())), 2)
                            .ToString();
                    txtTradeOffers.Text =
                        Math.Round(Convert.ToDecimal(dc.chkNull_0(dt.Rows[0]["TRADE_OFFER"].ToString())), 2).ToString();
                    txtClaimableDiscounts.Text =
                        Math.Round(Convert.ToDecimal(dc.chkNull_0(dt.Rows[0]["CLAIMABLE_DISCOUNT"].ToString())), 2)
                            .ToString();
                    drpDistributor.Enabled = false;
                    DrpPrincipal.Enabled = false;
                    DrpOrderBooker.Enabled = false;
                    DrpDeliveryMan.Enabled = false;
                    DrpRoute.Enabled = false;
                    //txtVisit.Enabled = false;
                    //txtProductiveCall.Enabled = false;
                    //txtOpeningReading.Enabled = false;
                    //txtCloseReading.Enabled = false;
                    //txtBillNoFrom.Enabled = false;
                    //txtBillNoTo.Enabled = false;


                    #endregion

                    #region Sale Order Detail

                    this.CreatTable();
                    DataTable dt1 = mPurchase.SelectPurchaseOrderDetail2(SaleInvoiceID);
                    Session.Add("PurchaseSKU", dt1);
                    this.LoadGird();
                    this.LoadSkuDetail();
                    btnCalculate.Enabled = true;
                    decimal TotalDamageValue = 0;
                    decimal TotalSchemeValue = 0;
                    decimal TotalRedemptionValue = 0;
                    foreach (DataRow dr in dt1.Rows)
                    {
                        TotalDamageValue += ((decimal.Parse(dc.chkNull_0(dr["DAMAGE_UNITS"].ToString())))*
                                             (decimal.Parse(dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));
                        TotalSchemeValue += ((decimal.Parse(dc.chkNull_0(dr["SCHEME_UNITS"].ToString())))*
                                             (decimal.Parse(dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));
                        TotalRedemptionValue += ((decimal.Parse(dc.chkNull_0(dr["REDEMPTION_OUT_UNITS"].ToString())))*
                                                 (decimal.Parse(dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));


                        if (decimal.Parse(dc.chkNull_0(dr["DAMAGE_UNITS"].ToString())) > 0)
                        {
                            IsDamage = true;
                        }

                    }

                    #endregion

                    txtTotalDamageValue.Text = Math.Round(TotalDamageValue, 2).ToString(CultureInfo.InvariantCulture);
                    txtTotalSchemeValue.Text = Math.Round(TotalSchemeValue, 2).ToString(CultureInfo.InvariantCulture);
                    txtTotalRedemptionOut.Text =
                        Math.Round(TotalRedemptionValue, 2).ToString(CultureInfo.InvariantCulture);

                }
                else
                {
                    this.LoadDistributor();
                    this.LoadPrincipal();
                    this.LoadArea();
                    this.LoadOrderBooker();
                    this.LoadDeliveryman();
                    this.LoadSkuDetail();
                    this.CreatTable();
                }
            }


            btnAddSku.Attributes.Add("onclick", "return ValidateForm();");
            btnSaveOrder.Attributes.Add("onclick", "return ValidateSaveOrder();");
            txtnumGrossSale.Attributes.Add("Readonly", "Readonly");
            txtTotalDamageValue.Attributes.Add("Readonly", "Readonly");
            txtTotalSchemeValue.Attributes.Add("Readonly", "Readonly");
            txtTotalRedemptionOut.Attributes.Add("Readonly", "Readonly");
            txtClaimableDiscounts.Attributes.Add("Readonly", "Readonly");
            txtGSTAmount.Attributes.Add("Readonly", "Readonly");
            txtTSTAmount.Attributes.Add("Readonly", "Readonly");
            txtNetAmount.Attributes.Add("Readonly", "Readonly");

        }
  
    }
    
    private void LoadDistributor()
    {
        int x = int.Parse(this.Session["CompanyId"].ToString());
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2, true);

    }
    private void LoadPrincipal()
    {

        SKUPriceDetailController PController = new SKUPriceDetailController();
        DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
        clsWebFormUtil.FillDropDownList(this.DrpPrincipal, m_dt, 0, 1, true);
    }
    private void LoadArea()
    {
        if (drpDistributor.Items.Count > 0)
        {
            DistributorAreaController mController = new DistributorAreaController();
            DataTable dt = mController.SelectDist_Area(Constants.LongNullValue, Constants.DateNullValue, Constants.DateNullValue, int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, null, null);
            clsWebFormUtil.FillDropDownList(DrpRoute, dt, 0, 6, true);
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
            SaleForceController mDController = new SaleForceController();
            DataTable m_dt = mDController.SelectSaleForceAssignedArea(Constants.SALES_FORCE_ORDERBOOKER, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpRoute.SelectedValue.ToString()), int.Parse(this.Session["CompanyId"].ToString()), Convert.ToInt32(DrpPrincipal.SelectedValue));
            clsWebFormUtil.FillDropDownList(this.DrpOrderBooker, m_dt, 0, 3, true);
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
            SaleForceController mDController = new SaleForceController();
            DataTable m_dt = mDController.SelectSaleForceAssignedArea(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpRoute.SelectedValue.ToString()), int.Parse(this.Session["CompanyId"].ToString()));
            clsWebFormUtil.FillDropDownList(this.DrpDeliveryMan, m_dt, 0, 3, true);
        }
        else
        {
            DrpDeliveryMan.Items.Clear();
        }
    }

    /// <summary>
    /// Creates Datatable For Order, Invoice And Sale Return
    /// </summary>
    private void CreatTable()
    {
        PurchaseSKU = new DataTable();
        PurchaseSKU.Columns.Add("SALE_ORDER_DETAIL_ID", typeof(long));
        PurchaseSKU.Columns.Add("DistributorId", typeof(int));
        PurchaseSKU.Columns.Add("SALE_ORDER_ID", typeof(int));
        PurchaseSKU.Columns.Add("SKU_ID", typeof(int));
        PurchaseSKU.Columns.Add("SKU_CODE", typeof(string));
        PurchaseSKU.Columns.Add("SKU_Name", typeof(string));
        PurchaseSKU.Columns.Add("BATCH_NO", typeof(string));
        PurchaseSKU.Columns.Add("UNIT_PRICE", typeof(decimal));

        PurchaseSKU.Columns.Add("ISSUE_CTN", typeof(int));
        PurchaseSKU.Columns.Add("ISSUE_UNITS", typeof(int));
        PurchaseSKU.Columns.Add("RETURN_UNITS", typeof(int));
        PurchaseSKU.Columns.Add("GROSS_UNITS", typeof(int));
        PurchaseSKU.Columns.Add("DAMAGE_UNITS", typeof(int));
        PurchaseSKU.Columns.Add("SCHEME_UNITS", typeof(int));
        PurchaseSKU.Columns.Add("REDEMPTION_OUT_UNITS", typeof(int));
        PurchaseSKU.Columns.Add("NET_SALE", typeof(int));
        PurchaseSKU.Columns.Add("NET_VALUE", typeof(int));
        PurchaseSKU.Columns.Add("TOTAL_UNITS", typeof(int));
        PurchaseSKU.Columns.Add("PACKING", typeof(string));
        PurchaseSKU.Columns.Add("RETURN_EMPTY_UNITS", typeof(int));

        PurchaseSKU.Columns.Add("AMOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("STANDARD_DISCOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("STANDARD_DISCOUNT_PER", typeof(decimal));
        PurchaseSKU.Columns.Add("EXTRA_DISCOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("RETAIL_AMOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("GST_RATE", typeof(decimal));
        PurchaseSKU.Columns.Add("GST_RATE_TP", typeof(decimal));
        PurchaseSKU.Columns.Add("GST_AMOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("TST_AMOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("CLAIM_EXTRA_AMOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("CLAIM_STANDARD_DISCOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("CLAIM_PER", typeof(decimal));
        PurchaseSKU.Columns.Add("SED_AMOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("NET_AMOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("QUANTITY_CTN", typeof(decimal));
        PurchaseSKU.Columns.Add("IS_DELETED", typeof(bool));

        PurchaseSKU.Columns.Add("TOTAL_AMOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("UNCLAIMABLE_DISCOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("WHOLESALE_DISCOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("TAX_COLLECTED", typeof(decimal));
        PurchaseSKU.Columns.Add("BRD", typeof(decimal));
        PurchaseSKU.Columns.Add("TRADE_OFFERS", typeof(decimal));
        PurchaseSKU.Columns.Add("OTHER_DISCOUNT", typeof(decimal));
        PurchaseSKU.Columns.Add("CLAIMABLE_DISCOUNT", typeof(decimal));

        this.Session.Add("PurchaseSKU", PurchaseSKU);

        PurchaseSKU1 = new DataTable();
        PurchaseSKU1.Columns.Add("SKU_CODE", typeof(string));
        this.Session.Add("PurchaseSKU1", PurchaseSKU1);
    }

    /// <summary>
    /// Creates Datatable For Free SKU
    /// </summary>
    private void CreateFreeSku()
    {
        dtFreeSKU = new DataTable();
        dtFreeSKU.Columns.Add("SKU_ID", typeof(int));
        dtFreeSKU.Columns.Add("SKU_Code", typeof(string));
        dtFreeSKU.Columns.Add("SKU_Name", typeof(string));
        dtFreeSKU.Columns.Add("UNIT_PRICE", typeof(decimal));
        dtFreeSKU.Columns.Add("Quantity", typeof(int));
        dtFreeSKU.Columns.Add("AMOUNT", typeof(decimal));
        dtFreeSKU.Columns.Add("GST_RATE", typeof(decimal));
        dtFreeSKU.Columns.Add("GST_AMOUNT", typeof(decimal));
        dtFreeSKU.Columns.Add("TST_AMOUNT", typeof(decimal));
        dtFreeSKU.Columns.Add("PROMOTION_ID", typeof(int));
        dtFreeSKU.Columns.Add("BASKET_ID", typeof(int));
        dtFreeSKU.Columns.Add("BASKET_DETAIL_ID", typeof(int));
        dtFreeSKU.Columns.Add("PROMOTION_OFFER_ID", typeof(int));
        this.Session.Add("dtFreeSKU", dtFreeSKU);
    }


    ///// <summary>
    ///// Loads SKU Data To SKU ListBox
    ///// </summary>
    private void LoadSkuDetail()
    {
        SKUPriceDetailController PController = new SKUPriceDetailController();
        if (int.Parse(DrpPrincipal.SelectedValue.ToString()) > 0)
        {
            int x = int.Parse(DrpPrincipal.SelectedValue.ToString());
            int y = int.Parse(drpDistributor.SelectedValue.ToString());
            int z = int.Parse(this.Session["UserId"].ToString());
            DateTime d = DateTime.Parse(this.Session["CurrentWorkDate"].ToString());
            //   DataTable Dtsku_Price = PController.SelectDataPrice(int.Parse(this.Session["PrincipalId"].ToString()), Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["DistributorId"].ToString()), int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 1, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
            DataTable Dtsku_Price = PController.SelectDataPrice2(int.Parse(DrpPrincipal.SelectedValue.ToString()), Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 1, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
            clsWebFormUtil.FillListBox(this.lstCode, Dtsku_Price, 10, 10, true);
            this.Session.Add("Dtsku_Price", Dtsku_Price);
        }
    }

                                                         

    /// <summary>
    /// Loads Order To Order Grid
    /// </summary>
    private void LoadGird()
    {
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        GrdPurchase.DataSource = PurchaseSKU;
        GrdPurchase.DataBind();
    }

    /// <summary>
    /// Sets Order SKU For Edit. This Function Runs When An Existing Order SKU Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        RowId = e.NewEditIndex;
        txtskuCode.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[1].Text;
        txtskuName.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[2].Text;
        txtPacking.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[3].Text;
        txtTP.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        txtIssueCtn.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[5].Text;
        txtIssueUnits.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[6].Text;
        //txtReturnUnits.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[7].Text;
        //txtReturnEmpty.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[8].Text;
        //txtGrossSaleUnits.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[9].Text;
        //txtDamegeUnits.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[10].Text;
        //txtSchemeUnits.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[11].Text;
        //txtredemptionUnits .Text = GrdPurchase.Rows[e.NewEditIndex].Cells[12].Text;
        //txtNetSaleUnits.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[13].Text;
        //txtNetValue.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[14].Text;

        txtskuCode.Enabled = false;
        btnAddSku.Text = "Update SKU";
        btnCalculate.Enabled = false;
        btnSaveOrder.Enabled = false;
        if (txtskuCode.Text == "C201" || txtskuCode.Text == "C202" || txtskuCode.Text == "C203" || txtskuCode.Text == "C204" || txtskuCode.Text == "C205" || txtskuCode.Text == "C206" || txtskuCode.Text == "C207")
        {
            //txtReturnEmpty.Enabled = true;
            txtIssueCtn.Enabled = false;
            //txtReturnUnits.Enabled = false;
            //txtDamegeUnits.Enabled = false;
            //txtSchemeUnits.Enabled = false;
            //txtredemptionUnits.Enabled = false;

        }
        else
        {
            //txtReturnEmpty.Enabled = false;
            txtIssueCtn.Enabled = true;
            //txtReturnUnits.Enabled = true;
            //txtDamegeUnits.Enabled = true;
            //txtSchemeUnits.Enabled = true;
            //txtredemptionUnits.Enabled = true;
        }


    }
    protected void grdPurchase1_RowEditing1(object sender, GridViewEditEventArgs e)
    {
        RowId = e.NewEditIndex;
        //txtskuCode.Text = grdPurchase1.Rows[e.NewEditIndex].Cells[0].Text;


        //if (UnitType == 0)
        //{
        //    //  txtQuantity.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[3].Text;
        //}
        //else
        //{
        //    // txtQuantity.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[7].Text;
        //}
        //txtskuCode.Enabled = false;
        //btnAddSku.Text = "Update SKU";

    }
    ///// <summary>
    ///// Deletes An SKU From Order Grid
    ///// </summary>
    ///// <param name="sender">object</param>
    ///// <param name="e">GridViewEditEventArgs</param>
    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        PurchaseSKU.Rows.RemoveAt(e.RowIndex);
        this.Session.Add("PurchaseSKU", PurchaseSKU);
        this.LoadGird();
        btnCalculate.Enabled = true;
        btnSaveOrder.Enabled = false;
    }
    protected void grdPurchase1_RowDeleting1(object sender, GridViewDeleteEventArgs e)
    {
        PurchaseSKU1 = (DataTable)this.Session["PurchaseSKU1"];
        PurchaseSKU1.Rows.RemoveAt(e.RowIndex);
        this.Session.Add("PurchaseSKU1", PurchaseSKU1);
        this.LoadGird();
        //  btnCalculate.Enabled = true;
        //  btnSave.Enabled = false;
    }
 

   

    ///// <summary>
    ///// Enables/Disables Controls
    ///// </summary>
    ///// <param name="CValue">Value</param>
    private void EnableDisableController(bool cValue)
    {
 

    }


    private void ClearAll()
    {
        txtskuCode.Text = "";
        txtskuName.Text = "";
        txtPacking.Text = "";
        txtTP.Text = "";
        txtIssueCtn.Text = "";
        txtIssueUnits.Text = "";
        //txtReturnUnits.Text = "";
        //txtGrossSaleUnits.Text = "";
        //txtDamegeUnits.Text = "";
        //txtSchemeUnits.Text = "";
        //txtredemptionUnits.Text = "";
        //txtNetSaleUnits.Text = "";
        txtNetAmount.Text = "";
       // txtNetValue.Text = "";
      //  txtTaxCollected.Text = "";
        //txtReturnEmpty.Text = "";
        btnAddSku.Text = "Add Sku";
        btnSaveOrder.Enabled = false;
        btnCalculate.Enabled = true;
        txtskuCode.Enabled = true;
        ScriptManager.GetCurrent(Page).SetFocus(txtskuCode);
    }

    /// <summary>
    /// Clears All Controls
    /// </summary>
    private void ClearMasterAll()
    {
        this.EnableDisableController(true);
        this.Session.Remove("PurchaseSKU");
        this.Session.Remove("dtFreeSKU");
        this.CreatTable();
        this.CreateFreeSku();
        this.LoadGird();
      
        
        txtnumGrossSale.Text = "";
        txtUnclaimAbleDiscount.Text = "";
        txtWholeSaleDiscount.Text = "";
        txtBRD.Text = "";
        txtTradeOffers.Text = "";
        txtOtherDiscounts.Text = "";
        txtClaimableDiscounts.Text = "";
        txtGSTAmount.Text = "";
        txtTSTAmount.Text = "";
        txtNetAmount.Text = "";
        txtVisit.Text = "";
        txtProductiveCall.Text = "";
        txtOpeningReading.Text = "";
        txtCloseReading.Text = "";
        txtBillNoFrom.Text = "";
        txtBillNoTo.Text = "";
        txtTaxCollected.Text = "";
        btnSaveOrder.Enabled = false;
        txtTotalRedemptionOut.Text = "";

        txtTotalSchemeValue.Text = "";
        txtTotalDamageValue.Text = "";
        RowId = 0;
      
        this.Session.Remove("SaleInvoiceID");
        ScriptManager.GetCurrent(Page).SetFocus(txtVisit);
        
    }

    private bool IsDayClosed()
    {
        bool flag = false;
        DistributorController DistrCtl = new DistributorController();
        DataTable dtDayClose = DistrCtl.MaxDayClose(int.Parse(drpDistributor.SelectedValue.ToString()), 3);
        if (Convert.ToDateTime(Session["CurrentWorkDate"]) == Convert.ToDateTime(dtDayClose.Rows[0]["DayClose"]))
        {
            flag = false;
        }
        else
        {
            flag = true;
        }

        return flag;
    }
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadArea();
        this.LoadOrderBooker();
        this.LoadDeliveryman();
        this.LoadSkuDetail();
        this.Session.Add("DistributorId", int.Parse(drpDistributor.SelectedValue.ToString()));
    }
    protected void DrpPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadOrderBooker();
        this.Session.Add("PrincipalId", int.Parse(DrpPrincipal.SelectedValue.ToString()));
        this.LoadSkuDetail();
        this.LoadDeliveryman();
    }
    protected void DrpRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Session.Add("Route", DrpRoute.SelectedItem.Text);
        this.LoadOrderBooker();
        this.LoadDeliveryman();
    }
    protected void DrpOrderBooker_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Session.Add("OrderBookerId", int.Parse(DrpOrderBooker.SelectedValue.ToString()));
    }
    protected void btnAddSku_Click(object sender, EventArgs e)
    {
        var dc = new DataControl();
        var Dtsku_Price = (DataTable)this.Session["Dtsku_Price"];
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        DataRow[] foundRows = Dtsku_Price.Select("SKU_CODE  = '" + txtskuCode.Text + "'");
        decimal mTradePrice = decimal.Parse(dc.chkNull_0(foundRows[0]["TRADE_PRICE"].ToString()));

        int mPackSize = int.Parse(dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString()));
        //packsize use in txtissuectn textchange event

        #region Btn text Add Sku
        if (btnAddSku.Text == "Add Sku")
        {
            if (CheckDublicateSku())
            {
                DataRow dr = PurchaseSKU.NewRow();
                dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                dr["PACKING"] = foundRows[0]["PACKSIZE"];
                dr["UNIT_PRICE"] = decimal.Parse (foundRows[0]["TRADE_PRICE"].ToString ());
                dr["ISSUE_CTN"] = int.Parse(dc.chkNull_0(txtIssueCtn.Text));
                dr["ISSUE_UNITS"] = int.Parse(dc.chkNull_0(txtIssueUnits.Text));
                //dr["RETURN_UNITS"] = int.Parse(dc.chkNull_0(txtReturnUnits.Text));
                //dr["RETURN_EMPTY_UNITS"] = int.Parse(dc.chkNull_0(txtReturnEmpty.Text));
                //dr["GROSS_UNITS"] = int.Parse(dc.chkNull_0(txtGrossSaleUnits.Text));
                //dr["DAMAGE_UNITS"] = int.Parse(dc.chkNull_0(txtDamegeUnits.Text));
                //dr["SCHEME_UNITS"] = int.Parse(dc.chkNull_0(txtSchemeUnits.Text));
                //dr["REDEMPTION_OUT_UNITS"] = int.Parse(dc.chkNull_0(txtredemptionUnits.Text));
                //dr["NET_SALE"] = int.Parse(dc.chkNull_0(txtNetSaleUnits.Text));
                //dr["NET_VALUE"] = decimal.Parse(dc.chkNull_0(txtNetValue.Text));

                #region GST


                if (foundRows[0]["GST_ON"].ToString().Trim() == "T")
                {
                   // dr["AMOUNT"] = mTradePrice * decimal.Parse(dr["QUANTITY_UNIT"].ToString());
                    dr["GST_RATE"] = foundRows[0]["GST_RATE_TP"];
                    decimal x = decimal .Parse (foundRows [0]["GST_RATE_TP"].ToString ());
                    dr["TST_AMOUNT"] = 0;
                    dr["BATCH_NO"] = "T";

                }
                else if (foundRows[0]["GST_ON"].ToString().Trim() == "R")
                {
                 //   dr["AMOUNT"] = mTradePrice * decimal.Parse(dr["QUANTITY_UNIT"].ToString());
                    dr["TST_AMOUNT"] = decimal.Parse(foundRows[0]["GST_RATE_TP"].ToString()) * decimal.Parse(dr["NET_SALE"].ToString());
                    dr["GST_RATE"] = 0;
                    dr["BATCH_NO"] = "R";

                }
                else
                {
                   // dr["AMOUNT"] = mTradePrice * decimal.Parse(dr["QUANTITY_UNIT"].ToString());
                   dr["TST_AMOUNT"] = 0;
                    dr["GST_RATE"] = 0;
                    dr["BATCH_NO"] = "E";
                }
                #endregion

                //if (int.Parse(dc.chkNull_0(txtDamegeUnits.Text)) > 0)
                //{
                //    IsDamage = true;
                //}
                PurchaseSKU.Rows.Add(dr);
               
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('SKU Already Exists')", true);
            }
        }
        #endregion
        #region btn update Sku
        else
        {
            DataRow dr = PurchaseSKU.Rows[RowId];
            dr["SKU_ID"] = foundRows[0]["SKU_ID"];
            dr["SKU_CODE"] = foundRows[0]["SKU_CODE"];
            dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
            dr["UNIT_PRICE"] = decimal .Parse ( foundRows[0]["TRADE_PRICE"].ToString ());
            dr["ISSUE_CTN"] = int.Parse(dc.chkNull_0(txtIssueCtn.Text));
            dr["ISSUE_UNITS"] = int.Parse(dc.chkNull_0(txtIssueUnits.Text));
            //dr["RETURN_UNITS"] = int.Parse(dc.chkNull_0(txtReturnUnits.Text));
            //dr["RETURN_EMPTY_UNITS"] = int.Parse(dc.chkNull_0(txtReturnEmpty.Text));
            //dr["GROSS_UNITS"] = int.Parse(dc.chkNull_0(txtGrossSaleUnits.Text));
            //dr["DAMAGE_UNITS"] = int.Parse(dc.chkNull_0(txtDamegeUnits.Text));
            //dr["SCHEME_UNITS"] = int.Parse(dc.chkNull_0(txtSchemeUnits.Text));
            //dr["REDEMPTION_OUT_UNITS"] = int.Parse(dc.chkNull_0(txtredemptionUnits.Text));
            //dr["NET_SALE"] = decimal.Parse(dc.chkNull_0(txtNetSaleUnits.Text));
            //dr["NET_VALUE"] = decimal.Parse(dc.chkNull_0(txtNetValue.Text));

            #region GST
            if (foundRows[0]["GST_ON"].ToString().Trim() == "T")
            {
                dr["GST_RATE"] = foundRows[0]["GST_RATE_TP"];
              //  dr["AMOUNT"] = mTradePrice * decimal.Parse(dr["QUANTITY_UNIT"].ToString());
                dr["TST_AMOUNT"] = 0;
                dr["BATCH_NO"] = "T";

            }
            else if (foundRows[0]["GST_ON"].ToString().Trim() == "R")
            {
              //  dr["AMOUNT"] = mTradePrice * decimal.Parse(dr["QUANTITY_UNIT"].ToString());
                dr["TST_AMOUNT"] = decimal.Parse(foundRows[0]["GST_RATE_TP"].ToString()) * decimal.Parse(dr["NET_SALE"].ToString());
                dr["GST_RATE"] = 0;
                dr["BATCH_NO"] = "R";

            }
            else
            {
              //  dr["AMOUNT"] = mTradePrice * decimal.Parse(dr["QUANTITY_UNIT"].ToString());
                dr["TST_AMOUNT"] = 0;
                dr["GST_RATE"] = 0;
                dr["BATCH_NO"] = "E";
            }
            #endregion

            //if (int.Parse(dc.chkNull_0(txtDamegeUnits.Text)) > 0)
            //{
            //    IsDamage = true;
            //}
        }
#endregion
        this.Session.Add("PurchaseSKU", PurchaseSKU);
        this.EnableDisableController(false);
        this.LoadGird();
        this.ClearAll();
        ScriptManager.GetCurrent(Page).SetFocus(txtskuCode);
       // txtReturnEmpty.Enabled = true;
        txtIssueCtn.Enabled = true;
        //txtReturnUnits.Enabled = true;
        //txtDamegeUnits.Enabled = true;
        //txtSchemeUnits.Enabled = true;
        //txtredemptionUnits.Enabled = true;
      
    }



    private bool CheckDublicateSku()
    {
        DataTable Dtsku_Price = (DataTable)this.Session["Dtsku_Price"];
        PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
        DataRow[] foundRows = PurchaseSKU.Select("SKU_CODE  = '" + txtskuCode.Text + "'");
        if (foundRows.Length == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void DrpDeliveryMan_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Session.Add("DeliveryManId", int.Parse(DrpDeliveryMan.SelectedValue.ToString()));
        this.LoadSkuDetail();
    }

    protected void btnSaveOrder_Click(object sender, EventArgs e)
       
    {

        long Sale_Order_ID=Constants.LongNullValue ;
        var pc = new PurchaseController();
        var loadpass = pc.selectLoadPassDocumentNo(int.Parse(drpDistributor.SelectedValue), int.Parse(DrpRoute.SelectedValue), long.Parse(DrpRoute.SelectedValue), int.Parse(DrpPrincipal.SelectedValue), int.Parse(DrpOrderBooker.SelectedValue), int.Parse(DrpDeliveryMan.SelectedValue), DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));

        if (IsDayClosed())
        {
            var userCtl = new UserController();

            userCtl.InsertUserLogoutTime(int.Parse(drpDistributor.SelectedValue), Convert.ToInt32(Session["UserID"]));
            this.Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("../Login.aspx");
        }
        else
        {
            
            string ManualID = null;
           
            var DC = new DataControl();
            PurchaseSKU = (DataTable)this.Session["PurchaseSKU"];
            dtFreeSKU = (DataTable)this.Session["dtFreeSKU"];
            var mOrderController = new OrderEntryController();
            if (btnSaveOrder.Text == "Save")
            {
              
                if (loadpass.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Load Pass Already Exist against this Delivery Man');", true);

                }
                else
                {
                    #region Sale Invoice Portion
                    /////////////////////////////////////////////////////////////////// Invoice Portion///////////////////////// 
                    foreach (DataRow dr in PurchaseSKU.Rows)
                    {
                        var mController = new PhaysicalStockController();
                        DataTable dtstock = mController.SelectSKUClosingStock(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(dr["SKU_ID"].ToString()), "", DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
                        if (dtstock.Rows.Count > 0)
                        {
                            if (int.Parse(dtstock.Rows[0][0].ToString()) < int.Parse(dr["ISSUE_UNITS"].ToString()))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('" + dr["SKU_Code"].ToString() + " Current Stock is " + dtstock.Rows[0][0].ToString() + "');", true);
                                return;
                            }
                            else if (Convert.ToDecimal(dtstock.Rows[0][1]) != Convert.ToDecimal(dr["UNIT_PRICE"]))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('" + dr["SKU_Code"].ToString() + " Order Rate is " + dr["UNIT_PRICE"].ToString() + " and Current Rate is " + dtstock.Rows[0][1].ToString() + "');", true);
                                return;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' + " + dr["SKU_Code"].ToString() + "No Stock Found');", true);
                            return;
                        }
                    }

                    var isValidInsert1 = mOrderController.Add_Invoice1(int.Parse(drpDistributor.SelectedValue.ToString()), ManualID, int.Parse(DrpRoute.SelectedValue), long.Parse(DrpRoute.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue), 0, 0, int.Parse(DrpOrderBooker.SelectedValue.ToString()), int.Parse(DrpDeliveryMan.SelectedValue), 0,
                    decimal.Parse(DC.chkNull_0(txtnumGrossSale.Text)), decimal.Parse(DC.chkNull_0(txtUnclaimAbleDiscount.Text)), decimal.Parse(DC.chkNull_0(txtClaimableDiscounts.Text)), decimal.Parse(DC.chkNull_0(txtBRD.Text)), decimal.Parse(DC.chkNull_0(txtGSTAmount.Text)), decimal.Parse(DC.chkNull_0(txtNetAmount.Text)), decimal.Parse(DC.chkNull_0(txtOtherDiscounts.Text)),
                    Constants.Order_Posted_Id, PurchaseSKU, dtFreeSKU, int.Parse(this.Session["UserId"].ToString()), 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), decimal.Parse(DC.chkNull_0(txtTradeOffers.Text)), decimal.Parse(DC.chkNull_0(txtTSTAmount.Text)), int.Parse(DC.chkNull_0(txtVisit.Text)), int.Parse(DC.chkNull_0(txtProductiveCall.Text)),
                    int.Parse(DC.chkNull_0(txtOpeningReading.Text)), int.Parse(DC.chkNull_0(txtCloseReading.Text)), int.Parse(DC.chkNull_0(txtBillNoFrom.Text)), int.Parse(DC.chkNull_0(txtBillNoTo.Text)), decimal.Parse(DC.chkNull_0(txtWholeSaleDiscount.Text)), decimal.Parse(DC.chkNull_0(txtTaxCollected.Text)), decimal.Parse(DC.chkNull_0(txtTotalDamageValue.Text)), IsDamage);



                    if (isValidInsert1)
                    {
                        ClearMasterAll();
                      
                    }


                    #endregion
                }



            }
            else
            {
                if (btnSaveOrder.Text == "UPDATE")
                {


                    #region saleInvoice Master/detail Updation
                    var isValidInsert1 = mOrderController.Update_Invoice(long.Parse(this.Session["SaleInvoiceID"].ToString()), int.Parse(drpDistributor.SelectedValue), ManualID, int.Parse(DrpRoute.SelectedValue), long.Parse(DrpRoute.SelectedValue), int.Parse(DrpPrincipal.SelectedValue), 0, 0, 
                        int.Parse(DrpOrderBooker.SelectedValue), int.Parse(DrpDeliveryMan.SelectedValue), Sale_Order_ID,
                         decimal.Parse(DC.chkNull_0(txtnumGrossSale.Text)), decimal.Parse(DC.chkNull_0(txtUnclaimAbleDiscount.Text)), 
                         decimal.Parse(DC.chkNull_0(txtClaimableDiscounts.Text)), decimal.Parse(DC.chkNull_0(txtBRD.Text)), decimal.Parse(DC.chkNull_0(txtGSTAmount.Text)),
                         decimal.Parse(DC.chkNull_0(txtNetAmount.Text)), decimal.Parse(DC.chkNull_0(txtOtherDiscounts.Text)), Constants.Order_Posted_Id, PurchaseSKU, dtFreeSKU, int.Parse(this.Session["UserId"].ToString()), Constants.DecimalNullValue, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()),
                         decimal.Parse(DC.chkNull_0(txtTradeOffers.Text)), decimal.Parse(DC.chkNull_0(txtTSTAmount.Text)), int.Parse(DC.chkNull_0(txtVisit.Text)), int.Parse(DC.chkNull_0(txtProductiveCall.Text)), int.Parse(DC.chkNull_0(txtOpeningReading.Text)), int.Parse(DC.chkNull_0(txtCloseReading.Text)), 
                         int.Parse(DC.chkNull_0(txtBillNoFrom.Text)), int.Parse(DC.chkNull_0(txtBillNoTo.Text)), decimal.Parse(DC.chkNull_0(txtWholeSaleDiscount.Text)), decimal.Parse(DC.chkNull_0(txtTaxCollected.Text)), decimal.Parse(DC.chkNull_0(txtTotalDamageValue.Text)),IsDamage);
                  


                    if (isValidInsert1)
                    {
                        this.ClearMasterAll();
                      
                    }

                    Response.Redirect("~/Forms/frmLoadPassEntry.aspx?Status=" + false + "&LevelType=3&LevelID=" + Request.QueryString["LevelID"]);
                    #endregion


                }
            }




        }

    }
  
    protected void btnCalculate_Click(object sender, EventArgs e)
    {
       
        decimal ObjGrossSales = 0;
        decimal ObjTotalDamageValue = 0;
        decimal ObjTotalSchemeValue = 0;
        decimal ObjTotalRedemptionValue = 0;
        decimal ObjTotalGST = 0;
        decimal ObjTotalTST = 0;
        
        var dc = new DataControl();
        var dt = (DataTable)this.Session["PurchaseSKU"];
        foreach (DataRow dr in dt.Rows)
        {
            ObjGrossSales += ((decimal.Parse(dc.chkNull_0(dr["GROSS_UNITS"].ToString ()))) *(decimal.Parse(dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));

            ObjTotalDamageValue += ((decimal.Parse(dc.chkNull_0(dr["DAMAGE_UNITS"].ToString()))) * (decimal.Parse(dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));

            ObjTotalSchemeValue += ((decimal.Parse(dc.chkNull_0(dr["SCHEME_UNITS"].ToString()))) * (decimal.Parse(dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));
            ObjTotalRedemptionValue += ((decimal.Parse(dc.chkNull_0(dr["REDEMPTION_OUT_UNITS"].ToString()))) * (decimal.Parse(dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));
            //
            #region GST Calculation
            decimal TempAmount = 0;
       

            TempAmount = decimal.Parse(dc.chkNull_0(dr["NET_VALUE"].ToString()));
      
            dr["GST_AMOUNT"] = (TempAmount) * decimal.Parse(dc.chkNull_0(dr["GST_RATE"].ToString())) / 100;
            ObjTotalTST += decimal.Parse(dc.chkNull_0(dr["TST_AMOUNT"].ToString()));
            ObjTotalGST += decimal.Parse(dc.chkNull_0(dr["GST_AMOUNT"].ToString()));
         
            #endregion

        }

        txtGSTAmount.Text = Convert.ToString(Math.Round(ObjTotalGST, 2));
        txtTSTAmount.Text = Convert.ToString(Math.Round(ObjTotalTST, 2));
        txtTotalDamageValue.Text = Convert.ToString(Math.Round(ObjTotalDamageValue, 2));
        txtTotalSchemeValue.Text = Convert.ToString(Math.Round(ObjTotalSchemeValue, 2));
        txtTotalRedemptionOut.Text = Convert.ToString(Math.Round(ObjTotalRedemptionValue, 2));

        txtnumGrossSale.Text =Math .Round (ObjGrossSales,2).ToString();
        txtClaimableDiscounts.Text =Math .Round (decimal.Parse(dc.chkNull_0(txtWholeSaleDiscount.Text)) + decimal.Parse(dc.chkNull_0(txtBRD.Text)) + decimal.Parse(dc.chkNull_0(txtTradeOffers.Text)) + decimal.Parse(dc.chkNull_0(txtOtherDiscounts.Text)),2).ToString ();
        txtNetAmount.Text = Math.Round(decimal.Parse(dc.chkNull_0(txtnumGrossSale.Text)) - decimal.Parse(dc.chkNull_0(txtUnclaimAbleDiscount.Text)) - decimal.Parse(dc.chkNull_0(txtClaimableDiscounts.Text)) + decimal.Parse(dc.chkNull_0(txtGSTAmount.Text)) + decimal.Parse(dc.chkNull_0(txtTSTAmount.Text)) + decimal.Parse(dc.chkNull_0(txtTaxCollected.Text)) - decimal.Parse(dc.chkNull_0(txtTotalDamageValue.Text)) - decimal.Parse(dc.chkNull_0(txtTotalSchemeValue.Text)) - decimal.Parse(dc.chkNull_0(txtTotalRedemptionOut.Text)), 2).ToString();
        btnSaveOrder.Enabled = true;
       
    }
}




   