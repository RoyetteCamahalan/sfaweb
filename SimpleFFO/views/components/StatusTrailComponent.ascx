<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatusTrailComponent.ascx.cs" Inherits="SimpleFFO.views.components.StatusTrailComponent" %>
<%@ Import Namespace="SimpleFFO.Model" %>
<asp:Panel ID="panelstatustrail" runat="server" CssClass="card card-info" Visible="false">
    <div class="card-header">
        <h3 class="card-title">Approval Trail</h3>
    </div>
    <div class="card-body">
        <asp:GridView runat="server" ID="dgvstatustrail"
            AutoGenerateColumns="false"
            ShowHeaderWhenEmpty="true"
            EmptyDataText="No record added"
            GridLines="None"
            CssClass="table table-bordered table-hover dataTable dtr-inline">
            <HeaderStyle HorizontalAlign="Center" />
            <Columns>
                <asp:TemplateField HeaderText="Date" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbldate" runat="server" Text='<%# Convert.ToDateTime(Eval("traildate")).ToString(AppModels.datetimeformat) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee" ItemStyle-Width="25%" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblname" runat="server" Text='<%# Eval("employee.firstname").ToString() + " " + Eval("employee.lastname").ToString() %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <span><%# Eval("remarks") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <label class='<%# getRequestStatusBadge((int)Eval("statusid")) %>'><%# AppModels.Status.getStatus(Convert.ToInt32(Eval("statusid"))) %></label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Panel>
<asp:Panel ID="panelapprovalaction" runat="server" CssClass="card card-success" Visible="false">
                <div class="card-header">
                    <h3 class="card-title">My Action</h3>
                </div>
                <asp:UpdatePanel ID="upanelactions" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="card-body">
                            <div class="form-group">
                                <label>Remarks</label>
                                <asp:TextBox ID="txtActionRemarks" runat="server" CssClass="form-control rounded-0" Text="" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                <span class="error invalid-feedback"><asp:Literal ID="lblerroractionremarks" runat="server"></asp:Literal></span>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 form-group">
                                    <asp:LinkButton ID="btnaccept" runat="server" CssClass="btn btn-success btn-flat float-right" Text="" CausesValidation="false" OnClick="btnSaveActivityAction_Click"></asp:LinkButton>
                                    <asp:LinkButton ID="btnreject" runat="server" CssClass="btn btn-danger btn-flat float-right" Style="margin-right: 5px;" Text="Reject" CausesValidation="false" OnClick="btnSaveActivityAction_Click"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnaccept" />
                        <asp:AsyncPostBackTrigger ControlID="btnreject" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>