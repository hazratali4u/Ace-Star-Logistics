<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmPurchaseEntry.aspx.cs" Inherits="Forms_frmPurchaseEntry" Title="SAMS: Stock Register" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">

        function ValidateForm() {
            var str;

            str = document.getElementById('<%=txtskuName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must Select SKU Name');
                return false;
            }
            str = document.getElementById('<%=txtQuantity.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must Enter Quantity');
                return false;
            }

            str = document.getElementById('<%=txtDocumentNo.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must Enter Invoice/DC No');
                return false;
            }

            str = document.getElementById('<%=txtBuiltyNo.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must Enter Builty No');
                return false;
            }

            return true;
        }
        function SearchList() {
            var l = document.getElementById('<%= lstCode.ClientID %>');
            var tb = document.getElementById('<%= txtskuCode.ClientID %>');

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
        function SearchedCode() {
            var l = document.getElementById('<%= lstCode.ClientID %>');
            var str;
            for (var i = 0; i < l.options.length; i++) {
                if (l.options[i].selected) {
                    str = l.options[i].value;
                    ClearSelection(l);
                    break;
                }
                else {
                    str = "";
                }
            }
            var stroption = document.getElementById("<%= txtskuCode.ClientID %>").value;
            if (str.length > 0) {
                document.getElementById("<%= txtskuCode.ClientID %>").value = str.substring(0, str.indexOf('-'));
                document.getElementById("<%= txtskuName.ClientID %>").value = str.substring(str.indexOf('-') + 1);
                document.getElementById("<%= Panel1.ClientID %>").className = "HidePanel";
            }
            else if (stroption.length == 0) {
                document.getElementById("<%= Panel1.ClientID %>").className = "ShowPanel";
                document.getElementById("<%= lstCode.ClientID %>").focus();
            }

    }
    function SelectCode(e) {
        var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
        if (key == 13) {
            e.preventDefault();
            var str = document.getElementById("<%= lstCode.ClientID %>").value;
                document.getElementById("<%= txtskuCode.ClientID %>").value = str.substring(0, str.indexOf('-'));
                document.getElementById("<%= txtskuName.ClientID %>").value = str.substring(str.indexOf('-') + 1);
                document.getElementById("<%= Panel1.ClientID %>").className = "HidePanel";
                document.getElementById("<%= txtQuantity.ClientID %>").focus();
            }
        }
        function ClearSelection(lb) {
            lb.selectedIndex = -1;
        }

    </script>
    <div id="right_data">
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td style="height: 24px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label2" runat="server" CssClass="lblbox" Height="14px" Text="Transaction Type"
                                                        Width="115px"></asp:Label></strong>
                                            </td>
                                            <td style="height: 24px">
                                                <asp:DropDownList ID="DrpDocumentType" runat="server" AutoPostBack="True" CssClass="DropList"
                                                    OnSelectedIndexChanged="DrpDocumentType_SelectedIndexChanged" Width="200px">
                                                    <asp:ListItem Value="2">Purchase</asp:ListItem>
                                                    <asp:ListItem Value="5">Transfer Out</asp:ListItem>
                                                    <asp:ListItem Value="3">Purchase Return</asp:ListItem>
                                                    <asp:ListItem Value="4">Transfer In</asp:ListItem>
                                                    <asp:ListItem Value="6">Damage</asp:ListItem>
                                                    <asp:ListItem Value="15">Redemption In</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 24px">
                                                <strong>
                                                    <asp:Label ID="Label5" runat="server" Width="5px"></asp:Label></strong>
                                            </td>
                                            <td style="width: 316px;" align="center" colspan="1" rowspan="8" valign="middle">
                                                <strong>
                                                    <asp:Label ID="Label6" runat="server" Width="249px" Height="4px"></asp:Label></strong>
                                                <div style="left: 10px; position: absolute; top: 345px">
                                                    <asp:Panel ID="Panel1" runat="server" BackColor="Silver" BorderColor="Silver" BorderStyle="Groove"
                                                        BorderWidth="1px" CssClass="HidePanel" Height="170px" Width="250px">
                                                        SKU List<br />
                                                        <asp:ListBox ID="lstCode" runat="server" Height="151px" onkeydown="SelectCode(event)"
                                                            SelectionMode="Multiple" Width="239px"></asp:ListBox>
                                                    </asp:Panel>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="height: 25px">
                                                <strong>
                                                    <asp:Label ID="lblDocumentNo" runat="server" Text="Document No" Width="94px"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="drpDocumentNo" runat="server" AutoPostBack="True" CssClass="DropList"
                                                    OnSelectedIndexChanged="drpDocumentNo_SelectedIndexChanged" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 25px"></td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="height: 25px">
                                                <strong>
                                                    <asp:Label ID="lbltoLocation" runat="server" CssClass="lblbox" Text="Principal" Width="94px"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="drpPrincipal" runat="server" AutoPostBack="True" CssClass="DropList"
                                                    OnSelectedIndexChanged="drpPrincipal_SelectedIndexChanged" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 25px"></td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="height: 25px">
                                                <strong>
                                                    <asp:Label ID="lblfromLocation" runat="server" CssClass="lblbox" Text="Purchase For"
                                                        Width="94px"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="drpDistributor" runat="server" AutoPostBack="True" CssClass="DropList"
                                                    Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 25px"></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label ID="Label4" runat="server" CssClass="lblbox" Text="Transfer To" Visible="False"
                                                        Width="113px" Height="16px"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList ID="DrpTransferFor" runat="server" CssClass="DropList" Visible="False"
                                                    Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="height: 25px"></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px;" align="left">
                                                <strong>
                                                    <asp:Label ID="Label1" runat="server" CssClass="lblbox" Text="INV/DC  No" Width="94px"></asp:Label></strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDocumentNo" runat="server" CssClass="txtBox" Width="195px"></asp:TextBox>
                                            </td>
                                            <td style="width: 1px" valign="top"></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px;" align="left">
                                                <strong>
                                                    <asp:Label ID="Label3" runat="server" CssClass="lblbox" Text="Builty No" Width="94px"></asp:Label></strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBuiltyNo" runat="server" CssClass="txtBox" Width="195px"></asp:TextBox>
                                            </td>
                                            <td style="width: 1px" valign="top"></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px;" align="left">
                                                <strong>
                                                    <asp:Label ID="lblTaxPer" runat="server" Text="Advance Tax %"></asp:Label>
                                                </strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTaxPer" runat="server" Width="195px" Text="0.10"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom"
                                                ValidChars="0123456789." TargetControlID="txtTaxPer">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 1px" valign="top"></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px" valign="top" align="left">
                                                <asp:CheckBox ID="ChbBatchNo" runat="server" AutoPostBack="True" OnCheckedChanged="ChbBatchNo_CheckedChanged"
                                                    Text="Batch No" Width="77px" Visible="False" />
                                            </td>
                                            <td style="height: 25px; width: 1px;" valign="top">
                                                <asp:CheckBox ID="ChbFreeSKU" runat="server" Width="121px" Text="Apply Free SKU"
                                                    AutoPostBack="True" OnCheckedChanged="ChbFreeSKU_CheckedChanged"></asp:CheckBox>
                                            </td>
                                            <td style="width: 1px; height: 25px" valign="top"></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <table border="1">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblskuCode" runat="server" Width="100%" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="  SKU Code" CssClass="lblbox" BackColor="#006699"></asp:Label>
                                            </td>
                                            <td style="height: 16px">
                                                <asp:Label ID="lblskuname" runat="server" Width="100%" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="   SKU Name" CssClass="lblbox" BackColor="#006699"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblquantity" runat="server" Width="100%" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Qty CTN" CssClass="lblbox" BackColor="#006699"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblUnits" runat="server" Width="100%" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Qty Unit" CssClass="lblbox"
                                                    BackColor="#006699"></asp:Label>
                                            </td>
                                            <td style="height: 16px" align="center">
                                                <asp:Label ID="lblFreeSku" runat="server" Width="100%" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Free sku" CssClass="lblbox" BackColor="#006699"
                                                    Enabled="False"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="lbltdamage" runat="server" Width="100%" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="T.Damage" CssClass="lblbox" BackColor="#006699"
                                                    Enabled="False"></asp:Label></td>

                                            <td>
                                                <asp:Label ID="Label7" runat="server" Width="100%" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Discount" CssClass="lblbox" BackColor="#006699"
                                                    Enabled="False"></asp:Label></td>

                                              <td>
                                                <asp:Label ID="Label8" runat="server" Width="100%" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Amount" CssClass="lblbox" BackColor="#006699"
                                                    Enabled="False"></asp:Label></td>
                                                                                        <td>
                                                <asp:Label ID="Label41" runat="server" Width="100%" Height="16px" ForeColor="White"
                                                    Font-Bold="True" Text="Add SKU" CssClass="lblbox" BackColor="#006699"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtskuCode" onkeyup="SearchList()" runat="server" Width="83px" CssClass="txtBox"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtskuName" runat="server" Width="185px" Font-Bold="True" CssClass="txtBox"
                                                    Enabled="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtQuantity" onfocus="SearchedCode()" runat="server" Width="70px"
                                                    CssClass="txtBox "></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBatchNo" runat="server" Width="76px" CssClass="txtBox"
                                                    Enabled="False" Visible="False">N/A</asp:TextBox>
                                                <asp:TextBox ID="txtUnit" runat="server" CssClass="txtBox" Width="76px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFreeSKU" runat="server" CssClass="txtBox" Enabled="False"
                                                    Width="70px">0</asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransitDamage" runat="server" CssClass="txtBox"
                                                    Width="70px"></asp:TextBox>
                                            </td>

                                             <td>
                                                <asp:TextBox ID="txtSkuDiscount" runat="server" CssClass="txtBox"
                                                    Width="70px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="txtBox"
                                                Enabled="False"    Width="70px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button AccessKey="A" ID="btnSave" OnClick="btnSave_Click" runat="server" Width="100px"
                                                    Font-Size="8pt" Text="Add Sku" ValidationGroup="vg" CssClass="Button" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="9">
                                                <asp:Panel ID="Panel2" runat="server" Width="885px" Height="130px" ScrollBars="Vertical"
                                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                                    <asp:GridView ID="GrdPurchase" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="White" CssClass="gridRow2" ForeColor="SteelBlue" HorizontalAlign="Center"
                                                        OnRowDeleting="GrdPurchase_RowDeleting" OnRowEditing="GrdPurchase_RowEditing"
                                                        ShowHeader="False" Width="100%">
                                                        <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next"
                                                            PreviousPageText="Previous" />
                                                        <RowStyle ForeColor="Black" />
                                                        <Columns>
                                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID">
                                                                <HeaderStyle CssClass="HidePanel" />
                                                                <ItemStyle CssClass="HidePanel" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SKU_CODE" HeaderText="SKU Code">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                                                    Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SKU_NAME" HeaderText="SKU Name">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                                                    Width="250px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Quantity" HeaderText="Quantity">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                    Width="100px" />
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="QuantityUnit" HeaderText="BatchNo">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                    Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="FREE_SKU" HeaderText="Free SKU">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                    Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TDAMAGE" HeaderText="T.DAMAGE">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                    Width="100px" />
                                                            </asp:BoundField>


                                                             <asp:BoundField DataField="Discount_AMOUNT" HeaderText="Discount" DataFormatString="{0:F2}">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                    Width="94px" />
                                                            </asp:BoundField>
                                                               <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:F2}">
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                    Width="83px" />
                                                            </asp:BoundField>
                                                            
                                                            <asp:CommandField HeaderText="Edit" ShowEditButton="True">
                                                                <ItemStyle BorderColor="Silver" BorderWidth="1px" Width="40px" />
                                                            </asp:CommandField>
                                                            <asp:TemplateField HeaderText="Delete">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                        Text="Delete"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" Width="45px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle BackColor="White" />
                                                        <PagerStyle BackColor="Transparent" />
                                                        <HeaderStyle BackColor="#007395" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"
                                                            VerticalAlign="Middle" />
                                                        <AlternatingRowStyle BackColor="#F2F2F2" CssClass="GridAlternateRowStyle" ForeColor="#333333" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table>
                                    <tr>

                                        <td align="right"><strong>
                                            <asp:Label ID="lblTotalCtn" runat="server" Width="298px" Height="16px"  Text="Total"></asp:Label></strong>
                                        </td>
                                        <td style="width:75px;">
                                            <asp:TextBox ID="txtTotalQuantityCtn" runat="server" Width="70px" CssClass="txtBox"
                                                ReadOnly="True"></asp:TextBox>
                                        </td>
                                       
                                        <td style="width:75px;">
                                            <asp:TextBox ID="txtTotalQuantity" runat="server" Width="70px"
                                                CssClass="txtBox" ReadOnly="True"></asp:TextBox>

                                        </td>
                                       <td style="width:75px;">
                                            <asp:TextBox ID="txtTotalfreeSKu" runat="server" Width="70px" CssClass="txtBox" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td style="width:75px;">
                                            <asp:TextBox ID="txtTotalTDamage" runat="server" Width="70px" CssClass="txtBox" ReadOnly="True"></asp:TextBox>
                                        </td>

                                        <td style="width:75px;">
                                            <asp:TextBox ID="txtTotalDiscount" runat="server" Width="70px" CssClass="txtBox" ReadOnly="True"></asp:TextBox>
                                        </td>
                                         <td style="width:75px;">
                                            <asp:TextBox ID="txtTotalAmount" runat="server" Width="70px" CssClass="txtBox" ReadOnly="True"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                         <td align="right" colspan="6">
                                             <strong>
                                                <asp:Label ID="lblTax" runat="server" Height="16px"  Text="Advance Tax"></asp:Label>
                                            </strong>
                                        </td>
                                        <td style="width:75px;">
                                            <asp:TextBox ID="txtTax" runat="server" Width="70px" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                         <td align="right" colspan="6"><strong>
                                            <asp:Label ID="Label9" runat="server" Width="298px" Height="16px"  Text="Net Value"></asp:Label></strong>
                                        </td>
                                        <td style="width:75px;">
                                            <asp:TextBox ID="txtNetValue" runat="server" Width="70px" CssClass="txtBox" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <asp:Button AccessKey="S" ID="btnSaveDocument" runat="server" Width="119px" Font-Size="8pt"
                                                Text="Save Document" UseSubmitBehavior="False" OnClick="btnSaveDocument_Click"
                                                CssClass="Button" />
                                             <asp:Button AccessKey="C" ID="btnCancel" runat="server" Width="120px" Font-Size="8pt"
                                                Text="Cancel" UseSubmitBehavior="False" OnClick="btnCancel_Click" CssClass="Button" />
                                        </td>
                                        <td>
                                           
                                        </td>
                                        <td colspan="4"></td>
                                    </tr>
                                </table>


                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
