using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAMSBusinessLayer.Classes;
using SAMSCommon.Classes;

/// <summary>
/// Form To Add, Edit Cheques
/// </summary>
public partial class Forms_frmChequeEntry : System.Web.UI.Page
{
    /// <summary>
    /// Page_Load Function Populates All Combos, ListBox And Grid On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    private void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DrpStatus.Items.Add(new ListItem("Cheque Received", "527"));
            DrpStatus.Items.Add(new ListItem("Cheque Deposit", "528"));
            DrpStatus.Items.Add(new ListItem("Cheque Realize", "529"));
            DrpStatus.Items.Add(new ListItem("Cheque Bounce", "530"));
            DrpStatus.Items.Add(new ListItem("Cheque Cancel", "560"));
            // by safdar
            this.LoadDropdownConvertto();
            lblStatusConvertTo.Visible = false;
            DrpStatusConvertTo.Visible = false;

            DateTime pOrderDate = DateTime.Parse(this.Session["CurrentWorkDate"].ToString());
            txtToDate.Text = pOrderDate.ToString("dd/MMM/yyyy");
            CalendarExtender2.SelectedDate = pOrderDate;


            this.LoadAccountHead();
            this.LoadPrincipal();
            this.LoadDistributor();
            this.LoadArea();
            this.LoadData();
            this.SelectCreditInvoice();
            this.LoadReceviedCheque();
            this.LoadDeliveryman();
            btnSave.Attributes.Add("onclick", "return ValidateForm();");
            lblBankAccount.Visible = false;
            DrpBankAccount.Visible = false;
        }
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    /// 
    private void LoadDropdownConvertto()
    {
        DrpStatusConvertTo.Items.Clear();
      //  DrpStatusConvertTo.Items.Add(new ListItem("Cheque Received", "527"));
        DrpStatusConvertTo.Items.Add(new ListItem("Cheque Deposit", "528"));
        DrpStatusConvertTo.Items.Add(new ListItem("Cheque Realize", "529"));
        DrpStatusConvertTo.Items.Add(new ListItem("Cheque Bounce", "530"));
        DrpStatusConvertTo.Items.Add(new ListItem("Cheque Cancel", "560"));
    }


    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDropDownList(this.drpDistributor, dt, 0, 2, true);
    }

    /// <summary>
    /// Loads Routes To Route Combo
    /// </summary>
    private void LoadArea()
    {

        DistributorAreaController mController = new DistributorAreaController();
        DataTable dt = mController.SelectDist_Area(Constants.LongNullValue, Constants.DateNullValue, Constants.DateNullValue, int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, null, null);
        clsWebFormUtil.FillDropDownList(DrpRoute, dt, 0, 6, true);

    }

    /// <summary>
    /// Loads Customers To Customer Combo
    /// </summary>
    private void LoadData()
    {
        GrdCredit.DataSource = null;
        GrdCredit.DataBind();
        CustomerDataController mController = new CustomerDataController();

        if (DrpChequeType.SelectedIndex == 1)
        {
            DataTable dtCustomer = mController.SelectPrincipalCustomer(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpRoute.SelectedValue.ToString()), Constants.IntNullValue, int.Parse(DrpPrincipal.SelectedValue.ToString()));
            clsWebFormUtil.FillDropDownList(this.DrpCustomer, dtCustomer, 0, 4, true);
            DrpRoute.Enabled = true;
        }
        else
        {
            if (drpDistributor.Items.Count > 0)
            {
                LedgerController LedgerCtl = new LedgerController();
                DataTable dtCredit = LedgerCtl.SelectCreditPendingInvoice(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()), Constants.LongNullValue, Constants.IntNullValue);
                clsWebFormUtil.FillDropDownList(this.DrpCustomer, dtCredit, 0, 1, true);

                DrpRoute.Enabled = false;
            }
        }
    }

    /// <summary>
    /// Resets Form Controls
    /// </summary>
    private void ClearAll()
    {
        txtChequeNo.Text = "";
        txtAmount.Text = "";
        txtBankName.Text = "";
        txtStartDate.Text = "";
        btnSave.Text = "Save";
        txtReceivedDate.Text = "";
        txtSlipNo.Text = "";
        txtRemarks.Text = "";
        lblStatusConvertTo.Visible = false;
        DrpStatusConvertTo.Visible = false;
        DrpStatus.Enabled = true;


        lblsaleforce.Visible = true;
        DrpDeliveryMan.Visible = true;
        lblcustomer.Visible = true;
        DrpCustomer.Visible = true;
        lblroute.Visible = true;
        DrpRoute.Visible = true;
        GrdCredit.Visible = true;
        Panel1.Visible = true;

        //DrpPrincipal.Enabled = true;
        //DrpBankAccount.Enabled = true;
        //DrpChequeType.Enabled = true;
        //DrpRoute.Enabled = true;



    }

    /// <summary>
    /// Loads Principals To Principal Combo
    /// </summary>
    private void LoadPrincipal()
    {
        SKUPriceDetailController PController = new SKUPriceDetailController();
        DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
        clsWebFormUtil.FillDropDownList(this.DrpPrincipal, m_dt, 0, 1, true);
    }

    /// <summary>
    /// Loads Cheques To Grid
    /// </summary>
    private void LoadReceviedCheque()
    {
        if (DrpStatus.SelectedValue.ToString() != Constants.Cheque_Clear.ToString())
        {
            ChequeEntryController CController = new ChequeEntryController();
            DataTable dt = CController.SelectChequeEntry(int.Parse(DrpStatus.SelectedValue.ToString()), DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()), DrpChequeType.SelectedIndex);
            this.Session.Add("dt", dt);
            GrdOrder.DataSource = dt;
            GrdOrder.DataBind();
        }
        else
        {
            GrdOrder.DataSource = null;
            GrdOrder.DataBind();
        }
    }

    /// <summary>
    /// Loads Crdit Invoices To Grid
    /// </summary>
    private void SelectCreditInvoice()
    {
        LedgerController CDC = new LedgerController();
        GrdCredit.DataSource = null;
        GrdCredit.DataBind();
        if (DrpCustomer.Items.Count > 0 && DrpChequeType.SelectedIndex != 1)
        {
            DataTable dtCredit = CDC.SelectCreditPendingInvoice(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()), long.Parse(DrpCustomer.SelectedValue.ToString()), 0);
            GrdCredit.DataSource = dtCredit;
            GrdCredit.DataBind();
        }
    }
    
    /// <summary>
    /// Saves Cheque Realization
    /// </summary>
    private void ChequeRealization()
    {
        LedgerController LController = new LedgerController();
        string MaxDocumentId = LController.SelectLedgerMaxDocumentId(Constants.Bank_Voucher, int.Parse(drpDistributor.SelectedValue.ToString()));
        decimal OfferAmount = decimal.Parse(txtAmount.Text);
        if (DrpChequeType.SelectedIndex == 0)
        {
            foreach (GridViewRow dr in GrdCredit.Rows)
            {
                CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                if (chRelized.Checked == true)
                {
                    if (decimal.Parse(dr.Cells[3].Text) >= OfferAmount)
                    {
                        LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), 107, int.Parse(drpDistributor.SelectedValue.ToString()), 0, OfferAmount,
                        DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), "Cheque Relization", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()),
                        txtChequeNo.Text, int.Parse(this.Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), dr.Cells[1].Text, Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, DrpDeliveryMan.SelectedValue.ToString());

                        LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), OfferAmount, 0,
                        DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), "Cheque Relization", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()),
                        txtChequeNo.Text, int.Parse(this.Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), dr.Cells[1].Text, Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, DrpDeliveryMan.SelectedValue.ToString());

                        OfferAmount = decimal.Parse(dr.Cells[3].Text) - OfferAmount;
                        LController.UpdateSaleInvoice(Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), int.Parse(drpDistributor.SelectedValue.ToString()), OfferAmount);
                        break;
                    }
                    else if (decimal.Parse(dr.Cells[3].Text) <= OfferAmount)
                    {
                        LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), 107, int.Parse(drpDistributor.SelectedValue.ToString()), 0, decimal.Parse(dr.Cells[3].Text),
                        DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), "Cheque Relization", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()),
                        txtChequeNo.Text, int.Parse(this.Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), dr.Cells[1].Text, Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, DrpDeliveryMan.SelectedValue.ToString());

                        LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), decimal.Parse(dr.Cells[3].Text), 0,
                        DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), "Cheque Relization", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()),
                        txtChequeNo.Text, int.Parse(this.Session["UserId"].ToString()), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), dr.Cells[1].Text, Constants.Document_Invoice, txtSlipNo.Text, Constants.DateNullValue, 18, DrpDeliveryMan.SelectedValue.ToString());

                        OfferAmount = OfferAmount - decimal.Parse(dr.Cells[3].Text);
                        LController.UpdateSaleInvoice(Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), int.Parse(drpDistributor.SelectedValue.ToString()), 0);
                    }
                }
            }
        }
        else
        {
            LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), 107, int.Parse(drpDistributor.SelectedValue.ToString()), 0, decimal.Parse(txtAmount.Text),
                DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), "Cheque Advance", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()),
                txtChequeNo.Text, int.Parse(this.Session["UserId"].ToString()), Constants.LongNullValue, null, Constants.IntNullValue, txtSlipNo.Text, Constants.DateNullValue, 20, "");

            LController.PostingCash_Bank_Account(Constants.Bank_Voucher, long.Parse(MaxDocumentId), long.Parse(DrpBankAccount.SelectedValue.ToString()), int.Parse(drpDistributor.SelectedValue.ToString()), decimal.Parse(txtAmount.Text), 0,
                DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), "Cheque Advance", DateTime.Now, int.Parse(DrpCustomer.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()),
                txtChequeNo.Text, int.Parse(this.Session["UserId"].ToString()), Constants.LongNullValue, null, Constants.IntNullValue, txtSlipNo.Text, Constants.DateNullValue, 20, "");
        }

    }
    
    /// <summary>
    /// Loads Deliverymen To Deliverman Comb
    /// </summary>
    private void LoadDeliveryman()
    {
        if (drpDistributor.Items.Count > 0)
        {
            SaleForceController mDController = new SaleForceController();
           // DataTable m_dt = mDController.SelectSaleForceAssignedArea(int.Parse(drpDistributor.SelectedValue.ToString()), Constants.IntNullValue, int.Parse(this.Session["CompanyId"].ToString()));
           // clsWebFormUtil.FillDropDownList(this.DrpDeliveryMan, m_dt, 0, 3, true);

            
                Distributor_UserController UCtl = new Distributor_UserController();
                DataTable dt = UCtl.SelectDistributorUser(Constants.SALES_FORCE_DELIVERYMAN, int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(this.Session["CompanyId"].ToString()));
                clsWebFormUtil.FillDropDownList(this.DrpDeliveryMan, dt, 0, 6, true);
            

        }

    }
    
    /// <summary>
    /// Loads Account Heads To Account Combo
    /// </summary>
    private void LoadAccountHead()
    {
        SAMSCommon.Classes.Configuration.GetAccountHead();
        AccountHeadController mAccountController = new AccountHeadController();
        DataTable dt = mAccountController.SelectAccountHead(Constants.AC_AccountHeadId, long.Parse(SAMSCommon.Classes.Configuration.BankDefaultType));
        clsWebFormUtil.FillDropDownList(DrpBankAccount, dt, 0, 4, true);
    }

    /// <summary>
    /// Save Or Updates a Cheque
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (IsDayClosed())
        {
            UserController UserCtl = new UserController();

            UserCtl.InsertUserLogoutTime(Convert.ToInt32(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"]));
            this.Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("../Login.aspx");
        }
        else
        {

            ChequeEntryController CController = new ChequeEntryController();

            DateTime ChequeDate;

            if (txtStartDate.Text.Length == 10)
            {
                ChequeDate = DateTime.Parse(ConvertDate.British_To_American(txtStartDate.Text));

            }
            else
            {
                ChequeDate = DateTime.Now;
            }

            int InvoiceCount = Constants.IntNullValue;
            

            if (btnSave.Text == "Save")
            {


                foreach (GridViewRow dr in GrdCredit.Rows)
                {
                    CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                    if (chRelized.Checked == true)
                    {
                        InvoiceCount++;
                        break;
                    }
                }

                if (InvoiceCount == Constants.IntNullValue)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Must Select Invoice');", true);
                    return;
                }

                if (DrpStatus.SelectedIndex == 0)
                {
                    HFChqueProcessId.Value = CController.InsertChequeEntry(int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()), long.Parse(DrpCustomer.SelectedValue.ToString()), txtChequeNo.Text, txtBankName.Text, ChequeDate,
                        DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), Constants.DateNullValue, Constants.DateNullValue, decimal.Parse(txtAmount.Text), int.Parse(DrpStatus.SelectedValue.ToString()), DateTime.Now, DrpChequeType.SelectedIndex, txtSlipNo.Text, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedValue.ToString()));


                    foreach (GridViewRow dr in GrdCredit.Rows)
                    {
                        CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                        if (chRelized.Checked == true)
                        {
                            CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                        }
                    }
                    this.SelectCreditInvoice();
                }
            }
            else if (btnSave.Text == "Edit")
            {
                #region check pending
                if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Pending && txtReceivedDate.Text == DateTime.Parse(this.Session["CurrentWorkDate"].ToString()).ToString("dd/MM/yyyy"))
                {
                    CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()), long.Parse(DrpCustomer.SelectedValue.ToString()), txtChequeNo.Text, txtBankName.Text, ChequeDate, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), Constants.DateNullValue, Constants.DateNullValue,
                       decimal.Parse(txtAmount.Text), int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, int.Parse(DrpBankAccount.SelectedValue.ToString()));

                    CController.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 1);

                    foreach (GridViewRow dr in GrdCredit.Rows)
                    {
                        CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                        if (chRelized.Checked == true)
                        {
                            CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                        }
                    }
                }
                #endregion
                #region check deposit
                else if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Deposit)
                {
                    CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), Constants.DateNullValue,
                         Constants.DecimalNullValue, int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, int.Parse(DrpBankAccount.SelectedValue.ToString()));
                }
                #endregion
                #region check bons or cancel
                else if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Bons || int.Parse(DrpStatusConvertTo.SelectedValue.ToString()) == Constants.Cheque_Cancel)
                {
                    CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()),
                                                     Constants.DecimalNullValue, int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedValue.ToString()));

                }
                #endregion
                #region check clear
                else if (int.Parse(DrpStatus.SelectedValue.ToString()) == Constants.Cheque_Clear)
                {

                    CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()),
                                                     Constants.DecimalNullValue, int.Parse(DrpStatus.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedValue.ToString()));

                    CController.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 1);

                    foreach (GridViewRow dr in GrdCredit.Rows)
                    {
                        CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                        if (chRelized.Checked == true)
                        {
                            CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                        }
                    }
                    this.ChequeRealization();
                    this.LoadData();
                    this.SelectCreditInvoice();
                }
                #endregion

                this.ClearAll();
                this.LoadReceviedCheque();

            }
            else
            {

                // by safdar
            foreach (GridViewRow drow in GrdOrder .Rows)
            {
                ChequeEntryController Ccontroller = new ChequeEntryController();

                CheckBox chSelect = (CheckBox)drow.Cells[0].FindControl("ChbIsSelect");
                if (chSelect.Checked == true)
                {
                    HFChqueProcessId.Value = drow.Cells[0].Text;
                    
                    #region check pending
                    if (int.Parse(DrpStatusConvertTo.SelectedValue.ToString()) == Constants.Cheque_Pending && txtReceivedDate.Text == DateTime.Parse(this.Session["CurrentWorkDate"].ToString()).ToString("dd/MM/yyyy"))
                    {
                        CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()), long.Parse(DrpCustomer.SelectedValue.ToString()), txtChequeNo.Text, txtBankName.Text, ChequeDate, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), Constants.DateNullValue, Constants.DateNullValue,
                           decimal.Parse(txtAmount.Text), int.Parse(DrpStatusConvertTo.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, int.Parse(DrpBankAccount.SelectedValue.ToString()));

                        CController.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 1);

                        foreach (GridViewRow dr in GrdCredit.Rows)
                        {
                            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                            if (chRelized.Checked == true)
                            {
                                CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                            }
                        }
                    }
