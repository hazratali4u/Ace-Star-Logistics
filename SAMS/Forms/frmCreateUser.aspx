<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmCreateUser.aspx.cs" Inherits="Forms_frmCreateUser" Title="SAMS: User Login" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
      <script language="JavaScript" type="text/javascript">
        function ValidateForm()
		{
			var str;
			
			str  = document.getElementById('<%=txtLoginId.ClientID%>').value; 
			if(str == null || str.length == 0)
			{
				alert('Must enter Login Id');
				return false;
			}
			str  = document.getElementById('<%=txtpassword.ClientID%>').value; 
			if(str == null || str.length == 0)
			{
				alert('Must enter Password');
				return false;
			}
			return true;		
		}
		
    </script>
    <div id="right_data">
    <div>
        <table width="100%">
            <tr>
                <td style="width: 100px">
                    <asp:UpdatePanel id="UpdatePanel2" runat="server">
                        <contenttemplate>
<TABLE><TBODY><TR><TD colSpan=2><asp:Label id="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label></TD></TR><TR><TD style="WIDTH: 98px">
<strong><asp:Label id="lbldesignationID" runat="server" Width="97px" Text="Base Location" CssClass="lblbox"></asp:Label></strong></TD><TD><asp:DropDownList id="ddDistributorId" runat="server" Width="205px" CssClass="DropList" OnSelectedIndexChanged="ddDistributorId_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>&nbsp; </TD></TR><TR><TD style="WIDTH: 98px; HEIGHT: 18px">
<strong><asp:Label id="Label1" runat="server" Width="69px" Text="Name" CssClass="lblbox"></asp:Label></strong></TD><TD style="HEIGHT: 18px"><asp:DropDownList id="DrpUser" runat="server" Width="205px" CssClass="DropList"></asp:DropDownList></TD></TR><TR><TD style="WIDTH: 98px">
<strong><asp:Label id="lblNickName" runat="server" Width="68px" Text="Roles" CssClass="lblbox"></asp:Label></strong></TD><TD><asp:DropDownList id="ddRole" runat="server" Width="205px" CssClass="DropList">
</asp:DropDownList></TD></TR><TR><TD style="WIDTH: 98px">
<strong><asp:Label id="Label3" runat="server" Width="82px" Text="Login Id" CssClass="lblbox"></asp:Label></strong></TD><TD><asp:TextBox id="txtLoginId" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox></TD></TR><TR><TD style="WIDTH: 98px">
<strong><asp:Label id="Label2" runat="server" Width="69px" Text="Password" CssClass="lblbox"></asp:Label></strong></TD><TD>
        <asp:TextBox id="txtpassword" runat="server" Width="200px" CssClass="txtBox " 
            MaxLength="20"></asp:TextBox></TD></TR><TR><TD style="WIDTH: 98px">&nbsp; </TD><TD><asp:CheckBox id="chkIsActive" runat="server" Width="93px" Text="IsActive" Checked="True"></asp:CheckBox></TD></TR><TR><TD style="WIDTH: 98px" align=right colSpan=1></TD><TD>&nbsp;
<asp:Button id="btnSave" onclick="btnSave_Click" runat="server" Width="82px" Font-Size="8pt" Text="Save" ValidationGroup="vg" CssClass="Button" /> 
<asp:Button id="btnCancel" runat="server" Width="73px" Font-Size="8pt" Text="Cancel" OnClick="btnCancel_Click" CssClass="Button" /> </TD></TR></TBODY></TABLE>
</contenttemplate>
                    </asp:UpdatePanel></td>               
            </tr>
        </table>        
           </div>
    <div>
        <asp:UpdatePanel id="UpdatePanel1" runat="server">
            <contenttemplate>
<DIV style="Z-INDEX: 101; LEFT: 481px; WIDTH: 100px; POSITION: absolute; TOP: 282px; HEIGHT: 100px" id="DIV1" onclick="return DIV1_onclick()"><asp:UpdateProgress id="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpdatePanel1"><ProgressTemplate>
<asp:ImageButton id="ImageButton1" runat="server" Width="26px" Height="23px" ImageUrl="~/App_Themes/Granite/Images/image003.gif"></asp:ImageButton>&nbsp; Loading.... 
</ProgressTemplate>
</asp:UpdateProgress> </DIV>
<TABLE class="tblhead" ><TBODY><TR><TD style="color:White;font-weight:bold;">
<asp:Label id="Label10" runat="server" Width="153px" Text="Select Searching Type"></asp:Label> </TD><TD style="WIDTH: 170px; HEIGHT: 21px" align=left><asp:DropDownList id="ddSearchType" runat="server" Width="200px" CssClass="DropList">
<asp:ListItem Value="SKU_code">All Records</asp:ListItem>
<asp:ListItem Value="USER_NAME">User Name</asp:ListItem>
<asp:ListItem Value="LOGIN_ID">Login</asp:ListItem>
<asp:ListItem Value="role_name">Role</asp:ListItem>
</asp:DropDownList> </TD><TD style="WIDTH: 224px; HEIGHT: 21px" align=left><asp:TextBox id="txtSeach" runat="server" Width="200px" CssClass="txtBox "></asp:TextBox> </TD><TD style="WIDTH: 250px; HEIGHT: 21px" align=left>
<asp:Button id="btnFilter" runat="server" Width="85px" Font-Size="8pt" Text="Filter" OnClick="btnFilter_Click"/> </TD></TR></TBODY></TABLE><asp:Panel id="Panel1" runat="server" Width="100%" Height="250px" ScrollBars="Vertical">
                                <asp:GridView id="Grid_users" runat="server" Width="99%" ForeColor="SteelBlue" CssClass="gridRow2" HorizontalAlign="Center" AutoGenerateColumns="False" BackColor="White" BorderColor="White" OnPageIndexChanging="Grid_users_PageIndexChanging" OnRowEditing="Grid_users_RowEditing">
<PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next" PreviousPageText="Previous"></PagerSettings>

<Columns>
<asp:BoundField DataField="USER_ID" HeaderText="User Id">
<HeaderStyle CssClass="HidePanel"></HeaderStyle>

<ItemStyle CssClass="HidePanel"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="Distributor_ID" HeaderText="Distributor">
<HeaderStyle CssClass="HidePanel"></HeaderStyle>

<ItemStyle CssClass="HidePanel "></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="USER_CODE" HeaderText="Code">
<ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="USER_NAME" HeaderText="Name">
<ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="LOGIN_ID" HeaderText="Login">
<ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="PASSWORD" HeaderText="Password">
<ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="role_name" HeaderText="Role">
<ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="IS_ACTIVE" HeaderText="Status">
<ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="ROLE_ID" HeaderText="ROLE_ID">
<HeaderStyle CssClass="HidePanel"></HeaderStyle>

<ItemStyle CssClass="HidePanel"></ItemStyle>
</asp:BoundField>
<asp:CommandField ShowEditButton="True" HeaderText="Edit">
<ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
</asp:CommandField>
</Columns>
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="tblhead"></HeaderStyle>
</asp:GridView> </asp:Panel> 
</contenttemplate>
        </asp:UpdatePanel>
        
    </div>   
    </div>
</asp:Content>

