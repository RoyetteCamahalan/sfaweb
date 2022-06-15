<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApprovalTree.aspx.cs" Inherits="SimpleFFO.views.ApprovalTree" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentAdditionalCSS" runat="server">
    <webopt:BundleReference runat="server" Path="~/Content/kanban" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Import Namespace="SimpleFFO.Model" %>
    <div class="content-wrapper kanban">
        <section class="content-header">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-6">
                        <h1>Branch Approval Tree</h1>
                    </div>
                </div>
            </div>
        </section>
        <section class="content pb-3">
            <asp:UpdatePanel ID="panelmain" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-lg-12 form-group">
                            <div class="col-sm-3 float-right">
                                <asp:DropDownList ID="cmbdistrictmanager" runat="server" CssClass="custom-select rounded-0" AutoPostBack="true" OnSelectedIndexChanged="cmbBranches_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="container-fluid h-100">
                        <asp:ListView ID="lstmodules" runat="server" OnItemDataBound="lstmodules_ItemDataBound">
                            <ItemTemplate>
                                <div class="card card-row card-secondary">
                                    <asp:UpdatePanel ID="panellstdetailcontainer" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="card-header bg-info">
                                                <h3 class="card-title"><%# Eval("name") %>
                                                </h3>

                                                <div class="card-tools">
                                                    <asp:LinkButton ID="btnedit" runat="server" CssClass="btn btn-tool" CausesValidation="false" OnClick="btnedit_Click" CommandArgument='<%# Eval("module_id") %>'>
                                                        <i id="btnediticon" runat="server" class="fas fa-pen"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="card-body">
                                                <asp:ListView ID="lstdetail" runat="server" OnItemDataBound="lstdetails_ItemDataBound" class="">
                                                    <ItemTemplate>

                                                        <div class="form-group" style="padding: 0.25rem 1rem 0 0; position: absolute; right: 0">
                                                            <asp:LinkButton ID="btnremove" runat="server" Visible="false" CausesValidation="false" OnClick="btnremove_Click" CommandArgument='<%# Eval("moduleid").ToString() + "_" +Container.DataItemIndex.ToString() %>'><i class="fas fa-times text-danger"></i></asp:LinkButton>
                                                        </div>
                                                        <div class="rounded-0" style="display: flex; margin-bottom: 1rem; padding: 1rem; box-shadow: 0 1px 3px rgba(0,0,0,.12), 0 1px 2px rgba(0,0,0,.24); background-color: #fff; border-left: 5px solid #e9ecef; border-left-color: #117a8b; align-items: center;">

                                                            <div style="flex-grow: 8;">
                                                                <p>
                                                                    <asp:Literal ID="lblemployeename" runat="server"></asp:Literal>
                                                                </p>
                                                                <div>
                                                                    <label style="font-weight: 400;">Action:</label>
                                                                    <asp:DropDownList ID="cmbstatus" runat="server" AppendDataBoundItems="true" CssClass="custom-select rounded-0" Style="width: auto" Enabled="false">
                                                                        <asp:ListItem Selected="True" Text="--Select Action--" Value="-1"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div style="flex-grow: 1; margin-top: 0.5rem">
                                                                <div class="float-right">
                                                                    <div class="row">
                                                                        <asp:LinkButton ID="btnup" runat="server" Style="text-align: center; width: 100%; color: #117a8b;" Visible="false" CausesValidation="false" CommandArgument='<%# Eval("moduleid").ToString() + "_" +Container.DataItemIndex.ToString() %>' OnClick="btnup_Click"><i class="fas fa-chevron-up"></i></asp:LinkButton>
                                                                    </div>
                                                                    <div class="row">
                                                                        <span style="width: 22px; text-align: center; background: #117a8b; border-radius: 50%; color: white;"><%# Container.DataItemIndex+1 %></span>
                                                                    </div>
                                                                    <div class="row">
                                                                        <asp:LinkButton ID="btndown" runat="server" Style="text-align: center; width: 100%; color: #117a8b;" Visible="false" CommandArgument='<%# Eval("moduleid").ToString() + "_" +Container.DataItemIndex.ToString() %>' OnClick="btndown_Click"><i class="fas fa-chevron-down"></i></asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>

                                                    <EmptyDataTemplate>
                                                        <p>
                                                            No Record Found!
                                                        </p>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                                <div class="form-group row" style="padding: 10px">
                                                    <asp:LinkButton ID="btnaddemployee" runat="server" CssClass="btn btn-info btn-flat btn-sm btn-block" CausesValidation="false"
                                                        Text="Add Employee" CommandArgument='<%# Eval("module_id") %>' OnClick="btnaddemployee_Click" Visible="false"></asp:LinkButton>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="cmbdistrictmanager" />
                </Triggers>
            </asp:UpdatePanel>
        </section>
    </div>
    <input type="button" id="btnShowModal" hidden="hidden" data-toggle="modal" data-target=".modal-simple-lib" />
    <div class="modal fade modal-simple-lib" tabindex="-1" role="dialog" aria-hidden="true">
        <asp:UpdatePanel UpdateMode="Conditional" ID="panelmodal" runat="server" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">

                        <div class="modal-header">
                            <h4 class="modal-title">
                                <asp:Literal ID="lblModalTitle" runat="server">Select Employee</asp:Literal></h4>
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <asp:DropDownList ID="cmbEmployees" runat="server" CssClass="form-control select2 rounded-0" Style="width: 100%" TabIndex="-1" AppendDataBoundItems="true">
                                    <asp:ListItem Selected="true" Value="0" Text="--Select--"></asp:ListItem>
                                </asp:DropDownList>
                                <span class="error invalid-feedback">'<%= AppModels.ErrorMessage.required %>'</span>
                            </div>
                        </div>
                        <div class="modal-footer justify-content-between">
                            <asp:Button ID="btnCloseModalBottom" runat="server" CssClass="btn btn-default" data-dismiss="modal" UseSubmitBehavior="false" CausesValidation="false" Text="Close" />
                            <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Submit" CausesValidation="false" OnClick="btnSave_Click"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentAdditionalJS" runat="server">
    <%: Scripts.Render("~/bundles/kanban") %>
    <script type="text/javascript">  
        $(document).ready(function () {
            bindJs();
        });

        function bindJs() {
            $('.select2').select2({
                dropdownParent: $('.modal-simple-lib')
            });
        };

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            // re-bind your jQuery events here
            bindJs()
        });
    </script>
</asp:Content>