#endregion
                    #region check deposit
                    else if (int.Parse(DrpStatusConvertTo.SelectedValue.ToString()) == Constants.Cheque_Deposit)
                    {
                        long x = long.Parse(HFChqueProcessId.Value);
                        CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), Constants.DateNullValue,
                             Constants.DecimalNullValue, int.Parse(DrpStatusConvertTo.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, int.Parse(DrpBankAccount.SelectedValue.ToString()));
                    }
                    #endregion
                    #region check bons or cancel
                    else if (int.Parse(DrpStatusConvertTo.SelectedValue.ToString()) == Constants.Cheque_Bons || int.Parse(DrpStatusConvertTo.SelectedValue.ToString()) == Constants.Cheque_Cancel)
                    {
                        CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()),
                                                         Constants.DecimalNullValue, int.Parse(DrpStatusConvertTo.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedValue.ToString()));

                    }
#endregion
                    #region check clear
                    else if (int.Parse(DrpStatusConvertTo.SelectedValue.ToString()) == Constants.Cheque_Clear)
                    {

                        CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()),
                                                         Constants.DecimalNullValue, int.Parse(DrpStatusConvertTo.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, Constants.LongNullValue);

                      //  CController.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 1);   // will delete from check_process_detail
                     
                        foreach (GridViewRow dr in GrdCredit.Rows)
                        {
                            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                            if (chRelized.Checked == true)
                            {
                                CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
                            }
                        }

                        #region Load data on edit button

                        HFChqueProcessId.Value = drow.Cells[0].Text;
                        DrpRoute.SelectedValue = drow.Cells[12].Text;
                        this.LoadData();
                        DrpCustomer.SelectedValue = drow.Cells[1].Text;
                        txtChequeNo.Text = drow.Cells[4].Text;
                        txtBankName.Text = drow.Cells[5].Text;
                        txtStartDate.Text = drow.Cells[6].Text;
                        txtReceivedDate.Text = drow.Cells[7].Text;
                        txtAmount.Text = drow.Cells[9].Text;
                        txtSlipNo.Text = drow.Cells[10].Text;
                        txtRemarks.Text = drow.Cells[11].Text;
                        DrpBankAccount.SelectedValue = drow.Cells[14].Text;
                        DrpDeliveryMan.SelectedValue = drow.Cells[15].Text;
                        this.SelectCreditInvoice();
                        DataTable dt = Ccontroller.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 0);

                        foreach (GridViewRow dr in GrdCredit.Rows)
                        {
                            foreach (DataRow dbr in dt.Rows)
                            {

                                if (Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]) == Convert.ToInt64(dbr["SALE_INVOICE_ID"]))
                                {
                                    CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                                    chRelized.Checked = true;
                                }
                            }
                        }
                        #endregion
                        this.ChequeRealization();
                        this.LoadData();
                        this.SelectCreditInvoice();
                    }
