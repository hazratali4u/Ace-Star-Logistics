<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmLoadPass.aspx.cs" Inherits="Forms_frmLoadPass" Title="SAMS: Order/Invoice Load Pass" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
        function startRequest(sender, e) {
            document.getElementById('<%=btnSaveOrder.ClientID%>').disabled = true;
            document.getElementById('<%=btnCalculate.ClientID%>').disabled = true;
        }
        function endRequest(sender, e) {
            document.getElementById('<%=btnSaveOrder.ClientID%>').disabled = false;
            document.getElementById('<%=btnCalculate.ClientID%>').disabled = false;
        }
        function ValidateSaveOrder() {
            var str;
            str = document.getElementById('<%=txtNetAmount.ClientID%>').value;
            if (str == null || str.length == 0 || str < 0) {
                alert('Must Select SKU and Press Calculate');
                return false;
            }
            return true;
        }
        function SearcSKUList(e) {
            var l = document.getElementById('<%= lstCode.ClientID %>');
            var tb = document.getElementById('<%= txtskuCode.ClientID %>');
            if (e.keyCode == 27) {
                document.getElementById("<%= txtWholeSaleDiscount.ClientID %>").focus();
            }
            else {
                if (tb.value == "") {
                    ClearSelection(l);
                }
                else {
                    for (var i = 0; i < l.options.length; i++) {
                        if (l.options[i].value.toLowerCase().match(tb.value.toLowerCase())) {
                            l.options[i].selected = true;
                            return false;
                        }
                        else {
                            ClearSelection(l);
                        }
                    }
                }
            }
            return false;
        }
        function SelectSkuCode(e) {

            var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
            if (key == 13) {
                e.preventDefault();
                var str = document.getElementById("<%= lstCode.ClientID %>").value;
                document.getElementById("<%= txtskuCode.ClientID %>").value = str.substring(0, str.indexOf('-'));
                document.getElementById("<%= txtskuName.ClientID %>").value = str.substring(str.indexOf('-') + 1, str.indexOf(':'));
                document.getElementById("<%= txtPacking.ClientID %>").value = str.substring(str.indexOf('/') + 1);
                document.getElementById("<%= txtTP.ClientID %>").value = str.substring(str.indexOf(':') + 1, str.indexOf('/'));
                document.getElementById("<%= Panel3.ClientID %>").className = "HidePanel";
                var skucode = document.getElementById("<%= txtskuCode.ClientID %>").value;
                if (skucode != 'C201' || skucode != 'C202' || skucode != 'C203' || skucode != 'C204' || skucode != 'C205' || skucode != 'C206' || skucode != 'C207') {
                    document.getElementById("<%= txtReturnEmpty.ClientID %>").disabled = true;
                }
                if (skucode == 'C201' || skucode == 'C202' || skucode == 'C203' || skucode == 'C204' || skucode == 'C205' || skucode == 'C206' || skucode == 'C207') {
                    document.getElementById("<%= txtReturnEmpty.ClientID %>").disabled = false;
                    document.getElementById("<%= txtIssueCtn.ClientID %>").value = 0;
                    document.getElementById("<%= txtIssueCtn.ClientID %>").disabled = true;
                    document.getElementById("<%= txtReturnUnits.ClientID %>").disabled = true;
                    document.getElementById("<%= txtDamegeUnits.ClientID %>").disabled = true;
                    document.getElementById("<%= txtSchemeUnits.ClientID %>").disabled = true;
                    document.getElementById("<%= txtReturnEmpty.ClientID %>").focus();
                }
                document.getElementById("<%= txtIssueCtn.ClientID %>").focus();
            }
        }
        function CalculateIssueUnit() {
            var str = document.getElementById("<%= txtPacking.ClientID %>").value;
            var pack = str.substring(0, str.indexOf('X'));
            document.getElementById("<%= txtIssueUnits.ClientID %>").value = document.getElementById("<%= txtIssueCtn.ClientID %>").value * pack;
        }
        function CalculateGrossSaleUnit() {
            var rtn = document.getElementById("<%= txtReturnUnits.ClientID %>").value;
            document.getElementById("<%= txtGrossSaleUnits.ClientID %>").value = document.getElementById("<%= txtIssueUnits.ClientID %>").value - rtn;
        }
        function CalculateNetSaleUnit() {
            var rtn = document.getElementById("<%= txtSchemeUnits.ClientID %>").value;
            var redem = document.getElementById("<%= txtredemptionUnits.ClientID %>").value;
            document.getElementById("<%= txtNetSaleUnits.ClientID %>").value = document.getElementById("<%= txtGrossSaleUnits.ClientID %>").value - rtn - redem;
            var netsale = document.getElementById("<%= txtNetSaleUnits.ClientID %>").value;
            document.getElementById("<%= txtNetValue.ClientID %>").value = document.getElementById("<%= txtTP.ClientID %>").value * netsale;
        }
        function ClearSelection(lb) {
            lb.selectedIndex = -1;
        }
        function SearchSKUCode() {
            var str = document.getElementById("<%= lstCode.ClientID %>").value;
            var stroption = document.getElementById("<%= txtskuCode.ClientID %>").value;
            if (str.length > 0) {
                document.getElementById("<%= txtskuCode.ClientID %>").value = str.substring(0, str.indexOf('-'));
                document.getElementById("<%= txtskuName.ClientID %>").value = str.substring(str.indexOf('-') + 1, str.indexOf(':'));
                document.getElementById("<%= txtPacking.ClientID %>").value = str.substring(str.indexOf('/') + 1);
                document.getElementById("<%= txtTP.ClientID %>").value = str.substring(str.indexOf(':') + 1, str.indexOf('/'));
                document.getElementById("<%= Panel3.ClientID %>").className = "HidePanel";
            }
            else if (stroption.length == 0) {
                document.getElementById("<%= Panel3.ClientID %>").className = "ShowPanel";
                document.getElementById("<%= lstCode.ClientID %>").focus();
            }
            ClearSelection(document.getElementById('<%= lstCode.ClientID %>'));
        }
    </script>
    <div id="right_data">
        <div>
            <span class="heading">Load Pass<br />
            </span>&nbsp;</div>
        <div>
            <table width="100%">
                <tr>
                    <td align="left">
                        <div style="left: 120px; position: absolute; top: 190px;">
                            <asp:Panel ID="Panel1" runat="server" Width="327px" Height="237px" BorderWidth="1px"
                                BorderStyle="Inset" BorderColor="White" BackColor="Silver" CssClass="HidePanel">
                                <table style="border-right: #ffffff thin groove; border-top: #ffffff thin groove;
                                    border-left: #ffffff thin groove; width: 99%; border-bottom: #ffffff thin groove">
                                    <tbody>
                                        <tr>
                                            <td style="border-bottom: black thin solid" align="left" colspan="2">
                                                &nbsp;<strong>Select Customer from List Press Enter</strong>
                                            </td>
                                            <td style="border-bottom: black thin solid" valign="top" align="right">
                                                <asp:Button AccessKey="S" ID="Button4" runat="server" Width="21px" Height="16px"
                                                    Font-Size="8pt" Text="X" BorderWidth="1px" BorderStyle="Groove" CssClass="Button" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <asp:ListBox ID="ListCustomer" onkeydown="SelectCode(event)" runat="server" Width="95%"
                                    Height="87%"></asp:ListBox>
                            </asp:Panel>
                        </div>
                        <asp:UpdatePanel ID="upProd" runat="server">
                            <ContentTemplate>
                                <div style="left: 15px; position: absolute; top: 275px; height: 270px;">
                                    <asp:Panel ID="Panel3" runat="server" Width="327px" Height="237px" BorderWidth="1px"
                                        BorderStyle="Inset" BorderColor="White" BackColor="Silver" CssClass="HidePanel">
                                        <table style="border-right: #ffffff thin groove; border-top: #ffffff thin groove;
                                            border-left: #ffffff thin groove; width: 99%; border-bottom: #ffffff thin groove">
                                            <tbody>
                                                <tr>
                                                    <td style="border-bottom: black thin solid" align="left" colspan="2">
                                                        &nbsp;<strong>Select SKU from List Press Enter</strong>
                                                    </td>
                                                    <td style="border-bottom: black thin solid" valign="top" align="right">
                                                        <asp:Button ID="Button5" runat="server" AccessKey="S" BorderStyle="Groove" BorderWidth="1px"
                                                            Font-Size="8pt" Height="20px" Text="X" Width="21px" CssClass="Button" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <asp:ListBox ID="lstCode" onkeydown="SelectSkuCode(event)" runat="server" Width="95%"
                                            Height="87%" SelectionMode="Multiple"></asp:ListBox>
                                    </asp:Panel>
                                    &nbsp;
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div style="z-index: 101; left: 612px; width: 100px; position: absolute; top: 369px;
                            height: 100px">
                            &nbsp;<asp:Panel ID="Panel21" runat="server">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel3">
                                    <ProgressTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" Height="28px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"
                                            Width="31px" />
                                        Wait Update</ProgressTemplate>
                                </asp:UpdateProgress>
                            </asp:Panel>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td align="left" style="width: 88px">
                                                <strong>
                                                    <asp:Label ID="lblLocation" runat="server" Width="94px" Text="Location" CssClass="lblbox"></asp:Label></strong>
                                            </td>
                                            <td style="width: 248px">
                                                <asp:DropDownList ID="drpDistributor" runat="server" Width="200px" CssClass="DropList"
                                                    AutoPostBack="True" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" style="height: 24px; width: 88px;">
                                                <strong>
                                                    <asp:Label ID="lblDeliveryMan" runat="server" Width="74px" Text="DeliveryMan" CssClass="lblbox"></asp:Label>
                                            </td>
                                            <td>
                                                <strong>
                                                    <asp:DropDownList ID="DrpDeliveryMan" runat="server" Width="200px" CssClass="DropList"
                                                        OnSelectedIndexChanged="DrpDeliveryMan_SelectedIndexChanged" TabIndex="4">
                                                    </asp:DropDownList>
                                                </strong>
                                            </td>
                                            <td valign="top" align="left" style="height: 24px; width: 113px;">
                                                <strong>
                                                    <asp:Label ID="lblOpeningReading" runat="server" CssClass="lblbox" TabIndex="1" Text="Opening Reading"
                                                        Style="text-align: left"></asp:Label></strong>
                                            </td>
                                            <td valign="middle" align="left" style="width: 133px">
                                                </strong>
                                                <asp:TextBox ID="txtOpeningReading" runat="server" Width="85px" TabIndex="7"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 88px">
                                                <strong>
                                                    <asp:Label ID="lblPrincipal" runat="server" Text="Principal"></asp:Label>
                                            </td>
                                            </strong>
                                            <td style="width: 248px; height: 25px" align="left">
                                                <asp:DropDownList ID="DrpPrincipal" runat="server" Width="200px" CssClass="DropList"
                                                    AutoPostBack="True" OnSelectedIndexChanged="DrpPrincipal_SelectedIndexChanged"
                                                    __designer:wfdid="w2" TabIndex="1">
                                                </asp:DropDownList>
                                            </td>
                                            <td valign="middle" align="left" style="width: 88px">
                                                <strong>
                                                    <asp:Label ID="lblVisit" runat="server" Text="Visit" Width="74px"></asp:Label></strong>
                                            </td>
                                            <td style="width: 231px">
                                                </strong><asp:TextBox ID="txtVisit" runat="server" Width="200px" TabIndex="5"></asp:TextBox>
                                            </td>
                                            <td valign="top" align="left" style="width: 113px">
                                                <strong>
                                                    <asp:Label ID="lblCloseReading" runat="server" Text="Closing Reading"></asp:Label></strong>
                                            </td>
                                            <td style="width: 133px">
                                                <asp:TextBox ID="txtCloseReading" runat="server" Width="85px" TabIndex="8"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 88px">
                                                <strong>
                                                    <asp:Label ID="lblRoute" runat="server" Text="Route"></asp:Label>
                                            </td>
                                            </strong>
                                            <td style="width: 248px; height: 25px" align="left">
                                                <asp:DropDownList ID="DrpRoute" runat="server" Width="200px" CssClass="DropList"
                                                    AutoPostBack="True" OnSelectedIndexChanged="DrpRoute_SelectedIndexChanged" TabIndex="2">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" valign="middle" style="width: 88px">
                                                <strong>
                                                    <asp:Label ID="lblProductiveCall" runat="server" Text="Productive Call" Width="89px"></asp:Label>
                                            </td>
                                            </strong>
                                            <td style="width: 133px">
                                                <asp:TextBox ID="txtProductiveCall" runat="server" Width="200px" TabIndex="6"></asp:TextBox>
                                            </td>
                                            <td style="width: 113px">
                                                <strong>
                                                    <asp:Label ID="lblBillNoFrom" runat="server" Text="Bill # From" Width="74px"></asp:Label>
                                            </td>
                                            </strong>
                                            <td>
                                                <asp:TextBox ID="txtBillNoFrom" runat="server" Width="85px" TabIndex="9"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 88px">
                                                <strong>
                                                    <asp:Label ID="lblOrderBooker" runat="server" CssClass="lblbox" Text="Order Booker"
                                                        Width="101px"></asp:Label>
                                                </strong>
                                            </td>
                                            <td style="width: 248px; height: 25px" align="left">
                                                <asp:DropDownList ID="DrpOrderBooker" runat="server" Width="200px" CssClass="DropList"
                                                    OnSelectedIndexChanged="DrpOrderBooker_SelectedIndexChanged" TabIndex="3">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" style="height: 23px; width: 88px;">
                                            </td>
                                            <td align="left" style="height: 23px; width: 231px;">
                                                &nbsp;
                                            </td>
                                            <td valign="top" align="left" style="width: 113px">
                                                <strong>
                                                    <asp:Label ID="lblBillNoTo" runat="server" Text="Bill # To" Height="16px" Width="69px"></asp:Label>
                                            </td>
                                            </strong>
                                            <td>
                                                <asp:TextBox ID="txtBillNoTo" runat="server" Width="85px" TabIndex="10"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                &nbsp;
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        &nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                    </td>
                    <td style="height: 126px">
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel5" runat="server" DefaultButton="btnAddSku" Width="989px">
                                    <table border="1">
                                        <tr>
                                            <td style="height: 16px" colspan="2">
                                                <asp:Label ID="lblskuCode" runat="server" Width="173px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="SKU" CssClass="lblbox;" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px; width: 42px;" colspan="1">
                                                <asp:Label ID="lblPacking" runat="server" Width="74px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Packing" CssClass="lblbox" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px; width: 47px;" colspan="1">
                                                <asp:Label ID="lblTP" runat="server" Width="47px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="T.P" CssClass="lblbox" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px" colspan="2">
                                                <asp:Label ID="lblIssue" runat="server" Width="107px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Issue" CssClass="lblbox" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px" colspan="1">
                                                <asp:Label ID="lblReturn" runat="server" Width="50px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Return" CssClass="lblbox" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px" colspan="1">
                                                <asp:Label ID="lblEmptyReturn" runat="server" Width="70px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Empty Return" CssClass="lblbox" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px; width: 49px;" colspan="1">
                                                <asp:Label ID="lblGrossSale" runat="server" Width="60px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="G.Sale" CssClass="lblbox" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px; width: 52px;" colspan="1">
                                                <asp:Label ID="lblDamage" runat="server" Width="50px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Damage" CssClass="lblbox" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px" colspan="1">
                                                <asp:Label ID="lblScheme" runat="server" Width="50px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Scheme" CssClass="lblbox" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px" colspan="1">
                                                <asp:Label ID="Label1" runat="server" Width="50px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Red. Out" CssClass="lblbox" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px" colspan="1">
                                                <asp:Label ID="lblNetSale" runat="server" Width="49px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="NetSale" CssClass="lblbox" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px" colspan="1">
                                                <asp:Label ID="lblNetValue" runat="server" Width="57px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Net Value" CssClass="lblbox" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px">
                                                <asp:Label ID="Label4" runat="server" BackColor="#006699" CssClass="lblbox" Font-Bold="True"
                                                    ForeColor="White" Height="16px" Text="Add SKU" Width="87px" Style="text-align: center"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 16px; width: 16px;">
                                                <asp:Label ID="lblSkuC" runat="server" Width="59px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="SKU Code" CssClass="lblbox" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px; width: 42px;">
                                                <asp:Label ID="lblSkuName" runat="server" Width="106px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="SKU Name" CssClass="lblbox" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px; width: 42px;">
                                                <asp:Label ID="Label13" runat="server" BackColor="#006699" CssClass="lblbox" Font-Bold="True"
                                                    ForeColor="White" Height="16px" Width="74px" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px; width: 47px;">
                                                <asp:Label ID="Label14" runat="server" Width="47px" Height="16px" ForeColor="White"
                                                    Font-Bold="True" CssClass="lblbox" BackColor="#006699" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td align="center" style="height: 16px; width: 48px;">
                                                <asp:Label ID="lblIssueCTN" runat="server" BackColor="#006699" CssClass="lblbox"
                                                    Font-Bold="True" ForeColor="White" Height="16px" Text="CTN" Width="55px" Enabled="False"
                                                    Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px; width: 16px;">
                                                <asp:Label ID="lblIssueUnits" runat="server" BackColor="#006699" CssClass="lblbox"
                                                    Font-Bold="True" ForeColor="White" Height="16px" Text="Units" Width="47px" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px; width: 35px;">
                                                <asp:Label ID="lblReturnUnits" runat="server" BackColor="#006699" CssClass="lblbox"
                                                    Font-Bold="True" ForeColor="White" Height="16px" Text="Units" Width="49px" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px; width: 35px;">
                                                <asp:Label ID="lblReturnEmptyUnits" runat="server" BackColor="#006699" CssClass="lblbox"
                                                    Font-Bold="True" ForeColor="White" Height="16px" Text="Units" Width="70px" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px; width: 49px;">
                                                <asp:Label ID="lblGrossSaleUnits" runat="server" BackColor="#006699" CssClass="lblbox"
                                                    Font-Bold="True" ForeColor="White" Height="16px" Text="Units" Width="60px" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px; width: 52px;">
                                                <asp:Label ID="lblDamageUnits" runat="server" BackColor="#006699" CssClass="lblbox"
                                                    Font-Bold="True" ForeColor="White" Height="16px" Text="Units" Width="51px" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px">
                                                <asp:Label ID="lblSchemeUnits" runat="server" BackColor="#006699" CssClass="lblbox"
                                                    Font-Bold="True" ForeColor="White" Height="16px" Text="Units" Width="49px" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px">
                                                <asp:Label ID="Label3" runat="server" BackColor="#006699" CssClass="lblbox" Font-Bold="True"
                                                    ForeColor="White" Height="16px" Text="Units" Width="49px" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px">
                                                <asp:Label ID="lblNetSaleUnits" runat="server" BackColor="#006699" CssClass="lblbox"
                                                    Font-Bold="True" ForeColor="White" Height="16px" Text="Units" Width="47px" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px">
                                                <asp:Label ID="lblNetValue1" runat="server" BackColor="#006699" CssClass="lblbox"
                                                    Font-Bold="True" ForeColor="White" Height="16px" Width="57px" Style="text-align: center"></asp:Label>
                                            </td>
                                            <td style="height: 16px">
                                                <asp:Label ID="Label2" runat="server" BackColor="#006699" CssClass="lblbox" Font-Bold="True"
                                                    ForeColor="White" Height="16px" Width="87px" Style="text-align: center"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 16px">
                                                <asp:TextBox ID="txtskuCode" onkeyup="SearcSKUList(event)" runat="server" Width="58px"
                                                    CssClass="txtBox" TabIndex="11"></asp:TextBox>
                                            </td>
                                            <td style="width: 42px">
                                                <asp:TextBox ID="txtskuName" runat="server" Width="105px" Font-Bold="True" CssClass="txtBox"
                                                    Enabled="False" TabIndex="12"></asp:TextBox>
                                            </td>
                                            <td style="width: 42px">
                                                <asp:TextBox ID="txtPacking" runat="server" CssClass="txtBox" Width="74px" Enabled="False"
                                                    TabIndex="13"></asp:TextBox>
                                            </td>
                                            <td style="width: 47px">
                                                <asp:TextBox ID="txtTP" runat="server" CssClass="txtBox" Width="45px" Enabled="False"
                                                    TabIndex="14"></asp:TextBox>
                                            </td>
                                            <td style="width: 48px">
                                                <asp:TextBox ID="txtIssueCtn" runat="server" onfocus="SearchSKUCode()" onblur="CalculateIssueUnit()"
                                                    Width="53px" TabIndex="15"></asp:TextBox>
                                            </td>
                                            <td style="width: 16px">
                                                <asp:TextBox ID="txtIssueUnits" runat="server" CssClass="txtBox" Width="43px" Enabled="False"
                                                    TabIndex="16"></asp:TextBox>
                                            </td>
                                            <td style="width: 35px">
                                                <asp:TextBox ID="txtReturnUnits" onblur="CalculateGrossSaleUnit()" runat="server"
                                                    CssClass="txtBox" Width="47px" TabIndex="17"></asp:TextBox>
                                            </td>
                                            <td style="width: 35px">
                                                <asp:TextBox ID="txtReturnEmpty" runat="server" CssClass="txtBox" Width="67px" TabIndex="18"></asp:TextBox>
                                            </td>
                                            <td style="width: 49px">
                                                <asp:TextBox ID="txtGrossSaleUnits" runat="server" CssClass="txtBox" Width="57px"
                                                    Enabled="False" TabIndex="19"></asp:TextBox>
                                            </td>
                                            <td style="width: 52px">
                                                <asp:TextBox ID="txtDamegeUnits" runat="server" CssClass="txtBox" Width="48px" TabIndex="20"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSchemeUnits" runat="server" CssClass="txtBox" Width="46px" onblur="CalculateNetSaleUnit()"
                                                    TabIndex="21"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtredemptionUnits" runat="server" CssClass="txtBox" Width="46px"
                                                    onblur="CalculateNetSaleUnit()" TabIndex="21"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNetSaleUnits" runat="server" CssClass="txtBox" Width="45px" Enabled="False"
                                                    TabIndex="22"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNetValue" runat="server" CssClass="txtBox" Width="55px" Enabled="False"
                                                    TabIndex="23"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnAddSku" runat="server" Font-Size="8pt" Text="Add Sku" ValidationGroup="vg"
                                                    Width="87px" AccessKey="A" CssClass="Button" AutoPostBack="True" OnClick="btnAddSku_Click"
                                                    TabIndex="24" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td align="left" colspan="7">
                                                <asp:Panel ID="Panel2" runat="server" Height="130px" ScrollBars="Vertical" Width="1000px"
                                                    BorderColor="Silver" BorderStyle="Groove" BorderWidth="1px">
                                                    <asp:GridView ID="GrdPurchase" runat="server" ForeColor="SteelBlue" CssClass="gridRow2"
                                                        BackColor="White" HorizontalAlign="Center" AutoGenerateColumns="False" BorderColor="White"
                                                        ShowHeader="False" OnRowDeleting="GrdPurchase_RowDeleting" OnRowEditing="GrdPurchase_RowEditing">
                                                        <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next"
                                                            PreviousPageText="Previous"></PagerSettings>
                                                        <Columns>
                                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID">
                                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SKU_CODE" HeaderText="SKU Code">
                                                                <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                                    Width="65px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SKU_NAME" HeaderText="SKU Name">
                                                                <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                                    Width="140px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PACKING" HeaderText="Packing">
                                                                <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                                    Width="100px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="UNIT_PRICE" HeaderText="T.P" DataFormatString="{0:F2}">
                                                                <ItemStyle HorizontalAlign="Left" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                                    Width="50px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ISSUE_CTN" HeaderText="CTN">
                                                                <ItemStyle HorizontalAlign="Right" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                                    Width="70px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ISSUE_UNITS" HeaderText="UNITS">
                                                                <ItemStyle HorizontalAlign="Right" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                                    Width="55px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RETURN_UNITS" HeaderText="UNITS">
                                                                <ItemStyle HorizontalAlign="Right" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                                    Width="60px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RETURN_EMPTY_UNITS" HeaderText="UNITS">
                                                                <ItemStyle HorizontalAlign="Right" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                                    Width="88px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="GROSS_UNITS" HeaderText="UNITS">
                                                                <ItemStyle HorizontalAlign="Right" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                                    Width="70px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DAMAGE_UNITS" HeaderText="UNITS">
                                                                <ItemStyle HorizontalAlign="Right" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                                    Width="65px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SCHEME_UNITS" HeaderText="UNITS">
                                                                <ItemStyle HorizontalAlign="Right" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                                    Width="65px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="REDEMPTION_OUT_UNITS" HeaderText="UNITS">
                                                                <ItemStyle HorizontalAlign="Right" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                                    Width="65px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NET_SALE" HeaderText="NET SALE" DataFormatString="{0:D}">
                                                                <ItemStyle HorizontalAlign="Right" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                                    Width="50px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NET_VALUE" HeaderText="NET SALE" DataFormatString="{0:F2}">
                                                                <ItemStyle HorizontalAlign="Right" BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid"
                                                                    Width="65px"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="QUANTITY_CTN" HeaderText="QUANTITY_CTN">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:CommandField ShowEditButton="True" HeaderText="Edit">
                                                                <ItemStyle BorderColor="Silver" BorderWidth="2px" Width="30px"></ItemStyle>
                                                            </asp:CommandField>
                                                            <asp:TemplateField HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                        CommandName="Delete"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderColor="Silver" BorderWidth="2px" BorderStyle="Solid" Width="45px">
                                                                </ItemStyle>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 100px">
                        <asp:HiddenField ID="hfBillBookNo" runat="server" />
                        <asp:HiddenField ID="hfDisableCtrl" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table width="100%">
                <tr>
                    <td align="left">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tbody>
                                    
                                        <tr>
                                            <td>
                                                <strong>
                                                    <asp:Label ID="Label5" runat="server" CssClass="lblbox" Text="Issue Stock Value"
                                                        Width="136px"></asp:Label>
                                                </strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIssueStockValue" runat="server" CssClass="txtBoxnNum" 
                                                    Font-Bold="True" ForeColor="Black" Width="150px"  style="text-align: right"></asp:TextBox>
                                            </td>
                                            <td style="width: 40px" >
                                            </td>
                                            <td align="left" style="height: 20px">
                                                <strong>
                                                   
                                                    <asp:Label ID="lblUnclaimableDiscount" runat="server" CssClass="lblbox" Height="16px"
                                                        Text="Unclaimable Discount" Width="139px" Font-Bold="True"></asp:Label>
                                                </strong>
                                            </td>
                                            <td style="width: 7px; height: 20px">
                                              
                                                <asp:TextBox ID="txtUnclaimAbleDiscount" runat="server" CssClass="txtBoxnNum" Font-Bold="True"
                                                    ForeColor="Black" Width="150px" TabIndex="31"  style="text-align: right" ></asp:TextBox>
                                            </td>
                                            <td style="width: 40px"></td>
                                            <td style="width: 123px; height: 7px" align="left">
                                                <strong>
                                                   
                                                    <asp:Label ID="lblGSTAmount" runat="server" CssClass="lblbox" 
                                                    Text="GST Amount" Width="75px"
                                                        Font-Bold="True"></asp:Label>
                                                </strong>
                                            </td>
                                            <td style="width: 7px; height: 7px" align="right">
                                                
                                                <asp:TextBox ID="txtGSTAmount" runat="server" CssClass="txtBoxnNum" Font-Bold="True"
                                                    ForeColor="Black" Width="150px"  style="text-align: right" TabIndex="37"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>
                                                    <asp:Label ID="lblnumGrossSale" runat="server" CssClass="lblbox" Text="Gross Amount"
                                                        Width="136px"></asp:Label>
                                                </strong>
                                            </td>
                                            <td>
                                               <asp:TextBox ID="txtnumGrossSale" runat="server" CssClass="txtBoxnNum" 
                                                    Font-Bold="True" ForeColor="Black" Width="150px"  style="text-align: right" TabIndex="30"></asp:TextBox>
                                            </td>
                                            <td style="width: 17px">
                                            </td>
                                            <td align="left">
                                                <strong>
                                                    
                                                    <asp:Label ID="lblWholeSaleDiscount" runat="server" CssClass="lblbox" Text="WholeSale Discount"
                                                        Width="128px" Height="16px"></asp:Label>
                                                </strong>
                                            </td>
                                            <td style="width: 7px">
                                               
                                                <asp:TextBox ID="txtWholeSaleDiscount" runat="server" CssClass="txtBoxnNum"  style="text-align: right" Font-Bold="True"
                                                    ForeColor="Black" Width="150px" onblur="CalculateClaimableDiscount(event)" TabIndex="32"></asp:TextBox>
                                            </td>
                                            <td></td>
                                            <td style="width: 123px">
                                                <strong>
                                                    <asp:Label ID="lblTSTAmount" runat="server" CssClass="lblbox" 
                                                    Text="TST Amount" Width="78px"
                                                        Font-Bold="True"></asp:Label>
                                                </strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTSTAmount" runat="server" CssClass="txtBoxnNum" Font-Bold="True"
                                                    ForeColor="Black" Width="150px"  style="text-align: right" TabIndex="38"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>
                                                    <asp:Label ID="lblTotalDamage" runat="server" CssClass="lblbox" Text="Damage Amount"
                                                        Width="136px"></asp:Label>
                                                </strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotalDamageValue" runat="server" CssClass="txtBoxnNum" 
                                                    Font-Bold="True" ForeColor="Black" Width="150px"  style="text-align: right" TabIndex="30"></asp:TextBox>
                                            </td>
                                            <td style="width: 17px">
                                            </td>
                                            <td style="height: 18px" align="left">
                                                <strong>
                                                    
                                                    <asp:Label ID="lblBRD" runat="server" CssClass="lblbox" Text="BRD" Width="130px"></asp:Label>
                                                </strong>
                                            </td>
                                            <td style="width: 7px; height: 18px">
                                               
                                                <asp:TextBox ID="txtBRD" runat="server" CssClass="txtBoxnNum" Font-Bold="True" ForeColor="Black"
                                                    Width="150px" TabIndex="33"  style="text-align: right"></asp:TextBox>
                                            </td>
                                            <td></td>
                                            <td style="width: 123px">
                                                <strong>
                                                    <asp:Label ID="Label7" runat="server" CssClass="lblbox" Text="Total Amount" Width="83px"
                                                        Font-Bold="True"></asp:Label>
                                                </strong>
                                            </td>
                                            <td>
                                               <asp:TextBox ID="TxtTotalAmount" runat="server" CssClass="txtBoxnNum" Font-Bold="True"
                                                    ForeColor="Black" Width="150px"  style="text-align: right" TabIndex="38"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>
                                                    <asp:Label ID="lblTotalSchemeValue" runat="server" CssClass="lblbox" Text="Scheme Amount"
                                                        Width="136px"></asp:Label>
                                                </strong>
                                            </td>
                                            <td>
                                                 <asp:TextBox ID="txtTotalSchemeValue" runat="server" CssClass="txtBoxnNum" 
                                                    Font-Bold="True"  style="text-align: right" ForeColor="Black" Width="150px" TabIndex="30"></asp:TextBox>
                                            </td>
                                            <td style="width: 17px">
                                            </td>
                                            <td style="width: 1px; height: 20px" align="left">
                                                <strong>
                                                   
                                                    <asp:Label ID="lblTradeOffer" runat="server" CssClass="lblbox" Text="Trade Offers"
                                                        Width="136px"></asp:Label>
                                                </strong>
                                            </td>
                                            <td style="width: 7px; height: 20px" align="right">
                                                
                                                <asp:TextBox ID="txtTradeOffers" runat="server" CssClass="txtBoxnNum" Font-Bold="True"
                                                    ForeColor="Black" Width="150px"  style="text-align: right" TabIndex="34"></asp:TextBox>
                                            </td>
                                            <td></td>
                                            <td style="width: 123px" align="left">
                                                <asp:Label ID="lblTaxCollected" runat="server" CssClass="lblbox" Text="Tax Collected"
                                                    Width="80px" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td style="width: 7px" >
                                          
                                           <asp:TextBox ID="txtTaxCollectedPer" runat="server" CssClass="txtBoxnNum" Font-Bold="True"
                                                    ForeColor="Black" Width="45px"  style="text-align: left" Enabled="true" TabIndex="38"></asp:TextBox>
                                                    %
                                                <asp:TextBox ID="txtTaxCollected" runat="server" CssClass="txtBoxnNum" Font-Bold="True"
                                                    ForeColor="Black" Width="86px"  style="text-align: right" Enabled="true" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td >
                                             <strong>
                                                    <asp:Label ID="Label6" runat="server" CssClass="lblbox" Text="Redemption Out" Width="136px"></asp:Label>
                                                </strong>
                                            </td>
                                            <td>
                                             <asp:TextBox ID="txtTotalRedemptionOut" runat="server" CssClass="txtBoxnNum"
                                                    Font-Bold="True"  style="text-align: right" ForeColor="Black" Width="150px" TabIndex="30"></asp:TextBox>
                                            </td>
                                            <td style="width: 17px">
                                            </td>
                                            <td style="width: 1px; height: 20px" align="left">
                                                <strong>
                                                   
                                                    <asp:Label ID="lblOtherDiscounts" runat="server" CssClass="lblbox" Text="Commission"
                                                        Width="133px"></asp:Label>
                                                </strong>
                                            </td>
                                            <td style="width: 7px; height: 20px" align="right">
                                              
                                                <asp:TextBox ID="txtOtherDiscounts" runat="server" CssClass="txtBoxnNum" Font-Bold="True"
                                                    ForeColor="Black" Width="150px"  style="text-align: right" TabIndex="35"></asp:TextBox>
                                            </td>
                                            <td></td>
                                            <td style="width: 123px"> <strong>
                                                    
                                                    <asp:Label ID="lblNetAmount" runat="server" CssClass="lblbox" 
                                                    Font-Bold="True" Text="Net Amount"
                                                        Width="75px" Height="18px"></asp:Label>
                                                </strong></td>
                                            <td>  <asp:TextBox ID="txtNetAmount" runat="server" CssClass="txtBoxnNum" 
                                                    Font-Bold="True" ForeColor="Black" Width="150px"  style="text-align: right"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td >
                                            </td>
                                            <td>
                                            </td>
                                            <td style="width: 17px">
                                            </td>
                                            <td style="width: 1px; height: 20px" align="left">
                                                <strong>
                                                    
                                                    <asp:Label ID="lblClaimAbleDiscount" runat="server" CssClass="lblbox" Text="Claimble Discounts"
                                                        Width="139px" Font-Bold="True"></asp:Label>
                                                </strong>
                                            </td>
                                            <td style="width: 7px; height: 20px" align="right">
                                               
                                                <asp:TextBox ID="txtClaimableDiscounts" runat="server" CssClass="txtBoxnNum" Font-Bold="True"
                                                    ForeColor="Black" Width="150px"  style="text-align: right" TabIndex="36"></asp:TextBox>
                                            </td>
                                            <td></td>
                                            <td style="width: 123px"></td>
                                            <td></td>
                                        </tr>
                                      
                                       
                                        <tr>
                                                          <td style="width: 100px" colspan ="2">
                                                                <asp:Button AccessKey="C" ID="btnCalculate" TabIndex="39" runat="server" Width="100px"
                                                                    Font-Size="8pt" Text="Calculate" Enabled="False" CssClass="Button" OnClick="btnCalculate_Click" />
                                                            
                                                                <asp:Button AccessKey="S" ID="btnSaveOrder" TabIndex="40" runat="server" Width="110px"
                                                                    Font-Size="8pt" Text="Save" CssClass="Button" OnClick="btnSaveOrder_Click" />
                                                            </td>
                                                          
                                                    
                                            </td>
                                            <td >
                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                             <td style="width: 123px"></td>
                                        </tr>
                                        <tr>
                                            <td style ="height:10px"></td> 
                                        </tr>
                                      
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        
                       
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
