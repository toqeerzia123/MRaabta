<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="Sequence_Issuance.aspx.cs" Inherits="MRaabta.Files.Sequence_Issuance" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="div2" runat="server" class="outer_box" style="display: none; background: #FFFFFF none repeat scroll 0 0;
        text-align: center; vertical-align: middle; height: 100%; position: absolute;
        width: 83%;">
        <img src="../images/Loading_Movie-02.gif" style="" />
    </div>
    <style>
        body
        {
            font-family: Calibri;
        }
        
        ul.tab
        {
            list-style-type: none;
            margin: 0;
            padding: 0;
            overflow: hidden;
            border: 1px solid #ccc;
            background-color: #f1f1f1;
        }
        
        /* Float the list items side by side */
        ul.tab li
        {
            float: left;
        }
        
        /* Style the links inside the list items */
        ul.tab li a
        {
            display: inline-block;
            color: black;
            text-align: center;
            padding: 14px 16px;
            text-decoration: none;
            transition: 0.3s;
            font-size: 17px;
        }
        
        /* Change background color of links on hover */
        ul.tab li a:hover
        {
            background-color: #ddd;
        }
        
        /* Create an active/current tablink class */
        ul.tab li a:focus, .active
        {
            background-color: #ccc;
        }
        
        /* Style the tab content */
        .tabcontent
        {
            display: none;
            padding: 6px 12px;
            border: 1px solid #ccc;
            border-top: none;
        }
    </style>
    <ul class="tab">
        <li><a href="javascript:void(0)" class="tablinks" onclick="openCity(event, 'BasicInfo')">
            Zone</a></li>
        <li><a href="javascript:void(0)" class="tablinks" onclick="openCity(event, 'Configuration')">
            Branch</a></li>
        <li><a href="javascript:void(0)" class="tablinks" onclick="openCity(event, 'billingNsales')">
            Express Center - Rider</a></li>
    </ul>
    <div id="nationalDiv" style="display: none; background: #FFFFFF none repeat scroll 0 0;
        text-align: center; vertical-align: middle; height: 100%; position: absolute;
        width: 83%;">
        <table style="width: 75%; margin-left: 12.5%;">
            <tr>
                <td style="width: 15%; text-align: right;">
                    Account No
                </td>
                <td style="width: 20%; text-align: left;">
                    <asp:TextBox ID="txt_nationWideAccountNo" runat="server"></asp:TextBox>
                </td>
                <td style="width: 15%; text-align: right;">
                    Group
                </td>
                <td style="width: 20%; text-align: left;">
                    <asp:DropDownList ID="dd_nationWideGroup" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Value="0">.::Select Group::.</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 30%; text-align: left;">
                </td>
            </tr>
            <tr>
                <td style="width: 15%; text-align: right;">
                    Parent Branch
                </td>
                <td style="width: 20%; text-align: left;">
                    <asp:DropDownList ID="dd_parentBranch" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Value="0">.::Select Parent::.</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 15%; text-align: right;">
                </td>
                <td style="width: 20%; text-align: left;">
                </td>
                <td style="width: 30%; text-align: left;">
                </td>
            </tr>
            <tr>
                <td style="width: 15%; text-align: right; vertical-align: top;">
                    Branches
                </td>
                <td colspan="3" style="width: 55%; text-align: left;">
                    <div style="height: 400px; width: 100%; overflow: scroll;">
                        <asp:CheckBoxList ID="chkList_Branches" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                            Width="100%">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td style="width: 30%; text-align: left; vertical-align: top;">
                    <asp:CheckBox ID="chk_checkAll" runat="server" Text="ALL" />
                </td>
            </tr>
            <tr>
                <td style="width: 15%; text-align: right;">
                </td>
                <td style="width: 55%; text-align: right; text-align: center;" colspan="3">
                    <asp:Button ID="btn_NationWideOk" runat="server" Text="OK" OnClientClick="NationalOK();"
                        Width="100px" CssClass="button" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btn_NationWideCancel" runat="server" Text="Cancel" OnClientClick="NationalCancel();"
                        Width="100px" CssClass="button" />
                </td>
                <td style="width: 30%; text-align: left;">
                </td>
            </tr>
        </table>
    </div>
    <div id="BasicInfo" class="tabcontent">
        <asp:Label ID="ZoneError" runat="server" Font-Bold="true"></asp:Label>
        <table width="100%">
            <tr>
                <td>
                    Zone
                </td>
                <td>
                    <asp:DropDownList ID="dd_Zone" runat="server" CssClass="dropdown" AppendDataBoundItems="true"
                        Width="200px">
                        <asp:ListItem Value="0"> Select Zone </asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
            <tr>
                <td>
                    Product
                </td>
                <td>
                    <asp:DropDownList ID="dd_Product" runat="server" CssClass="dropdown" Width="200px"
                        AppendDataBoundItems="true">
                        <asp:ListItem Value="0"> Select Product </asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
            <tr>
                <td>
                    Sequence Start
                </td>
                <td class="input-field" style="width: 25% !important;">
                    <asp:TextBox ID="txt_startSeq" runat="server" MaxLength="16" Width="200px" CssClass="textBox"></asp:TextBox>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
            <tr>
                <td class="field">
                    Sequence End
                </td>
                <td class="input-field" style="width: 25% !important;">
                    <asp:TextBox ID="txt_seqend" runat="server" MaxLength="16" Width="200px" CssClass="textBox"></asp:TextBox>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
        </table>
        <div style="width: 100%; text-align: center;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btn_zoneSearch" runat="server" Text="Search" CssClass="button" UseSubmitBehavior="false"
                        OnClick="btn_zoneSearch_Click" />
                    &nbsp; &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btn_Save" runat="server" Text="Submit" CssClass="button" OnClick="btn_Save_Click"
                        UseSubmitBehavior="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
            <span id="Span1" class="tbl-large">
                <asp:GridView ID="gv_Main" runat="server" CssClass="mGrid" AutoGenerateColumns="false"
                    AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                    OnRowCommand="gv_Main_RowCommand">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btn_MainRemove" runat="server" Text="Delete" CssClass="button" CommandName="Del"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnClientClick="return confirm('Are you certain you want to delete this product?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Zone" DataField="Zone" />
                        <asp:BoundField HeaderText="Start Seq" DataField="SequenceStart" />
                        <asp:BoundField HeaderText="End Seq" DataField="EndSequence" />
                        <asp:BoundField HeaderText="Product" DataField="Product" />
                        <asp:BoundField HeaderText="Issued Qty" DataField="Qty" />
                        <asp:BoundField HeaderText="Issued Date" DataField="Created_On" />
                        <asp:BoundField HeaderText="Issued By" DataField="U_NAME" />
                    </Columns>
                </asp:GridView>
            </span>
        </div>
    </div>
    <div id="Configuration" class="tabcontent">
        <asp:Label ID="BranchError" runat="server" Font-Bold="true"></asp:Label>
        <table width="100%">
            <tr>
                <td>
                    Zone
                </td>
                <td>
                    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" CssClass="dropdown"
                        AppendDataBoundItems="true" Width="200px" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                        <asp:ListItem Value="0"> Select Zone </asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
            <tr>
                <td>
                    Branch
                </td>
                <td>
                    <asp:DropDownList ID="dd_Branch" runat="server" CssClass="dropdown" Width="200px"
                        AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="dd_Branch_SelectedIndexChanged">
                        <asp:ListItem Value="0"> Select Branch </asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
            <tr>
                <td>
                    Product
                </td>
                <td>
                    <asp:DropDownList ID="DropDownList2" runat="server" CssClass="dropdown" Width="200px"
                        AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged">
                        <asp:ListItem Value="0"> Select Product </asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
            <tr>
                <td>
                    Sequence Start
                </td>
                <td class="input-field" style="width: 25% !important;">
                    <asp:TextBox ID="TextBox1" runat="server" MaxLength="20" Width="200px" CssClass="textBox"></asp:TextBox>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
            <tr>
                <td class="field">
                    Sequence End
                </td>
                <td class="input-field" style="width: 25% !important;">
                    <asp:TextBox ID="TextBox2" runat="server" MaxLength="20" Width="200px" CssClass="textBox"></asp:TextBox>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
        </table>
        <div style="width: 100%; text-align: center;">
            <asp:UpdatePanel ID="up_1" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btn_searchBranchSeq" runat="server" Text="Search" CssClass="button"
                        UseSubmitBehavior="false" OnClick="btn_searchBranchSeq_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btn_Save1" runat="server" Text="Submit" CssClass="button" OnClick="btn_Save1_Click"
                        UseSubmitBehavior="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
            <span id="Span2" class="tbl-large">
                <asp:GridView ID="GridView1" runat="server" CssClass="mGrid" AutoGenerateColumns="false"
                    AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                    OnRowCommand="GridView1_RowCommand">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btn_branchDelete" runat="server" Text="Delete" CommandName="Del"
                                    CssClass="button" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Zone" DataField="Zone" />
                        <asp:BoundField HeaderText="Branch" DataField="Bname" />
                        <asp:BoundField HeaderText="Start Seq" DataField="SequenceStart" />
                        <asp:BoundField HeaderText="End Seq" DataField="SequenceEnd" />
                        <asp:BoundField HeaderText="Product" DataField="Product" />
                        <asp:BoundField HeaderText="Issued Qty" DataField="QTY" />
                        <asp:BoundField HeaderText="Issued By" DataField="CreatedBy" />
                        <asp:BoundField HeaderText="Issued On" DataField="Created_On" />
                    </Columns>
                </asp:GridView>
            </span>
        </div>
    </div>
    <div id="billingNsales" class="tabcontent">
        <asp:Label ID="ECError" runat="server" Font-Bold="true"></asp:Label>
        <table width="100%">
            <tr>
                <td>
                    Zone
                </td>
                <td>
                    <asp:DropDownList ID="DropDownList3" runat="server" CssClass="dropdown" AutoPostBack="true"
                        AppendDataBoundItems="true" Width="200px" OnSelectedIndexChanged="DropDownList3_SelectedIndexChanged">
                        <asp:ListItem Value="0"> Select Zone </asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
            <tr>
                <td>
                    Branch
                </td>
                <td>
                    <asp:DropDownList ID="DropDownList4" runat="server" CssClass="dropdown" AutoPostBack="true"
                        AppendDataBoundItems="true" Width="200px" OnSelectedIndexChanged="DropDownList4_SelectedIndexChanged">
                        <asp:ListItem Value="0"> Select Branch </asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:RadioButtonList ID="rb_list" runat="server" RepeatDirection="Horizontal" 
                        RepeatLayout="Table" onselectedindexchanged="rb_list_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="R" Selected="True">R</asp:ListItem>
                        <asp:ListItem Value="E">E</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    Express Center
                </td>
                <td>
                    <telerik:RadComboBox ID="dd_ExpressCenter" runat="server" AutoPostBack="true" Skin="Metro"
                        AppendDataBoundItems="true" AllowCustomText="true" MarkFirstMatch="true" Width="200px"
                        CssClass="dropdown" OnSelectedIndexChanged="dd_ExpressCenter_SelectedIndexChanged">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="Select Express Center" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
            <tr>
                <td>
                    Rider Code
                </td>
                <td>
                    <telerik:RadComboBox ID="dd_ridercode" runat="server" AutoPostBack="true" Skin="Metro"
                        AppendDataBoundItems="true" AllowCustomText="true" MarkFirstMatch="true" Width="200px"
                        CssClass="dropdown" OnSelectedIndexChanged="dd_ridercode_SelectedIndexChanged">
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Text="Select Ridercode" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
            <tr>
                <td>
                    Product
                </td>
                <td>
                    <asp:DropDownList ID="DropDownList5" runat="server" CssClass="dropdown" Width="200px">
                        <asp:ListItem Value="0"> Select Product </asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
            <tr>
                <td>
                    Sequence Start
                </td>
                <td class="input-field" style="width: 25% !important;">
                    <asp:TextBox ID="TextBox3" runat="server" MaxLength="16" Width="200px" CssClass="textBox"></asp:TextBox>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
            <tr>
                <td class="field">
                    Sequence End
                </td>
                <td class="input-field" style="width: 25% !important;">
                    <asp:TextBox ID="TextBox4" runat="server" MaxLength="16" Width="200px" CssClass="textBox"></asp:TextBox>
                </td>
                <td colspan="2" width="50%">
                </td>
            </tr>
        </table>
        <div style="width: 100%; text-align: center;">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btn_searchExpressSeq" runat="server" Text="Search" CssClass="button"
                        UseSubmitBehavior="false" OnClick="btn_searchExpressSeq_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btn_save2" runat="server" Text="Submit" CssClass="button" OnClick="btn_save2_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
            <span id="Span3" class="tbl-large">
                <asp:GridView ID="GridView2" runat="server" CssClass="mGrid" AutoGenerateColumns="false"
                    AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                    OnRowCommand="GridView2_RowCommand">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btn_ecDelete" runat="server" CssClass="button" Text="Delete" CommandName="Del"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Zone" DataField="zone" />
                        <asp:BoundField HeaderText="Branch" DataField="Bname" />
                        <asp:BoundField HeaderText="Express Center" DataField="ECName" />
                        <asp:BoundField HeaderText="Rider" DataField="RiderName" />
                        <asp:BoundField HeaderText="Seq Start" DataField="SequenceStart" />
                        <asp:BoundField HeaderText="Seq End" DataField="SequenceEnd" />
                        <asp:BoundField HeaderText="Product" DataField="Product" />
                        <asp:BoundField HeaderText="Issued Qty" DataField="Qty" />
                        <asp:BoundField HeaderText="Issued By" DataField="CreatedBy" />
                        <asp:BoundField HeaderText="Issued On" DataField="Created_On" />
                    </Columns>
                </asp:GridView>
            </span>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        function Myfunc(cityName) {
            var i, tabcontent, tablinks;
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }
            document.getElementById(cityName).style.display = "block";
        }
        function openCity(evt, cityName) {
            debugger;
            var i, tabcontent, tablinks;
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }
            tablinks = document.getElementsByClassName("tablinks");
            for (i = 0; i < tablinks.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" active", "");
            }
            document.getElementById(cityName).style.display = "block";
            evt.currentTarget.className += " active";
        }

        function Show_Hide_By_Display() {
            document.getElementById('<%=div2.ClientID %>').style.display = "";
        }

        function National() {
            //         

        }

        function NationalOK() {
            document.getElementById('nationalDiv').style.display = "none";
            openCity(event, 'BasicInfo');
        }
        function NationalCancel() {
            debugger;
            document.getElementById('nationalDiv').style.display = "none";
            openCity(event, 'BasicInfo');
        }
    </script>
</asp:Content>