#endregion
                }
                
            }
            
            }
            //end safdar
            this.ClearAll();
            this.LoadReceviedCheque();
        }


                #region Origional 



            //    if (int.Parse(DrpStatusConvertTo.SelectedValue.ToString()) == Constants.Cheque_Pending && txtReceivedDate.Text == DateTime.Parse(this.Session["CurrentWorkDate"].ToString()).ToString("dd/MM/yyyy"))
            //    {
            //        CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), int.Parse(drpDistributor.SelectedValue.ToString()), int.Parse(DrpPrincipal.SelectedValue.ToString()), long.Parse(DrpCustomer.SelectedValue.ToString()), txtChequeNo.Text, txtBankName.Text, ChequeDate, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), Constants.DateNullValue, Constants.DateNullValue,
            //           decimal.Parse(txtAmount.Text), int.Parse(DrpStatusConvertTo.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, int.Parse(DrpBankAccount.SelectedValue.ToString()));

            //        CController.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 1);

            //        foreach (GridViewRow dr in GrdCredit.Rows)
            //        {
            //            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
            //            if (chRelized.Checked == true)
            //            {
            //                CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
            //            }
            //        }
            //    }
            //    else if (int.Parse(DrpStatusConvertTo.SelectedValue.ToString()) == Constants.Cheque_Deposit)
            //    {
            //        CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()), Constants.DateNullValue,
            //             Constants.DecimalNullValue, int.Parse(DrpStatusConvertTo.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, int.Parse(DrpBankAccount.SelectedValue.ToString()));
            //    }
            //    else if (int.Parse(DrpStatusConvertTo.SelectedValue.ToString()) == Constants.Cheque_Bons || int.Parse(DrpStatusConvertTo.SelectedValue.ToString()) == Constants.Cheque_Cancel)
            //    {
            //        CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()),
            //                                         Constants.DecimalNullValue, int.Parse(DrpStatusConvertTo.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedValue.ToString()));

            //    }

            //    else if (int.Parse(DrpStatusConvertTo.SelectedValue.ToString()) == Constants.Cheque_Clear)
            //    {

            //        CController.UpdateChequeEntry(long.Parse(HFChqueProcessId.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.LongNullValue, null, null, Constants.DateNullValue, Constants.DateNullValue, Constants.DateNullValue, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()),
            //                                         Constants.DecimalNullValue, int.Parse(DrpStatusConvertTo.SelectedValue.ToString()), Constants.DateNullValue, txtSlipNo.Text, DrpChequeType.SelectedIndex, txtRemarks.Text, long.Parse(DrpBankAccount.SelectedValue.ToString()));

            //        CController.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 1);

            //        foreach (GridViewRow dr in GrdCredit.Rows)
            //        {
            //            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
            //            if (chRelized.Checked == true)
            //            {
            //                CController.InsertChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]));
            //            }
            //        }
            //        this.ChequeRealization();
            //        this.LoadData();
            //        this.SelectCreditInvoice();
            //    }
            //}

           
