using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommon.Classes;

/// <summary>
/// Form To Take Order, Invoice And Sale Return(Step2)
/// </summary>
public partial class Forms_frmLoadPass : Page
{
    #region Variables

    DataTable _purchaseSku;
    DataTable _purchaseSku2;
    static DataTable _purchaseSku1;
    DataTable _dtFreeSku;
    private static int _rowId;
    private static bool _isDamage = false;

    #endregion
    readonly PurchaseController _mPurchase = new PurchaseController();
    readonly DataControl _dc = new DataControl();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            btnSaveOrder.Enabled = false;
            //Added By Hassan----///
            Session.Remove("_purchaseSku2");
            Session.Remove("DeliveryManID");
            //-------------\\
            if (int.Parse(Session["SaleInvoiceID"].ToString()) != -1)
            {
                hfDisableCtrl.Value = "0";
                btnSaveOrder.Text = "UPDATE";
                int saleInvoiceId = int.Parse( Session["SaleInvoiceID"].ToString());
              
                #region SALE Order MATER

                
                DataTable dt = _mPurchase.SelectPurchaseOrderDocumentNo2(Constants.LongNullValue, Constants.IntNullValue, 0, saleInvoiceId);
                clsWebFormUtil.FillDropDownList( drpDistributor, dt, 1, 2, true);
                clsWebFormUtil.FillDropDownList( DrpPrincipal, dt, 5, 6, true);
                clsWebFormUtil.FillDropDownList( DrpRoute, dt,3, 4, true);
              //  clsWebFormUtil.FillDropDownList( DrpDeliveryMan, dt, 7, 8, true);
                clsWebFormUtil.FillDropDownList( DrpOrderBooker, dt, 9, 10, true);
               
                LoadDeliveryman();
              

                txtnumGrossSale.Text = Math.Round(Convert.ToDecimal(_dc.chkNull_0 (dt.Rows[0]["TOTAL_AMOUNT"].ToString())),2).ToString();
                txtUnclaimAbleDiscount.Text = Math.Round(Convert.ToDecimal(_dc.chkNull_0 (dt.Rows[0]["UNCLAIMABLE_AMOUNT"].ToString())), 2).ToString();
                txtWholeSaleDiscount.Text = Math.Round(Convert.ToDecimal(_dc.chkNull_0 (dt.Rows[0]["WHOLESALE_DISCOUNT"].ToString())), 2).ToString();
                txtGSTAmount.Text = Math.Round(Convert.ToDecimal(_dc.chkNull_0 (dt.Rows[0]["GST_AMOUNT"].ToString())), 2).ToString();
                txtTSTAmount.Text = Math.Round(Convert.ToDecimal(_dc.chkNull_0 (dt.Rows[0]["TST_AMOUNT"].ToString())), 2).ToString();
                txtTaxCollected.Text = Math.Round(Convert.ToDecimal(_dc.chkNull_0(dt.Rows[0]["TAX_COLLECTED"].ToString())), 2).ToString();
                txtNetAmount.Text = Math.Round(Convert.ToDecimal(_dc.chkNull_0 (dt.Rows[0]["TOTAL_NET_VALUE"].ToString())), 2).ToString();
                TxtTotalAmount.Text = Math.Round(Convert.ToDecimal(_dc.chkNull_0(dt.Rows[0]["TOTAL_NET_VALUE"].ToString()))-Convert.ToDecimal(_dc.chkNull_0(dt.Rows[0]["TAX_COLLECTED"].ToString())), 2).ToString();

                if (Math.Round(Convert.ToDecimal(_dc.chkNull_0(TxtTotalAmount.Text))) > 0)
                {
                    txtTaxCollectedPer.Text = Math.Round(Convert.ToDecimal(_dc.chkNull_0(txtTaxCollected.Text)) / Convert.ToDecimal(_dc.chkNull_0(TxtTotalAmount.Text)) * 100, 2).ToString();
                }
                else
                { txtTaxCollectedPer.Text = "0";
                }
                txtBRD.Text = Math.Round(Convert.ToDecimal(_dc.chkNull_0(dt.Rows[0]["BRD"].ToString())), 2).ToString();
                txtVisit.Text = dt.Rows[0]["VISIT"].ToString();
                txtProductiveCall.Text = _dc.chkNull_0 (dt.Rows[0]["PRODUCTIVE_CALL"].ToString());
                txtOpeningReading.Text =_dc.chkNull_0 ( dt.Rows[0]["OPENING_READING"].ToString());
                txtCloseReading.Text = _dc.chkNull_0 (dt.Rows[0]["CLOSING_READING"].ToString());
                txtBillNoFrom.Text = _dc.chkNull_0 (dt.Rows[0]["BILL_NO_FROM"].ToString());
                txtBillNoTo.Text =_dc.chkNull_0 ( dt.Rows[0]["BILL_NO_TO"].ToString());
                txtOtherDiscounts.Text = Math.Round(Convert.ToDecimal(_dc.chkNull_0 (dt.Rows[0]["OTHER_DISCOUNT"].ToString())), 2).ToString();
                txtTradeOffers.Text = Math.Round(Convert.ToDecimal(_dc.chkNull_0 (dt.Rows[0]["TRADE_OFFER"].ToString())), 2).ToString();
                txtClaimableDiscounts.Text = Math.Round(Convert.ToDecimal(_dc.chkNull_0(dt.Rows[0]["CLAIMABLE_DISCOUNT"].ToString())), 2).ToString();
                drpDistributor.Enabled = false;
                DrpPrincipal.Enabled = false;
                DrpOrderBooker.Enabled = false;
                DrpDeliveryMan.SelectedValue = dt.Rows[0]["DELIVERYMAN_ID"].ToString();

                //Added by Hassan-------------------------\\For Check Duplicate on Update Load Pass 
                Session.Add("DeliveryManID", DrpDeliveryMan.SelectedValue);

                //DrpDeliveryMan.Enabled = false;
                DrpRoute.Enabled = false;
                //txtVisit.Enabled = false;
                //txtProductiveCall.Enabled = false;
                //txtOpeningReading.Enabled = false;
                //txtCloseReading.Enabled = false;
                //txtBillNoFrom.Enabled = false;
                //txtBillNoTo.Enabled = false;


                #endregion
                #region Sale Order Detail
                CreatTable();
                DataTable  dt1 = _mPurchase.SelectPurchaseOrderDetail2(saleInvoiceId);
                Session.Add("PurchaseSKU", dt1);
                LoadGird();
                LoadSkuDetail();
                btnCalculate.Enabled = true;
                decimal totalIssueValue = 0;
                decimal totalDamageValue = 0;
                decimal totalSchemeValue = 0;
                decimal totalRedemptionValue = 0;
                foreach (DataRow dr in dt1.Rows)
                {
                    totalIssueValue += ((decimal.Parse(_dc.chkNull_0(dr["ISSUE_UNITS"].ToString()))) * (decimal.Parse(_dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));
                    totalDamageValue += ((decimal.Parse(_dc.chkNull_0(dr["DAMAGE_UNITS"].ToString()))) * (decimal.Parse(_dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));
                    totalSchemeValue += ((decimal.Parse(_dc.chkNull_0(dr["SCHEME_UNITS"].ToString()))) * (decimal.Parse(_dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));
                    totalRedemptionValue += ((decimal.Parse(_dc.chkNull_0(dr["REDEMPTION_OUT_UNITS"].ToString()))) * (decimal.Parse(_dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));
                
                    
                    if (decimal.Parse(_dc.chkNull_0(dr["DAMAGE_UNITS"].ToString())) > 0)
                {
                    _isDamage = true;
                }
                
                }
                #endregion
                txtIssueStockValue.Text = Math.Round(totalIssueValue, 2).ToString(CultureInfo.InvariantCulture);
                txtTotalDamageValue.Text = Math.Round(totalDamageValue, 2).ToString(CultureInfo.InvariantCulture);
                txtTotalSchemeValue.Text =Math.Round(totalSchemeValue,2).ToString(CultureInfo.InvariantCulture);
                txtTotalRedemptionOut.Text = Math.Round(totalRedemptionValue, 2).ToString(CultureInfo.InvariantCulture);
               
            }
            else
            {
              LoadDistributor();
              LoadPrincipal();
              LoadArea();
              LoadOrderBooker();
              LoadDeliveryman();
              LoadSkuDetail();
              CreatTable();
              hfDisableCtrl.Value = "";
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
            txtIssueStockValue.Attributes.Add("Readonly", "Readonly");
            txtTaxCollected.Attributes.Add("Readonly", "Readonly");
            TxtTotalAmount.Attributes.Add("Readonly", "Readonly");
            
            if (hfDisableCtrl.Value == "")
            {
                EnableDisableController(false);
            }
            else
            {
                CreateTableSkus();                
            }
        }
  
    }
    #region Load 

    private void LoadDistributor()
    {
        int x = int.Parse( Session["CompanyId"].ToString());
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse( Session["UserId"].ToString()), int.Parse( Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList( drpDistributor, dt, 0, 2, true);

    }
    private void LoadPrincipal()
    {

        SKUPriceDetailController PController = new SKUPriceDetailController();
        DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse( Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse( Session["CurrentWorkDate"].ToString()));
        clsWebFormUtil.FillDropDownList( DrpPrincipal, m_dt, 0, 1, true);
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
            DataTable m_dt = mDController.SelectSaleForceAssignedArea(Constants.SALES_FORCE_ORDERBOOKER, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpRoute.SelectedValue.ToString()), int.Parse( Session["CompanyId"].ToString()), Convert.ToInt32(DrpPrincipal.SelectedValue));
            clsWebFormUtil.FillDropDownList( DrpOrderBooker, m_dt, 0, 3, true);
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
            DataTable mDt = mDController.SelectSaleForceAssignedArea(int.Parse(drpDistributor.SelectedValue), int.Parse(DrpRoute.SelectedValue), int.Parse( Session["CompanyId"].ToString()));
            clsWebFormUtil.FillDropDownList( DrpDeliveryMan, mDt, 0, 3, true);
        }
        else
        {
            DrpDeliveryMan.Items.Clear();
        }
    }
    private void LoadSkuDetail()
    {
        var pController = new SKUPriceDetailController();
        if (int.Parse(DrpPrincipal.SelectedValue) > 0)
        {
            //int x = int.Parse(DrpPrincipal.SelectedValue);
           // int y = int.Parse(drpDistributor.SelectedValue);
           // int z = int.Parse(Session["UserId"].ToString());
          //  DateTime d = DateTime.Parse(Session["CurrentWorkDate"].ToString());
            //   DataTable Dtsku_Price = PController.SelectDataPrice(int.Parse( Session["PrincipalId"].ToString()), Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse( Session["DistributorId"].ToString()), int.Parse( Session["UserId"].ToString()), Constants.IntNullValue, 1, DateTime.Parse( Session["CurrentWorkDate"].ToString()));
            DataTable dtskuPrice = pController.SelectDataPrice2(int.Parse(DrpPrincipal.SelectedValue), Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(drpDistributor.SelectedValue), int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 1, DateTime.Parse(Session["CurrentWorkDate"].ToString()));
            clsWebFormUtil.FillListBox(lstCode, dtskuPrice, 10, 10, true);
            Session.Add("Dtsku_Price", dtskuPrice);
        }
    }

    #endregion
    #region DataTable Creation

    private void CreateTableSkus()
    {
        _purchaseSku2 = new DataTable();
        _purchaseSku2.Columns.Add("SKU_CODE", typeof(string));
        Session.Add("_purchaseSku2", _purchaseSku2);
    }

    private void CreatTable()
    {
        _purchaseSku = new DataTable();
        _purchaseSku.Columns.Add("SALE_ORDER_DETAIL_ID", typeof(long));
        _purchaseSku.Columns.Add("DistributorId", typeof(int));
        _purchaseSku.Columns.Add("SALE_ORDER_ID", typeof(int));
        _purchaseSku.Columns.Add("SKU_ID", typeof(int));
        _purchaseSku.Columns.Add("SKU_CODE", typeof(string));
        _purchaseSku.Columns.Add("SKU_Name", typeof(string));
        _purchaseSku.Columns.Add("BATCH_NO", typeof(string));
        _purchaseSku.Columns.Add("UNIT_PRICE", typeof(decimal));

        _purchaseSku.Columns.Add("ISSUE_CTN", typeof(int));
        _purchaseSku.Columns.Add("ISSUE_UNITS", typeof(int));
        _purchaseSku.Columns.Add("RETURN_UNITS", typeof(int));
        _purchaseSku.Columns.Add("GROSS_UNITS", typeof(int));
        _purchaseSku.Columns.Add("DAMAGE_UNITS", typeof(int));
        _purchaseSku.Columns.Add("SCHEME_UNITS", typeof(int));
        _purchaseSku.Columns.Add("REDEMPTION_OUT_UNITS", typeof(int));
        _purchaseSku.Columns.Add("NET_SALE", typeof(int));
        _purchaseSku.Columns.Add("NET_VALUE", typeof(int));
        _purchaseSku.Columns.Add("TOTAL_UNITS", typeof(int));
        _purchaseSku.Columns.Add("PACKING", typeof(string));
        _purchaseSku.Columns.Add("RETURN_EMPTY_UNITS", typeof(int));

        _purchaseSku.Columns.Add("AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("STANDARD_DISCOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("STANDARD_DISCOUNT_PER", typeof(decimal));
        _purchaseSku.Columns.Add("EXTRA_DISCOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("RETAIL_AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("GST_RATE", typeof(decimal));
        _purchaseSku.Columns.Add("GST_RATE_TP", typeof(decimal));
        _purchaseSku.Columns.Add("GST_AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("TST_AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("CLAIM_EXTRA_AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("CLAIM_STANDARD_DISCOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("CLAIM_PER", typeof(decimal));
        _purchaseSku.Columns.Add("SED_AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("NET_AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("QUANTITY_CTN", typeof(decimal));
        _purchaseSku.Columns.Add("IS_DELETED", typeof(bool));

        _purchaseSku.Columns.Add("TOTAL_AMOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("UNCLAIMABLE_DISCOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("WHOLESALE_DISCOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("TAX_COLLECTED", typeof(decimal));
        _purchaseSku.Columns.Add("BRD", typeof(decimal));
        _purchaseSku.Columns.Add("TRADE_OFFERS", typeof(decimal));
        _purchaseSku.Columns.Add("OTHER_DISCOUNT", typeof(decimal));
        _purchaseSku.Columns.Add("CLAIMABLE_DISCOUNT", typeof(decimal));

         Session.Add("PurchaseSKU", _purchaseSku);

        _purchaseSku1 = new DataTable();
        _purchaseSku1.Columns.Add("SKU_CODE", typeof(string));
         Session.Add("PurchaseSKU1", _purchaseSku1);
    }
    private void CreateFreeSku()
    {
        _dtFreeSku = new DataTable();
        _dtFreeSku.Columns.Add("SKU_ID", typeof(int));
        _dtFreeSku.Columns.Add("SKU_Code", typeof(string));
        _dtFreeSku.Columns.Add("SKU_Name", typeof(string));
        _dtFreeSku.Columns.Add("UNIT_PRICE", typeof(decimal));
        _dtFreeSku.Columns.Add("Quantity", typeof(int));
        _dtFreeSku.Columns.Add("AMOUNT", typeof(decimal));
        _dtFreeSku.Columns.Add("GST_RATE", typeof(decimal));
        _dtFreeSku.Columns.Add("GST_AMOUNT", typeof(decimal));
        _dtFreeSku.Columns.Add("TST_AMOUNT", typeof(decimal));
        _dtFreeSku.Columns.Add("PROMOTION_ID", typeof(int));
        _dtFreeSku.Columns.Add("BASKET_ID", typeof(int));
        _dtFreeSku.Columns.Add("BASKET_DETAIL_ID", typeof(int));
        _dtFreeSku.Columns.Add("PROMOTION_OFFER_ID", typeof(int));
         Session.Add("dtFreeSKU", _dtFreeSku);
    }

    #endregion
    #region Grid

    private void LoadGird()
    {
        _purchaseSku = (DataTable) Session["PurchaseSKU"];
        GrdPurchase.DataSource = _purchaseSku;
        GrdPurchase.DataBind();
    }
    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {

        _rowId = e.NewEditIndex;
        txtskuCode.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[1].Text;
        txtskuName.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[2].Text;
        txtPacking.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[3].Text;
        txtTP.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[4].Text;
        txtIssueCtn.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[5].Text;
        txtIssueUnits.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[6].Text;
        txtReturnUnits.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[7].Text;
        txtReturnEmpty.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[8].Text;
        txtGrossSaleUnits.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[9].Text;
        txtDamegeUnits.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[10].Text;
        txtSchemeUnits.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[11].Text;
        txtredemptionUnits .Text = GrdPurchase.Rows[e.NewEditIndex].Cells[12].Text;
        txtNetSaleUnits.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[13].Text;
        txtNetValue.Text = GrdPurchase.Rows[e.NewEditIndex].Cells[14].Text;

        txtskuCode.Enabled = false;
        btnAddSku.Text = "Update SKU";
        btnCalculate.Enabled = false;
        btnSaveOrder.Enabled = false;
        if (txtskuCode.Text == "C201" || txtskuCode.Text == "C202" || txtskuCode.Text == "C203" || txtskuCode.Text == "C204" || txtskuCode.Text == "C205" || txtskuCode.Text == "C206" || txtskuCode.Text == "C207")
        {
            txtReturnEmpty.Enabled = true;
            txtIssueCtn.Enabled = false;
            txtReturnUnits.Enabled = false;
            txtDamegeUnits.Enabled = false;
            txtSchemeUnits.Enabled = false;
            txtredemptionUnits.Enabled =false;

        }
        else
        {
            txtReturnEmpty.Enabled = false;
            txtIssueCtn.Enabled = true;
            txtReturnUnits.Enabled = true;
            txtDamegeUnits.Enabled = true;
            txtSchemeUnits.Enabled = true;
            txtredemptionUnits.Enabled = true;
        }
        _purchaseSku2 = (DataTable)Session["_purchaseSku2"];
        if (_purchaseSku2 != null)
        {
            DataRow[] foundRows = _purchaseSku2.Select("SKU_CODE  = '" + txtskuCode.Text + "'");
            if (foundRows.Length == 0)
            {
                EnableDisableController(true);
            }
            else
            {
                txtDamegeUnits.Focus();
            }
        }
        else
        {
            EnableDisableController(true);
        }

    }
    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        _purchaseSku = (DataTable) Session["PurchaseSKU"];
        _purchaseSku.Rows.RemoveAt(e.RowIndex);
        
         Session.Add("PurchaseSKU", _purchaseSku);

         _purchaseSku2 = (DataTable)Session["_purchaseSku2"];
        if (_purchaseSku2 != null)
        {
            if (_purchaseSku2.Rows.Count > 0)
            {
                int row = e.RowIndex;

                DataRow[] foundRows = _purchaseSku2.Select("SKU_CODE  = '" + GrdPurchase.Rows[row].Cells[1].Text + "'");
                if (foundRows.Length > 0)
                {
                    foreach (DataRow frow in foundRows)
                    {
                        _purchaseSku2.Rows.Remove(frow);
                    }
                   
                    Session.Add("_purchaseSku2", _purchaseSku2);
                }
            }
        }
        LoadGird();
        btnCalculate.Enabled = true;
        btnSaveOrder.Enabled = false;
        if (hfDisableCtrl.Value == "")
        {
            EnableDisableController(true);
        }
        
    }

    #endregion
    #region Sel/Index Change
    
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
         LoadArea();
         LoadOrderBooker();
         LoadDeliveryman();
         LoadSkuDetail();
         Session.Add("DistributorId", int.Parse(drpDistributor.SelectedValue.ToString()));
    }
    protected void DrpPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
         LoadOrderBooker();
         Session.Add("PrincipalId", int.Parse(DrpPrincipal.SelectedValue.ToString()));
         LoadSkuDetail();
         LoadDeliveryman();
    }
    protected void DrpRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
         Session.Add("Route", DrpRoute.SelectedItem.Text);
         LoadOrderBooker();
         LoadDeliveryman();
    }
    protected void DrpOrderBooker_SelectedIndexChanged(object sender, EventArgs e)
    {
         Session.Add("OrderBookerId", int.Parse(DrpOrderBooker.SelectedValue.ToString()));
    }
    protected void DrpDeliveryMan_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session.Add("DeliveryManId", int.Parse(DrpDeliveryMan.SelectedValue));
        LoadSkuDetail();
    }
   

    #endregion
    private bool CheckDublicateSku()
    {
        _purchaseSku = (DataTable) Session["PurchaseSKU"];
        DataRow[] foundRows = _purchaseSku.Select("SKU_CODE  = '" + txtskuCode.Text + "'");
        if (foundRows.Length == 0)
        {
            return true;
        }
        return false;
    }    
    #region Click Operations

    protected void btnAddSku_Click(object sender, EventArgs e)
    {
        var dtskuPrice = (DataTable)Session["Dtsku_Price"];
        _purchaseSku = (DataTable)Session["PurchaseSKU"];
        DataRow[] foundRows = dtskuPrice.Select("SKU_CODE  = '" + txtskuCode.Text + "'");
        // decimal mTradePrice = decimal.Parse(dc.chkNull_0(foundRows[0]["TRADE_PRICE"].ToString()));

        //   int mPackSize = int.Parse(dc.chkNull_0(foundRows[0]["UNITS_IN_CASE"].ToString()));
        
        //packsize use in txtissuectn textchange event

        #region Btn text Add Sku
        if (btnAddSku.Text == "Add Sku")
        {
            if (CheckDublicateSku())
            {
                DataRow dr = _purchaseSku.NewRow();
                dr["SKU_ID"] = foundRows[0]["SKU_ID"];
                dr["SKU_Code"] = foundRows[0]["SKU_CODE"];
                dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
                dr["PACKING"] = foundRows[0]["PACKSIZE"];
                dr["UNIT_PRICE"] = decimal.Parse(foundRows[0]["TRADE_PRICE"].ToString());
                dr["ISSUE_CTN"] = int.Parse(_dc.chkNull_0(txtIssueCtn.Text));
                dr["ISSUE_UNITS"] = int.Parse(_dc.chkNull_0(txtIssueUnits.Text));
                dr["RETURN_UNITS"] = int.Parse(_dc.chkNull_0(txtReturnUnits.Text));
                dr["RETURN_EMPTY_UNITS"] = int.Parse(_dc.chkNull_0(txtReturnEmpty.Text));
                dr["GROSS_UNITS"] = int.Parse(_dc.chkNull_0(txtGrossSaleUnits.Text));
                dr["DAMAGE_UNITS"] = int.Parse(_dc.chkNull_0(txtDamegeUnits.Text));
                dr["SCHEME_UNITS"] = int.Parse(_dc.chkNull_0(txtSchemeUnits.Text));
                dr["REDEMPTION_OUT_UNITS"] = int.Parse(_dc.chkNull_0(txtredemptionUnits.Text));
                dr["NET_SALE"] = int.Parse(_dc.chkNull_0(txtNetSaleUnits.Text));
                dr["NET_VALUE"] = decimal.Parse(_dc.chkNull_0(txtNetValue.Text));

                #region GST


                if (foundRows[0]["GST_ON"].ToString().Trim() == "T")
                {
                    // dr["AMOUNT"] = mTradePrice * decimal.Parse(dr["QUANTITY_UNIT"].ToString());
                    dr["GST_RATE"] = foundRows[0]["GST_RATE_TP"];
                    decimal x = decimal.Parse(foundRows[0]["GST_RATE_TP"].ToString());
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

                if (int.Parse(_dc.chkNull_0(txtDamegeUnits.Text)) > 0)
                {
                    _isDamage = true;
                    //Added by hassan
                    if (int.Parse(Session["SaleInvoiceID"].ToString()) != -1)
                    {
                        _purchaseSku2 = (DataTable)Session["_purchaseSku2"];
                        DataRow dr2 = _purchaseSku2.NewRow();
                        dr2["SKU_Code"] = foundRows[0]["SKU_CODE"];
                        _purchaseSku2.Rows.Add(dr2);

                        Session.Add("_purchaseSku2", _purchaseSku2);
                    }
                    //--------------------------------\\
                }
                _purchaseSku.Rows.Add(dr);

            }

            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('SKU Already Exists')", true);
            }
        }
        #endregion
        #region btn update Sku
        else
        {
            DataRow dr = _purchaseSku.Rows[_rowId];
            dr["SKU_ID"] = foundRows[0]["SKU_ID"];
            dr["SKU_CODE"] = foundRows[0]["SKU_CODE"];
            dr["SKU_Name"] = foundRows[0]["SKU_NAME"];
            dr["UNIT_PRICE"] = decimal.Parse(foundRows[0]["TRADE_PRICE"].ToString());
            dr["ISSUE_CTN"] = int.Parse(_dc.chkNull_0(txtIssueCtn.Text));
            dr["ISSUE_UNITS"] = int.Parse(_dc.chkNull_0(txtIssueUnits.Text));
            dr["RETURN_UNITS"] = int.Parse(_dc.chkNull_0(txtReturnUnits.Text));
            dr["RETURN_EMPTY_UNITS"] = int.Parse(_dc.chkNull_0(txtReturnEmpty.Text));
            dr["GROSS_UNITS"] = int.Parse(_dc.chkNull_0(txtGrossSaleUnits.Text));
            dr["DAMAGE_UNITS"] = int.Parse(_dc.chkNull_0(txtDamegeUnits.Text));
            dr["SCHEME_UNITS"] = int.Parse(_dc.chkNull_0(txtSchemeUnits.Text));
            dr["REDEMPTION_OUT_UNITS"] = int.Parse(_dc.chkNull_0(txtredemptionUnits.Text));
            dr["NET_SALE"] = decimal.Parse(_dc.chkNull_0(txtNetSaleUnits.Text));
            dr["NET_VALUE"] = decimal.Parse(_dc.chkNull_0(txtNetValue.Text));

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

            if (int.Parse(_dc.chkNull_0(txtDamegeUnits.Text)) > 0)
            {
                _isDamage = true;
                
            }
        }
        #endregion
        Session.Add("PurchaseSKU", _purchaseSku);
        decimal objIssuevalue = _purchaseSku.Rows.Cast<DataRow>().Sum(dr => decimal.Parse(_dc.chkNull_0(dr["UNIT_PRICE"].ToString())) * decimal.Parse(_dc.chkNull_0(dr["ISSUE_UNITS"].ToString())));
        //foreach (DataRow dr in PurchaseSKU.Rows)
        //{
        //    objGrossSales += decimal.Parse(dc.chkNull_0(dr["UNIT_PRICE"].ToString())) * decimal.Parse(dc.chkNull_0(dr["ISSUE_AMOUNT"].ToString()));
        //}
        txtIssueStockValue.Text = Convert.ToString(Math.Round(objIssuevalue, 2));

        LoadGird();
        ClearAll();
        var scriptManager = ScriptManager.GetCurrent(Page);
        if (scriptManager != null) scriptManager.SetFocus(txtskuCode);
        txtReturnEmpty.Enabled = true;
        txtIssueCtn.Enabled = true;
        txtReturnUnits.Enabled = true;
        txtDamegeUnits.Enabled = true;
        txtSchemeUnits.Enabled = true;
        txtredemptionUnits.Enabled = true;

        if (hfDisableCtrl.Value == "")
        {
            EnableDisableController(true);
        }

    }
    protected void btnSaveOrder_Click(object sender, EventArgs e)
       
    {

        const long saleOrderId = Constants.LongNullValue;
        var pc = new PurchaseController();
        DataTable loadpass = pc.selectLoadPassDocumentNo(int.Parse(drpDistributor.SelectedValue), int.Parse(DrpRoute.SelectedValue), long.Parse(DrpRoute.SelectedValue), int.Parse(DrpPrincipal.SelectedValue), int.Parse(DrpOrderBooker.SelectedValue), int.Parse(DrpDeliveryMan.SelectedValue), DateTime.Parse( Session["CurrentWorkDate"].ToString()));

        if (IsDayClosed())
        {
            var userCtl = new UserController();

            userCtl.InsertUserLogoutTime(int.Parse(drpDistributor.SelectedValue), Convert.ToInt32(Session["UserID"]));
             Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("../Login.aspx");
        }
        else
        {
            
            string manualId = null;
           
            
            _purchaseSku = (DataTable) Session["PurchaseSKU"];
            _dtFreeSku = (DataTable) Session["dtFreeSKU"];
            var mOrderController = new OrderEntryController();
            if (btnSaveOrder.Text == "Save")
            {
              
                if (loadpass.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this,  GetType(), "msg","alert('Load Pass Already Exist against this Delivery Man');", true);

                }
                else
                {
                    #region Sale Invoice Portion
                    /////////////////////////////////////////////////////////////////// Invoice Portion///////////////////////// 
                    foreach (DataRow dr in _purchaseSku.Rows)
                    {
                        var mController = new PhaysicalStockController();
                        DataTable dtstock = mController.SelectSKUClosingStock(int.Parse(drpDistributor.SelectedValue), int.Parse(dr["SKU_ID"].ToString()), "", DateTime.Parse( Session["CurrentWorkDate"].ToString()));
                        if (dtstock.Rows.Count > 0)
                        {
                            if (int.Parse(dtstock.Rows[0][0].ToString()) < int.Parse(dr["ISSUE_UNITS"].ToString()))
                            {
                                ScriptManager.RegisterStartupScript(this,  GetType(), "msg", "alert('" + dr["SKU_Code"] + " Current Stock is " + dtstock.Rows[0][0] + "');", true);
                                return;
                            }
                            else if (Convert.ToDecimal(dtstock.Rows[0][1]) != Convert.ToDecimal(dr["UNIT_PRICE"]))
                            {
                                ScriptManager.RegisterStartupScript(this,  GetType(), "msg", "alert('" + dr["SKU_Code"] + " Order Rate is " + dr["UNIT_PRICE"] + " and Current Rate is " + dtstock.Rows[0][1] + "');", true);
                                return;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this,  GetType(), "msg", "alert(' + " + dr["SKU_Code"] + "No Stock Found');", true);
                            return;
                        }
                    }

                    var isValidInsert1 = mOrderController.Add_Invoice1(int.Parse(drpDistributor.SelectedValue), manualId, int.Parse(DrpRoute.SelectedValue), long.Parse(DrpRoute.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue), 0, 0, int.Parse(DrpOrderBooker.SelectedValue.ToString()), int.Parse(DrpDeliveryMan.SelectedValue), 0,
                    decimal.Parse(_dc.chkNull_0(txtnumGrossSale.Text)), decimal.Parse(_dc.chkNull_0(txtUnclaimAbleDiscount.Text)), decimal.Parse(_dc.chkNull_0(txtClaimableDiscounts.Text)), decimal.Parse(_dc.chkNull_0(txtBRD.Text)), decimal.Parse(_dc.chkNull_0(txtGSTAmount.Text)), decimal.Parse(_dc.chkNull_0(txtNetAmount.Text)), decimal.Parse(_dc.chkNull_0(txtOtherDiscounts.Text)),
                    Constants.Order_Posted_Id, _purchaseSku, _dtFreeSku, int.Parse( Session["UserId"].ToString()), 0, DateTime.Parse( Session["CurrentWorkDate"].ToString()), decimal.Parse(_dc.chkNull_0(txtTradeOffers.Text)), decimal.Parse(_dc.chkNull_0(txtTSTAmount.Text)), int.Parse(_dc.chkNull_0(txtVisit.Text)), int.Parse(_dc.chkNull_0(txtProductiveCall.Text)),
                    int.Parse(_dc.chkNull_0(txtOpeningReading.Text)), int.Parse(_dc.chkNull_0(txtCloseReading.Text)), int.Parse(_dc.chkNull_0(txtBillNoFrom.Text)), int.Parse(_dc.chkNull_0(txtBillNoTo.Text)), decimal.Parse(_dc.chkNull_0(txtWholeSaleDiscount.Text)), decimal.Parse(_dc.chkNull_0(txtTaxCollected.Text)), decimal.Parse(_dc.chkNull_0(txtTotalDamageValue.Text)), _isDamage);



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
                    int dmID=Convert.ToInt32(Session["DeliveryManID"]);
                    int sID=Convert.ToInt32(Session["SaleInvoiceID"]);
                    DataRow[] foundRows = loadpass.Select("DELIVERYMAN_ID ='" + dmID + "' AND SALE_INVOICE_ID <>'" + sID + "'");
                    if (foundRows.Length > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Load Pass Already Exist against this Delivery Man');", true);

                    }
                   
                    else
                    {
                        #region saleInvoice Master/detail Updation
                        var isValidInsert1 = mOrderController.Update_Invoice(long.Parse(Session["SaleInvoiceID"].ToString()), int.Parse(drpDistributor.SelectedValue), manualId, int.Parse(DrpRoute.SelectedValue), long.Parse(DrpRoute.SelectedValue), int.Parse(DrpPrincipal.SelectedValue), 0, 0,
                            int.Parse(DrpOrderBooker.SelectedValue), int.Parse(DrpDeliveryMan.SelectedValue), saleOrderId,
                             decimal.Parse(_dc.chkNull_0(txtnumGrossSale.Text)), decimal.Parse(_dc.chkNull_0(txtUnclaimAbleDiscount.Text)),
                             decimal.Parse(_dc.chkNull_0(txtClaimableDiscounts.Text)), decimal.Parse(_dc.chkNull_0(txtBRD.Text)), decimal.Parse(_dc.chkNull_0(txtGSTAmount.Text)),
                             decimal.Parse(_dc.chkNull_0(txtNetAmount.Text)), decimal.Parse(_dc.chkNull_0(txtOtherDiscounts.Text)), Constants.Order_Posted_Id, _purchaseSku, _dtFreeSku, int.Parse(Session["UserId"].ToString()), Constants.DecimalNullValue, DateTime.Parse(Session["CurrentWorkDate"].ToString()),
                             decimal.Parse(_dc.chkNull_0(txtTradeOffers.Text)), decimal.Parse(_dc.chkNull_0(txtTSTAmount.Text)), int.Parse(_dc.chkNull_0(txtVisit.Text)), int.Parse(_dc.chkNull_0(txtProductiveCall.Text)), int.Parse(_dc.chkNull_0(txtOpeningReading.Text)), int.Parse(_dc.chkNull_0(txtCloseReading.Text)),
                             int.Parse(_dc.chkNull_0(txtBillNoFrom.Text)), int.Parse(_dc.chkNull_0(txtBillNoTo.Text)), decimal.Parse(_dc.chkNull_0(txtWholeSaleDiscount.Text)), decimal.Parse(_dc.chkNull_0(txtTaxCollected.Text)), decimal.Parse(_dc.chkNull_0(txtTotalDamageValue.Text)), _isDamage);



                        if (isValidInsert1)
                        {
                            ClearMasterAll();

                        }

                        Response.Redirect("~/Forms/frmLoadPassEntry.aspx?Status=" + false + "&LevelType=3&LevelID=" + Request.QueryString["LevelID"]);
                        #endregion

                    }
                }
            }




        }

    }
  
    protected void btnCalculate_Click(object sender, EventArgs e)
    {
       
        decimal objGrossSales = 0;
        decimal objTotalDamageValue = 0;
        decimal objTotalSchemeValue = 0;
        decimal objTotalRedemptionValue = 0;
        decimal objTotalGst = 0;
        decimal objTotalTst = 0;
        
        
        var dt = (DataTable) Session["PurchaseSKU"];
        foreach (DataRow dr in dt.Rows)
        {
            objGrossSales += ((decimal.Parse(_dc.chkNull_0(dr["GROSS_UNITS"].ToString ()))) *(decimal.Parse(_dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));

            objTotalDamageValue += ((decimal.Parse(_dc.chkNull_0(dr["DAMAGE_UNITS"].ToString()))) * (decimal.Parse(_dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));

            objTotalSchemeValue += ((decimal.Parse(_dc.chkNull_0(dr["SCHEME_UNITS"].ToString()))) * (decimal.Parse(_dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));
            objTotalRedemptionValue += ((decimal.Parse(_dc.chkNull_0(dr["REDEMPTION_OUT_UNITS"].ToString()))) * (decimal.Parse(_dc.chkNull_0(dr["UNIT_PRICE"].ToString()))));
            //
            #region GST Calculation
            decimal tempAmount = 0;
       

            tempAmount = decimal.Parse(_dc.chkNull_0(dr["NET_VALUE"].ToString()));
      
            dr["GST_AMOUNT"] = (tempAmount) * decimal.Parse(_dc.chkNull_0(dr["GST_RATE"].ToString())) / 100;
            objTotalTst += decimal.Parse(_dc.chkNull_0(dr["TST_AMOUNT"].ToString()));
            objTotalGst += decimal.Parse(_dc.chkNull_0(dr["GST_AMOUNT"].ToString()));
         
            #endregion

        }

        txtGSTAmount.Text = Convert.ToString(Math.Round(objTotalGst, 2));
        txtTSTAmount.Text = Convert.ToString(Math.Round(objTotalTst, 2));
        txtTotalDamageValue.Text = Convert.ToString(Math.Round(objTotalDamageValue, 2));
        txtTotalSchemeValue.Text = Convert.ToString(Math.Round(objTotalSchemeValue, 2));
        txtTotalRedemptionOut.Text = Convert.ToString(Math.Round(objTotalRedemptionValue, 2));

        txtnumGrossSale.Text =Math .Round (objGrossSales,2).ToString();
        txtClaimableDiscounts.Text =Math .Round (decimal.Parse(_dc.chkNull_0(txtWholeSaleDiscount.Text)) + decimal.Parse(_dc.chkNull_0(txtBRD.Text)) + decimal.Parse(_dc.chkNull_0(txtTradeOffers.Text)) + decimal.Parse(_dc.chkNull_0(txtOtherDiscounts.Text)),2).ToString ();
       //
        TxtTotalAmount.Text = Math.Round(decimal.Parse(_dc.chkNull_0(txtnumGrossSale.Text)) - decimal.Parse(_dc.chkNull_0(txtUnclaimAbleDiscount.Text)) - decimal.Parse(_dc.chkNull_0(txtClaimableDiscounts.Text)) + decimal.Parse(_dc.chkNull_0(txtGSTAmount.Text)) + decimal.Parse(_dc.chkNull_0(txtTSTAmount.Text))  - decimal.Parse(_dc.chkNull_0(txtTotalDamageValue.Text)) - decimal.Parse(_dc.chkNull_0(txtTotalSchemeValue.Text)) - decimal.Parse(_dc.chkNull_0(txtTotalRedemptionOut.Text)), 2).ToString();
        txtTaxCollected.Text = Math.Round(decimal.Parse(_dc.chkNull_0(TxtTotalAmount.Text)) * decimal.Parse(_dc.chkNull_0(txtTaxCollectedPer.Text)) / 100,2).ToString();
        //
        txtNetAmount.Text = Math.Round(decimal.Parse(_dc.chkNull_0(txtnumGrossSale.Text)) - decimal.Parse(_dc.chkNull_0(txtUnclaimAbleDiscount.Text)) - decimal.Parse(_dc.chkNull_0(txtClaimableDiscounts.Text)) + decimal.Parse(_dc.chkNull_0(txtGSTAmount.Text)) + decimal.Parse(_dc.chkNull_0(txtTSTAmount.Text)) + decimal.Parse(_dc.chkNull_0(txtTaxCollected.Text)) - decimal.Parse(_dc.chkNull_0(txtTotalDamageValue.Text)) - decimal.Parse(_dc.chkNull_0(txtTotalSchemeValue.Text)) - decimal.Parse(_dc.chkNull_0(txtTotalRedemptionOut.Text)), 2).ToString();
       
        btnSaveOrder.Enabled = true;
       
    }

    #endregion
    private void EnableDisableController(bool cValue)
    {
        txtVisit.Attributes.Remove("Readonly");
        txtProductiveCall.Attributes.Remove("Readonly");
        txtCloseReading.Attributes.Remove("Readonly");
        txtBillNoTo.Attributes.Remove("Readonly");
        txtUnclaimAbleDiscount.Attributes.Remove("Readonly");
        txtWholeSaleDiscount.Attributes.Remove("Readonly");
        txtBRD.Attributes.Remove("Readonly");
        txtTradeOffers.Attributes.Remove("Readonly");
        txtOtherDiscounts.Attributes.Remove("Readonly");
        txtTaxCollected.Attributes.Remove("Readonly");
        txtReturnUnits.Enabled = true;
        txtReturnEmpty.Enabled = true;
        txtDamegeUnits.Enabled = true;
        txtSchemeUnits.Enabled = true;
        txtredemptionUnits.Enabled = true;                
    }
    private void ClearAll()
    {
        txtskuCode.Text = "";
        txtskuName.Text = "";
        txtPacking.Text = "";
        txtTP.Text = "";
        txtIssueCtn.Text = "";
        txtIssueUnits.Text = "";
        txtReturnUnits.Text = "";
        txtGrossSaleUnits.Text = "";
        txtDamegeUnits.Text = "";
        txtSchemeUnits.Text = "";
        txtredemptionUnits.Text = "";
        txtNetSaleUnits.Text = "";
        txtNetAmount.Text = "";
        txtNetValue.Text = "";
        txtReturnEmpty.Text = "";
        btnAddSku.Text = "Add Sku";
        btnSaveOrder.Enabled = false;
        btnCalculate.Enabled = true;
        txtskuCode.Enabled = true;
        var scriptManager = ScriptManager.GetCurrent(Page);
        if (scriptManager != null) scriptManager.SetFocus(txtskuCode);
    }
    private void ClearMasterAll()
    {
        if (hfDisableCtrl.Value == "")
        {
            EnableDisableController(true);
        }
        Session.Remove("PurchaseSKU");
        Session.Remove("_purchaseSku2");
        Session.Remove("dtFreeSKU");
        Session.Remove("DeliveryManID");
        CreatTable();
        CreateFreeSku();
        LoadGird();


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
        _rowId = 0;

        Session.Remove("SaleInvoiceID");
        var scriptManager = ScriptManager.GetCurrent(Page);
        if (scriptManager != null) scriptManager.SetFocus(txtVisit);
    }
    private bool IsDayClosed()
    {
        bool flag;
        var distrCtl = new DistributorController();
        DataTable dtDayClose = distrCtl.MaxDayClose(int.Parse(drpDistributor.SelectedValue), 3);
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
}