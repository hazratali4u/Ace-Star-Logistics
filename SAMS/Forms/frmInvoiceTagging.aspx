<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmInvoiceTagging.aspx.cs" Inherits="Forms_frmInvoiceTagging" Title="SAMS: Credit Tagging Entry" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
     <div id="right_data">
     <div >
        <table width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel id="UpdatePanel2" runat="server">
                        <contenttemplate>
<TABLE><TBODY><TR><TD style="WIDTH: 100px" align=left>
<strong><asp:Label id="lbltoLocation" runat="server" Width="67px" Text="Location" CssClass="lblbox"></asp:Label></strong> </TD><TD style="WIDTH: 100px"><asp:DropDownList id="drpDistributor" runat="server" Width="250px" CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged"></asp:DropDownList></TD><TD style="HEIGHT: 25px"></TD></TR><TR><TD style="WIDTH: 100px" vAlign=middle align=left>
<strong> <asp:Label id="Label2" runat="server" Width="67px" Text="Principal" CssClass="lblbox"></asp:Label></strong> </TD><TD style="WIDTH: 100px" align=left><asp:DropDownList id="DrpPrincipal" runat="server" Width="250px" CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="DrpPrincipal_SelectedIndexChanged">
                                </asp:DropDownList></TD><TD style="HEIGHT: 25px" align=left></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 12px" vAlign=top align=left>
                                <strong> <asp:Label id="Label1" runat="server" Width="58px" Text="Route      " CssClass="lblbox" __designer:wfdid="w1"></asp:Label></strong> </TD><TD style="WIDTH: 100px; HEIGHT: 12px" align=left><asp:DropDownList id="DrpRoute" runat="server" Width="249px" CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="DrpRoute_SelectedIndexChanged" __designer:wfdid="w2"></asp:DropDownList></TD><TD style="HEIGHT: 25px" align=left></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 12px" vAlign=top align=left>
                                <strong> <asp:Label id="Label4" runat="server" Width="55px" Text="Customer" CssClass="lblbox"></asp:Label></strong> </TD><TD style="WIDTH: 100px; HEIGHT: 12px" align=left><asp:DropDownList id="DrpCustomer" runat="server" Width="250px" CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="DrpCustomer_SelectedIndexChanged">
                                </asp:DropDownList></TD><TD style="HEIGHT: 25px" align=left></TD></TR><TR><TD style="WIDTH: 100px; HEIGHT: 12px" vAlign=top align=left></TD><TD style="WIDTH: 100px; HEIGHT: 12px" align=left>
                                <asp:Button id="btnView" onclick="btnView_Click" runat="server" Width="107px" Font-Size="8pt" Text="Save" CssClass="Button" /> </TD><TD style="HEIGHT: 25px" align=left></TD></TR></TBODY></TABLE>
</contenttemplate>
                    </asp:UpdatePanel>&nbsp;&nbsp;
                </td>
            </tr>
        </table>
        
           </div>
    <div >
    <table width="100%">
        <tr>
            <td>
                        <table style="border-right: silver thin inset; border-top: silver thin inset; border-left: silver thin inset;
                            width: 650px; border-bottom: silver thin inset; height: 251px;">
                            <tbody>
                                <tr>
                                    <td align="left" colspan="5" style="height: 21px">
                                        <asp:UpdatePanel id="UpdatePanel1" runat="server">
                                            <contenttemplate>
<asp:Panel id="Panel1" runat="server" Width="712px" Height="250px" ScrollBars="Vertical"><asp:GridView id="GrdCredit" runat="server" Width="98%" ForeColor="SteelBlue" CssClass="gridRow2" HorizontalAlign="Center" BorderColor="White" BackColor="White" AutoGenerateColumns="False" DataKeyNames="SALE_INVOICE_ID">
<PagerSettings PreviousPageText="Previous" Mode="NextPrevious" LastPageText="" FirstPageText="" NextPageText="Next"></PagerSettings>
<Columns>
<asp:TemplateField HeaderText="Select">
<ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver"></ItemStyle>
<ItemTemplate>
<asp:CheckBox id="ChbIsAssigned" runat="server" Width="41px" __designer:wfdid="w1"></asp:CheckBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="MANUAL_INVOICE_ID" HeaderText="Invoice No">
<ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="DOCUMENT_DATE" HeaderText="Invoice Date">
<ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataFormatString="{0:F2}" DataField="CURRENT_CREDIT_AMOUNT" HeaderText="Credit Amount">
<ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="Silver" HorizontalAlign="Center"></ItemStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="Credit Type"><ItemTemplate>
<asp:DropDownList id="DrpCreditType" runat="server" Width="130px">
<%--<asp:ListItem Value="600">Normal Credit</asp:ListItem>--%>
<asp:ListItem Value="601">Income Tax Challan</asp:ListItem>
<asp:ListItem Value="602">Shelf Rent</asp:ListItem>
<asp:ListItem Value="645">Disputed Credit</asp:ListItem>
</asp:DropDownList> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Amount"><ItemTemplate>
<asp:TextBox id="txtAmount" runat="server" Width="100px" CssClass="txtBox" __designer:wfdid="w3"></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Remarks"><ItemTemplate>
<asp:TextBox id="txtRemarks" runat="server" Width="169px" CssClass="txtBox" __designer:wfdid="w1"></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
</Columns>
<HeaderStyle HorizontalAlign="Center" CssClass="tblhead" VerticalAlign="Middle"></HeaderStyle>
</asp:GridView> </asp:Panel> 
</contenttemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                
            </td>
        </tr>
    </table>
    </div>
    </div>
</asp:Content>