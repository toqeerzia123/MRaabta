<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerBranchMapping.aspx.cs" Inherits="MRaabta.Files.CustomerBranchMapping" MasterPageFile="~/BtsMasterPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 230px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Bts_Css/Mazen.css" rel="stylesheet" />
    <link href="../mazen/css/sweetalert.css" rel="stylesheet" />
    <link href="../AutoComplete/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../mazen/js/sweetalert-dev.js"></script>
   <script type="text/javascript" src="../AutoComplete/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="../AutoComplete/jquery-ui.js"></script>
    <script type="text/javascript">

        $(function () {
            SearchText();
            SearchTextGrd();
        });

        var prmInstance = Sys.WebForms.PageRequestManager.getInstance();
        prmInstance.add_endRequest(function () {
            //you need to re-bind your jquery events here
            SearchText();
            SearchTextGrd();
        });
        function SearchText() {
            debugger;
            $("[id$=txtSearch]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Files/WebService1.asmx/GetCustomers") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('-')[0],
                                    val: item.split('-')[1]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hdnCustomerId.ClientID %>").val(i.item.val);
                    $("[id$=hfCp]").val(i.item.label);
                    CallTextChange();
                },
                minLength: 4
            });
        }

        function SearchTextGrd() {
            //alert($("*[id=GridView1] input[id*=txtSearchGrd]"));
            debugger;
            $("*[id=GridView1] input[id*=txtSearchGrd]").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Files/WebService1.asmx/GetCustomers") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('-')[0],
                                    val: item.split('-')[1]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hdnCustomerIdGrd.ClientID %>").val(i.item.val);
                    $("[id$=hfCpGrd]").val(i.item.label);
                    CallTextChange1();
                },
                minLength: 4
            });

        }
        function CallTextChange1() {
            __doPostBack("<%=hdnCustomerIdGrd.ClientID %>", "");
        }

        function CallTextChange() {
            __doPostBack("<%=hdnCustomerId.ClientID %>", "");
        }
    </script>

    <style>
        #overlay {
            position: fixed;
            z-index: 99;
            top: 0px;
            left: 0px;
            background-color: #f8f8f8;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=90);
            opacity: 0.9;
            -moz-opacity: 0.9;
        }

        #theprogress {
            background-color: #fff;
            border: 1px solid #ccc;
            padding: 10px;
            width: 300px;
            line-height: 30px;
            text-align: center;
            filter: Alpha(Opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

        #modalprogress {
            position: absolute;
            top: 40%;
            left: 50%;
            margin: -11px 0 0 -150px;
            color: #990000;
            font-weight: bold;
            font-size: 14px;
        }
    </style>
    <asp:UpdateProgress ID="prgLoadingStatus" runat="server" DynamicLayout="true">
        <ProgressTemplate>
            <div id="overlay">
                <div id="modalprogress">
                    <div id="theprogress">
                        <asp:Image ID="imgWaitIcon" runat="server" ImageAlign="AbsMiddle" ImageUrl="../mazen/images/wait.gif" />
                        Please wait...
                    </div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="panel1">

        <ContentTemplate>
            <asp:HiddenField ID="hfCp" runat="server" />
            <asp:HiddenField ID="hdnCustomerId" runat="server" OnValueChanged="hdnCustomerId_ValueChanged" />
            <asp:HiddenField ID="hdnCustomerIdGrd" runat="server" OnValueChanged="ddlCustomerID_SelectedIndexChanged" />
            <asp:HiddenField ID="hfCpGrd" runat="server" />

            <%-- End Search box --%>
            <div class="row main-body newPanel">
                <%--<div class="col-lg-12 col-md-12 col-sm-12 screen-name">
            <div style="float: left;">
                <h3>Customer Branch Mapping</h3>
            </div>
        </div>--%>
                <fieldset class="fieldsetSmall">
                    <span id="ContentPlaceHolder1_Errorid" style="color: Red; font-weight: bold;"></span>
                    <legend style="font-size: large;"><b>Customer Branch Mapping</b></legend>
                    <div class="col-lg-12 col-md-12 col-sm-12">

                        <table>
                            <tr>
                                <td>
                                    <b>Branch</b>
                                </td>
                                <td>
                                    <b>Customer</b>
                                </td>
                                <td>
                                    <b>Location</b>
                                </td>
                                <%-- <td >
                        <b>Rider</b>
                    </td>--%>
                                <td>
                                    <b>Shipment Type</b>
                                </td>
                                <td>
                                    <b>Route </b>
                                </td>
                            </tr>
                            <tr>

                                <td style="width: 225px;">
                                    <asp:DropDownList ID="ddlBranchCode" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                                    <%--<asp:TextBox ID="" CssClass="form-control" runat="server" />--%>
                                </td>

                                <td style="width: 225px;">
                                    <asp:TextBox ID="txtSearch" runat="server" Style="margin: 10px 0 0; height: 31px; width: 275px;"></asp:TextBox>
                                    <%--                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtSearch" ForeColor="Red" ValidationExpression="^([A-z][A-Za-z]*\s+[A-Za-z]*)|([A-z][A-Za-z]*)$" ErrorMessage="Invalid Name" />--%>
                                    <%-- <asp:DropDownList ID="ddlCustomerID"  CssClass="form-control" runat="server">
                        </asp:DropDownList>--%>
                                    <%--                        <asp:TextBox ID="txtAccountNumber" Enabled="true" runat="server" OnTextChanged="txtAccountNumber_TextChanged"  />--%>


                                </td>

                                <td style="width: 225px;">
                                    <asp:DropDownList ID="ddlLocationID" CssClass="form-control" runat="server">
                                    </asp:DropDownList>

                                </td>

                                <%-- <td style="width: 225px;">
                        <asp:DropDownList ID="ddlRiderCode"  CssClass="form-control" runat="server">
                        </asp:DropDownList>

                    </td>--%>
                                <td style="width: 225px;">
                                    <asp:DropDownList ID="ddlIsHeavy" AutoPostBack="true" OnSelectedIndexChanged="ddlShipmentTypeDDL_SelectedIndexChanged" CssClass="form-control" runat="server">
                                    </asp:DropDownList>

                                </td>
                                <td style="width: 225px;">
                                    <asp:DropDownList ID="ddlRouteCode" CssClass="form-control" runat="server">
                                    </asp:DropDownList>

                                </td>
                                <td>
                                    <div class="btn_div" style="">
                                        <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" OnClick="btnSubmit_Click" Text="ADD" Style="margin-top: 9px;" />
                                    </div>
                                </td>
                                <td>
                                    <div class="btn_div" style="">
                                        <asp:Button ID="btnSearch" CssClass="btn btn-success" runat="server" OnClick="btnSearch_Click" Text="Search" Style="margin-top: 9px;" />
                                    </div>
                                </td>
                            </tr>
                        </table>

                    </div>

                    <br />
                    <div class="col-lg-12 col-md-12 col-sm-12">
                        <asp:UpdatePanel ID="grdUp1" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                    AutoGenerateDeleteButton="True" AutoGenerateEditButton="True" DataKeyNames="ClientRouteID"
                                    OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDataBound="GridView1_RowDataBound"
                                    OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing"
                                    OnRowUpdating="GridView1_RowUpdating" AllowPaging="true"
                                    OnPageIndexChanging="OnPageIndexChanging" PageSize="50"
                                    class="table table-bordered table-hover" Font-Size="Medium" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="true" BorderColor="#DA7A4D" Width="100%" BorderStyle="Solid" BorderWidth="5px">
                                    <AlternatingRowStyle BackColor="White" />

                                    <Columns>

                                        <asp:TemplateField HeaderText="Branch">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlBranchCode" Width="150px" Height="25px" runat="server">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Branch") %>'>  
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlCustomerID" OnSelectedIndexChanged="ddlCustomerID_SelectedIndexChanged" AutoPostBack="true" Width="250px" Height="25px" runat="server">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtSearchGrd" Visible="false" runat="server"></asp:TextBox>


                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>'>  
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlLocationID" Width="150px" Height="25px" runat="server">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location") %>'>  
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Shipment Type">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlIsHeavy" AutoPostBack="true" OnSelectedIndexChanged="ddlShipmentType_SelectedIndexChanged" Width="150px" Height="25px" runat="server">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipmentType" runat="server" Text='<%# Bind("ShipmentType") %>'>  
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Route">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlRouteCode" Width="150px" Height="25px" runat="server">
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRoute" runat="server" Text='<%# Bind("Route") %>'>  
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <EditRowStyle BackColor="#82aae8" />
                                    <FooterStyle BackColor="#F27031" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#DA7A4D" Font-Bold="True" ForeColor="#ffffff" HorizontalAlign="Left" />
                                    <PagerStyle BackColor="#DA7A4D" ForeColor="#ffffff" Font-Bold="true" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