#endregion
    
    }

    /// <summary>
    /// Cancels Cheque Entry
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.ClearAll();
    }

    /// <summary>
    /// Deletes Cheque
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdOrder_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ChequeEntryController CController = new ChequeEntryController();
        CController.DeleteChequeEntry(long.Parse(GrdOrder.Rows[e.RowIndex].Cells[0].Text));
        this.LoadReceviedCheque();
    }

    /// <summary>
    /// Loads Credit Invoices
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.SelectCreditInvoice();
    }

    /// <summary>
    /// Loads Routes, Customers, Credit Invoices And Cheques
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadArea();
        this.LoadData();
        this.SelectCreditInvoice();
        this.LoadReceviedCheque();
    }

    /// <summary>
    /// Loads Customers, Credit Invoices And Cheques
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadData();
        this.SelectCreditInvoice();
        this.LoadReceviedCheque();
    }

    /// <summary>
    /// Sets Cheque Data For Edit. This Function Runs When An Existing Cheque Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdOrder_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            // by safdar
            DrpStatus.Enabled = false;
            //lblStatusConvertTo.Visible = true;
            //DrpStatusConvertTo.Visible = true;
            


            ChequeEntryController Ccontroller = new ChequeEntryController();
            HFChqueProcessId.Value = GrdOrder.Rows[e.NewEditIndex].Cells[0].Text;
            // DrpRoute.SelectedValue = GrdOrder.Rows[e.NewEditIndex].Cells[11].Text; BY SAFDAR
            DrpRoute.SelectedValue = GrdOrder.Rows[e.NewEditIndex].Cells[12].Text;

            this.LoadData();

            DrpCustomer.SelectedValue = GrdOrder.Rows[e.NewEditIndex].Cells[1].Text;

            txtChequeNo.Text = GrdOrder.Rows[e.NewEditIndex].Cells[4].Text;
            txtBankName.Text = GrdOrder.Rows[e.NewEditIndex].Cells[5].Text;
            txtStartDate.Text = GrdOrder.Rows[e.NewEditIndex].Cells[6].Text;
            txtReceivedDate.Text = GrdOrder.Rows[e.NewEditIndex].Cells[7].Text;
            txtAmount.Text = GrdOrder.Rows[e.NewEditIndex].Cells[9].Text;
            txtSlipNo.Text = GrdOrder.Rows[e.NewEditIndex].Cells[10].Text;
            txtRemarks.Text = GrdOrder.Rows[e.NewEditIndex].Cells[11].Text;
            DrpBankAccount.SelectedValue = GrdOrder.Rows[e.NewEditIndex].Cells[14].Text;
            try
            {
                DrpDeliveryMan.SelectedValue = GrdOrder.Rows[e.NewEditIndex].Cells[15].Text;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Sale Force not found.');", true);
            }

            btnSave.Text = "Edit";
            this.SelectCreditInvoice();

            DataTable dt = Ccontroller.SelectChequeEntryInvoice(long.Parse(HFChqueProcessId.Value), 0);

            foreach (GridViewRow dr in GrdCredit.Rows)
            {
                foreach (DataRow dbr in dt.Rows)
                {

                    if (Convert.ToInt64(GrdCredit.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]) == Convert.ToInt64(dbr["SALE_INVOICE_ID"]))
                    {
                        CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                        chRelized.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Invoice not found for selected cheque');", true);
        }
    }

    /// <summary>
    /// Loads Cheques
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (int.Parse(DrpStatus.SelectedValue) == Constants.Cheque_Pending)
        {
            DrpStatusConvertTo.Items.Clear();
            DrpStatusConvertTo.Items.Add(new ListItem("Cheque Deposit", "528"));
            DrpStatusConvertTo.Items.Add(new ListItem("Cheque Realize", "529"));
            DrpStatusConvertTo.Items.Add(new ListItem("Cheque Bounce", "530"));
            DrpStatusConvertTo.Items.Add(new ListItem("Cheque Cancel", "560"));
        }
        else if (int.Parse(DrpStatus.SelectedValue) == Constants.Cheque_Deposit)
        {
            DrpStatusConvertTo.Items.Clear();

            DrpStatusConvertTo.Items.Add(new ListItem("Cheque Realize", "529"));
            DrpStatusConvertTo.Items.Add(new ListItem("Cheque Bounce", "530"));
            DrpStatusConvertTo.Items.Add(new ListItem("Cheque Cancel", "560"));
        }
        else
        {
            this.LoadDropdownConvertto();
        }



        if (btnSave.Text == "Save")
        {            
            this.LoadReceviedCheque();
        }
    }

    /// <summary>
    /// Loads Customers And Cheques
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpChequeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadData();
        this.LoadReceviedCheque();
    }

    /// <summary>
    /// Loads Customers And Credit Invoices
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadData();
        this.SelectCreditInvoice();
    }

    /// <summary>
    /// Filters Cheuqe Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)this.Session["dt"];
        DateTime d = DateTime.Parse(txtToDate.Text);
        string x = d.ToString("dd/MM/yyyy");
        switch (ddSearchType.SelectedIndex)
        {
            case 1:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'" + "AND " + "CHEQUE_DATE " + " like '%" + x + "%'";
                break;
            case 2:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'" + "AND " + "CHEQUE_DATE " + " like '%" + x + "%'";
                break;
            case 3:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'" + "AND " + "CHEQUE_DATE " + " like '%" + x + "%'";
                break;
            case 4:
                dt.DefaultView.RowFilter = ddSearchType.SelectedValue.ToString() + " like '%" + txtSeach.Text + "%'" + "AND " + "CHEQUE_DATE " + " like '%" + x + "%'";
                break;
            default:
                dt.DefaultView.RowFilter = "CHEQUE_NO" + " like '%" + "" + "%'" + "AND " + "CHEQUE_DATE " + " like '%" + x + "%'";
                break;
        }
        GrdOrder.DataSource = dt.DefaultView;
        GrdOrder.DataBind();
    }

    private bool IsDayClosed()
    {
        bool flag = false;
        DistributorController DistrCtl = new DistributorController();
        DataTable dtDayClose = DistrCtl.MaxDayClose(Convert.ToInt32(drpDistributor.SelectedValue), 3);
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
    protected void GrdOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        
    }
    protected void ChbIsSelect_SelectedIndexChanged(object sender, EventArgs e)
    {

        GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
        int index = row.RowIndex;
        CheckBox cb1 = (CheckBox)GrdOrder.Rows[index].FindControl("ChbIsSelect");

        if (cb1.Checked == true)
        {
            #region check
            if (int.Parse(DrpStatus.SelectedValue) == Constants.Cheque_Deposit)
            {
                lblsaleforce.Visible = false;
                DrpDeliveryMan.Visible = false;
                lblcustomer.Visible = false;
                DrpCustomer.Visible = false;
                lblroute.Visible = false;
                DrpRoute.Visible = false;
                GrdCredit.Visible = false;
                Panel1.Visible = false;
                //DrpPrincipal.Enabled = false;
               // DrpBankAccount.Enabled = false;
                //DrpChequeType.Enabled = false;
                //DrpRoute.Enabled = false;

            //    txtAccountNo.Enabled = false;
              lblBankAccount.Visible = true;
                DrpBankAccount.Visible = true;


            }
            DrpStatus.Enabled = false;
            txtChequeNo.ReadOnly = true;
            //  txtBankName.ReadOnly = true;
            //   DrpCustomerBank.Enabled = false;
            txtAmount.ReadOnly = true;
            txtStartDate.ReadOnly = true;
            lblStatusConvertTo.Visible = true;
            DrpStatusConvertTo.Visible = true;
            btnSave.Text = "Update";
           lblBankAccount.Visible = true;
            DrpBankAccount.Visible = true;
         // txtAccountNo.Enabled = false;

            for (int i = 0; i < GrdOrder.Columns.Count; i++)
            {
                if (GrdOrder.Columns[i] is CommandField)
                {
                    GrdOrder.Columns[i].Visible = false;
                }
            }

            #endregion
        }

        else if (cb1.Checked == false)
        {

            #region Unchecked
            if (int.Parse(DrpStatus.SelectedValue) == Constants.Cheque_Deposit)
            {
                lblsaleforce.Visible = true;
                DrpDeliveryMan.Visible = true;
                lblcustomer.Visible = true;
                DrpCustomer.Visible = true;
                lblroute.Visible = true;
                DrpRoute.Visible = true;
                GrdCredit.Visible = true;
                Panel1.Visible = true;
                //DrpPrincipal.Enabled = false;
                //DrpBankAccount.Enabled = false;
                //DrpChequeType.Enabled = false;
                //DrpRoute.Enabled = false;

              //  txtAccountNo.Enabled = true;
             //   lblBankAccount.Visible = false;
                DrpBankAccount.Visible = false;

            }
            DrpStatus.Enabled = true;
            txtChequeNo.ReadOnly = false;
            //  txtBankName.ReadOnly = true;
           // DrpCustomerBank.Enabled = true;
            txtAmount.ReadOnly = false;
            txtStartDate.ReadOnly = false;
            lblStatusConvertTo.Visible = false;
            DrpStatusConvertTo.Visible = false;
            btnSave.Text = "Save";
          lblBankAccount.Visible = false;
            DrpBankAccount.Visible = false;
        //    txtAccountNo.Enabled = true;


            for (int i = 0; i < GrdOrder.Columns.Count; i++)
            {
                if (GrdOrder.Columns[i] is CommandField)
                {
                    GrdOrder.Columns[i].Visible = true;
                }
            }



            #endregion
        }

     }
    protected void ChbSelectAll_CheckedChanged(object sender, EventArgs e)
    {

        if (ChbSelectAll.Checked == true)
        {
            #region checked
            if (int.Parse(DrpStatus.SelectedValue) == Constants.Cheque_Deposit)
            {
                lblsaleforce.Visible = false;
                DrpDeliveryMan.Visible = false;
                lblcustomer.Visible = false;
                DrpCustomer.Visible = false;
                lblroute.Visible = false;
                DrpRoute.Visible = false;
                GrdCredit.Visible = false;
                Panel1.Visible = false;
                //DrpPrincipal.Enabled = false;
                DrpBankAccount.Enabled = false;
                //DrpChequeType.Enabled = false;
                //DrpRoute.Enabled = false;

        //        txtAccountNo.Enabled = false;
              lblBankAccount.Visible = true;
                DrpBankAccount.Visible = true;


            }
            DrpStatus.Enabled = false;
            txtChequeNo.ReadOnly = true;
            //  txtBankName.ReadOnly = true;
      //      DrpCustomerBank.Enabled = false;
            txtAmount.ReadOnly = true;
            txtStartDate.ReadOnly = true;
            lblStatusConvertTo.Visible = true;
            DrpStatusConvertTo.Visible = true;
            btnSave.Text = "Update";
        //    lblBankAccount.Visible = true;
            DrpBankAccount.Visible = true;
           // txtAccountNo.Enabled = false;

            for (int i = 0; i < GrdOrder.Columns.Count; i++)
            {
                if (GrdOrder.Columns[i] is CommandField)
                {
                    GrdOrder.Columns[i].Visible = false;
                }
            }


            #endregion
        }
        else
        {
            #region Unchecked
            if (int.Parse(DrpStatus.SelectedValue) == Constants.Cheque_Deposit)
            {
                lblsaleforce.Visible = true;
                DrpDeliveryMan.Visible = true;
                lblcustomer.Visible = true;
                DrpCustomer.Visible = true;
                lblroute.Visible = true;
                DrpRoute.Visible = true;
                GrdCredit.Visible = true;
                Panel1.Visible = true;
                //DrpPrincipal.Enabled = false;
                //DrpBankAccount.Enabled = false;
                //DrpChequeType.Enabled = false;
                //DrpRoute.Enabled = false;

              //  txtAccountNo.Enabled = true;
                lblBankAccount.Visible = false;
                DrpBankAccount.Visible = false;









            }
            DrpStatus.Enabled = true;
            txtChequeNo.ReadOnly = false;
            //  txtBankName.ReadOnly = true;
           // DrpCustomerBank.Enabled = true;
            txtAmount.ReadOnly = false;
            txtStartDate.ReadOnly = false;
            lblStatusConvertTo.Visible = false;
            DrpStatusConvertTo.Visible = false;
            btnSave.Text = "Save";
         //   lblBankAccount.Visible = false;
            DrpBankAccount.Visible = false;
         //   txtAccountNo.Enabled = true;


            for (int i = 0; i < GrdOrder.Columns.Count; i++)
            {
                if (GrdOrder.Columns[i] is CommandField)
                {
                    GrdOrder.Columns[i].Visible = true;
                }
            }



            #endregion
        }
    }
}
