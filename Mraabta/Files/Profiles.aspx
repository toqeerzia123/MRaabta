<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeBehind="Profiles.aspx.cs" Inherits="MRaabta.Files.Profiles" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../Js/FusionCharts.js" type="text/javascript"></script>

    
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

<script type="text/javascript">

    $(function () {
        $("[id*=chkAll_zone]").bind("click", function () {
            if ($(this).is(":checked")) {
                $("[id*=dd_zone] input").attr("checked", "checked");
            } else {
                $("[id*=dd_zone] input").removeAttr("checked");
            }
        });
        $("[id*=dd_zone] input").bind("click", function () {
            if ($("[id*=dd_zone] input:checked").length == $("[id*=dd_zone] input").length) {
                $("[id*=chkAll_zone]").attr("checked", "checked");
            } else {
                $("[id*=chkAll_zone]").removeAttr("checked");
            }
        });
    });

    $(function () {
        $("[id*=chkAll_branch]").bind("click", function () {
            if ($(this).is(":checked")) {
                $("[id*=dd_branch] input").attr("checked", "checked");
            } else {
                $("[id*=dd_branch] input").removeAttr("checked");
            }
        });
        $("[id*=dd_branch] input").bind("click", function () {
            if ($("[id*=dd_branch] input:checked").length == $("[id*=dd_branch] input").length) {
                $("[id*=chkAll_branch]").attr("checked", "checked");
            } else {
                $("[id*=chkAll_branch]").removeAttr("checked");
            }
        });
    });
        
</script>

<%--<script type="text/javascript">
$(document).ready(function() {

    $('[id$=chkAll_zone]').click(function () {
        $('input:checkbox').not(this).prop('checked', this.checked);
    });

    $("[id*=dd_zone]").change(function () {
        if ($('input[id*=dd_zone][type=checkbox]:checked').length == $('input[id*=dd_zone][type=checkbox]').length) {
            $('[id$=chkAll_zone]').prop('checked', true);
        } else {
            $('[id$=chkAll_zone]').prop('checked', false);
        }
    });

});

</script>--%>

    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="8" align="center" class="head_column">
                <h3>
                    Profile Info</h3>
            </td>
        </tr>
    </table>
    <table class="input-form">
        <tr>
            <td class="heading">
                <div><h2>Profile Header</h2></div>
            </td>
        </tr>
        <tr>
            <td class="field">
                Name:
            </td>
            <td class="input-field">
                <asp:TextBox ID="txt_profile_name" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="field">
                Zone Name:
            </td>
            <td class="input-field1">
                <%--<asp:DropDownList ID="dd_zone1" runat="server" CssClass="dropdown" >
                    <asp:ListItem Value="">Select Zone Name</asp:ListItem>
                </asp:DropDownList>--%>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <%--<asp:CheckBox ID="chkAll_zone" Text="Select All"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="branch_SelectedIndexChanged" />--%>
                        <asp:CheckBox ID="chkAll_zone" Text="Select All"  runat="server" AutoPostBack="true" OnCheckedChanged="btn_check_Click"  />

                        
                        <asp:CheckBoxList ID="dd_zone" runat="server" CssClass="chklist" AppendDataBoundItems="true"
                            AutoPostBack="true" OnSelectedIndexChanged="branch_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="field">
                Branch Name:
            </td>
            <td class="input-field1">
                <%--<asp:DropDownList ID="dd_branch" runat="server" CssClass="dropdown" AppendDataBoundItems="true"
                            AutoPostBack="true" OnSelectedIndexChanged="expresscenter_SelectedIndexChanged">
                    <asp:ListItem Value="">Select Branch Name</asp:ListItem>
                </asp:DropDownList>--%>
                <asp:UpdatePanel ID="up_1" runat="server">
                    <ContentTemplate>
                    <asp:CheckBox ID="chkAll_branch" Text="Select All"  runat="server" AutoPostBack="true" OnSelectedIndexChanged="expresscenter_SelectedIndexChanged" />
                        <asp:CheckBoxList ID="dd_branch" runat="server" CssClass="chklist" AppendDataBoundItems="true"
                            AutoPostBack="true" OnSelectedIndexChanged="expresscenter_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="field">
                Express Center Name:
            </td>
            <td class="input-field1">
                <%-- <asp:DropDownList ID="dd_expresscenter" runat="server" CssClass="dropdown">
                    <asp:ListItem Value="">Select Express Center Name</asp:ListItem>
                </asp:DropDownList>--%>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:CheckBoxList ID="dd_expresscenter" runat="server" CssClass="chklist" AppendDataBoundItems="false">
                        </asp:CheckBoxList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Save Profile Header" CssClass="button1" OnClick="Btn_header_Click" />           
            </td>
        </tr>
    </table>

    <asp:GridView ID="GridView" runat="server" CssClass="profiletbl" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="Profile_Name" HeaderText="Profile Name" ItemStyle-Width="10" />
            <asp:BoundField DataField="ZoneName" HeaderText="Zone Name" ItemStyle-Width="30" />
            <asp:BoundField DataField="BranchName" HeaderText="Branch Name" ItemStyle-Width="30" />
            <asp:BoundField DataField="ExpressCenterName" HeaderText="Express Center Name" ItemStyle-Width="30" />                
        </Columns>
    </asp:GridView>


    <table class="input-form">
        <tr>
            <td class="heading">
                <div><h2>Profile Detail</h2></div>
            </td>
        </tr>
        <tr>
            <td class="field">
                Main Menu Name:
            </td>
            <td class="input-field">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="dd_mainmenu" runat="server" CssClass="dropdown" AppendDataBoundItems="true"
                            AutoPostBack="true" OnSelectedIndexChanged="childmenu_SelectedIndexChanged">
                            <asp:ListItem Value="">Select Main Menu</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="field">
                Child Menu Name:
            </td>
            <td class="input-field">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="dd_childmenu" runat="server" CssClass="dropdown" AutoPostBack="true">
                            <asp:ListItem Value="">Select Child Menu</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="submit" runat="server" Text="Save" CssClass="button1" OnClick="Btn_detail_Click" />           
            </td>
        </tr>
    </table>

    <asp:GridView ID="GridView2" runat="server" CssClass="profiletbl" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="Profile_Name" HeaderText="Profile Name" ItemStyle-Width="10" />
            <asp:BoundField DataField="MainMenu" HeaderText="Main Menu Name" ItemStyle-Width="30" />
            <asp:BoundField DataField="ChildMenu" HeaderText="Child Menu Name" ItemStyle-Width="30" />
            <asp:BoundField DataField="EntryDateTime" HeaderText="Entry Date Time" ItemStyle-Width="30" />                
        </Columns>
    </asp:GridView>
       
    <div class="btn">   
        <asp:Button ID="saveall" runat="server" Text="Save All" CssClass="button1 saveall"
            OnClick="Btn_Save_All_Click" />           
    </div>

</asp:Content>
