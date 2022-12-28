<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="GeneratePickupRequestList.aspx.cs" Inherits="MRaabta.Files.GeneratePickupRequestList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Bts_Css/Mazen.css" rel="stylesheet" />
    <link href="../mazen/css/sweetalert.css" rel="stylesheet" />
    <script src="../mazen/js/sweetalert-dev.js"></script>
    <script type="text/javascript">

        function disableButton(btn) {
            setTimeout(function () { btn.disabled = true; }, 2);
            return true;
        }
    </script>
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
            <asp:Panel ID="rd_1" runat="server">
                <div class="row main-body newPanel">
                    <fieldset class="fieldsetSmall">
                        <span id="ContentPlaceHolder1_Errorid" style="color: Red; font-weight: bold;"></span>
                        <legend style="font-size: medium;"><b>Pickup Request List</b></legend>
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <div class="btn_div" style="margin: 0px 0px;">


                                <asp:Button ID="btnRunScheduler" OnClientClick="return disableButton(this);" CssClass="btn btn-info" runat="server" OnClick="btnRunScheduler_Click" Text="Run Scheduler" />
                            </div>


                            <%--<div style=" text-align: center; "><h1 style=" border-bottom: 1px solid #d4d0d0; border-top: 1px solid #d4d0d0; ">UNASSIGNED</h1></div>--%>
                            <table class="dashboard-count">
                                <thead>
                                    <tr>
                                        <th colspan="8">Unassigned</th>
                                    </tr>
                                    <tr>
                                        <th>Cash on Delivery</th>
                                        <th>Domestic</th>

                                        <th>International</th>

                                        <th>Road n Rail</th>
                                    </tr>
                                </thead>
                                <tr>

                                    <td>
                                        <asp:LinkButton ID="Linkbtn_COD" runat="server" OnClick="Linkbtn_CashOnDelivery_Click" ToolTip="Cash on Delivery Unassigned Request"></asp:LinkButton>
                                    </td>

                                    <td>
                                        <asp:LinkButton ID="Linkbtn_Domestic" runat="server" OnClick="Linkbtn_Domestic_Click" ToolTip="Domestic Unassigned Request"></asp:LinkButton>
                                    </td>




                                    <td>
                                        <asp:LinkButton ID="Linkbtn_International" runat="server" OnClick="Linkbtn_International_Click" ToolTip="International Unassigned Request"></asp:LinkButton>
                                    </td>


                                    <td>
                                        <asp:LinkButton ID="Linkbtn_Road_Rail" runat="server" OnClick="Linkbtn_Road_Rail_Click" ToolTip="Road n Rail Unassigned Request"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <%--<div style="text-align: center;">
                    <h1 style="border-bottom: 1px solid #d4d0d0; border-top: 1px solid #d4d0d0;">Filters</h1>
                </div>--%>
                            <table style="width: 100%">

                                <tr>
                                    <td>
                                        <b>Pickup ID</b>

                                    </td>

                                    <td>
                                        <b>Pickup Date</b>

                                    </td>

                                    <td>
                                        <b>Account No</b>

                                    </td>

                                    <td>
                                        <b>Route Code</b>

                                    </td>
                                </tr>
                                <tr>
                                    <td>

                                        <asp:TextBox ID="txtPickupID" runat="server">
                                        </asp:TextBox>
                                    </td>

                                    <td>
                                        <asp:TextBox ID="txtPickupDate" TextMode="Date" Style="width: 230px; margin-top: 10px; height: 33px" runat="server">
                                        </asp:TextBox>
                                    </td>

                                    <td>
                                        <asp:TextBox ID="txtAccountNO" runat="server">
                                        </asp:TextBox>
                                    </td>

                                    <td>
                                        <asp:TextBox ID="txtRouteCode" runat="server">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Product</b>

                                    </td>
                                    <td>
                                        <b>Status</b>

                                    </td>
                                    <td><%--<b> Sub Status</b>--%></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlProduct" Style="width: 230px;" CssClass="form-control" runat="server">
                                        </asp:DropDownList>

                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" Style="width: 230px;" CssClass="form-control" runat="server" EnableViewState="true" AutoPostBack="True" OnSelectedIndexChanged="checkStatus_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSubStatus" Style="width: 230px;" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSubmit" CssClass="btn btn-success" runat="server" OnClick="btnSubmit_Click" Text="Search" />
                                        <td>


                                            <td></td>
                                </tr>
                            </table>

                        </div>

                    </fieldset>

                    <div class="tbd">
                        <asp:GridView ID="grdPR" runat="server" AutoGenerateColumns="False" AllowPaging="True" OnRowCommand="grdPR_RowCommand" OnRowDataBound="grdPR_RowDataBound"
                            OnPageIndexChanging="OnPageIndexChanging"
                            class="" CellPadding="4" ForeColor="#333333"
                            GridLines="None" AllowSorting="True" Font-Bold="True" Font-Overline="False"
                            Font-Strikeout="False" Font-Underline="False" EnableTheming="False">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderText="" Visible="true">
                                    <ItemTemplate>
                                        <b>
                                            <asp:LinkButton ID="lnkView" runat="server" Style="padding: 0px 0px; font-weight: 0;" CssClass="btn btn-link" OnClick="linkRequesID_Click" CommandArgument='<%#Eval("PickupRequestID") %>'>View</asp:LinkButton></b>


                                        <%-- <%# Bind("SchedulerID") %>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pickup ID" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPickupRequestID" runat="server" Text='<%# Bind("PickupRequestID") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Time slot" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTimeslot" runat="server" Text='<%# Bind("Timeslot") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblArea" runat="server" Text='<%# Bind("Product") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Service" Visible="true" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPickupDate" runat="server" Text='<%# Bind("Service") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Weight Type" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWeightTypeID" runat="server" Text='<%# Bind("WeightTypeID") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("FName") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contact Number" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContactNo" runat="server" Text='<%# Bind("ContactNo") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Address" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("Address") %>'>  
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Route Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlRouteCode" Width="125px" runat="server" AutoPostBack="true" DataTextField='RouteCode' OnSelectedIndexChanged="EditRouteCode">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rider Code" Visible="true">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlRiderCode" Width="125px" runat="server">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" Visible="true">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlStatus" OnSelectedIndexChanged="changesubstatus_onindexchanged" AutoPostBack="true" Width="125px" runat="server" DataTextField='Status'>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reason" Visible="true">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlSubStatus" Width="125px" runat="server" DataTextField='SubStatus'>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Reason" Visible="true">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlReason" Width="125px" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Remarks" runat="server" Visible="true" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRemarks" MaxLength="100" runat="server" Text='<%# Bind("AdditionalRemarks") %>'>
                                        </asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- <asp:BoundField ItemStyle-Width="150px" DataField="consignmentNumber" HeaderText="Consignment Number" />
                        <%-- <asp:BoundField ItemStyle-Width="150px" DataField="consignmentNumber" HeaderText="Consignment Number" />
                <asp:BoundField ItemStyle-Width="150px" DataField="consigner" HeaderText="consigner Name" />
                <asp:BoundField ItemStyle-Width="150px" DataField="CnDataSyncOn" HeaderText="Cn Date" DataFormatString="{0:d}" />
                <asp:BoundField ItemStyle-Width="150px" DataField="serviceTypeName" HeaderText="serviceTypeName" />
                <asp:BoundField ItemStyle-Width="150px" DataField="remarks" HeaderText="Remarks" />
                <asp:BoundField ItemStyle-Width="150px" DataField="LoadSheetNo" HeaderText="LoadSheet No" />--%>

                                <asp:TemplateField HeaderText="Action" Visible="true">
                                    <ItemTemplate>

                                        <%-- <asp:LinkButton ID="btnUpdate" runat="server" Text="Update" CommandName="CmdUpdate" CommandArgument='<%#Bind("PickupRequestID") %>'>
                                </asp:LinkButton>--%>
                                        <asp:Button ID="btnUpdate" OnClientClick="return disableButton(this);" Style="height: 30px; font-size: smaller; text-align: center; padding: 2px 2px" CssClass="btn btn-success" runat="server"
                                            CommandName="CmdUpdate" CommandArgument='<%#Bind("PickupRequestID") %>' Text="Update" />
                                    </ItemTemplate>
                                    <ItemStyle />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#f26726" Font-Bold="True" ForeColor="White" Width="200px" />
                            <HeaderStyle BackColor="#f26726" Font-Bold="True" ForeColor="White" BorderColor="White" BorderStyle="Solid" BorderWidth="1px" />
                            <PagerStyle BackColor="#f26726" ForeColor="White" HorizontalAlign="Center" Width="200px" />
                            <RowStyle BackColor="#fbefe9" ForeColor="#333333" Font-Bold="false" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="false" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </div>
                </div>
                <asp:LinkButton ID="Linkbtn_Aviation" runat="server" Visible="false" OnClick="Linkbtn_Aviation_Click" ToolTip="Aviation Sale Unassigned Request"></asp:LinkButton>
                <asp:LinkButton ID="Linkbtn_Import" runat="server" Visible="false" OnClick="Linkbtn_Import_Click" ToolTip="Import Unassigned Request"></asp:LinkButton>
                <asp:LinkButton ID="Linkbtn_Jazz_Card" runat="server" Visible="false" OnClick="Linkbtn_Jazz_Card_Click" ToolTip="Jazz Card Unassigned Request"></asp:LinkButton>
                <asp:LinkButton ID="Linkbtn_Jazz_Cash" runat="server" Visible="false" OnClick="Linkbtn_Jazz_Cash_Click" ToolTip="JAzz cash Unassigned Request"></asp:LinkButton>
            </asp:Panel>
        </ContentTemplate>

    </asp:UpdatePanel>

    <script type="text/javascript" language="javascript">
        function DisableDropDowns() {
            //if (document.getElementById("ContentPlaceHolder1_grdPR")) {
            //    //alert("Rows = " + document.getElementById("ContentPlaceHolder1_grdPR").getElementsByTagName("tr").length);
            //    var count = (document.getElementById("ContentPlaceHolder1_grdPR").getElementsByTagName("tr").length) - 1;
            //    if (count > 0) {
            //        var i = 0;
            //        for (i = 0; i <= count; i++) {
            //            if (document.getElementById('ContentPlaceHolder1_grdPR_ddlStatus_' + i) != null) {
            //                if (document.getElementById('ContentPlaceHolder1_grdPR_ddlStatus_' + i).value == "1") {
            //                    document.getElementById('ContentPlaceHolder1_grdPR_ddlStatus_' + i).disabled = true;
            //                }

            //                if (document.getElementById('ContentPlaceHolder1_grdPR_ddlStatus_' + i).value != "6") {
            //                    document.getElementById('ContentPlaceHolder1_grdPR_ddlSubStatus_' + i).disabled = true;
            //                } 

            //            }
            //        }
            //    }
            //}
        }
        DisableDropDowns();

    </script>
</asp:Content>
