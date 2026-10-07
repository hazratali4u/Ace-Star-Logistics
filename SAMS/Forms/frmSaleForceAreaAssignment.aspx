<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmSaleForceAreaAssignment.aspx.cs" Inherits="Forms_frmSaleForceAreaAssignment" Title="SAMS: Sale Force Assignment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" Runat="Server">
      
    <script language="JavaScript" type="text/javascript">
    function ChRouteListSelect()
    {    
        var chkBoxList = document.getElementById('<%= ChbAreaList.ClientID %>');
        var chkBox = document.getElementById('<%= ChbSelectAll.ClientID %>');
        if(chkBox.checked == true)
        {
            var chkBoxCount= chkBoxList.getElementsByTagName("input");
        
            for(var i=0;i<chkBoxCount.length;i++) 
            {
                chkBoxCount[i].checked = true;
            }
        }
        else
        {
            var chkBoxCount= chkBoxList.getElementsByTagName("input");
        
            for(var i=0;i<chkBoxCount.length;i++) 
            {
                chkBoxCount[i].checked = false;
            }
        }
                
    }
    
    function UnCheckRouteAll()
    {
    var chkBox = document.getElementById('<%= ChbSelectAll.ClientID %>');
    var chkBoxList = document.getElementById('<%= ChbAreaList.ClientID %>');
    var chkBoxCount= chkBoxList.getElementsByTagName("input");
    var count = 0;
    for(var i=0;i<chkBoxCount.length;i++) 
     {
        if(chkBoxCount[i].checked == false)
        {
            count +=1;
        }
     }
     if(count > 0)
     {
        chkBox.checked = false;
     }
     else
     {
        chkBox.checked = true;
     }         
}

    function ChPrincipalListSelect()
    {    
        var chkBoxList = document.getElementById('<%= ChbPrincipal.ClientID %>');
        var chkBox = document.getElementById('<%= ChPrincipal.ClientID %>');
        if(chkBox.checked == true)
        {
            var chkBoxCount= chkBoxList.getElementsByTagName("input");
        
            for(var i=0;i<chkBoxCount.length;i++) 
            {
                chkBoxCount[i].checked = true;
            }
        }
        else
        {
            var chkBoxCount= chkBoxList.getElementsByTagName("input");
        
            for(var i=0;i<chkBoxCount.length;i++) 
            {
                chkBoxCount[i].checked = false;
            }
        }                
    }
    
    function UnCheckPrincipalAll()
    {
    var chkBox = document.getElementById('<%= ChPrincipal.ClientID %>');
    var chkBoxList = document.getElementById('<%= ChbPrincipal.ClientID %>');
    var chkBoxCount= chkBoxList.getElementsByTagName("input");
    var count = 0;
    for(var i=0;i<chkBoxCount.length;i++) 
     {
        if(chkBoxCount[i].checked == false)
        {
            count +=1;
        }
     }
     if(count > 0)
     {
        chkBox.checked = false;
     }
     else
     {
        chkBox.checked = true;
     }         
}

     </script>
    <div id="right_data">
        <table width="100%">
            <tr>
                <td align="left" valign="top">
                
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
<TABLE><TBODY><TR>
    <td colspan="2">
        <asp:Label id="lblmsg" runat="server" Visible="False" Width="175px" ForeColor="Red"></asp:Label>&nbsp;
    </td>
    <TD style="HEIGHT: 17px"></TD></TR><TR><TD align="left" style="height: 25px">
    <strong> <asp:Label id="Label1" runat="server" Width="67px" Text="Locaton" CssClass="lblbox"></asp:Label></strong></TD><TD style="HEIGHT: 10px"><asp:DropDownList id="drpDistributor" runat="server" Width="200px" CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged"></asp:DropDownList></TD><TD style="HEIGHT: 10px"></TD></TR><TR><TD align="left" style="height: 25px">
    <strong><asp:Label id="Label11" runat="server" Width="81px" Text="Designation" CssClass="lblbox"></asp:Label></strong></TD><TD style="HEIGHT: 9px"><asp:DropDownList id="DrpDesignation" runat="server" Width="200px" CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="DrpDesignation_SelectedIndexChanged"></asp:DropDownList></TD></TR><TR><TD align="left" style="height: 25px">
    <strong><asp:Label id="Label2" runat="server" Width="83px" Text="Sale Force" CssClass="lblbox"></asp:Label></strong></TD><TD style="HEIGHT: 13px"><asp:DropDownList id="DrpSaleForce" runat="server" Width="200px" CssClass="DropList" AutoPostBack="True" OnSelectedIndexChanged="DrpSaleForce_SelectedIndexChanged"></asp:DropDownList></TD></TR>
    <TR><TD style="HEIGHT: 25px"></TD><TD align="left">
        <asp:Button ID="btnSave" runat="server" Font-Size="8pt" OnClick="btnSave_Click" Text="Save" ValidationGroup="vg" Width="82px" CssClass="Button" /></TD></TR><TR><TD align=center colSpan=2></TD></TR></TBODY></TABLE>
</ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel id="UpdatePanel2" runat="server"><contenttemplate>
<TABLE><TR><TD style="WIDTH: 100px" rowSpan=1 align="left">
<strong><asp:Label id="Label4" runat="server" Width="114px" Text="Route List" CssClass="lblbox"></asp:Label></strong></TD><TD style="WIDTH: 102px" vAlign=bottom align=center></TD><TD style="WIDTH: 102px" rowSpan=1 align="left">
<strong><asp:Label id="Label6" runat="server" Text="Principal List"></asp:Label></strong></TD></TR>
    <tr>
        <td align="left" rowspan="1" style="width: 100px">
            <asp:CheckBox ID="ChbSelectAll" runat="server" CssClass="DropList" Font-Size="8pt"
                onclick="ChRouteListSelect()" Text="Select All" Width="75px" /></td>
        <td align="center" style="width: 102px" valign="bottom">
        </td>
        <td align="left" rowspan="1" style="width: 102px">
            <asp:CheckBox ID="ChPrincipal" runat="server" Font-Size="8pt"
                onclick="ChPrincipalListSelect()" Text="Select All" Width="75px" /></td>
    </tr>
    <TR><TD rowSpan=4 align="left">
    <asp:Panel ID="Panel1" runat="server" BorderColor="Silver" BorderStyle="Groove" BorderWidth="1px"
        Height="200px" ScrollBars="Vertical" Width="200px" BackColor="White">
        <asp:CheckBoxList ID="ChbAreaList" runat="server" onclick = "UnCheckRouteAll()">
        </asp:CheckBoxList></asp:Panel>
</TD><TD style="WIDTH: 102px" vAlign=middle align=center></TD><TD rowSpan=4 align="left">
    <asp:Panel ID="Panel2" runat="server" BorderColor="Silver" BorderStyle="Groove" BorderWidth="1px"
        Height="200px" ScrollBars="Vertical" Width="200px" BackColor="White">
        <asp:CheckBoxList ID="ChbPrincipal" runat="server" onclick = "UnCheckPrincipalAll();">
        </asp:CheckBoxList>
        <div style="z-index: 101; left: -97px; width: 100px; position: absolute; top: 216px;
            height: 100px">
        </div>
    </asp:Panel>
</TD></TR><TR><TD style="WIDTH: 102px" align=center>&nbsp;</TD></TR><TR><TD style="WIDTH: 102px" align=center>
    &nbsp; &nbsp; &nbsp; &nbsp;
</TD></TR><TR><TD style="WIDTH: 102px" vAlign=middle align=center>
    &nbsp;</TD></TR></TABLE>
</contenttemplate>
                    </asp:UpdatePanel>
                </td>                
            </tr>
           
        </table>
        </div>

</asp:Content>
