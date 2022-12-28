<%@ Page Language="C#" AutoEventWireup="true" Title="Cash Recovery" MasterPageFile="~/BtsMasterPage.Master" CodeBehind="RecoveryReport_Main.aspx.cs" Inherits="MRaabta.Files.RecoveryReport_Main" %>


<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <script>
        function Loaderjs(source) {
            document.getElementById('loaderDiv').style.display = 'block';
            document.getElementsByClassName('BtnUpdate').disabled = true;
            source.style.visibility = 'hidden'             
        }
    </script>
    <style>
        .Wheat {
            background: Wheat !important;
        }

        .mGrid input {
            width: 45px !important;
        }

        .hiddencol {
            display: none;
        }

        .input-field {
            float: left;
            width: 50%;
        }

        .input-field1 {
            float: left;
        }

        .colorHead {
            color: red;
            background-color: blue;
        }

        .buttonLink a {
            color: #fff;
            text-decoration: navajowhite;
            text-transform: uppercase;
            background: #f26726;
            padding: 10px;
            cursor: pointer;
        }

            .buttonLink a:hover {
                border: solid 1px Black;
            }
    </style>
    <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
        <tr>
            <td colspan="12" align="center" class="head_column">
                <h3 style="text-align: center">Cash Sale Collection
                </h3>
            </td>
        </tr>
    </table>

    <table style="margin-left: 5px; font-size: medium; color: black; padding-bottom: 0px; width: 100%; margin-top: 3px">


        <tr class="">
            <td class="field" style="width: 16% !important; text-align: left !important; padding-right: 10px !important;">
                <b>Express Center Code:</b>
            </td>
            <td style="width: 20%; text-align: left;">
                <asp:TextBox ID="ExpressCentertxt" runat="server" MaxLength="20" Width="180px"> </asp:TextBox>
            </td>


            <td style="width: 10%">
                <asp:Button ID="btn_get" runat="server" Text="Get" CssClass="button" OnClick="btn_Get_Data_Click" OnClientClick="Loaderjs(this)" Width="100px" />
            </td>

            <td>
                <b>
                    <div runat="server" id="statusMsg" style="color: red; font: bold"></div>
                </b>
            </td>
        </tr>

    </table>
    <div class="tbl-large">
        <table class="input-form" style="width: 40%; position: sticky; left: 40%;" runat="server" id="tblheader" visible="false">
            <tr>
                <td class="field">Zone:
                </td>
                <td class="input-field">
                    <asp:Label ID="lbl_zone" runat="server"></asp:Label></td>
                <td class="field">Branch:
                </td>
                <td class="input-field1">
                    <asp:Label ID="lbl_branch" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="field">EC Name:
                </td>
                <td class="input-field">
                    <asp:Label ID="lbl_ecname" runat="server"></asp:Label></td>
                <td class="field">EC CODE:
                </td>
                <td class="input-field1">
                    <asp:Label ID="lbl_eccode" runat="server"></asp:Label></td>
                <td class="input-field1" style="display: none">
                    <asp:Label ID="hd_lbleccode" runat="server" Visible="false"></asp:Label>

                </td>
            </tr>
        </table>
        <div id="loaderDiv" style="display: none">
        </div>
        <asp:GridView ID="ReportMainGrid" runat="server" AutoGenerateColumns="false" OnRowDataBound="ReportMainGrid_RowDataBound"
            CssClass="mGrid" OnPageIndexChanging="ReportMainGrid_PageIndexChanging"
            AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" AllowPaging="true"
            BorderWidth="1px" PageSize="40" OnDataBound="OnDataBound">
            <Columns>
                <asp:TemplateField HeaderText="S. No.">
                    <ItemTemplate>
                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="2%" />
                </asp:TemplateField>

                <asp:BoundField DataField="BookingCode" HeaderText="Booking<br>Code" HtmlEncode="false">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>

                <%--<asp:HyperLinkField DataNavigateUrlFields="DSSPNumber" Target="_blank" HeaderText="DSSP" ItemStyle-HorizontalAlign="Center"
                    DataNavigateUrlFormatString="RecoveryReport.aspx?DSSPNumber={0}" ItemStyle-Width="5%"
                    DataTextField="DSSPNumber" />--%>

                <asp:BoundField DataField="DSSPNumber" HeaderText="DSSP No">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="CreatedOn" HeaderText="DSSP<br>Created Date" HtmlEncode="false">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="CNCount" HeaderText="Total<br>CN" HtmlEncode="false">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField DataField="TotalAmount" HeaderText="Total<br>Amount" HtmlEncode="false">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="RiderAmount" HeaderText="Rider<br>Amount" HtmlEncode="false">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="ECAmount" HeaderText="EC<br>Amount" HtmlEncode="false">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                
                <asp:BoundField DataField="outstanding" HeaderText="Out<br>standing" HtmlEncode="false">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:TemplateField ItemStyle-Width="4%" HeaderText="Collected<br>Amount" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCollectedAmount" runat="server" Text='<%#Bind("CollectAmount")%>' Visible="false" />
                        <%--<asp:TextBox ID="CollectAmount_txt" runat="server" Width="60%" />--%>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="4%" HeaderText="Domestic" ControlStyle-BackColor="Wheat" HeaderStyle-BackColor="Wheat" HeaderStyle-CssClass="Wheat">
                    <ItemTemplate>
                        <asp:Label ID="lblcashdomestic" runat="server" Text='<%#Bind("CASH_Domestic")%>' Visible="false" />
                        <asp:TextBox ID="cashdomestic_txt" runat="server" Width="60%" Text='<%#Bind("CASH_Domestic")%>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="10%" HeaderText="Int." ControlStyle-BackColor="Wheat">
                    <ItemTemplate>
                        <asp:Label ID="lblcashInternational" runat="server" Text='<%#Bind("CASH_International")%>' Visible="false" />
                        <asp:TextBox ID="cashInternational_txt" runat="server" Width="100%" Text='<%#Bind("CASH_International")%>' />

                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="4%" HeaderText="RNR" ControlStyle-BackColor="Wheat">
                    <ItemTemplate>
                        <asp:Label ID="lblcashRoadRail" runat="server" Text='<%#Bind("CASH_RoadRail")%>' Visible="false" />
                        <asp:TextBox ID="cashRoadRail_txt" runat="server" Width="60%" Text='<%#Bind("CASH_RoadRail")%>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="4%" HeaderText="Domestic" ControlStyle-BackColor="Yellow">
                    <ItemTemplate>
                        <asp:Label ID="lblccdomestic" runat="server" Text='<%#Bind("CREDIT_CARD_Domestic")%>' Visible="false" />
                        <asp:TextBox ID="ccdomestic_txt" runat="server" Width="60%" Text='<%#Bind("CREDIT_CARD_Domestic")%>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="4%" HeaderText="Int." ControlStyle-BackColor="Yellow">
                    <ItemTemplate>
                        <asp:Label ID="lblccinternational" runat="server" Text='<%#Bind("CREDIT_CARD_International")%>' Visible="false" />
                        <asp:TextBox ID="ccinternational_txt" runat="server" Width="60%" Text='<%#Bind("CREDIT_CARD_International")%>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="4%" HeaderText="RNR" ControlStyle-BackColor="Yellow">
                    <ItemTemplate>
                        <asp:Label ID="lblccrnr" runat="server" Text='<%#Bind("CREDIT_CARD_RoadRail")%>' Visible="false" />
                        <asp:TextBox ID="ccrnr_txt" runat="server" Width="60%" Text='<%#Bind("CREDIT_CARD_RoadRail")%>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="4%" HeaderText="Domestic" ControlStyle-BackColor="Turquoise">
                    <ItemTemplate>
                        <asp:Label ID="lblqrdomestic" runat="server" Text='<%#Bind("QR_Domestic")%>' Visible="false" />
                        <asp:TextBox ID="qrdomestic_txt" runat="server" Width="60%" Text='<%#Bind("QR_Domestic")%>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="4%" HeaderText="Int." ControlStyle-BackColor="Turquoise">
                    <ItemTemplate>
                        <asp:Label ID="lblqrint" runat="server" Text='<%#Bind("QR_International")%>' Visible="false" />
                        <asp:TextBox ID="qrInt_txt" runat="server" Width="60%" Text='<%#Bind("QR_International")%>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="4%" HeaderText="RNR" ControlStyle-BackColor="Turquoise">
                    <ItemTemplate>
                        <asp:Label ID="lblqrrnr" runat="server" Text='<%#Bind("QR_RoadRail")%>' Visible="false" />
                        <asp:TextBox ID="qrrnr_txt" runat="server" Width="60%" Text='<%#Bind("QR_RoadRail")%>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="4%" HeaderText="Payment" ControlStyle-BackColor="#ffcccc">
                    <ItemTemplate>
                        <asp:Label ID="lblriderpayment" runat="server" Text='<%#Bind("RiderPayment")%>' Visible="false" />
                        <asp:TextBox ID="riderpayment_txt" runat="server" Width="60%" Text='<%#Bind("RiderPayment")%>' />
                        <asp:TextBox ID="riderpayment_txt_hidden" runat="server" Visible="false" Width="60%" Text='<%#Bind("RiderPayment")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CollectAmount" HeaderText="Outstanding" HtmlEncode="false" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:TemplateField HeaderText="Action" ItemStyle-Width="7%" ItemStyle-CssClass="buttonLink" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="Update" CommandName="Update" OnClientClick="Loaderjs(this)" CssClass="BtnUpdate" OnClick="lnkUpdate_Click" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">Update</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
