<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="SearchClientGroups.aspx.cs" Inherits="MRaabta.Files.SearchClientGroups" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function ChangeMode() {
            debugger;

            var mode = document.getElementById('<%= rbtn_mode.ClientID %>');
            var btn_create = document.getElementById('<%= btn_Create.ClientID %>');
            var btn_search = document.getElementById('<%= btn_search.ClientID %>');
            var txt_grpName = document.getElementById('<%= txt_grpName.ClientID %>');
            var txt_grpID = document.getElementById('<%= txt_grpID.ClientID %>');
            var dd_grpName = document.getElementById('<%= dd_grpBranch.ClientID %>');
            var selectedValue = "";
            for (var i = 0; i < mode.rows[0].cells.length; i++) {
                if (mode.rows[0].cells[i].children[0].checked) {
                    selectedValue = mode.rows[0].cells[i].children[0].defaultValue;
                }
            }
            if (selectedValue == "1") {

                btn_create.style.display = 'block';
                btn_search.style.display = 'none';
                txt_grpID.value = "";
                txt_grpName.value = "";
                dd_grpName.selectedIndex = 0;
                //dd_grpName.style.disabled = false;
            }
            else {
                btn_create.style.display = 'none';
                btn_search.style.display = 'block';
                txt_grpID.value = "";
                txt_grpName.value = "";
                dd_grpName.selectedIndex = 0;
            }

        }
    </script>
    <table style="width: 95%; font-family: Calibri; font-size: medium; border-collapse: collapse;
        border-radius: 10px; box-shadow: 0 0 1px #000; margin: 2.5%;">
        <tr>
            <td style="width: 100%; text-align: center; font-size: large; font-variant: small-caps;"
                colspan="6">
                <b>Client Groups</b>
            </td>
        </tr>
        <tr>
            <td style="width: 10%; padding-left: 5%;">
                <b>Mode</b>
            </td>
            <td style="width: 80%; padding-right:5%; text-align: left;" colspan="5">
                <asp:RadioButtonList ID="rbtn_mode" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                    onchange="ChangeMode();">
                    <asp:ListItem Value="0" Selected="True">Existing</asp:ListItem>
                    <asp:ListItem Value="1">New</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td style="width: 10%; padding-left: 5%;">
                Group ID
            </td>
            <td style="width: 5%;">
                <asp:TextBox ID="txt_grpID" runat="server" Enabled="false" Width="90%"></asp:TextBox>
            </td>
            <td style="width: 10%;">
                Group Name
            </td>
            <td style="width: 15%;">
                <asp:TextBox ID="txt_grpName" runat="server" Width="90%"></asp:TextBox>
            </td>
            <td style="width: 10%;">
                Collection Branch
            </td>
            <td style="width: 20%; padding-right:5%">
                <asp:DropDownList ID="dd_grpBranch" runat="server" Enabled="true" AppendDataBoundItems="true" Width="95%">
                    <asp:ListItem Value="0">.::Select Branch::.</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 10%; padding-left: 5%;">
            </td>
            <td style="width: 5%;">
            </td>
            <td style="width: 10%;">
            </td>
            <td style="width: 15%;">
            </td>
            <td style="width: 10%;">
            </td>
            <td style="width: 20%; padding-right:5%; text-align:right;">
                <asp:Button ID="btn_search" runat="server" CssClass="button" Text="Search Group"
                    OnClick="btn_search_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_Create" runat="server" CssClass="button" Text="Create Group"
                    OnClick="btn_Create_Click" Style="display: none;" />
            </td>
        </tr>
    </table>
    <div style="width: 100%; padding-left: 10%; padding-right: 10%;" class="tbl-large">
        <asp:GridView ID="gv_existingGroups" runat="server" AutoGenerateColumns="false" Width="80%"
            ShowHeaderWhenEmpty="true" EmptyDataText="No Groups Found" CssClass="mGrid" ShowFooter="true">
            <Columns>
                <asp:BoundField HeaderText="GRP ID" DataField="ID" />
                <asp:BoundField HeaderText="Name" DataField="NAME" />
                <asp:BoundField HeaderText="Collection Center" DataField="BranchName" />
                <asp:BoundField HeaderText="Status" DataField="GRPSTATUS" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
