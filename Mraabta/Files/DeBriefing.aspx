<%@ Page Title="" Language="C#" MasterPageFile="~/BtsMasterPage.Master" AutoEventWireup="true" CodeBehind="DeBriefing.aspx.cs" Inherits="MRaabta.Files.DeBriefing" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .outer_box {
            background: #444 none repeat scroll 0 0;
            height: 110%;
            left: 0;
            opacity: 0.9;
            position: absolute;
            top: -1%;
            width: 100%;
        }


        .pop_div {
            background: #eee none repeat scroll 0 0;
            border-radius: 10px;
            height: 500px;
            left: 21%;
            position: relative;
            top: 15%;
            width: 1008px;
        }

        .btn_ok {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: -18px;
            padding: 1px 14px;
            position: relative;
            top: 67%;
        }

        .btn_cancel {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: 22%;
            padding: 1px 14px;
            position: relative;
            top: 42%;
        }

        .pop_div > span {
            float: left;
            line-height: 40px;
            text-align: center;
            width: 100%;
        }

        .tbl-large div {
            position: static;
        }

        .btninaspclass {
            font-weight: bold;
        }

        .tabsClass {
        }
    </style>
    <script type="text/javascript">
        function loader() {
            document.getElementById('<%=loader.ClientID %>').style.display = "";
        }
    </script>
    <div runat="server" id="loader" style="float: left; opacity: 0.7; position: absolute; text-align: center; display: none; top: 50%; width: 84% !important;">
        <div class="loader">
            <img src="../images/Loading_Movie-02.gif" />
        </div>
    </div>


    <fieldset>
        <legend style="font-size: small;">De-Briefing</legend>
        <asp:Label ID="Errorid" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Large"></asp:Label>


        <table style="font-family: Calibri; font-size: small; padding-bottom: 0px !important; padding-top: 10px !important; margin-top: 5px; width: 97% !important"
            class="input-form">
            <tr>
                <td class="field" style="width: 3% !important; text-align: right">CN #:
                </td>
                <td class="input-field" style="width: 13%">
                    <asp:TextBox ID="txt_consignment" runat="server" Width="90%" OnTextChanged="txt_consignment_TextChanged" AutoPostBack="true"
                        onchange="loader();"></asp:TextBox>
                    <asp:RequiredFieldValidator ErrorMessage="Insert CN#" ForeColor="Red" ControlToValidate="txt_consignment" runat="server" />
                </td>
                <td class="field" style="width: 8% !important; text-align: right">Comments:</td>
                <td class="input-field" style="width: 18%">
                    <asp:TextBox ID="txt_comments" TextMode="MultiLine" runat="server" Width="95%"></asp:TextBox>
                    <asp:RequiredFieldValidator ErrorMessage="Insert Comments" ForeColor="Red" ControlToValidate="txt_comments" runat="server" />

                </td>
                <td class="field" style="width: 8% !important; text-align: right">Runsheet Number:</td>
                <td class="input-field" style="width: 10%">
                    <%-- <asp:DropDownList ID="ddl_runsheetNumber" runat="server" Width="132" Height="30px">
                    </asp:DropDownList>--%>
                    <asp:TextBox ID="txt_runsheetNumber" runat="server" Width="90%"></asp:TextBox>
                </td>
                <td class="field" style="width: 7% !important; text-align: right">Reason:</td>
                <td class="input-field" style="width: 10%">
                    <asp:DropDownList ID="ddl_reason" runat="server" Width="122" Height="26px">
                    </asp:DropDownList>
                </td>
                <td class="field" style="width: 7% !important; text-align: right">Status:</td>
                <td class="input-field" style="width: 10%">
                    <asp:DropDownList ID="ddl_status" runat="server" Width="122" Height="26px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfv1" runat="server" ForeColor="Red" ControlToValidate="ddl_status" InitialValue="Select Status" ErrorMessage="Please select status" />
                </td>

            </tr>
            <tr>
                <td class="field" style="width: 10% !important; text-align: right">Shipper Name:</td>
                <td class="input-field" style="width: 15%">
                    <asp:TextBox ID="txt_shipperName" runat="server" Width="90%"></asp:TextBox>
                </td>

                <td class="field" style="width: 12% !important; text-align: right">Shipper Address:                 
                </td>
                <td class="input-field" style="width: 24%">
                    <asp:TextBox ID="txt_shipperAddress" runat="server" Width="90%"></asp:TextBox>

                </td>
                <td class="field" style="width: 12% !important; text-align: right">Shipper Contact:</td>
                <td class="input-field" style="width: 15%">
                    <asp:TextBox ID="txt_shipperContact" runat="server" Width="90%"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td class="field" style="width: 10% !important; text-align: right">Consignee Name:</td>
                <td class="input-field" style="width: 15%">
                    <asp:TextBox ID="txt_ConsigneeName" runat="server" Width="90%"></asp:TextBox>
                </td>

                <td class="field" style="width: 12% !important; text-align: right">Consignee Address:                 
                </td>
                <td class="input-field" style="width: 24%">
                    <asp:TextBox ID="txt_ConsigneeAddress" runat="server" Width="90%"></asp:TextBox>

                </td>
                <td class="field" style="width: 12% !important; text-align: right">Consignee Contact:</td>
                <td class="input-field" style="width: 15%">
                    <asp:TextBox ID="txt_ConsigneeContact" runat="server" Width="90%"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td colspan="6" class="field" style="width: 16% !important; text-align: left">
                    <asp:Button ID="btn_save" Text="Save" OnClick="btn_save_Click" runat="server" CssClass="button1"
                        Width="60px" />
                </td>

            </tr>
        </table>

    </fieldset>
    <fieldset>
        <legend style="font-size: small;">De-Briefing History</legend>
        <table style="font-family: Calibri; font-size: small; padding-bottom: 0px !important; padding-top: 10px !important; margin-top: 5px; width: 97% !important"
            class="input-form">
            <tr>
                <td class="field" style="width: 12% !important; text-align: right">Shipper Name:</td>
                <td class="input-field" style="width: 20%">
                    <asp:Label ID="lbl_shipperName" runat="server" />
                </td>

                <td class="field" style="width: 12% !important; text-align: right">Shipper Address:                 
                </td>
                <td class="input-field" style="width: 20%">
                    <asp:Label ID="lbl_shipperAddress" runat="server" />

                </td>
                <td class="field" style="width: 12% !important; text-align: right">Shipper Contact:</td>
                <td class="input-field" style="width: 20%">
                    <asp:Label ID="lbl_shipperContact" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="field" style="width: 12% !important; text-align: right">Consignee Name:</td>
                <td class="input-field" style="width: 20%">
                    <asp:Label ID="lbl_consigneeName" runat="server" />
                </td>

                <td class="field" style="width: 12% !important; text-align: right">Consignee Address:                 
                </td>
                <td class="input-field" style="width: 20%">
                    <asp:Label ID="lbl_consigneeAddress" runat="server" />

                </td>
                <td class="field" style="width: 12% !important; text-align: right">Consignee Contact:</td>
                <td class="input-field" style="width: 20%">
                    <asp:Label ID="lbl_consigneeContact" runat="server" />
                </td>

            </tr>
            <tr>
                <td class="field" style="width: 12% !important; text-align: right">Booking Date:</td>
                <td class="input-field" style="width: 20%">
                    <asp:Label ID="lbl_bookingDate" runat="server" />
                </td>

                <td class="field" style="width: 12% !important; text-align: right">Weight:                 
                </td>
                <td class="input-field" style="width: 20%">
                    <asp:Label ID="lbl_weight" runat="server" />

                </td>
                <td class="field" style="width: 12% !important; text-align: right">Pieces:</td>
                <td class="input-field" style="width: 20%">
                    <asp:Label ID="lbl_pieces" runat="server" />
                </td>

            </tr>
            <tr>
                <td class="field" style="width: 12% !important; text-align: right">Origin:</td>
                <td class="input-field" style="width: 20%">
                    <asp:Label ID="lbl_origin" runat="server" />
                </td>

                <td class="field" style="width: 12% !important; text-align: right">Destination:                 
                </td>
                <td class="input-field" style="width: 20%">
                    <asp:Label ID="lbl_Destination" runat="server" />

                </td>
                <td class="field" style="width: 12% !important; text-align: right">COD Amount:</td>
                <td class="input-field" style="width: 20%">
                    <asp:Label ID="lbl_CODamount" runat="server" />
                </td>

            </tr>
        </table>
        <span id="Table_1" class="tbl-large">
            <asp:GridView ID="gv_debriefing" runat="server" AutoGenerateColumns="false" CssClass="mGrid floatgrid"
                AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                        <ItemTemplate>
                            <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--                    <asp:BoundField HeaderText="Shipper" DataField="shipperName" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="7%"></asp:BoundField>
                    <asp:BoundField HeaderText="Shipper Address" DataField="shipperAddress" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Shipper Contact" DataField="shipperContact" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="7%"></asp:BoundField>
                    <asp:BoundField HeaderText="Consignee" DataField="consigneeName" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Width="7%"></asp:BoundField>
                    <asp:BoundField HeaderText="Consignee Address" DataField="consigneeAddress" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Consignee Contact" DataField="consigneeContact" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Width="7%"></asp:BoundField>--%>
                    <asp:BoundField HeaderText="Runsheet" DataField="runsheetNumber" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Comments" DataField="comments" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Width="10%"></asp:BoundField>
                     <asp:BoundField HeaderText="Reason" DataField="reason" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Width="10%"></asp:BoundField>
                     <asp:BoundField HeaderText="Status" DataField="value" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Date" DataField="createdOn" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="7%"></asp:BoundField>
                    <%-- <asp:BoundField HeaderText="Zone" DataField="zoneCode" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Width="7%"></asp:BoundField>--%>
                    <asp:BoundField HeaderText="City" DataField="branchCode" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Width="7%"></asp:BoundField>
                    <asp:BoundField HeaderText="Created By" DataField="U_NAME" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Width="7%"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </span>
    </fieldset>

    <fieldset>
        <legend style="font-size: small;">Consignment Tracking Details</legend>
        <span id="Table_1" class="tbl-large">
            <asp:GridView ID="gv_tracking" runat="server" AutoGenerateColumns="false" CssClass="mGrid floatgrid"
                AlternatingRowStyle-CssClass="alt" Style="word-wrap: break-word;">
                <Columns>

                    <asp:BoundField HeaderText="Transaction Time" DataField="transactionTime" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="5%"></asp:BoundField>
                    <asp:BoundField HeaderText="Event" DataField="TrackingStatus" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField HeaderText="Location" DataField="currentLocation" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="5%"></asp:BoundField>
                    <asp:BoundField HeaderText="Message" DataField="Detail" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Width="50%"></asp:BoundField>
                </Columns>
            </asp:GridView>
    </fieldset>

</asp:Content>

