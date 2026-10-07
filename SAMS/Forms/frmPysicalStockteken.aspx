<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmPysicalStockteken.aspx.cs" Inherits="Forms_frmPysicalStockteken"
    Title="SAMS: Physical Stock Taking" %>

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


            return true;
        }
        function SearcSKUList() {
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

        function SearchSKUCode() {
            var str = document.getElementById("<%= lstCode.ClientID %>").value;
            var stroption = document.getElementById("<%= txtskuCode.ClientID %>").value;

            if (str.length > 0) {
                document.getElementById("<%= txtskuCode.ClientID %>").value = str.substring(0, str.indexOf('-'));
                document.getElementById("<%= txtskuName.ClientID %>").value = str.substring(str.indexOf('-') + 1, str.indexOf(':'));
                document.getElementById("<%= txtUnitRate.ClientID %>").value = str.substring(str.indexOf(':') + 1);
                //document.getElementById("<%= Panel3.ClientID %>").className = "HidePanel";

            }
            // else if(stroption.length == 0)
            //{
            //       document.getElementById("<%= Panel3.ClientID %>").className = "ShowPanel";
            //       document.getElementById("<%= lstCode.ClientID %>").focus();
            //}
            // ClearSelection(document.getElementById('<%= lstCode.ClientID %>'));

        }
        function SelectSkuCode(e) {
            var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
            if (key == 13) {
                e.preventDefault();
                var str = document.getElementById("<%= lstCode.ClientID %>").value;
                document.getElementById("<%= txtskuCode.ClientID %>").value = str.substring(0, str.indexOf('-'));
                document.getElementById("<%= txtskuName.ClientID %>").value = str.substring(str.indexOf('-') + 1, str.indexOf(':'));
                document.getElementById("<%= txtUnitRate.ClientID %>").value = str.substring(str.indexOf(':') + 1);
                //document.getElementById("<%= Panel3.ClientID %>").className = "HidePanel";

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
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label Width="74px" CssClass="lblbox" ID="lblDocumentNo" runat="server" Text="Location"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList CssClass="DropList" ID="drpDistributor" runat="server" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center" style="width: 316px" colspan="1" rowspan="2" valign="middle">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 25px" align="left">
                                                <strong>
                                                    <asp:Label CssClass="lblbox" ID="lbltoLocation" runat="server" Text="Principal" Width="73px"></asp:Label></strong>
                                            </td>
                                            <td style="height: 25px">
                                                <asp:DropDownList AutoPostBack="True" CssClass="DropList" ID="drpPrincipal" OnSelectedIndexChanged="drpPrincipal_SelectedIndexChanged"
                                                    runat="server" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" style="height: 25px">
                                                <asp:Panel ID="Panel3" runat="server" Width="323px" Height="150px" BorderWidth="1px"
                                                    BorderStyle="Inset" BorderColor="White" BackColor="Silver">
                                                    <asp:ListBox ID="lstCode" onkeydown="SelectSkuCode(event)" runat="server" Width="99%"
                                                        Height="98%" SelectionMode="Multiple"></asp:ListBox>
                                                </asp:Panel>
                                            </td>
                                            <td style="width: 316px" align="center" colspan="1" rowspan="1" valign="middle">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            &nbsp;</div>
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style="height: 16px">
                                            <strong>
                                                <asp:Label ID="lblskuCode" runat="server" BackColor="#006699" CssClass="lblbox" Font-Bold="True"
                                                    ForeColor="White" Height="16px" Text="  SKU Code" Width="83px"></asp:Label></strong>
                                        </td>
                                        <td style="height: 16px">
                                            <strong>
                                                <asp:Label ID="lblskuname" runat="server" BackColor="#006699" CssClass="lblbox" Font-Bold="True"
                                                    ForeColor="White" Height="16px" Text="   SKU Name" Width="200px"></asp:Label></strong>
                                        </td>
                                        <td style="height: 16px">
                                            <strong>
                                                <asp:Label ID="lblquantity" runat="server" BackColor="#006699" CssClass="lblbox"
                                                    Font-Bold="True" ForeColor="White" Height="16px" Text="Sale Qty" Width="80px"></asp:Label></strong>
                                        </td>
                                        
                                        <td style="height: 16px;">
                                            <strong>
                                                <asp:Label ID="lblFreeSKU" runat="server" BackColor="#006699" CssClass="lblbox" Font-Bold="True"
                                                    ForeColor="White" Height="16px" Text="Un Sale Qty" Width="87px"></asp:Label></strong>
                                        </td>
                                        <td style="height: 16px">
                                            <strong>
                                                <asp:Label ID="Label1" runat="server" BackColor="#006699" CssClass="lblbox" Font-Bold="True"
                                                    ForeColor="White" Height="16px" Text="Unit Rate" Width="80px"></asp:Label></strong>
                                        </td>
                                        <td style="height: 16px">
                                            <strong>
                                                <asp:Label ID="Label41" runat="server" BackColor="#006699" CssClass="lblbox" Font-Bold="True"
                                                    ForeColor="White" Height="16px" Text="Add SKU" Width="96%"></asp:Label></strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 9px">
                                            <asp:TextBox ID="txtskuCode" runat="server" CssClass="txtBox" onkeyup="SearcSKUList()"
                                                Width="83px"></asp:TextBox>
                                        </td>
                                        <td style="height: 9px">
                                            <asp:TextBox ID="txtskuName" runat="server" CssClass="txtBox" Enabled="False" Font-Bold="True"
                                                Width="200px"></asp:TextBox>
                                        </td>
                                        <td style="height: 9px">
                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="txtBox " onfocus="SearchSKUCode()"
                                                Width="80px"></asp:TextBox>
                                        </td>
                                       
                                        <td style="height: 9px;">
                                            <asp:TextBox ID="txtusaleableqty" runat="server" CssClass="txtBox " onfocus="SearchSKUCode()"
                                                Width="80px"></asp:TextBox>
                                        </td>
                                        <td style="height: 9px">
                                            <asp:TextBox ID="txtUnitRate" runat="server" CssClass="txtBox" Width="75px">0</asp:TextBox>
                                        </td>
                                        <td style="height: 9px">
                                            <asp:Button ID="btnSave" runat="server" AccessKey="A" Font-Size="8pt" OnClick="btnSave_Click"
                                                Text="Save" ValidationGroup="vg" Width="87px" CssClass="Button" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="6">
                                            <asp:Panel ID="Panel2" runat="server" BorderColor="Silver" BorderStyle="Groove" BorderWidth="1px"
                                                Height="150px" ScrollBars="Vertical" Width="640px">
                                                <asp:GridView ID="GrdPurchase" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    BorderColor="White" CssClass="gridRow2" ForeColor="SteelBlue" HorizontalAlign="Center"
                                                    OnRowDeleting="GrdPurchase_RowDeleting" OnRowEditing="GrdPurchase_RowEditing"
                                                    ShowHeader="False" Width="632px">
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
                                                                Width="85px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SKU_NAME" HeaderText="SKU Name">
                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Left"
                                                                Width="205px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SALEABLE_QUANTITY" HeaderText="Quantity">
                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                Width="82px" />
                                                        </asp:BoundField>
                                                      
                                                        <asp:BoundField DataField="UNSALEABLE_QUANTITY" HeaderText="PRICE">
                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="2px" HorizontalAlign="Right"
                                                                Width="82px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="UNIT_RATE" HeaderText="UNIT_RATE" DataFormatString="{0:F2}">
                                                            <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="70px"
                                                                HorizontalAlign="Right" />
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
                                </table>
                                &nbsp; &nbsp;
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        &nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
