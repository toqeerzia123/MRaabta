<%@ Page Language="C#" Title="Packing Material Request List" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="PackingMaterialRequestList.aspx.cs" Inherits="MRaabta.Files.PackingMaterialRequestList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Bts_Css/Mazen.css" rel="stylesheet" />
    <link href="../mazen/css/sweetalert.css" rel="stylesheet" />
    <script src="../mazen/js/sweetalert-dev.js"></script>
    <script type="text/javascript" language="javascript">
        function numeric(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && ((charCode >= 48 && charCode <= 57)))
                return true;
            else {

                return false;
            }
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
        <progresstemplate>
        <div id="overlay">
            <div id="modalprogress">
                <div id="theprogress">
                    <asp:Image ID="imgWaitIcon" runat="server" ImageAlign="AbsMiddle" ImageUrl="../mazen/images/wait.gif" />
                    Please wait...
                </div>
            </div>
        </div>
    </progresstemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="panel1">
        <contenttemplate>
            <div class="row main-body newPanel">
                <fieldset class="fieldsetSmall">
                    <span id="ContentPlaceHolder1_Errorid" style="color: Red; font-weight: bold;"></span>
                    <legend style="font-size: medium;"><b>Packing Material Request List</b></legend>
                    <div class="row main-body newPanel">

                        <%--<div class="col-lg-12 col-md-12 col-sm-12 screen-name">
            <div style="float: left;">
                <h3>Pickup Request List </h3>
            </div>
        </div>--%>
                        <div class="btn_div">
                    <asp:Button ID="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" Text="Add New Request" />
                </div>
                        <div class="col-lg-12 col-md-12 col-sm-12">

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 70px">
                                        <b>Packing Request ID</b>
                                    </td>
                                    <td style="width: 70px">
                                        <asp:TextBox ID="txtPackingRequestID" onkeypress="return numeric(event)" MaxLength="10" Width="150px" Height="25px" runat="server">
                                        </asp:TextBox>
                                    </td>
                                 <td style="width: 70px">
                                        <b>Request Date</b>
                                    </td>
                                    <td style="width: 70px">
                                                              <asp:TextBox ID="txtRequesDate" TextMode="Date" Enabled="true" ClientIDMode="Static" runat="server" Style="margin: 10px 0;" />
              
                                       
                                    </td>
                                   <td style="width: 70px">
                                        <b>Account No</b>
                                    </td>
                                    <td style="width: 140px">
                                        <asp:TextBox ID="txtAccountNO" MaxLength="50" Width="150px" Height="25px" runat="server">
                                        </asp:TextBox>
                                    </td>
                                   
                                </tr>
                                <tr >
                                     <td style="width: 70px">
                                        <b>Status</b>
                                    </td>
                                  <td style="width: 70px">
                                        <asp:DropDownList ID="ddlStatus" runat="server">
                                            <asp:ListItem Text="All"  Selected Value=""></asp:ListItem>
                                            <asp:ListItem Text="Waiting For Approval"  Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Waiting For Issued" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Pending Invoice" Value="3"></asp:ListItem>
                                               <asp:ListItem Text="Invoice Saved" Value="4"></asp:ListItem>
                                               <asp:ListItem Text="Posted" Value="5"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                      <td style="width: 70px" colspan="5" >
                                        <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" OnClick="btnSubmit_Click" Text="Search" />

                                    </td>
                                    </tr>
                            </table>
                        </div>

                    </div>
                    <br />
                    <div>
                        <asp:GridView ID="grPML" runat="server" AutoGenerateColumns="false" AllowPaging="True" OnRowCommand="grPML_RowCommand" OnRowDataBound="grPML_RowDataBound"
                            OnPageIndexChanging="grPML_PageIndexChanging" Font-Size="Medium" CellPadding="4" ForeColor="#333333" GridLines="None" Font-Bold="True" Width="100%"
                            EnableTheming="False" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="5px" EmptyDataText="No Request Generated">
                            <Columns>
                                <asp:TemplateField HeaderText="Packing Request ID" Visible="true">
                                    <ItemTemplate>

                                        <asp:LinkButton ID="linkRequesID" runat="server" OnClick="linkRequesID_Click" Text='<%# Bind("PackingRequestID") %>'>  
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account NO" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountNO" runat="server" Text='<%# Bind("accountNo") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountName" runat="server" Text='<%# Bind("name") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocationName" runat="server" Text='<%# Bind("LocationID") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Request Date" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestDate" runat="server" Text='<%# Bind("RequestDate") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("AppStatus") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <EditRowStyle BackColor="#999999" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="1px" />
                            <FooterStyle BackColor="#CC3399" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#DA7A4D" Font-Bold="True" ForeColor="White" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                            <PagerStyle BackColor="#DA7A4D" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#fbefe9" ForeColor="#333333" Font-Bold="false" HorizontalAlign="Center" BorderColor="#DA7A4D" BorderStyle="Solid" BorderWidth="1px" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="false" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </div>


                </fieldset>
            </div>
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
